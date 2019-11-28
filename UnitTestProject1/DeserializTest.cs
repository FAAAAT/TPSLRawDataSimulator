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

        [TestMethod]
        public void Test() {
            var realData1 = new byte[] {
                0x74,0x70,0x73,0x6C,0x01,0x00,0x1D,0x02,0x38,0x36,0x38,0x34,0x37,0x34,0x30,0x34,0x32,0x38,0x33,0x37,0x33,0x32,0x36,0x30,0x19,0x11,0x28,0x11,0x59,0x16,0x64,0x0E,0x04,0x40,0x00,0x00,0x27,0x16,0x69,0x6F,0x74
            };

            RawBinaryFormatter formatter = new RawBinaryFormatter();
            var wrapper = (TPSLWrapper)formatter.Deserialize(new MemoryStream(realData1), typeof(TPSLWrapper));
            wrapper.FillObject(formatter);
            Assert.IsTrue(true);

        }
        [TestMethod]
        public void Test2() {
            // this is the example
            // to avoid nagle issue.
            var realData2 = new byte[] {
                0x74,0x70,0x73,0x6C,0x01,0x00,0x1D,0x02,0x38,0x36,0x38,0x34,0x37,0x34,0x30,0x34,0x32,0x38,0x33,0x37,0x33,0x32,0x36,0x30,0x19,0x11,0x28,0x11,0x59,0x16,0x64,0x0E,0x04,0x40,0x00,0x00,0x27,0x16,0x69,0x6F,0x74
                ,0x74,0x70,0x73,0x6C,0x01,0x00,0x1D,0x02,0x38
            };
            BoundAutoStreamTransfers transfers = new BoundAutoStreamTransfers();
            transfers.addBytes(realData2);
            Assert.IsTrue(transfers.TryGetAnObject(typeof(TPSLWrapper), (Encoding.ASCII.GetBytes("tpsl"), Encoding.ASCII.GetBytes("iot")), out var wrapper2));
           
        }

    }
}
