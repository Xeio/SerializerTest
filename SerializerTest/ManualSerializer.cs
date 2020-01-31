using System.Collections.Generic;
using System.Text.Json;
using TestObjects;

namespace SerializerTest
{
    public static class ManualSerializer
    {
        public static void Serialize(List<TestObj> objects, Utf8JsonWriter writer)
        {
            writer.WriteStartArray();
            var fooStringName = JsonEncodedText.Encode("FooString");
            var barDecimalName = JsonEncodedText.Encode("BarDecimal");
            var bazIntName = JsonEncodedText.Encode("BazInt");
            foreach (var obj in objects)
            {
                writer.WriteStartObject();
                writer.WriteString(fooStringName, obj.FooString);
                writer.WriteNumber(barDecimalName, obj.BarDecimal);
                writer.WriteNumber(bazIntName, obj.BazInt);
                writer.WriteEndObject();
            }
            writer.WriteEndArray();
            writer.Flush();
        }
    }
}
