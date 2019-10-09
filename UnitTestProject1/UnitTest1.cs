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
            Assert.AreEqual(result, 0xFF);
        }
    }
}
