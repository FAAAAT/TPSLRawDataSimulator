using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.IO;
using System.Linq;
using TPSLRawDataSimulator;
using System.Runtime.InteropServices;

namespace UnitTestProject1
{
    [TestClass]
    public class BytesHelperTest
    {
        [TestMethod]
        public void Test_BytesHelper_GetBytes()
        {
            var result = BytesHelper.GetBytes(0x04030201, true);
            Assert.IsTrue(Enumerable.SequenceEqual(new byte[] { 4, 3, 2, 1 }, result));
            Assert.IsTrue(Enumerable.SequenceEqual(new byte[] { 1, 2, 3, 4 }, BytesHelper.GetBytes(0x04030201, false)));
        }

        [TestMethod]
        public void Test_BytesHelper_GetTypedObjectFromBytes()
        {
            //big endian
            var buffer = new byte[] { 0x7F, 0xFF, 0xFF, 0xFF };
            var result = (int)BytesHelper.GetTypedObjectFromBytes(buffer, typeof(int), true);
            Assert.AreEqual(int.MaxValue, result);
            //little endian
            buffer = new byte[] { 0x01, 0x00, 0x00, 0x00 };
            result = (int)BytesHelper.GetTypedObjectFromBytes(buffer, typeof(int), false);
            Assert.AreEqual(1, result);
            //little endian not enough bytes
            Assert.ThrowsException<ArgumentException>(() =>
            {
                buffer = new byte[] { 0x01, 0x00 };
                BytesHelper.GetTypedObjectFromBytes(buffer, typeof(int), false);
                Assert.AreEqual(1, result);
            });
            //little endian not enough bytes padding zero
            buffer = new byte[] { 0x01, 0x00 };
            buffer = BytesHelper.DeserializeAutoPaddingOrTruncate(buffer, typeof(int), false);
            result = (int)BytesHelper.GetTypedObjectFromBytes(buffer, typeof(int), false);
            Assert.AreEqual(1, result);
            //little endian more bytes than expected
            Assert.ThrowsException<ArgumentException>(() =>
            {
                buffer = new byte[] { 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 };
                result = (int)BytesHelper.GetTypedObjectFromBytes(buffer, typeof(int), false);
                Assert.AreEqual(1, result);
            });
            //little endian more bytes than expected truncation
            buffer = new byte[] { 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x01 };
            buffer = BytesHelper.DeserializeAutoPaddingOrTruncate(buffer, typeof(int), false);
            result = (int)BytesHelper.GetTypedObjectFromBytes(buffer, typeof(int), false);
            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public void Test_BytesHelper_GetTypedObjectFromStream()
        {
            // this method is no need to invoke DeserializeAutoPaddingOrTruncate.

        }

        [TestMethod]
        public void Test_ByteHelper_GetArrayFromStream()
        {
            //大端
            var buffer = new byte[] { 0x7F, 0xFF, 0xFF, 0xFF };
            var stream = new MemoryStream(buffer);
            var result = (int[])BytesHelper.GetArrayFromStream(stream, typeof(int), buffer.Length, true);
            Assert.IsTrue(Enumerable.SequenceEqual(new[] { int.MaxValue }, result));
            //小端
            buffer = new byte[] { 0x01, 0x00, 0x00, 0x00 };
            stream = new MemoryStream(buffer);
            result = (int[])BytesHelper.GetArrayFromStream(stream, typeof(int), buffer.Length, false);
            Assert.IsTrue(Enumerable.SequenceEqual(new[] { 1 }, result));
            //小端 位数不够
            Assert.ThrowsException<ArgumentException>(() =>
            {
                buffer = new byte[] { 0x01, 0x00 };
                stream = new MemoryStream(buffer);
                result = (int[])BytesHelper.GetArrayFromStream(stream, typeof(int), buffer.Length, false);
                Assert.IsTrue(Enumerable.SequenceEqual(new[] { 1 }, result));
            });
            //小端 位数不够 填充0
            buffer = new byte[] { 0x01, 0x00 };
            buffer = BytesHelper.DeserializeAutoPaddingOrTruncate(buffer, typeof(int), false);
            stream = new MemoryStream(buffer);
            result = (int[])BytesHelper.GetArrayFromStream(stream, typeof(int), buffer.Length, false);
            Assert.IsTrue(Enumerable.SequenceEqual(new[] { 1 }, result));
            //小端 位数过多
            Assert.ThrowsException<ArgumentException>(() =>
            {
                buffer = new byte[] { 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 };
                stream = new MemoryStream(buffer);
                result = (int[])BytesHelper.GetArrayFromStream(stream, typeof(int), buffer.Length, false);
                Assert.IsTrue(Enumerable.SequenceEqual(new[] { 1 }, result));
            });
            //小端 位数过多 截断
            buffer = new byte[] { 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x01 };
            buffer = BytesHelper.DeserializeAutoPaddingOrTruncate(buffer, typeof(int), false);
            stream = new MemoryStream(buffer);
            result = (int[])BytesHelper.GetArrayFromStream(stream, typeof(int), buffer.Length, false);
            Assert.IsTrue(Enumerable.SequenceEqual(new[] { 1 }, result));

        }

    }
}
