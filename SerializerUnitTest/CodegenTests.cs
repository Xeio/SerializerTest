using NUnit.Framework;
using SerializerTest;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using TestObjects;

namespace SerializerUnitTest
{
    public class CodegenTests
    {
        [Test]
        public void TestCodegenSerializer()
        {
            var testList = new List<TestObj>()
            {
                new TestObj(){ FooString = "TestString", BarDecimal = 9.23m, BazInt = 77 },
                new TestObj(){ FooString = "OtherTestString", BarDecimal = 113m, BazInt = -44 },
            };

            CodegenEqualHelper(testList);
            CodegenEqualHelper(testList, "Cached delegates returned a different value.");
        }

        [Test]
        public void TestEnumerablePrimitives()
        {
            CodegenEqualHelper(Enumerable.Range(5, 17), "Enumerable range ints");

            CodegenEqualHelper(new List<int>()
            {
                1, 2, 5, 0, 99, -153
            }.Where((i) => true), "List of ints");

            CodegenEqualHelper(new List<short>()
            {
                1, 2, 5, 0, 99, -153
            }.Where((i) => true), "List of shorts");

            CodegenEqualHelper(new List<bool>()
            {
                true, true, true, false, true, false, false
            }.Where((i) => true), "List of bools");

            CodegenEqualHelper(new List<string>()
            {
                "foo", "bar", "\"baz'''"
            }.Where((i) => true), "Enumerable of strings");


            CodegenEqualHelper(new List<Guid>()
            {
                Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()
            }.Where((i) => true), "Enumerable of Guids");


            CodegenEqualHelper(new List<DateTime>()
            {
                DateTime.Now, DateTime.UtcNow, DateTime.Now.AddDays(7)
            }.Where((i) => true), "Enumerable of DateTimes");


            CodegenEqualHelper(new List<short>()
            {
                8, 12, -117, 257
            }.Where((i) => true), "Enumerable of shorts");
        }

        [Test]
        public void TestIListPrimitives()
        {
            CodegenEqualHelper(Enumerable.Range(5, 17).ToArray(), "Array of ints");

            CodegenEqualHelper(new List<int>()
            {
                1, 2, 5, 0, 99, -153
            }, "List of ints");

            CodegenEqualHelper(new List<short>()
            {
                1, 2, 5, 0, 99, -153
            }, "List of shorts");

            CodegenEqualHelper(new List<bool>()
            {
                true, true, true, false, true, false, false
            }, "List of bools");

            CodegenEqualHelper(new List<string>()
            {
                "foo", "bar", "\"baz'''"
            }, "Enumerable of strings");


            CodegenEqualHelper(new List<Guid>()
            {
                Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()
            }, "Enumerable of Guids");


            CodegenEqualHelper(new List<DateTime>()
            {
                DateTime.Now, DateTime.UtcNow, DateTime.Now.AddDays(7)
            }, "Enumerable of DateTimes");


            CodegenEqualHelper(new List<short>()
            {
                8, 12, -117, 257
            }, "Enumerable of shorts");
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

            CodegenEqualHelper(testList);
        }

        [Test]
        public void TestEverythingObj()
        {
            var testList = new List<EverythingObj>()
            {
                new EverythingObj(),
                new EverythingObj(){
                    Byte = 0xFE, ByteN = 0x03,
                    DateTime = new DateTime(2020, 1, 24, 14, 17, 12),
                    DateTimeN = new DateTime(2020, 1, 24, 1, 0, 0, DateTimeKind.Utc),
                    DateTimeOffset = new DateTimeOffset(new DateTime(2020, 1, 24), TimeSpan.FromHours(7)),
                    DateTimeOffsetN = new DateTimeOffset(new DateTime(2020, 1, 24), TimeSpan.FromHours(7)),
                    Decimal = 999.773m, DecimalN = 33.333333m,
                    Double = 4237.9993d, DoubleN = -3234.392d,
                    Float = 9939243f, FloatN = 222.77f,
                    Guid = new Guid("1ba0c09f-b4a9-4abf-8eb9-934cccc4b611"),
                    GuidN = new Guid("e5464ff7-335d-45ca-8b58-900c8e697f3b"),
                    Int = 7, IntN = -123123,
                    Long = 8888888, LongN =-44444444444,
                    Short = 21, ShortN = -111,
                    UInt = 73823434, UIntN = 12312,
                    ULong = 123123123123, ULongN = 38412398412934,
                    String = "TEST",
                    IntA = new []{5, 8, 99, -11, 0},
                    StringA = new []{"foo", "Bar", "Ba\"''z"},
                    StringL = new List<string>(){"foo1", "Bar1", "Ba\"''z1"},
                    StringE = new []{"foo2", "Bar2", "Ba\"''z2"},
                    Bool = true,
                    BoolN = false
                }
            };

            CodegenEqualHelper(testList);
        }

        [Test]
        public void TestAnonymousType()
        {
            CodegenEqualHelper(new { A = "foo", B = 77, C = 77.223f });
        }

        [Test]
        public void TestTuple()
        {
            CodegenEqualHelper((Name:"Foo", Value:87, FloatyThing:99.3f));
        }

        [Test]
        public void TestListOfComplexObjectProperty()
        {
            var testList = new ObjectListPropertyObj()
            {
                TestObjList = new List<TestObj>()
                {
                    new TestObj(){ FooString = "TestString", BarDecimal = 9.23m, BazInt = 77 },
                    new TestObj(){ FooString = "OtherTestString", BarDecimal = 113m, BazInt = -44 }
                },
                TestObjEnumerable = new List<TestObj>()
                {
                    new TestObj(){ FooString = "TestString2", BarDecimal = 19.23m, BazInt = 177 },
                    new TestObj(){ FooString = "OtherTestString2", BarDecimal = 1113m, BazInt = -144 }
                },
            };

            CodegenEqualHelper(testList);
        }

        private void CodegenEqualHelper<T>(T obj, string message = null)
        {
            var knownGood = JsonSerializer.Serialize(obj);

            var memoryStream = new MemoryStream();
            CodegenSerializer.Serialize(obj, new Utf8JsonWriter(memoryStream));
            var serializedOutput = Encoding.UTF8.GetString(memoryStream.ToArray());

            Assert.AreEqual(knownGood, serializedOutput, message);
        }
    }
}