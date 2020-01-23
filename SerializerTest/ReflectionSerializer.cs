using System.Collections.Generic;
using System.Reflection;
using System.Text.Json;

namespace SerializerTest
{
    public static class ReflectionSerializer
    {
        public static void Serialize<T>(List<T> objects, Utf8JsonWriter writer)
        {
            var members = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty);
            writer.WriteStartArray();
            foreach (var obj in objects)
            {
                writer.WriteStartObject();
                foreach (var member in members)
                {
                    switch (member.GetValue(obj))
                    {
                        case string s:
                            writer.WriteString(member.Name, s);
                            break;
                        case decimal d:
                            writer.WriteNumber(member.Name, d);
                            break;
                        case int i:
                            writer.WriteNumber(member.Name, i);
                            break;
                    }
                }
                writer.WriteEndObject();
            }
            writer.WriteEndArray();
            writer.Flush();
        }
    }
}
