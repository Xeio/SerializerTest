using System.Collections.Generic;

namespace TestObjects
{
    public class ObjectListPropertyObj
    {
        public List<TestObj> TestObjList { get; set; }
        public IEnumerable<TestObj> TestObjEnumerable { get; set; }
    }
}
