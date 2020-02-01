using System;
using System.Collections.Generic;
using System.Text.Json;

namespace SerializerTest
{
    public static class StringEnumerableWriters
    {
		public static readonly Dictionary<Type, Delegate> StringEnumerableDelegates = new Dictionary<Type, Delegate>()
		{
			{ typeof(String), (Action<IEnumerable<String>, Utf8JsonWriter, JsonEncodedText?>)WriteEnumerableString},
			{ typeof(DateTime), (Action<IEnumerable<DateTime>, Utf8JsonWriter, JsonEncodedText?>)WriteEnumerableDateTime},
			{ typeof(Guid), (Action<IEnumerable<Guid>, Utf8JsonWriter, JsonEncodedText?>)WriteEnumerableGuid},
			{ typeof(DateTimeOffset), (Action<IEnumerable<DateTimeOffset>, Utf8JsonWriter, JsonEncodedText?>)WriteEnumerableDateTimeOffset},
			{ typeof(JsonEncodedText), (Action<IEnumerable<JsonEncodedText>, Utf8JsonWriter, JsonEncodedText?>)WriteEnumerableJsonEncodedText},
		};

		public static void WriteEnumerableString(IEnumerable<String> values, Utf8JsonWriter writer, JsonEncodedText? name)
		{
			if (name == null)
			{
				writer.WriteStartArray();
			}
			else
			{
				writer.WriteStartArray((JsonEncodedText)name);
			}
            foreach (var val in values)
            {
                writer.WriteStringValue(val);
            }
            writer.WriteEndArray();
		}

		public static void WriteEnumerableDateTime(IEnumerable<DateTime> values, Utf8JsonWriter writer, JsonEncodedText? name)
		{
			if (name == null)
			{
				writer.WriteStartArray();
			}
			else
			{
				writer.WriteStartArray((JsonEncodedText)name);
			}
            foreach (var val in values)
            {
                writer.WriteStringValue(val);
            }
            writer.WriteEndArray();
		}

		public static void WriteEnumerableGuid(IEnumerable<Guid> values, Utf8JsonWriter writer, JsonEncodedText? name)
		{
			if (name == null)
			{
				writer.WriteStartArray();
			}
			else
			{
				writer.WriteStartArray((JsonEncodedText)name);
			}
            foreach (var val in values)
            {
                writer.WriteStringValue(val);
            }
            writer.WriteEndArray();
		}

		public static void WriteEnumerableDateTimeOffset(IEnumerable<DateTimeOffset> values, Utf8JsonWriter writer, JsonEncodedText? name)
		{
			if (name == null)
			{
				writer.WriteStartArray();
			}
			else
			{
				writer.WriteStartArray((JsonEncodedText)name);
			}
            foreach (var val in values)
            {
                writer.WriteStringValue(val);
            }
            writer.WriteEndArray();
		}


		public static void WriteEnumerableJsonEncodedText(IEnumerable<JsonEncodedText> values, Utf8JsonWriter writer, JsonEncodedText? name)
		{
			if (name == null)
			{
				writer.WriteStartArray();
			}
			else
			{
				writer.WriteStartArray((JsonEncodedText)name);
			}
            foreach (var val in values)
            {
                writer.WriteStringValue(val);
            }
            writer.WriteEndArray();
		}
	}
}