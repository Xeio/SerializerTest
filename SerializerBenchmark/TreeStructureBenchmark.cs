using BenchmarkDotNet.Attributes;
using SerializerTest;
using System;
using System.IO;
using System.Text.Json;
using TestObjects;

namespace SerializerBenchmark
{
    public class TreeStructureBenchmark
    {
        private Random _rand;
        private TestTree TestObjects;
        private JsonWriterOptions Options;

        [GlobalSetup]
        public void Setup()
        {
            _rand = new Random(5765113); //Just choosing a seed so our test is always the same

            TestObjects = GenerateTree(20);
            Options = new JsonWriterOptions() { SkipValidation = true };
        }

        private TestTree GenerateTree(int maxDepth)
        {
            var tree = new TestTree() { Value = _rand.Next().ToString() };
            if(maxDepth > 0)
            {
                maxDepth--;
                tree.Left = GenerateTree(maxDepth - 1);
                tree.Right = GenerateTree(maxDepth - 1);
            }
            return tree;
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
