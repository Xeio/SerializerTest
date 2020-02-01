using System.Collections.Generic;
using System.Text.Json;
using TestObjects;

namespace SerializerTest
{
    public static class ManualSerializer
    {
        static readonly JsonEncodedText _fooStringName = JsonEncodedText.Encode("FooString");
        static readonly JsonEncodedText _barDecimalName = JsonEncodedText.Encode("BarDecimal");
        static readonly JsonEncodedText _bazIntName = JsonEncodedText.Encode("BazInt");

        public static void Serialize(List<TestObj> objects, Utf8JsonWriter writer)
        {
            writer.WriteStartArray();
            
            foreach (var obj in objects)
            {
                writer.WriteStartObject();
                writer.WriteString(_fooStringName, obj.FooString);
                writer.WriteNumber(_barDecimalName, obj.BarDecimal);
                writer.WriteNumber(_bazIntName, obj.BazInt);
                writer.WriteEndObject();
            }
            writer.WriteEndArray();
            writer.Flush();
        }
    }
}
