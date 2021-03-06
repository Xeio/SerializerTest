﻿using BenchmarkDotNet.Attributes;
using SerializerTest;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using TestObjects;

namespace SerializerBenchmark
{
    public class DefaultBenchmarks
    {
        private List<TestObj> TestObjects;
        private JsonWriterOptions Options;

        [GlobalSetup]
        public void Setup()
        {
            TestObjects = new List<TestObj>() {
                new TestObj(){ FooString = "TestString", BarDecimal = 9.23m, BazInt = 77 },
                new TestObj(){ FooString = "OtherTestString", BarDecimal = 113m, BazInt = -44 },
                new TestObj(){ FooString = "Bob", BarDecimal = -5.55m, BazInt = 0 },
                new TestObj(){ FooString = "Sally", BarDecimal = 0m, BazInt = -77777 },
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
        public void ManualSerializerBench()
        {
            using var writer = new Utf8JsonWriter(Stream.Null, Options);
            ManualSerializer.Serialize(TestObjects, writer);
        }

        [Benchmark]
        public void ReflectionSerializerBench()
        {
            using var writer = new Utf8JsonWriter(Stream.Null, Options);
            ReflectionSerializer.Serialize(TestObjects, writer);
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
