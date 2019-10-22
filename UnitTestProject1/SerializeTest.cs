using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.Reflection;
using TPSLRawDataSimulator;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace UnitTestProject1
{
    [TestClass]
    public class SerializeTest
    {


        [TestMethod]
        public void TestMethod1()
        {
            var memberInfos = typeof(TestArray).GetMembers(BindingFlags.Instance | BindingFlags.Public);
            foreach (MemberInfo mInfo in memberInfos) {
                Trace.WriteLine(mInfo.Name);
            }
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void TestMethod2() {
            var bytes = new byte[] { 0xFF, 0x00, 0x00, 0xF0 };
            var result = BitConverter.ToInt16(bytes);
            var a = BitConverter.GetBytes(0xF001);
            foreach (var i in a) {
                Trace.Write(Convert.ToString(i, 2)+',');
            }
            Assert.AreEqual(result, 0xFF);
        }

        [TestMethod]
        public void TestNormalValueObjectSerializeInStruct() {
            var obj = ObjectsInitializer.InitATestPureValueObject();
            RawBinaryFormatter formatter = new RawBinaryFormatter();
            var mStream = new MemoryStream();
            formatter.Serialize(mStream,obj);
            var buffer = mStream.ToArray();
            var expected = ObjectsInitializer.GetATestPureValueObjectMemoryLayout();
            Assert.IsTrue(Enumerable.SequenceEqual<byte>(buffer, expected));

        }

        [TestMethod]
        public void TestArrayValueObjectSerializeInStruct() {
            var obj = ObjectsInitializer.InitATestArrayValueObject();
            RawBinaryFormatter formatter = new RawBinaryFormatter();
            var mStream = new MemoryStream();
            formatter.Serialize(mStream, obj);
            var buffer = mStream.ToArray();
            var expected = ObjectsInitializer.GetATestArrayValueObjectMemeryLayout();
            Assert.IsTrue(Enumerable.SequenceEqual<byte>(buffer, expected));
        }

        [TestMethod]
        public void TestMarshal() {
            RawBinaryFormatter formatter = new RawBinaryFormatter();
            MemoryStream ms = new MemoryStream();
            var marshalas = new MarshalAs() {
                fU1 = 0x01020304,
                fU2 = 0x01020304,
                fU4 = 0x01020304,
                fU8 = 0x01020304,
                fR4 = 1566.71,
                fR8 = 1587.22,
            
            };
            formatter.Serialize(ms, marshalas);
            var buffer = ms.ToArray();
            Assert.IsTrue(Enumerable.SequenceEqual(new byte[] {4, 3, 4,1,2,3,4,0,0,0,0,1,2,3,4, 0x44,0xC3,0xD6,0xB8, 0x40,0x98,0xCC,0xE1,0x47,0xAE,0x14,0x7B }, buffer
                ));
        }

        [TestMethod]
        public void Test_ArrayValueSpecifiedEndianObject()
        {
            var obj = ObjectsInitializer.InitATestArraySpecifiedEndianValueObject();
            RawBinaryFormatter formatter = new RawBinaryFormatter();
            var mStream = new MemoryStream();
            formatter.Serialize(mStream, obj);
            var buffer = mStream.ToArray();
            var expected = ObjectsInitializer.GetATestArrayValueObjectMemeryLayoutBigEndian();
            Assert.IsTrue(Enumerable.SequenceEqual<byte>(buffer, expected));
        }

    }
}
