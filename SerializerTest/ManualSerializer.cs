using System.Collections.Generic;
using System.Text.Json;

namespace SerializerTest
{
    public static class ManualSerializer
    {
        public static void Serialize(List<TestObj> objects, Utf8JsonWriter writer)
        {
            writer.WriteStartArray();
            foreach (var obj in objects)
            {
                writer.WriteStartObject();
                writer.WriteString("FooString", obj.FooString);
                writer.WriteNumber("BarDecimal", obj.BarDecimal);
                writer.WriteNumber("BazInt", obj.BazInt);
                writer.WriteEndObject();
            }
            writer.WriteEndArray();
            writer.Flush();
        }
    }
}
