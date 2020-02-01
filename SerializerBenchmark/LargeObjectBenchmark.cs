using BenchmarkDotNet.Attributes;
using SerializerTest;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using TestObjects;

namespace SerializerBenchmark
{
    public class LargeObjectBenchmark
    {
        private EverythingObj TestObjects;
        private JsonWriterOptions Options;

        [GlobalSetup]
        public void Setup()
        {
            TestObjects = new EverythingObj()
            {
                Byte = 0xFE,
                ByteN = 0x03,
                DateTime = new DateTime(2020, 1, 24, 14, 17, 12),
                DateTimeN = new DateTime(2020, 1, 24, 1, 0, 0, DateTimeKind.Utc),
                DateTimeOffset = new DateTimeOffset(new DateTime(2020, 1, 24), TimeSpan.FromHours(7)),
                DateTimeOffsetN = new DateTimeOffset(new DateTime(2020, 1, 24), TimeSpan.FromHours(7)),
                Decimal = 999.773m,
                DecimalN = 33.333333m,
                Double = 4237.9993d,
                DoubleN = -3234.392d,
                Float = 9939243f,
                FloatN = 222.77f,
                Guid = new Guid("1ba0c09f-b4a9-4abf-8eb9-934cccc4b611"),
                GuidN = new Guid("e5464ff7-335d-45ca-8b58-900c8e697f3b"),
                Int = 7,
                IntN = -123123,
                Long = 8888888,
                LongN = -44444444444,
                Short = 21,
                ShortN = -111,
                UInt = 73823434,
                UIntN = 12312,
                ULong = 123123123123,
                ULongN = 38412398412934,
                String = "TEST",
                IntA = new[] { 5, 8, 99, -11, 0 },
                StringA = new[] { "foo", "Bar", "Ba\"''z" },
                StringL = new List<string>() { "foo1", "Bar1", "Ba\"''z1" },
                StringE = new[] { "foo2", "Bar2", "Ba\"''z2" },
                Bool = true,
                BoolN = false
            };
            Options = new JsonWriterOptions() { SkipValidation = true };
        }

        [Benchmark]
        public void SystemTextJsonBench()
        {
            using var writer = new Utf8JsonWriter(Stream.Null, Options);
            JsonSerializer.Serialize(writer, TestObjects);
        }

        [Benchmark]
        public void CodegenSerializerBench()
        {
            using var writer = new Utf8JsonWriter(Stream.Null, Options);
            CodegenSerializer.Serialize(TestObjects, writer);
        }

        [Benchmark]
        public void Utf8JsonBench()
        {
            Utf8Json.JsonSerializer.Serialize(Stream.Null, TestObjects);
        }
    }
}
