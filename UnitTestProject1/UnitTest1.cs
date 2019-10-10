using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.Reflection;

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
            public int fInt;
            public uint fUInt;
            public long fLong;
            public ulong fULong;
            public short fShort;
            public ushort fUShort;
            public decimal fDecimal;
            public float fFloat;
            public double fDouble;
            public char fChar;
            public bool fBool;

            public int pInt { get; set; }
            public uint pUInt { get; set; }
            public long pLong { get; set; }
            public ulong pULong { get; set; }
            public short pShort { get; set; }
            public ushort pUShort { get; set; }
            public decimal pDecimal { get; set; }
            public float pFloat { get; set; }
            public double pDouble { get; set; }
            public char pChar { get; set; }
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
            obj.fDecimal = 0x4440;
            obj.fFloat = 0x5550;
            obj.fDouble = 0x6660;
            obj.fChar = (char)0x7770;
                

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

        
    }
}
