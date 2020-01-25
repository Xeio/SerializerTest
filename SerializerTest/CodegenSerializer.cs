using System;
using System.Collections;
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

        public static void Serialize<T>(T obj, Utf8JsonWriter writer)
        {
            var method = GetOrPopulateCacheDelegate<T>();
            method(obj, writer, null);
            writer.Flush();
        }

        private static Delegate BuildCache<T>()
        {
            var type = typeof(T);
            var enumerableType = type.GetInterfaces().FirstOrDefault(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IEnumerable<>));
            if (enumerableType != null)
            {
                var enumerableGenericType = enumerableType.GetGenericArguments()[0];
                var delegateType = typeof(Action<,,>).MakeGenericType(enumerableType, typeof(Utf8JsonWriter), typeof(string));
                if (enumerableGenericType == typeof(string))
                {
                    return (Action<IEnumerable<string>, Utf8JsonWriter, string>)WriteEnumerableString;
                }
                if (NumericEnumerableWriters.NumericEnumerableDelegates.TryGetValue(enumerableGenericType, out var del))
                {
                    return del;
                }
                return WriteEnumerable(enumerableGenericType);
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
            writeObjectBlockContents.AddRange(BuildPropertyWriters(type, objectParam, writerParam));
            writeObjectBlockContents.Add(Expression.Call(writerParam, "WriteEndObject", null));

            var fullMethodBlock = Expression.Block(writeObjectBlockContents);

            var lambda = Expression.Lambda(fullMethodBlock, objectParam, writerParam, nameParam);
            return lambda.Compile();
        }

        private static Delegate WriteEnumerable(Type enumerableGenericType)
        {
            var objectParam = Expression.Parameter(typeof(IEnumerable<>).MakeGenericType(enumerableGenericType), "object");
            var writerParam = Expression.Parameter(typeof(Utf8JsonWriter), "writer");
            var nameParam = Expression.Parameter(typeof(string), "name");

            var enumerator = Expression.Variable(typeof(IEnumerator<>).MakeGenericType(enumerableGenericType));
            var breakLabel = Expression.Label("objectsLoopBreak");
            var obj = Expression.Variable(enumerableGenericType);

            var writeObjectBlockContents = new List<Expression>
            {
                Expression.Assign(obj, Expression.Property(enumerator, "Current")),
                Expression.Call(writerParam, "WriteStartObject", null)
            };
            writeObjectBlockContents.AddRange(BuildPropertyWriters(enumerableGenericType, obj, writerParam));
            writeObjectBlockContents.Add(Expression.Call(writerParam, "WriteEndObject", null));

            var objectsLoopBody = Expression.IfThenElse(
                                    Expression.IsTrue(Expression.Call(enumerator, typeof(IEnumerator).GetMethod("MoveNext"))),
                                    Expression.Block(writeObjectBlockContents),
                                    Expression.Goto(breakLabel)
                                    );

            var fullMethodBlock = Expression.Block(
                    new[] { enumerator, obj },
                    Expression.Call(writerParam, "WriteStartArray", null),
                    Expression.Assign(enumerator, Expression.Call(objectParam, "GetEnumerator", null)),
                    Expression.Loop(objectsLoopBody, breakLabel),
                    Expression.Call(writerParam, "WriteEndArray", null),
                    Expression.Call(enumerator, typeof(IDisposable).GetMethod("Dispose"))
                );

            var lambda = Expression.Lambda(fullMethodBlock, objectParam, writerParam, nameParam);
            return lambda.Compile();
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

        private static void WriteEnumerableString(IEnumerable<string> strings, Utf8JsonWriter writer, string name)
        {
            if (name == null)
            {
                writer.WriteStartArray();
            }
            else
            {
                writer.WriteStartArray(name);
            }
            foreach (var s in strings)
            {
                writer.WriteStringValue(s);
            }
            writer.WriteEndArray();
        }

        private static IEnumerable<Expression> BuildPropertyWriters(Type type, Expression objectParam, Expression writerParam)
        {
            var members = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty);

            foreach (PropertyInfo property in members)
            {
                if (GetPrimitiveWriterExpression(property, objectParam, writerParam, out var writePrimitiveExpression))
                {
                    yield return writePrimitiveExpression;
                }
                else
                {
                    yield return
                        Expression.IfThenElse(
                            Expression.Equal(Expression.Property(objectParam, property), Expression.Constant(null)),
                            Expression.Call(writerParam, "WriteNull", null, Expression.Constant(property.Name)),
                            Expression.Invoke(Expression.Call(typeof(CodegenSerializer), nameof(GetOrPopulateCacheDelegate), new[] { property.PropertyType }), Expression.Property(objectParam, property), writerParam, Expression.Constant(property.Name)));
                }

            }
        }

        private static bool GetPrimitiveWriterExpression(PropertyInfo property, Expression objectParam, Expression writerParam, out Expression expression)
        {
            expression = null;
            Expression propertyExpression = Expression.Property(objectParam, property);
            var propertyType = property.PropertyType;
            var underlyingType = Nullable.GetUnderlyingType(property.PropertyType);
            if (underlyingType != null)
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

            if (underlyingType != null && expression != null)
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