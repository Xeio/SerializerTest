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
            method(obj, writer);
            writer.Flush();
        }

        private static Delegate BuildCache<T>()
        {
            var type = typeof(T);
            var members = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty);

            var enumerableType = type.GetInterfaces().FirstOrDefault(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IEnumerable<>));
            if(enumerableType != null)
            {
                var enumerableGenericType = enumerableType.GetGenericArguments()[0];
                var delegateType = typeof(Action<,>).MakeGenericType(enumerableType, typeof(Utf8JsonWriter));
                return EnumerableMethod.MakeGenericMethod(enumerableGenericType).CreateDelegate(delegateType);
            }

            var objectParam = Expression.Parameter(type, "object");
            var writerParam = Expression.Parameter(typeof(Utf8JsonWriter), "writer");

            var writeObjectBlockContents = new List<Expression>
            {
                Expression.Call(writerParam, "WriteStartObject", null)
            };
            foreach (PropertyInfo property in members)
            {
                if (property.PropertyType == typeof(string))
                {
                    writeObjectBlockContents.Add(
                        Expression.Call(writerParam, "WriteString", null,
                            Expression.Constant(property.Name), Expression.Property(objectParam, property)));
                }
                else if (property.PropertyType == typeof(decimal))
                {
                    writeObjectBlockContents.Add(
                        Expression.Call(writerParam, "WriteNumber", null,
                            Expression.Constant(property.Name), Expression.Property(objectParam, property)));
                }
                else if (property.PropertyType == typeof(int))
                {
                    writeObjectBlockContents.Add(
                        Expression.Call(writerParam, "WriteNumber", null,
                            Expression.Constant(property.Name), Expression.Property(objectParam, property)));
                }
                //TODO: Other Primitives
                //TODO: Enumerables?
                //TODO: Structs?
                //TODO: Object types (including potentially recursive serialization!)
            }
            writeObjectBlockContents.Add(Expression.Call(writerParam, "WriteEndObject", null));

            var fullMethodBlock = Expression.Block(writeObjectBlockContents);

            var lambda = Expression.Lambda(fullMethodBlock, objectParam, writerParam);
            return lambda.Compile(false);
        }

        private static void WriteEnumerable<T>(IEnumerable<T> objects, Utf8JsonWriter writer)
        {
            var method = GetOrPopulateCacheDelegate<T>();
            writer.WriteStartArray();
            foreach (var obj in objects)
            {
                method(obj, writer);
            }
            writer.WriteEndArray();
        }

        private static Action<T, Utf8JsonWriter> GetOrPopulateCacheDelegate<T>()
        {
            Cache.TryGetValue(typeof(T), out var del);
            if (del == null)
            {
                Cache[typeof(T)] = del = BuildCache<T>();
            }
            return (Action<T, Utf8JsonWriter>)del;
        }
    }
}
