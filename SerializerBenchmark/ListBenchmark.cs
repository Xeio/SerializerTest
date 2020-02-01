using BenchmarkDotNet.Attributes;
using SerializerTest;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using TestObjects;

namespace SerializerBenchmark
{
    public class ListBenchmark
    {
        private List<TestObj> SmallList;
        private IList<TestObj> LargeList;
        private List<string> StringList;
        private JsonWriterOptions Options;

        [GlobalSetup]
        public void Setup()
        {
            SmallList = new List<TestObj>() {
                new TestObj(){ FooString = "TestString", BarDecimal = 9.23m, BazInt = 77 },
                new TestObj(){ FooString = "OtherTestString", BarDecimal = 113m, BazInt = -44 },
                new TestObj(){ FooString = "Bob", BarDecimal = -5.55m, BazInt = 0 },
                new TestObj(){ FooString = "Sally", BarDecimal = 0m, BazInt = -77777 },
            };

            LargeList = new List<TestObj>();
            StringList = new List<string>();
            foreach(var i in Enumerable.Range(0, 100))
            {
                LargeList.Add(new TestObj() { FooString = "Testing " + i, BazInt = i, BarDecimal = i + 7.7m });
                StringList.Add("Testing " + i);
            }

            Options = new JsonWriterOptions() { SkipValidation = true };
        }

        [Benchmark]
        public void SystemTextJsonBenchSmallList()
        {
            using var writer = new Utf8JsonWriter(Stream.Null, Options);
            JsonSerializer.Serialize(writer, SmallList);
        }

        [Benchmark]
        public void CodegenSerializerBenchSmallList()
        {
            using var writer = new Utf8JsonWriter(Stream.Null, Options);
            CodegenSerializer.Serialize(SmallList, writer);
        }

        [Benchmark]
        public void Utf8JsonBenchSmallList()
        {
            Utf8Json.JsonSerializer.Serialize(Stream.Null, SmallList);
        }

        [Benchmark]
        public void SystemTextJsonBenchLargeList()
        {
            using var writer = new Utf8JsonWriter(Stream.Null, Options);
            JsonSerializer.Serialize(writer, LargeList);
        }

        [Benchmark]
        public void CodegenSerializerBenchLargeList()
        {
            using var writer = new Utf8JsonWriter(Stream.Null, Options);
            CodegenSerializer.Serialize(LargeList, writer);
        }

        [Benchmark]
        public void Utf8JsonBenchLargeList()
        {
            Utf8Json.JsonSerializer.Serialize(Stream.Null, LargeList);
        }

        [Benchmark]
        public void SystemTextJsonBenchStringList()
        {
            using var writer = new Utf8JsonWriter(Stream.Null, Options);
            JsonSerializer.Serialize(writer, StringList);
        }

        [Benchmark]
        public void CodegenSerializerBenchStringList()
        {
            using var writer = new Utf8JsonWriter(Stream.Null, Options);
            CodegenSerializer.Serialize(StringList, writer);
        }

        [Benchmark]
        public void Utf8JsonBenchStringList()
        {
            Utf8Json.JsonSerializer.Serialize(Stream.Null, StringList);
        }
    }
}
