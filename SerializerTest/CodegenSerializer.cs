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
        public static void Serialize<T>(T obj, Utf8JsonWriter writer)
        {
            DelegateCacheType<T>.cachedDelegate(obj, writer, null);
            writer.Flush();
        }
    }

    internal class DelegateCacheType<CacheType>
    {
        public static Action<CacheType, Utf8JsonWriter, JsonEncodedText?> cachedDelegate;
        static DelegateCacheType()
        {
            cachedDelegate = (Action<CacheType, Utf8JsonWriter, JsonEncodedText?>)BuildCache<CacheType>();
        }

        private static Delegate BuildCache<T>()
        {
            var type = typeof(T);

            var ilistType = type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IList<>) ? type :
                type.GetInterfaces().FirstOrDefault(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IList<>));
            if (ilistType != null && !type.IsValueType)
            {
                var ilistGenericType = ilistType.GetGenericArguments()[0];
                if (EnumerableWriters.EnumerableDelegates.TryGetValue(ilistType, out var del))
                {
                    return del;
                }
                return WriteIList(ilistGenericType);
            }

            var enumerableType = type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>) ? type :
                type.GetInterfaces().FirstOrDefault(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IEnumerable<>));
            if (enumerableType != null)
            {
                var enumerableGenericType = enumerableType.GetGenericArguments()[0];
                if (EnumerableWriters.EnumerableDelegates.TryGetValue(enumerableType, out var del))
                {
                    return del;
                }
                return WriteEnumerable(enumerableGenericType);
            }

            var objectParam = Expression.Parameter(type, "object");
            var writerParam = Expression.Parameter(typeof(Utf8JsonWriter), "writer");
            var nameParam = Expression.Parameter(typeof(JsonEncodedText?), "name");

            var writeObjectBlockContents = new List<Expression>
            {
                Expression.IfThenElse(
                    Expression.Equal(nameParam, Expression.Constant(null)),
                    Expression.Call(writerParam, "WriteStartObject", null),
                    Expression.Call(writerParam, "WriteStartObject", null, Expression.Property(nameParam, "Value")))
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
            var nameParam = Expression.Parameter(typeof(JsonEncodedText?), "name");

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

        private static Delegate WriteIList(Type enumerableGenericType)
        {
            var objectParam = Expression.Parameter(typeof(IList<>).MakeGenericType(enumerableGenericType), "object");
            var writerParam = Expression.Parameter(typeof(Utf8JsonWriter), "writer");
            var nameParam = Expression.Parameter(typeof(JsonEncodedText?), "name");

            var breakLabel = Expression.Label("objectsLoopBreak");
            var obj = Expression.Variable(enumerableGenericType);
            var loopVar = Expression.Variable(typeof(int));
            var countVar = Expression.Variable(typeof(int));

            var loopBlockContents = new List<Expression>
            {
                Expression.Assign(obj, Expression.Property(objectParam, "Item", loopVar)),
                Expression.Call(writerParam, "WriteStartObject", null)
            };
            loopBlockContents.AddRange(BuildPropertyWriters(enumerableGenericType, obj, writerParam));
            loopBlockContents.Add(Expression.Call(writerParam, "WriteEndObject", null));
            loopBlockContents.Add(Expression.PostIncrementAssign(loopVar));

            var objectsLoopBody = Expression.IfThenElse(
                                    Expression.LessThan(loopVar, countVar),
                                    Expression.Block(loopBlockContents),
                                    Expression.Goto(breakLabel)
                                    );

            var countMemberInfo = typeof(ICollection<>).MakeGenericType(enumerableGenericType).GetProperty("Count");
            var fullMethodBlock = Expression.Block(
                    new[] { obj, loopVar, countVar },
                    Expression.Call(writerParam, "WriteStartArray", null),
                    Expression.Assign(loopVar, Expression.Constant(0)),
                    Expression.Assign(countVar, Expression.Property(objectParam, countMemberInfo)),
                    Expression.Loop(objectsLoopBody, breakLabel),
                    Expression.Call(writerParam, "WriteEndArray", null)
                );

            var lambda = Expression.Lambda(fullMethodBlock, objectParam, writerParam, nameParam);
            return lambda.Compile();
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
                    var fieldInfo = typeof(DelegateCacheType<>).MakeGenericType(property.PropertyType).GetField(nameof(cachedDelegate));
                    var propertyNameExpression = Expression.Constant(JsonEncodedText.Encode(property.Name));
                    yield return
                        Expression.IfThenElse(
                            Expression.Equal(Expression.Property(objectParam, property), Expression.Constant(null)),
                            Expression.Call(writerParam, "WriteNull", null, propertyNameExpression),
                            Expression.Invoke(Expression.Field(null, fieldInfo), Expression.Property(objectParam, property), writerParam, Expression.Convert(propertyNameExpression, typeof(JsonEncodedText?))));
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

            var propertyNameExpression = Expression.Constant(JsonEncodedText.Encode(property.Name));
            if (propertyType == typeof(string) || propertyType == typeof(DateTime)
                     || propertyType == typeof(DateTimeOffset) || propertyType == typeof(Guid)
                      || propertyType == typeof(JsonEncodedText))
            {
                expression =
                    Expression.Call(writerParam, "WriteString", null,
                        propertyNameExpression, propertyExpression);
            }
            else if (propertyType == typeof(decimal) || propertyType == typeof(double) ||
                        propertyType == typeof(float) || propertyType == typeof(int) ||
                        propertyType == typeof(long) || propertyType == typeof(uint) ||
                        propertyType == typeof(ulong))
            {
                expression =
                    Expression.Call(writerParam, "WriteNumber", null,
                        propertyNameExpression, propertyExpression);
            }
            else if (propertyType == typeof(short) || propertyType == typeof(ushort)
                        || propertyType == typeof(byte) || propertyType == typeof(sbyte))
            {
                expression =
                    Expression.Call(writerParam, "WriteNumber", null,
                        propertyNameExpression, Expression.Convert(Expression.Property(objectParam, property), typeof(int)));
            }
            else if (propertyType == typeof(bool))
            {
                expression =
                    Expression.Call(writerParam, "WriteBoolean", null,
                        propertyNameExpression, propertyExpression);
            }

            if (underlyingType != null && expression != null)
            {
                //If we're a nullable, wrap the whole thing in a null check
                expression = Expression.IfThenElse(
                    Expression.Equal(Expression.Property(objectParam, property), Expression.Constant(null)),
                    Expression.Call(writerParam, "WriteNull", null, propertyNameExpression),
                    expression);
            }

            return expression != null;
        }
    }
}