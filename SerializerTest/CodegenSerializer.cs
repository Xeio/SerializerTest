using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json;

namespace SerializerTest
{
    public static class CodegenSerializer
    {
        private static Dictionary<Type, Delegate> Cache { get; } = new Dictionary<Type, Delegate>();
        private static readonly MethodInfo EnumerableMethod = typeof(CodegenSerializer).GetMethod(nameof(WriteEnumerable), BindingFlags.NonPublic | BindingFlags.Static);

        public static void Serialize<T>(T obj, Utf8JsonWriter writer)
        {
            var method = GetOrPopulateCacheDelegate<T>();
            method(obj, writer, null);
            writer.Flush();
        }

        private static Delegate BuildCache<T>()
        {
            var type = typeof(T);
            if (type.IsValueType)
            { throw new NotImplementedException(); }
            var members = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty);

            var enumerableType = type.GetInterfaces().FirstOrDefault(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IEnumerable<>));
            if(enumerableType != null)
            {
                var enumerableGenericType = enumerableType.GetGenericArguments()[0];
                var delegateType = typeof(Action<,,>).MakeGenericType(enumerableType, typeof(Utf8JsonWriter), typeof(string));
                if(enumerableGenericType == typeof(string))
                {
                    return (Action<IEnumerable<string>, Utf8JsonWriter, string>)WriteEnumerableString;
                }
                if(NumericEnumerableWriters.NumericEnumerableDelegates.TryGetValue(enumerableGenericType, out var del))
                {
                    return del;
                }
                return EnumerableMethod.MakeGenericMethod(enumerableGenericType).CreateDelegate(delegateType);
            }

            var objectParam = Expression.Parameter(type, "object");
            var writerParam = Expression.Parameter(typeof(Utf8JsonWriter), "writer");
            var nameParam = Expression.Parameter(typeof(string), "name");

            var writeObjectBlockContents = new List<Expression>
            {
                Expression.IfThenElse(
                    Expression.Equal(nameParam, Expression.Constant(null)),
                    Expression.Call(writerParam, "WriteStartObject", null),
                    Expression.Call(writerParam, "WriteStartObject", null, nameParam))
            };
            foreach (PropertyInfo property in members)
            {
                if(GetPrimitiveWriterExpression(property, objectParam, writerParam, out var writePrimitiveExpression))
                {
                    writeObjectBlockContents.Add(writePrimitiveExpression);
                }
                //TODO: Structs?
                else
                {
                    writeObjectBlockContents.Add(
                        Expression.IfThenElse(
                            Expression.Equal(Expression.Property(objectParam, property), Expression.Constant(null)),
                            Expression.Call(writerParam, "WriteNull", null, Expression.Constant(property.Name)),
                            Expression.Invoke(Expression.Call(typeof(CodegenSerializer), nameof(GetOrPopulateCacheDelegate), new[] { property.PropertyType }), Expression.Property(objectParam, property), writerParam, Expression.Constant(property.Name))));
                }
            }
            writeObjectBlockContents.Add(Expression.Call(writerParam, "WriteEndObject", null));

            var fullMethodBlock = Expression.Block(writeObjectBlockContents);

            var lambda = Expression.Lambda(fullMethodBlock, objectParam, writerParam, nameParam);
            return lambda.Compile(false);
        }

        private static void WriteEnumerable<T>(IEnumerable<T> objects, Utf8JsonWriter writer, string _)
        {
            var method = GetOrPopulateCacheDelegate<T>();
            writer.WriteStartArray();
            foreach (var obj in objects)
            {
                method(obj, writer, null);
            }
            writer.WriteEndArray();
        }

        private static Action<T, Utf8JsonWriter, string> GetOrPopulateCacheDelegate<T>()
        {
            Cache.TryGetValue(typeof(T), out var del);
            if (del == null)
            {
                Cache[typeof(T)] = del = BuildCache<T>();
            }
            return (Action<T, Utf8JsonWriter, string>)del;
        }

        private static void WriteEnumerableString(IEnumerable<string> strings, Utf8JsonWriter writer, string _)
        {
            writer.WriteStartArray();
            foreach (var s in strings)
            {
                writer.WriteStringValue(s);
            }
            writer.WriteEndArray();
        }

        private static bool GetPrimitiveWriterExpression(PropertyInfo property, Expression objectParam, Expression writerParam, out Expression expression)
        {
            expression = null;
            Expression propertyExpression = Expression.Property(objectParam, property);
            var propertyType = property.PropertyType;
            var underlyingType = Nullable.GetUnderlyingType(property.PropertyType);
            if(underlyingType != null)
            {
                propertyType = underlyingType;
                //Have to cast nullable types to non-null so the method parameters match (we'll add the check for null at the end)
                propertyExpression = Expression.Convert(Expression.Property(objectParam, property), underlyingType);
            }

            if (propertyType == typeof(string) || propertyType == typeof(DateTime)
                     || propertyType == typeof(DateTimeOffset) || propertyType == typeof(Guid)
                      || propertyType == typeof(JsonEncodedText))
            {
                expression =
                    Expression.Call(writerParam, "WriteString", null,
                        Expression.Constant(property.Name), propertyExpression);
            }
            else if (propertyType == typeof(decimal) || propertyType == typeof(double) ||
                        propertyType == typeof(float) || propertyType == typeof(int) ||
                        propertyType == typeof(long) || propertyType == typeof(uint) ||
                        propertyType == typeof(ulong))
            {
                expression =
                    Expression.Call(writerParam, "WriteNumber", null,
                        Expression.Constant(property.Name), propertyExpression);
            }
            else if (propertyType == typeof(short) || propertyType == typeof(ushort)
                        || propertyType == typeof(byte) || propertyType == typeof(sbyte))
            {
                expression =
                    Expression.Call(writerParam, "WriteNumber", null,
                        Expression.Constant(property.Name), Expression.Convert(Expression.Property(objectParam, property), typeof(int)));
            }

            if(underlyingType != null && expression != null)
            {
                //If we're a nullable, wrap the whole thing in a null check
                expression = Expression.IfThenElse(
                    Expression.Equal(Expression.Property(objectParam, property), Expression.Constant(null)),
                    Expression.Call(writerParam, "WriteNull", null, Expression.Constant(property.Name)),
                    expression);
            }

            return expression != null;
        }
    }
}
