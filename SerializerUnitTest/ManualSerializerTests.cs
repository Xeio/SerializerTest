using NUnit.Framework;
using SerializerTest;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using TestObjects;

namespace SerializerUnitTest
{
    public class ManualSerializerTests
    {
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
    }
}