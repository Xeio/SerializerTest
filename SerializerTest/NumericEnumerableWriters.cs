using System;
using System.Collections.Generic;
using System.Text.Json;

namespace SerializerTest
{
    public static class NumericEnumerableWriters
    {
		public static readonly Dictionary<Type, Delegate> NumericEnumerableDelegates = new Dictionary<Type, Delegate>()
		{
			{ typeof(Decimal), (Action<IEnumerable<Decimal>, Utf8JsonWriter, JsonEncodedText?>)WriteEnumerableDecimal},
			{ typeof(Double), (Action<IEnumerable<Double>, Utf8JsonWriter, JsonEncodedText?>)WriteEnumerableDouble},
			{ typeof(Single), (Action<IEnumerable<Single>, Utf8JsonWriter, JsonEncodedText?>)WriteEnumerableSingle},
			{ typeof(Int32), (Action<IEnumerable<Int32>, Utf8JsonWriter, JsonEncodedText?>)WriteEnumerableInt32},
			{ typeof(Int64), (Action<IEnumerable<Int64>, Utf8JsonWriter, JsonEncodedText?>)WriteEnumerableInt64},
			{ typeof(UInt32), (Action<IEnumerable<UInt32>, Utf8JsonWriter, JsonEncodedText?>)WriteEnumerableUInt32},
			{ typeof(UInt64), (Action<IEnumerable<UInt64>, Utf8JsonWriter, JsonEncodedText?>)WriteEnumerableUInt64},
		};

		public static void WriteEnumerableDecimal(IEnumerable<Decimal> numbers, Utf8JsonWriter writer, JsonEncodedText? name)
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
                writer.WriteNumberValue(num);
            }
            writer.WriteEndArray();
		}

		public static void WriteEnumerableDouble(IEnumerable<Double> numbers, Utf8JsonWriter writer, JsonEncodedText? name)
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
                writer.WriteNumberValue(num);
            }
            writer.WriteEndArray();
		}

		public static void WriteEnumerableSingle(IEnumerable<Single> numbers, Utf8JsonWriter writer, JsonEncodedText? name)
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
                writer.WriteNumberValue(num);
            }
            writer.WriteEndArray();
		}

		public static void WriteEnumerableInt32(IEnumerable<Int32> numbers, Utf8JsonWriter writer, JsonEncodedText? name)
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
                writer.WriteNumberValue(num);
            }
            writer.WriteEndArray();
		}

		public static void WriteEnumerableInt64(IEnumerable<Int64> numbers, Utf8JsonWriter writer, JsonEncodedText? name)
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
                writer.WriteNumberValue(num);
            }
            writer.WriteEndArray();
		}

		public static void WriteEnumerableUInt32(IEnumerable<UInt32> numbers, Utf8JsonWriter writer, JsonEncodedText? name)
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
                writer.WriteNumberValue(num);
            }
            writer.WriteEndArray();
		}

		public static void WriteEnumerableUInt64(IEnumerable<UInt64> numbers, Utf8JsonWriter writer, JsonEncodedText? name)
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
                writer.WriteNumberValue(num);
            }
            writer.WriteEndArray();
		}

	}
}