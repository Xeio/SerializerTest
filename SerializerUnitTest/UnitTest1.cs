using NUnit.Framework;
using SerializerTest;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

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
        }
    }
}