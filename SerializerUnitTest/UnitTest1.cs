using NUnit.Framework;
using SerializerTest;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using TestObjects;

namespace SerializerUnitTest
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestManualSerializer()
        {
            var testList = new List<TestObj>()
            {
                new TestObj(){ FooString = "Test\"St\\ring", BarDecimal = 9.23m, BazInt = 77 },
                new TestObj(){ FooString = "OtherTestString", BarDecimal = 113m, BazInt = -44 },
            };

            var knownGood = JsonSerializer.Serialize(testList);

            var memoryStream = new MemoryStream();
            ManualSerializer.Serialize(testList, new Utf8JsonWriter(memoryStream));
            var serializedOutput = Encoding.UTF8.GetString(memoryStream.ToArray());

            Assert.AreEqual(knownGood, serializedOutput);
        }

        [Test]
        public void TestReflectionSerializer()
        {
            var testList = new List<TestObj>()
            {
                new TestObj(){ FooString = "TestString", BarDecimal = 9.23m, BazInt = 77 },
                new TestObj(){ FooString = "OtherTestString", BarDecimal = 113m, BazInt = -44 },
            };

            var knownGood = JsonSerializer.Serialize(testList);

            var memoryStream = new MemoryStream();
            ReflectionSerializer.Serialize(testList, new Utf8JsonWriter(memoryStream));
            var serializedOutput = Encoding.UTF8.GetString(memoryStream.ToArray());

            Assert.AreEqual(knownGood, serializedOutput);
        }

        [Test]
        public void TestCodegenSerializer()
        {
            var testList = new List<TestObj>()
            {
                new TestObj(){ FooString = "TestString", BarDecimal = 9.23m, BazInt = 77 },
                new TestObj(){ FooString = "OtherTestString", BarDecimal = 113m, BazInt = -44 },
            };

            var knownGood = JsonSerializer.Serialize(testList);

            var memoryStream = new MemoryStream();
            CodegenSerializer.Serialize(testList, new Utf8JsonWriter(memoryStream));
            var serializedOutput = Encoding.UTF8.GetString(memoryStream.ToArray());

            Assert.AreEqual(knownGood, serializedOutput);

            var memoryStream2 = new MemoryStream();
            CodegenSerializer.Serialize(testList, new Utf8JsonWriter(memoryStream2));
            var serializedOutput2 = Encoding.UTF8.GetString(memoryStream2.ToArray());

            Assert.AreEqual(serializedOutput, serializedOutput2, "Cached delegates returned a different value.");
        }

        [Test]
        public void TestListOfPrimitives()
        {
            var testList = new List<int>()
            {
                1, 2, 5, 0, 99, -153
            };

            var knownGood = JsonSerializer.Serialize(testList);

            var memoryStream = new MemoryStream();
            CodegenSerializer.Serialize(testList, new Utf8JsonWriter(memoryStream));
            var serializedOutput = Encoding.UTF8.GetString(memoryStream.ToArray());

            Assert.AreEqual(knownGood, serializedOutput, "Enumerable of ints");

            var testList2 = new List<string>()
            {
                "foo", "bar", "\"baz'''"
            };

            knownGood = JsonSerializer.Serialize(testList);

            memoryStream = new MemoryStream();
            CodegenSerializer.Serialize(testList, new Utf8JsonWriter(memoryStream));
            serializedOutput = Encoding.UTF8.GetString(memoryStream.ToArray());

            Assert.AreEqual(knownGood, serializedOutput, "Enumerable of strings");
        }

        [Test]
        public void TestTreeStructure()
        {
            var testList = new List<TestTree>()
            {
                new TestTree(){ Value = "Top A",
                    Left = new TestTree(){ Value = "A Level 2 (L)" },
                    Right = new TestTree() { Value = "A Level 2 (R)"} 
                },
                new TestTree(){ Value = "Top B",
                    Left = new TestTree() { Value = "B Level 2 (L)" } 
                }
            };

            var knownGood = JsonSerializer.Serialize(testList);

            var memoryStream = new MemoryStream();
            CodegenSerializer.Serialize(testList, new Utf8JsonWriter(memoryStream));
            var serializedOutput = Encoding.UTF8.GetString(memoryStream.ToArray());

            Assert.AreEqual(knownGood, serializedOutput);
        }
    }
}