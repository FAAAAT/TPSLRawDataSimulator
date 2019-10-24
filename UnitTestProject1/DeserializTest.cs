using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;
using TPSLRawDataSimulator;
using System.IO;
using System.Linq;

namespace UnitTestProject1
{
    [TestClass]
    public class DeserializTest
    {
        [TestMethod]
        public void Test_RawBinaryFormatter_Deserialize() {
            var expected = ObjectsInitializer.InitATestPureValueObject();
            var result = ObjectsInitializer.GetATestPureValueObjectMemoryLayout();
            var actual = new RawBinaryFormatter().Deserialize(new MemoryStream(result.ToArray()), typeof(NormalValueObject));
            Assert.AreEqual(expected, actual);
        }
    }
}
