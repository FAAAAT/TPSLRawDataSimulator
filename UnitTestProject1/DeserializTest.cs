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
        public void Test_RawBinaryFormatter_Deserialize_PureValueObject() {
            var expected = ObjectsInitializer.InitATestPureValueObject();
            var result = ObjectsInitializer.GetATestPureValueObjectMemoryLayout();
            var actual = new RawBinaryFormatter().Deserialize(new MemoryStream(result.ToArray()), typeof(NormalValueObject));
            Assert.IsTrue(expected.Equals(actual));
        }

        [TestMethod]
        public void Test_RawBinaryFormatter_Deserialize_ArrayValueObject() {
            var expected = ObjectsInitializer.InitATestArrayValueObject();
            var result = ObjectsInitializer.GetATestArrayValueObjectMemeryLayout();
            var actual = new RawBinaryFormatter().Deserialize(new MemoryStream(result.ToArray()), typeof(ArrayValueObject));
            Assert.IsTrue(expected.Equals(actual));
        }

        [TestMethod]
        public void Test_RawBinaryFormatter_Deserialize_ArrayValueObjectWithBigEndian() {
            var expected = ObjectsInitializer.InitATestArraySpecifiedEndianValueObject();
            var result = ObjectsInitializer.GetATestArrayValueObjectMemeryLayoutBigEndian();
            var actual = new RawBinaryFormatter().Deserialize(new MemoryStream(result.ToArray()), typeof(ArrayValueSpecifiedEndianObject));
            Assert.IsTrue(expected.Equals(actual));
        }
    }
}
