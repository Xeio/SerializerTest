using System;
using System.Collections.Generic;
using System.Text.Json;

namespace SerializerTest
{
    public static class EnumerableWriters
    {
		public static readonly Dictionary<Type, Delegate> EnumerableDelegates = new Dictionary<Type, Delegate>()
		{
			{ typeof(Decimal), (Action<IEnumerable<Decimal>, Utf8JsonWriter, JsonEncodedText?>)WriteEnumerableDecimal},
			{ typeof(Double), (Action<IEnumerable<Double>, Utf8JsonWriter, JsonEncodedText?>)WriteEnumerableDouble},
			{ typeof(Single), (Action<IEnumerable<Single>, Utf8JsonWriter, JsonEncodedText?>)WriteEnumerableSingle},
			{ typeof(Int32), (Action<IEnumerable<Int32>, Utf8JsonWriter, JsonEncodedText?>)WriteEnumerableInt32},
			{ typeof(Int64), (Action<IEnumerable<Int64>, Utf8JsonWriter, JsonEncodedText?>)WriteEnumerableInt64},
			{ typeof(UInt32), (Action<IEnumerable<UInt32>, Utf8JsonWriter, JsonEncodedText?>)WriteEnumerableUInt32},
			{ typeof(UInt64), (Action<IEnumerable<UInt64>, Utf8JsonWriter, JsonEncodedText?>)WriteEnumerableUInt64},
			{ typeof(Int16), (Action<IEnumerable<Int16>, Utf8JsonWriter, JsonEncodedText?>)WriteEnumerableInt16},
			{ typeof(UInt16), (Action<IEnumerable<UInt16>, Utf8JsonWriter, JsonEncodedText?>)WriteEnumerableUInt16},
			{ typeof(SByte), (Action<IEnumerable<SByte>, Utf8JsonWriter, JsonEncodedText?>)WriteEnumerableSByte},
			{ typeof(String), (Action<IEnumerable<String>, Utf8JsonWriter, JsonEncodedText?>)WriteEnumerableString},
			{ typeof(DateTime), (Action<IEnumerable<DateTime>, Utf8JsonWriter, JsonEncodedText?>)WriteEnumerableDateTime},
			{ typeof(Guid), (Action<IEnumerable<Guid>, Utf8JsonWriter, JsonEncodedText?>)WriteEnumerableGuid},
			{ typeof(DateTimeOffset), (Action<IEnumerable<DateTimeOffset>, Utf8JsonWriter, JsonEncodedText?>)WriteEnumerableDateTimeOffset},
			{ typeof(Boolean), (Action<IEnumerable<Boolean>, Utf8JsonWriter, JsonEncodedText?>)WriteEnumerableBoolean},
		};

		public static void WriteEnumerableDecimal(IEnumerable<Decimal> values, Utf8JsonWriter writer, JsonEncodedText? name)
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
                writer.WriteNumberValue(val);
            }
            writer.WriteEndArray();
		}

		public static void WriteEnumerableDouble(IEnumerable<Double> values, Utf8JsonWriter writer, JsonEncodedText? name)
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
                writer.WriteNumberValue(val);
            }
            writer.WriteEndArray();
		}

		public static void WriteEnumerableSingle(IEnumerable<Single> values, Utf8JsonWriter writer, JsonEncodedText? name)
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
                writer.WriteNumberValue(val);
            }
            writer.WriteEndArray();
		}

		public static void WriteEnumerableInt32(IEnumerable<Int32> values, Utf8JsonWriter writer, JsonEncodedText? name)
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
                writer.WriteNumberValue(val);
            }
            writer.WriteEndArray();
		}

		public static void WriteEnumerableInt64(IEnumerable<Int64> values, Utf8JsonWriter writer, JsonEncodedText? name)
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
                writer.WriteNumberValue(val);
            }
            writer.WriteEndArray();
		}

		public static void WriteEnumerableUInt32(IEnumerable<UInt32> values, Utf8JsonWriter writer, JsonEncodedText? name)
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
                writer.WriteNumberValue(val);
            }
            writer.WriteEndArray();
		}

		public static void WriteEnumerableUInt64(IEnumerable<UInt64> values, Utf8JsonWriter writer, JsonEncodedText? name)
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
                writer.WriteNumberValue(val);
            }
            writer.WriteEndArray();
		}

		public static void WriteEnumerableInt16(IEnumerable<Int16> values, Utf8JsonWriter writer, JsonEncodedText? name)
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
                writer.WriteNumberValue(val);
            }
            writer.WriteEndArray();
		}

		public static void WriteEnumerableUInt16(IEnumerable<UInt16> values, Utf8JsonWriter writer, JsonEncodedText? name)
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
                writer.WriteNumberValue(val);
            }
            writer.WriteEndArray();
		}

		public static void WriteEnumerableSByte(IEnumerable<SByte> values, Utf8JsonWriter writer, JsonEncodedText? name)
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
                writer.WriteNumberValue(val);
            }
            writer.WriteEndArray();
		}

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

		public static void WriteEnumerableBoolean(IEnumerable<Boolean> values, Utf8JsonWriter writer, JsonEncodedText? name)
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
                writer.WriteBooleanValue(val);
            }
            writer.WriteEndArray();
		}

	}
}