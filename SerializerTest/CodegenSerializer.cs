using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json;

namespace SerializerTest
{
    public static class CodegenSerializer
    {
        private static Dictionary<Type, Delegate> Cache { get; } = new Dictionary<Type, Delegate>();

        public static void Serialize<T>(List<T> objects, Utf8JsonWriter writer)
        {
            Cache.TryGetValue(typeof(T), out var del);
            if (del == null)
            {
                Cache[typeof(T)] = del = BuildCache<T>();
            }
            var method = (Action<List<T>, Utf8JsonWriter>)del;
            method(objects, writer);
            writer.Flush();
        }

        private static Delegate BuildCache<T>()
        {
            var type = typeof(T);
            var members = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty);
            var objectsParam = Expression.Parameter(typeof(List<T>), "objects");
            var writerParam = Expression.Parameter(typeof(Utf8JsonWriter), "writer");

            var breakLabel = Expression.Label("objectsLoopBreak");
            var loopVar = Expression.Variable(typeof(int));
            var objectsLength = Expression.Property(objectsParam, "Count");
            var obj = Expression.Variable(type);

            var loopBlockContents = new List<Expression>
            {
                Expression.Assign(obj, Expression.Property(objectsParam, "Item", loopVar)),
                Expression.Call(writerParam, "WriteStartObject", null)
            };
            foreach (PropertyInfo property in members)
            {
                if (property.PropertyType == typeof(string))
                {
                    loopBlockContents.Add(
                        Expression.Call(writerParam, "WriteString", null,
                            Expression.Constant(property.Name), Expression.Property(obj, property)));
                }
                else if (property.PropertyType == typeof(decimal))
                {
                    loopBlockContents.Add(
                        Expression.Call(writerParam, "WriteNumber", null,
                            Expression.Constant(property.Name), Expression.Property(obj, property)));
                }
                else if (property.PropertyType == typeof(int))
                {
                    loopBlockContents.Add(
                        Expression.Call(writerParam, "WriteNumber", null,
                            Expression.Constant(property.Name), Expression.Property(obj, property)));
                }
                //TODO: Other Primitives
                //TODO: Enumerables?
                //TODO: Structs?
                //TODO: Object types (including potentially recursive serialization!)
            }
            loopBlockContents.Add(Expression.Call(writerParam, "WriteEndObject", null));
            loopBlockContents.Add(Expression.PreIncrementAssign(loopVar));

            var objectsLoopBody = Expression.IfThenElse(
                                    Expression.LessThan(loopVar, objectsLength),
                                    Expression.Block(loopBlockContents),
                                    Expression.Goto(breakLabel)
                                    );

            var fullMethodBlock = Expression.Block(
                    //Block variables
                    new[] { loopVar, obj },
                    Expression.Call(writerParam, "WriteStartArray", null),
                    //Loop
                    Expression.Assign(loopVar, Expression.Constant(0)),
                    Expression.Loop(objectsLoopBody, breakLabel),
                    Expression.Call(writerParam, "WriteEndArray", null)
                ); ;

            var lambda = Expression.Lambda(fullMethodBlock, objectsParam, writerParam);
            return lambda.Compile(false);
        }
    }
}
