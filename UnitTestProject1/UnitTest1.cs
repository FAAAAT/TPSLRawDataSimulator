using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.Reflection;
using TPSLRawDataSimulator;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        public struct testArray
        {

            public int[] IA1 { get; set; }
            public uint[] UA2;
        }

        public struct NormalValueObject {
            [MemberIndex(Index = 10)]
            public int fInt;
            [MemberIndex(Index = 20)]
            public uint fUInt;
            [MemberIndex(Index = 30)]
            public long fLong;
            [MemberIndex(Index = 40)]
            public ulong fULong;
            [MemberIndex(Index = 50)]
            public short fShort;
            [MemberIndex(Index = 60)]
            public ushort fUShort;
            // Decimal is not support now.
            //public decimal fDecimal;
            [MemberIndex(Index = 70)]
            public float fFloat;
            [MemberIndex(Index = 80)]
            public double fDouble;
            [MemberIndex(Index = 90)]
            public char fChar;
            [MemberIndex(Index = 100)]
            public bool fBool;

            [MemberIndex(Index = 110)]
            public int pInt { get; set; }
            [MemberIndex(Index = 120)]
            public uint pUInt { get; set; }
            [MemberIndex(Index = 130)]
            public long pLong { get; set; }
            [MemberIndex(Index = 140)]
            public ulong pULong { get; set; }
            [MemberIndex(Index = 150)]
            public short pShort { get; set; }
            [MemberIndex(Index = 160)]
            public ushort pUShort { get; set; }
            // Decimal is not support now.
            //public decimal pDecimal { get; set; }
            [MemberIndex(Index = 170)]
            public float pFloat { get; set; }
            [MemberIndex(Index = 180)]
            public double pDouble { get; set; }
            [MemberIndex(Index = 190)]
            public char pChar { get; set; }
            [MemberIndex(Index = 200)]
            public bool pBool { get; set; }

        }

        public NormalValueObject InitATestPureValueObject() {
            NormalValueObject obj = new NormalValueObject();
            obj.fInt = 0x11111110;
            obj.fUInt = 0x11111111;
            obj.fLong = 0x2222222222222220;
            obj.fULong = 0x2222222222222221;
            obj.fShort = 0x3330;
            obj.fUShort = 0x3331;
            //obj.fDecimal = 0x4440;
            obj.fFloat = 0x5550;
            obj.fDouble = 0x6660;
            obj.fChar = (char)0x7770;
            obj.fBool = true;

            obj.pInt = 0x11111110;
            obj.pUInt = 0x11111111;
            obj.pLong = 0x2222222222222220;
            obj.pULong = 0x2222222222222221;
            obj.pShort = 0x3330;
            obj.pUShort = 0x3331;
            //obj.fDecimal = 0x4440;
            obj.pFloat = 12.5f;
            obj.pDouble = 18952.31d;
            obj.pChar = (char)0x7770;
            obj.pBool = false;
            return obj;
        }

        public IEnumerable<byte> GetATestPureValueObjectMemoryLayout()
        {
            List<byte> result = new List<byte>();
            var resultObj = InitATestPureValueObject();
            result.AddRange(BitConverter.GetBytes(resultObj.fInt));
            result.AddRange(BitConverter.GetBytes(resultObj.fUInt));
            result.AddRange(BitConverter.GetBytes(resultObj.fLong));
            result.AddRange(BitConverter.GetBytes(resultObj.fULong));
            result.AddRange(BitConverter.GetBytes(resultObj.fShort));
            result.AddRange(BitConverter.GetBytes(resultObj.fUShort));
            result.AddRange(BitConverter.GetBytes(resultObj.fFloat));
            result.AddRange(BitConverter.GetBytes(resultObj.fDouble));
            result.AddRange(BitConverter.GetBytes(resultObj.fChar));
            result.AddRange(BitConverter.GetBytes(resultObj.fBool));

            result.AddRange(BitConverter.GetBytes(resultObj.pInt));
            result.AddRange(BitConverter.GetBytes(resultObj.pUInt));
            result.AddRange(BitConverter.GetBytes(resultObj.pLong));
            result.AddRange(BitConverter.GetBytes(resultObj.pULong));
            result.AddRange(BitConverter.GetBytes(resultObj.pShort));
            result.AddRange(BitConverter.GetBytes(resultObj.pUShort));
            result.AddRange(BitConverter.GetBytes(resultObj.pFloat));
            result.AddRange(BitConverter.GetBytes(resultObj.pDouble));
            result.AddRange(BitConverter.GetBytes(resultObj.pChar));
            result.AddRange(BitConverter.GetBytes(resultObj.pBool));
            return result;

        }

        [TestMethod]
        public void TestMethod1()
        {
            var memberInfos = typeof(testArray).GetMembers(BindingFlags.Instance | BindingFlags.Public);
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
            var obj = InitATestPureValueObject();
            RawBinaryFormatter formatter = new RawBinaryFormatter();
            var mStream = new MemoryStream();
            formatter.Serialize(mStream,obj);
            var buffer = mStream.ToArray();
            var expected = GetATestPureValueObjectMemoryLayout();
            Assert.IsTrue(Enumerable.SequenceEqual<byte>(buffer, expected));

        }
       
    }
}
