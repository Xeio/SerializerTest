using System;
using System.Collections.Generic;
using System.Text.Json;

namespace SerializerTest
{
    public static class EnumerableWriters
    {
		public static readonly Dictionary<Type, Delegate> EnumerableDelegates = new Dictionary<Type, Delegate>()
		{
			{ typeof(IEnumerable<Decimal>), (Action<IEnumerable<Decimal>, Utf8JsonWriter, JsonEncodedText>)WriteIEnumerableDecimal},
			{ typeof(IEnumerable<Double>), (Action<IEnumerable<Double>, Utf8JsonWriter, JsonEncodedText>)WriteIEnumerableDouble},
			{ typeof(IEnumerable<Single>), (Action<IEnumerable<Single>, Utf8JsonWriter, JsonEncodedText>)WriteIEnumerableSingle},
			{ typeof(IEnumerable<Int32>), (Action<IEnumerable<Int32>, Utf8JsonWriter, JsonEncodedText>)WriteIEnumerableInt32},
			{ typeof(IEnumerable<Int64>), (Action<IEnumerable<Int64>, Utf8JsonWriter, JsonEncodedText>)WriteIEnumerableInt64},
			{ typeof(IEnumerable<UInt32>), (Action<IEnumerable<UInt32>, Utf8JsonWriter, JsonEncodedText>)WriteIEnumerableUInt32},
			{ typeof(IEnumerable<UInt64>), (Action<IEnumerable<UInt64>, Utf8JsonWriter, JsonEncodedText>)WriteIEnumerableUInt64},
			{ typeof(IEnumerable<Int16>), (Action<IEnumerable<Int16>, Utf8JsonWriter, JsonEncodedText>)WriteIEnumerableInt16},
			{ typeof(IEnumerable<UInt16>), (Action<IEnumerable<UInt16>, Utf8JsonWriter, JsonEncodedText>)WriteIEnumerableUInt16},
			{ typeof(IEnumerable<SByte>), (Action<IEnumerable<SByte>, Utf8JsonWriter, JsonEncodedText>)WriteIEnumerableSByte},
			{ typeof(IEnumerable<String>), (Action<IEnumerable<String>, Utf8JsonWriter, JsonEncodedText>)WriteIEnumerableString},
			{ typeof(IEnumerable<DateTime>), (Action<IEnumerable<DateTime>, Utf8JsonWriter, JsonEncodedText>)WriteIEnumerableDateTime},
			{ typeof(IEnumerable<Guid>), (Action<IEnumerable<Guid>, Utf8JsonWriter, JsonEncodedText>)WriteIEnumerableGuid},
			{ typeof(IEnumerable<DateTimeOffset>), (Action<IEnumerable<DateTimeOffset>, Utf8JsonWriter, JsonEncodedText>)WriteIEnumerableDateTimeOffset},
			{ typeof(IEnumerable<Boolean>), (Action<IEnumerable<Boolean>, Utf8JsonWriter, JsonEncodedText>)WriteIEnumerableBoolean},
			{ typeof(IList<Decimal>), (Action<IList<Decimal>, Utf8JsonWriter, JsonEncodedText>)WriteIListDecimal},
			{ typeof(IList<Double>), (Action<IList<Double>, Utf8JsonWriter, JsonEncodedText>)WriteIListDouble},
			{ typeof(IList<Single>), (Action<IList<Single>, Utf8JsonWriter, JsonEncodedText>)WriteIListSingle},
			{ typeof(IList<Int32>), (Action<IList<Int32>, Utf8JsonWriter, JsonEncodedText>)WriteIListInt32},
			{ typeof(IList<Int64>), (Action<IList<Int64>, Utf8JsonWriter, JsonEncodedText>)WriteIListInt64},
			{ typeof(IList<UInt32>), (Action<IList<UInt32>, Utf8JsonWriter, JsonEncodedText>)WriteIListUInt32},
			{ typeof(IList<UInt64>), (Action<IList<UInt64>, Utf8JsonWriter, JsonEncodedText>)WriteIListUInt64},
			{ typeof(IList<Int16>), (Action<IList<Int16>, Utf8JsonWriter, JsonEncodedText>)WriteIListInt16},
			{ typeof(IList<UInt16>), (Action<IList<UInt16>, Utf8JsonWriter, JsonEncodedText>)WriteIListUInt16},
			{ typeof(IList<SByte>), (Action<IList<SByte>, Utf8JsonWriter, JsonEncodedText>)WriteIListSByte},
			{ typeof(IList<String>), (Action<IList<String>, Utf8JsonWriter, JsonEncodedText>)WriteIListString},
			{ typeof(IList<DateTime>), (Action<IList<DateTime>, Utf8JsonWriter, JsonEncodedText>)WriteIListDateTime},
			{ typeof(IList<Guid>), (Action<IList<Guid>, Utf8JsonWriter, JsonEncodedText>)WriteIListGuid},
			{ typeof(IList<DateTimeOffset>), (Action<IList<DateTimeOffset>, Utf8JsonWriter, JsonEncodedText>)WriteIListDateTimeOffset},
			{ typeof(IList<Boolean>), (Action<IList<Boolean>, Utf8JsonWriter, JsonEncodedText>)WriteIListBoolean},
		};

		public static readonly Dictionary<Type, Delegate> EnumerableDelegatesNoName = new Dictionary<Type, Delegate>()
		{
			{ typeof(IEnumerable<Decimal>), (Action<IEnumerable<Decimal>, Utf8JsonWriter>)WriteNoNameIEnumerableDecimal},
			{ typeof(IEnumerable<Double>), (Action<IEnumerable<Double>, Utf8JsonWriter>)WriteNoNameIEnumerableDouble},
			{ typeof(IEnumerable<Single>), (Action<IEnumerable<Single>, Utf8JsonWriter>)WriteNoNameIEnumerableSingle},
			{ typeof(IEnumerable<Int32>), (Action<IEnumerable<Int32>, Utf8JsonWriter>)WriteNoNameIEnumerableInt32},
			{ typeof(IEnumerable<Int64>), (Action<IEnumerable<Int64>, Utf8JsonWriter>)WriteNoNameIEnumerableInt64},
			{ typeof(IEnumerable<UInt32>), (Action<IEnumerable<UInt32>, Utf8JsonWriter>)WriteNoNameIEnumerableUInt32},
			{ typeof(IEnumerable<UInt64>), (Action<IEnumerable<UInt64>, Utf8JsonWriter>)WriteNoNameIEnumerableUInt64},
			{ typeof(IEnumerable<Int16>), (Action<IEnumerable<Int16>, Utf8JsonWriter>)WriteNoNameIEnumerableInt16},
			{ typeof(IEnumerable<UInt16>), (Action<IEnumerable<UInt16>, Utf8JsonWriter>)WriteNoNameIEnumerableUInt16},
			{ typeof(IEnumerable<SByte>), (Action<IEnumerable<SByte>, Utf8JsonWriter>)WriteNoNameIEnumerableSByte},
			{ typeof(IEnumerable<String>), (Action<IEnumerable<String>, Utf8JsonWriter>)WriteNoNameIEnumerableString},
			{ typeof(IEnumerable<DateTime>), (Action<IEnumerable<DateTime>, Utf8JsonWriter>)WriteNoNameIEnumerableDateTime},
			{ typeof(IEnumerable<Guid>), (Action<IEnumerable<Guid>, Utf8JsonWriter>)WriteNoNameIEnumerableGuid},
			{ typeof(IEnumerable<DateTimeOffset>), (Action<IEnumerable<DateTimeOffset>, Utf8JsonWriter>)WriteNoNameIEnumerableDateTimeOffset},
			{ typeof(IEnumerable<Boolean>), (Action<IEnumerable<Boolean>, Utf8JsonWriter>)WriteNoNameIEnumerableBoolean},
			{ typeof(IList<Decimal>), (Action<IList<Decimal>, Utf8JsonWriter>)WriteNoNameIListDecimal},
			{ typeof(IList<Double>), (Action<IList<Double>, Utf8JsonWriter>)WriteNoNameIListDouble},
			{ typeof(IList<Single>), (Action<IList<Single>, Utf8JsonWriter>)WriteNoNameIListSingle},
			{ typeof(IList<Int32>), (Action<IList<Int32>, Utf8JsonWriter>)WriteNoNameIListInt32},
			{ typeof(IList<Int64>), (Action<IList<Int64>, Utf8JsonWriter>)WriteNoNameIListInt64},
			{ typeof(IList<UInt32>), (Action<IList<UInt32>, Utf8JsonWriter>)WriteNoNameIListUInt32},
			{ typeof(IList<UInt64>), (Action<IList<UInt64>, Utf8JsonWriter>)WriteNoNameIListUInt64},
			{ typeof(IList<Int16>), (Action<IList<Int16>, Utf8JsonWriter>)WriteNoNameIListInt16},
			{ typeof(IList<UInt16>), (Action<IList<UInt16>, Utf8JsonWriter>)WriteNoNameIListUInt16},
			{ typeof(IList<SByte>), (Action<IList<SByte>, Utf8JsonWriter>)WriteNoNameIListSByte},
			{ typeof(IList<String>), (Action<IList<String>, Utf8JsonWriter>)WriteNoNameIListString},
			{ typeof(IList<DateTime>), (Action<IList<DateTime>, Utf8JsonWriter>)WriteNoNameIListDateTime},
			{ typeof(IList<Guid>), (Action<IList<Guid>, Utf8JsonWriter>)WriteNoNameIListGuid},
			{ typeof(IList<DateTimeOffset>), (Action<IList<DateTimeOffset>, Utf8JsonWriter>)WriteNoNameIListDateTimeOffset},
			{ typeof(IList<Boolean>), (Action<IList<Boolean>, Utf8JsonWriter>)WriteNoNameIListBoolean},
		};
		public static void WriteIEnumerableDecimal(IEnumerable<Decimal> values, Utf8JsonWriter writer, JsonEncodedText name)
		{
			writer.WriteStartArray(name);
            foreach (var val in values)
            {
                writer.WriteNumberValue(val);
            }
            writer.WriteEndArray();
		}

		public static void WriteNoNameIEnumerableDecimal(IEnumerable<Decimal> values, Utf8JsonWriter writer)
		{
			writer.WriteStartArray();
            foreach (var val in values)
            {
                writer.WriteNumberValue(val);
            }
            writer.WriteEndArray();
		}

		public static void WriteIEnumerableDouble(IEnumerable<Double> values, Utf8JsonWriter writer, JsonEncodedText name)
		{
			writer.WriteStartArray(name);
            foreach (var val in values)
            {
                writer.WriteNumberValue(val);
            }
            writer.WriteEndArray();
		}

		public static void WriteNoNameIEnumerableDouble(IEnumerable<Double> values, Utf8JsonWriter writer)
		{
			writer.WriteStartArray();
            foreach (var val in values)
            {
                writer.WriteNumberValue(val);
            }
            writer.WriteEndArray();
		}

		public static void WriteIEnumerableSingle(IEnumerable<Single> values, Utf8JsonWriter writer, JsonEncodedText name)
		{
			writer.WriteStartArray(name);
            foreach (var val in values)
            {
                writer.WriteNumberValue(val);
            }
            writer.WriteEndArray();
		}

		public static void WriteNoNameIEnumerableSingle(IEnumerable<Single> values, Utf8JsonWriter writer)
		{
			writer.WriteStartArray();
            foreach (var val in values)
            {
                writer.WriteNumberValue(val);
            }
            writer.WriteEndArray();
		}

		public static void WriteIEnumerableInt32(IEnumerable<Int32> values, Utf8JsonWriter writer, JsonEncodedText name)
		{
			writer.WriteStartArray(name);
            foreach (var val in values)
            {
                writer.WriteNumberValue(val);
            }
            writer.WriteEndArray();
		}

		public static void WriteNoNameIEnumerableInt32(IEnumerable<Int32> values, Utf8JsonWriter writer)
		{
			writer.WriteStartArray();
            foreach (var val in values)
            {
                writer.WriteNumberValue(val);
            }
            writer.WriteEndArray();
		}

		public static void WriteIEnumerableInt64(IEnumerable<Int64> values, Utf8JsonWriter writer, JsonEncodedText name)
		{
			writer.WriteStartArray(name);
            foreach (var val in values)
            {
                writer.WriteNumberValue(val);
            }
            writer.WriteEndArray();
		}

		public static void WriteNoNameIEnumerableInt64(IEnumerable<Int64> values, Utf8JsonWriter writer)
		{
			writer.WriteStartArray();
            foreach (var val in values)
            {
                writer.WriteNumberValue(val);
            }
            writer.WriteEndArray();
		}

		public static void WriteIEnumerableUInt32(IEnumerable<UInt32> values, Utf8JsonWriter writer, JsonEncodedText name)
		{
			writer.WriteStartArray(name);
            foreach (var val in values)
            {
                writer.WriteNumberValue(val);
            }
            writer.WriteEndArray();
		}

		public static void WriteNoNameIEnumerableUInt32(IEnumerable<UInt32> values, Utf8JsonWriter writer)
		{
			writer.WriteStartArray();
            foreach (var val in values)
            {
                writer.WriteNumberValue(val);
            }
            writer.WriteEndArray();
		}

		public static void WriteIEnumerableUInt64(IEnumerable<UInt64> values, Utf8JsonWriter writer, JsonEncodedText name)
		{
			writer.WriteStartArray(name);
            foreach (var val in values)
            {
                writer.WriteNumberValue(val);
            }
            writer.WriteEndArray();
		}

		public static void WriteNoNameIEnumerableUInt64(IEnumerable<UInt64> values, Utf8JsonWriter writer)
		{
			writer.WriteStartArray();
            foreach (var val in values)
            {
                writer.WriteNumberValue(val);
            }
            writer.WriteEndArray();
		}

		public static void WriteIEnumerableInt16(IEnumerable<Int16> values, Utf8JsonWriter writer, JsonEncodedText name)
		{
			writer.WriteStartArray(name);
            foreach (var val in values)
            {
                writer.WriteNumberValue(val);
            }
            writer.WriteEndArray();
		}

		public static void WriteNoNameIEnumerableInt16(IEnumerable<Int16> values, Utf8JsonWriter writer)
		{
			writer.WriteStartArray();
            foreach (var val in values)
            {
                writer.WriteNumberValue(val);
            }
            writer.WriteEndArray();
		}

		public static void WriteIEnumerableUInt16(IEnumerable<UInt16> values, Utf8JsonWriter writer, JsonEncodedText name)
		{
			writer.WriteStartArray(name);
            foreach (var val in values)
            {
                writer.WriteNumberValue(val);
            }
            writer.WriteEndArray();
		}

		public static void WriteNoNameIEnumerableUInt16(IEnumerable<UInt16> values, Utf8JsonWriter writer)
		{
			writer.WriteStartArray();
            foreach (var val in values)
            {
                writer.WriteNumberValue(val);
            }
            writer.WriteEndArray();
		}

		public static void WriteIEnumerableSByte(IEnumerable<SByte> values, Utf8JsonWriter writer, JsonEncodedText name)
		{
			writer.WriteStartArray(name);
            foreach (var val in values)
            {
                writer.WriteNumberValue(val);
            }
            writer.WriteEndArray();
		}

		public static void WriteNoNameIEnumerableSByte(IEnumerable<SByte> values, Utf8JsonWriter writer)
		{
			writer.WriteStartArray();
            foreach (var val in values)
            {
                writer.WriteNumberValue(val);
            }
            writer.WriteEndArray();
		}

		public static void WriteIEnumerableString(IEnumerable<String> values, Utf8JsonWriter writer, JsonEncodedText name)
		{
			writer.WriteStartArray(name);
            foreach (var val in values)
            {
                writer.WriteStringValue(val);
            }
            writer.WriteEndArray();
		}

		public static void WriteNoNameIEnumerableString(IEnumerable<String> values, Utf8JsonWriter writer)
		{
			writer.WriteStartArray();
            foreach (var val in values)
            {
                writer.WriteStringValue(val);
            }
            writer.WriteEndArray();
		}

		public static void WriteIEnumerableDateTime(IEnumerable<DateTime> values, Utf8JsonWriter writer, JsonEncodedText name)
		{
			writer.WriteStartArray(name);
            foreach (var val in values)
            {
                writer.WriteStringValue(val);
            }
            writer.WriteEndArray();
		}

		public static void WriteNoNameIEnumerableDateTime(IEnumerable<DateTime> values, Utf8JsonWriter writer)
		{
			writer.WriteStartArray();
            foreach (var val in values)
            {
                writer.WriteStringValue(val);
            }
            writer.WriteEndArray();
		}

		public static void WriteIEnumerableGuid(IEnumerable<Guid> values, Utf8JsonWriter writer, JsonEncodedText name)
		{
			writer.WriteStartArray(name);
            foreach (var val in values)
            {
                writer.WriteStringValue(val);
            }
            writer.WriteEndArray();
		}

		public static void WriteNoNameIEnumerableGuid(IEnumerable<Guid> values, Utf8JsonWriter writer)
		{
			writer.WriteStartArray();
            foreach (var val in values)
            {
                writer.WriteStringValue(val);
            }
            writer.WriteEndArray();
		}

		public static void WriteIEnumerableDateTimeOffset(IEnumerable<DateTimeOffset> values, Utf8JsonWriter writer, JsonEncodedText name)
		{
			writer.WriteStartArray(name);
            foreach (var val in values)
            {
                writer.WriteStringValue(val);
            }
            writer.WriteEndArray();
		}

		public static void WriteNoNameIEnumerableDateTimeOffset(IEnumerable<DateTimeOffset> values, Utf8JsonWriter writer)
		{
			writer.WriteStartArray();
            foreach (var val in values)
            {
                writer.WriteStringValue(val);
            }
            writer.WriteEndArray();
		}

		public static void WriteIEnumerableBoolean(IEnumerable<Boolean> values, Utf8JsonWriter writer, JsonEncodedText name)
		{
			writer.WriteStartArray(name);
            foreach (var val in values)
            {
                writer.WriteBooleanValue(val);
            }
            writer.WriteEndArray();
		}

		public static void WriteNoNameIEnumerableBoolean(IEnumerable<Boolean> values, Utf8JsonWriter writer)
		{
			writer.WriteStartArray();
            foreach (var val in values)
            {
                writer.WriteBooleanValue(val);
            }
            writer.WriteEndArray();
		}

		public static void WriteIListDecimal(IList<Decimal> values, Utf8JsonWriter writer, JsonEncodedText name)
		{
			writer.WriteStartArray(name);
            foreach (var val in values)
            {
                writer.WriteNumberValue(val);
            }
            writer.WriteEndArray();
		}

		public static void WriteNoNameIListDecimal(IList<Decimal> values, Utf8JsonWriter writer)
		{
			writer.WriteStartArray();
            foreach (var val in values)
            {
                writer.WriteNumberValue(val);
            }
            writer.WriteEndArray();
		}

		public static void WriteIListDouble(IList<Double> values, Utf8JsonWriter writer, JsonEncodedText name)
		{
			writer.WriteStartArray(name);
            foreach (var val in values)
            {
                writer.WriteNumberValue(val);
            }
            writer.WriteEndArray();
		}

		public static void WriteNoNameIListDouble(IList<Double> values, Utf8JsonWriter writer)
		{
			writer.WriteStartArray();
            foreach (var val in values)
            {
                writer.WriteNumberValue(val);
            }
            writer.WriteEndArray();
		}

		public static void WriteIListSingle(IList<Single> values, Utf8JsonWriter writer, JsonEncodedText name)
		{
			writer.WriteStartArray(name);
            foreach (var val in values)
            {
                writer.WriteNumberValue(val);
            }
            writer.WriteEndArray();
		}

		public static void WriteNoNameIListSingle(IList<Single> values, Utf8JsonWriter writer)
		{
			writer.WriteStartArray();
            foreach (var val in values)
            {
                writer.WriteNumberValue(val);
            }
            writer.WriteEndArray();
		}

		public static void WriteIListInt32(IList<Int32> values, Utf8JsonWriter writer, JsonEncodedText name)
		{
			writer.WriteStartArray(name);
            foreach (var val in values)
            {
                writer.WriteNumberValue(val);
            }
            writer.WriteEndArray();
		}

		public static void WriteNoNameIListInt32(IList<Int32> values, Utf8JsonWriter writer)
		{
			writer.WriteStartArray();
            foreach (var val in values)
            {
                writer.WriteNumberValue(val);
            }
            writer.WriteEndArray();
		}

		public static void WriteIListInt64(IList<Int64> values, Utf8JsonWriter writer, JsonEncodedText name)
		{
			writer.WriteStartArray(name);
            foreach (var val in values)
            {
                writer.WriteNumberValue(val);
            }
            writer.WriteEndArray();
		}

		public static void WriteNoNameIListInt64(IList<Int64> values, Utf8JsonWriter writer)
		{
			writer.WriteStartArray();
            foreach (var val in values)
            {
                writer.WriteNumberValue(val);
            }
            writer.WriteEndArray();
		}

		public static void WriteIListUInt32(IList<UInt32> values, Utf8JsonWriter writer, JsonEncodedText name)
		{
			writer.WriteStartArray(name);
            foreach (var val in values)
            {
                writer.WriteNumberValue(val);
            }
            writer.WriteEndArray();
		}

		public static void WriteNoNameIListUInt32(IList<UInt32> values, Utf8JsonWriter writer)
		{
			writer.WriteStartArray();
            foreach (var val in values)
            {
                writer.WriteNumberValue(val);
            }
            writer.WriteEndArray();
		}

		public static void WriteIListUInt64(IList<UInt64> values, Utf8JsonWriter writer, JsonEncodedText name)
		{
			writer.WriteStartArray(name);
            foreach (var val in values)
            {
                writer.WriteNumberValue(val);
            }
            writer.WriteEndArray();
		}

		public static void WriteNoNameIListUInt64(IList<UInt64> values, Utf8JsonWriter writer)
		{
			writer.WriteStartArray();
            foreach (var val in values)
            {
                writer.WriteNumberValue(val);
            }
            writer.WriteEndArray();
		}

		public static void WriteIListInt16(IList<Int16> values, Utf8JsonWriter writer, JsonEncodedText name)
		{
			writer.WriteStartArray(name);
            foreach (var val in values)
            {
                writer.WriteNumberValue(val);
            }
            writer.WriteEndArray();
		}

		public static void WriteNoNameIListInt16(IList<Int16> values, Utf8JsonWriter writer)
		{
			writer.WriteStartArray();
            foreach (var val in values)
            {
                writer.WriteNumberValue(val);
            }
            writer.WriteEndArray();
		}

		public static void WriteIListUInt16(IList<UInt16> values, Utf8JsonWriter writer, JsonEncodedText name)
		{
			writer.WriteStartArray(name);
            foreach (var val in values)
            {
                writer.WriteNumberValue(val);
            }
            writer.WriteEndArray();
		}

		public static void WriteNoNameIListUInt16(IList<UInt16> values, Utf8JsonWriter writer)
		{
			writer.WriteStartArray();
            foreach (var val in values)
            {
                writer.WriteNumberValue(val);
            }
            writer.WriteEndArray();
		}

		public static void WriteIListSByte(IList<SByte> values, Utf8JsonWriter writer, JsonEncodedText name)
		{
			writer.WriteStartArray(name);
            foreach (var val in values)
            {
                writer.WriteNumberValue(val);
            }
            writer.WriteEndArray();
		}

		public static void WriteNoNameIListSByte(IList<SByte> values, Utf8JsonWriter writer)
		{
			writer.WriteStartArray();
            foreach (var val in values)
            {
                writer.WriteNumberValue(val);
            }
            writer.WriteEndArray();
		}

		public static void WriteIListString(IList<String> values, Utf8JsonWriter writer, JsonEncodedText name)
		{
			writer.WriteStartArray(name);
            foreach (var val in values)
            {
                writer.WriteStringValue(val);
            }
            writer.WriteEndArray();
		}

		public static void WriteNoNameIListString(IList<String> values, Utf8JsonWriter writer)
		{
			writer.WriteStartArray();
            foreach (var val in values)
            {
                writer.WriteStringValue(val);
            }
            writer.WriteEndArray();
		}

		public static void WriteIListDateTime(IList<DateTime> values, Utf8JsonWriter writer, JsonEncodedText name)
		{
			writer.WriteStartArray(name);
            foreach (var val in values)
            {
                writer.WriteStringValue(val);
            }
            writer.WriteEndArray();
		}

		public static void WriteNoNameIListDateTime(IList<DateTime> values, Utf8JsonWriter writer)
		{
			writer.WriteStartArray();
            foreach (var val in values)
            {
                writer.WriteStringValue(val);
            }
            writer.WriteEndArray();
		}

		public static void WriteIListGuid(IList<Guid> values, Utf8JsonWriter writer, JsonEncodedText name)
		{
			writer.WriteStartArray(name);
            foreach (var val in values)
            {
                writer.WriteStringValue(val);
            }
            writer.WriteEndArray();
		}

		public static void WriteNoNameIListGuid(IList<Guid> values, Utf8JsonWriter writer)
		{
			writer.WriteStartArray();
            foreach (var val in values)
            {
                writer.WriteStringValue(val);
            }
            writer.WriteEndArray();
		}

		public static void WriteIListDateTimeOffset(IList<DateTimeOffset> values, Utf8JsonWriter writer, JsonEncodedText name)
		{
			writer.WriteStartArray(name);
            foreach (var val in values)
            {
                writer.WriteStringValue(val);
            }
            writer.WriteEndArray();
		}

		public static void WriteNoNameIListDateTimeOffset(IList<DateTimeOffset> values, Utf8JsonWriter writer)
		{
			writer.WriteStartArray();
            foreach (var val in values)
            {
                writer.WriteStringValue(val);
            }
            writer.WriteEndArray();
		}

		public static void WriteIListBoolean(IList<Boolean> values, Utf8JsonWriter writer, JsonEncodedText name)
		{
			writer.WriteStartArray(name);
            foreach (var val in values)
            {
                writer.WriteBooleanValue(val);
            }
            writer.WriteEndArray();
		}

		public static void WriteNoNameIListBoolean(IList<Boolean> values, Utf8JsonWriter writer)
		{
			writer.WriteStartArray();
            foreach (var val in values)
            {
                writer.WriteBooleanValue(val);
            }
            writer.WriteEndArray();
		}

	}
}