using System;
using System.Collections.Generic;
using System.Text.Json;

namespace SerializerTest
{
    public static class StringEnumerableWriters
    {
		public static readonly Dictionary<Type, Delegate> StringEnumerableDelegates = new Dictionary<Type, Delegate>()
		{
			{ typeof(DateTime), (Action<IEnumerable<DateTime>, Utf8JsonWriter, JsonEncodedText?>)WriteEnumerableDateTime},
			{ typeof(Guid), (Action<IEnumerable<Guid>, Utf8JsonWriter, JsonEncodedText?>)WriteEnumerableGuid},
			{ typeof(DateTimeOffset), (Action<IEnumerable<DateTimeOffset>, Utf8JsonWriter, JsonEncodedText?>)WriteEnumerableDateTimeOffset},
			{ typeof(JsonEncodedText), (Action<IEnumerable<JsonEncodedText>, Utf8JsonWriter, JsonEncodedText?>)WriteEnumerableJsonEncodedText},
		};

		public static void WriteEnumerableDateTime(IEnumerable<DateTime> numbers, Utf8JsonWriter writer, JsonEncodedText? name)
		{
			if (name == null)
			{
				writer.WriteStartArray();
			}
			else
			{
				writer.WriteStartArray((JsonEncodedText)name);
			}
            foreach (var num in numbers)
            {
                writer.WriteStringValue(num);
            }
            writer.WriteEndArray();
		}

		public static void WriteEnumerableGuid(IEnumerable<Guid> numbers, Utf8JsonWriter writer, JsonEncodedText? name)
		{
			if (name == null)
			{
				writer.WriteStartArray();
			}
			else
			{
				writer.WriteStartArray((JsonEncodedText)name);
			}
            foreach (var num in numbers)
            {
                writer.WriteStringValue(num);
            }
            writer.WriteEndArray();
		}

		public static void WriteEnumerableDateTimeOffset(IEnumerable<DateTimeOffset> numbers, Utf8JsonWriter writer, JsonEncodedText? name)
		{
			if (name == null)
			{
				writer.WriteStartArray();
			}
			else
			{
				writer.WriteStartArray((JsonEncodedText)name);
			}
            foreach (var num in numbers)
            {
                writer.WriteStringValue(num);
            }
            writer.WriteEndArray();
		}


		public static void WriteEnumerableJsonEncodedText(IEnumerable<JsonEncodedText> numbers, Utf8JsonWriter writer, JsonEncodedText? name)
		{
			if (name == null)
			{
				writer.WriteStartArray();
			}
			else
			{
				writer.WriteStartArray((JsonEncodedText)name);
			}
            foreach (var num in numbers)
            {
                writer.WriteStringValue(num);
            }
            writer.WriteEndArray();
		}
	}
}