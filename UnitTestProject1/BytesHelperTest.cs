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
        public void Test_BytesHelper_GetTypedObjectFromBytes_TruncationTest()
        {
            #region INT 类型
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
            #endregion

            #region uint类型
            //大端
            buffer = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF };
            var uintResult = (uint)BytesHelper.GetTypedObjectFromBytes(buffer, typeof(uint), true);
            Assert.AreEqual(uint.MaxValue, uintResult);
            //小端
            buffer = new byte[] { 0x01, 0x00, 0x00, 0x00 };
            uintResult = (uint)BytesHelper.GetTypedObjectFromBytes(buffer, typeof(uint), false);
            Assert.AreEqual((uint)1, uintResult);
            //小端 位数不够
            Assert.ThrowsException<ArgumentException>(() =>
            {
                buffer = new byte[] { 0x01, 0x00 };
                uintResult = (uint)BytesHelper.GetTypedObjectFromBytes(buffer, typeof(uint), false);
                Assert.AreEqual((uint)1, uintResult);
            });
            //小端 位数不够 补0
            buffer = new byte[] { 0x01, 0x00 };
            buffer = BytesHelper.DeserializeAutoPaddingOrTruncate(buffer, typeof(uint), false);
            uintResult = (uint)BytesHelper.GetTypedObjectFromBytes(buffer, typeof(uint), false);
            Assert.AreEqual((uint)1, uintResult);
            //小端 位数 过多异常
            Assert.ThrowsException<ArgumentException>(() =>
            {
                buffer = new byte[] { 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 };
                uintResult = (uint)BytesHelper.GetTypedObjectFromBytes(buffer, typeof(uint), false);
                Assert.AreEqual((uint)1, uintResult);

            });
            // 小端 位数过多 截断处理
            buffer = new byte[] { 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 };
            buffer = BytesHelper.DeserializeAutoPaddingOrTruncate(buffer, typeof(uint), false);
            uintResult = (uint)BytesHelper.GetTypedObjectFromBytes(buffer, typeof(uint), false);
            Assert.AreEqual((uint)1, uintResult);
            #endregion

            #region byte 类型
            //大端
            buffer = new byte[] { 0xFF };
            var byteResult = (byte)BytesHelper.GetTypedObjectFromBytes(buffer, typeof(byte), true);
            Assert.AreEqual(byte.MaxValue, byteResult);
            //小端
            buffer = new byte[] { 0x01 };
            byteResult = (byte)BytesHelper.GetTypedObjectFromBytes(buffer, typeof(byte), false);
            Assert.AreEqual((byte)1, byteResult);
            //小端 位数不够
            Assert.ThrowsException<ArgumentException>(() =>
            {
                buffer = Array.Empty<byte>();
                byteResult = (byte)BytesHelper.GetTypedObjectFromBytes(buffer, typeof(byte), false);
                Assert.AreEqual((byte)1, byteResult);
            });
            //小端 位数不够 补0
            buffer = Array.Empty<byte>();
            buffer = BytesHelper.DeserializeAutoPaddingOrTruncate(buffer, typeof(byte), false);
            byteResult = (byte)BytesHelper.GetTypedObjectFromBytes(buffer, typeof(byte), false);
            Assert.AreEqual((byte)0, byteResult);
            //小端 位数 过多异常
            Assert.ThrowsException<ArgumentException>(() =>
            {
                buffer = new byte[] { 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 };
                byteResult = (byte)BytesHelper.GetTypedObjectFromBytes(buffer, typeof(byte), false);
                Assert.AreEqual((byte)1, byteResult);

            });
            // 小端 位数过多 截断处理
            buffer = new byte[] { 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 };
            buffer = BytesHelper.DeserializeAutoPaddingOrTruncate(buffer, typeof(byte), false);
            byteResult = (byte)BytesHelper.GetTypedObjectFromBytes(buffer, typeof(byte), false);
            Assert.AreEqual((byte)1, byteResult);
            #endregion

            #region sbyte类型
            //大端
            buffer = new byte[] { 0x7F };
            var sbyteResult = Convert.ToSByte(BytesHelper.GetTypedObjectFromBytes(buffer, typeof(sbyte), true));
            Assert.AreEqual(sbyte.MaxValue, sbyteResult);
            //小端
            buffer = new byte[] { 0x01 };
            sbyteResult = Convert.ToSByte(BytesHelper.GetTypedObjectFromBytes(buffer, typeof(sbyte), false));
            Assert.AreEqual((sbyte)1, sbyteResult);
            //小端 位数不够
            Assert.ThrowsException<ArgumentException>(() =>
            {
                buffer = new byte[] { };
                sbyteResult = Convert.ToSByte(BytesHelper.GetTypedObjectFromBytes(buffer, typeof(sbyte), false));
                Assert.AreEqual((sbyte)1, sbyteResult);
            });
            //小端 位数不够 补0
            buffer = new byte[] { };
            buffer = BytesHelper.DeserializeAutoPaddingOrTruncate(buffer, typeof(sbyte), false);
            sbyteResult = Convert.ToSByte(BytesHelper.GetTypedObjectFromBytes(buffer, typeof(sbyte), false));
            Assert.AreEqual((sbyte)0, sbyteResult);
            //小端 位数 过多异常
            Assert.ThrowsException<ArgumentException>(() =>
            {
                buffer = new byte[] { 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 };
                sbyteResult = Convert.ToSByte(BytesHelper.GetTypedObjectFromBytes(buffer, typeof(sbyte), false));
                Assert.AreEqual((sbyte)1, sbyteResult);

            });
            // 小端 位数过多 截断处理
            buffer = new byte[] { 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 };
            buffer = BytesHelper.DeserializeAutoPaddingOrTruncate(buffer, typeof(sbyte), false);
            sbyteResult = Convert.ToSByte(BytesHelper.GetTypedObjectFromBytes(buffer, typeof(sbyte), false));
            Assert.AreEqual((sbyte)1, sbyteResult);
            #endregion

            #region short类型
            //大端
            buffer = new byte[] { 0x7F, 0xFF };
            var shortResult = (short)BytesHelper.GetTypedObjectFromBytes(buffer, typeof(short), true);
            Assert.AreEqual(short.MaxValue, shortResult);
            //小端
            buffer = new byte[] { 0x01, 0x00 };
            shortResult = (short)BytesHelper.GetTypedObjectFromBytes(buffer, typeof(short), false);
            Assert.AreEqual((short)1, shortResult);
            //小端 位数不够
            Assert.ThrowsException<ArgumentException>(() =>
            {
                buffer = new byte[] { 0x01, };
                shortResult = (short)BytesHelper.GetTypedObjectFromBytes(buffer, typeof(short), false);
                Assert.AreEqual((short)1, shortResult);
            });
            //小端 位数不够 补0
            buffer = new byte[] { 0x01,};
            buffer = BytesHelper.DeserializeAutoPaddingOrTruncate(buffer, typeof(short), false);
            shortResult = (short)BytesHelper.GetTypedObjectFromBytes(buffer, typeof(short), false);
            Assert.AreEqual((short)1, shortResult);
            //小端 位数 过多异常
            Assert.ThrowsException<ArgumentException>(() =>
            {
                buffer = new byte[] { 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 };
                shortResult = (short)BytesHelper.GetTypedObjectFromBytes(buffer, typeof(short), false);
                Assert.AreEqual((short)1, shortResult);

            });
            // 小端 位数过多 截断处理
            buffer = new byte[] { 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 };
            buffer = BytesHelper.DeserializeAutoPaddingOrTruncate(buffer, typeof(short), false);
            shortResult = (short)BytesHelper.GetTypedObjectFromBytes(buffer, typeof(short), false);
            Assert.AreEqual((short)1, shortResult);
            #endregion

            #region ushort类型
            //大端
            buffer = new byte[] { 0xFF, 0xFF };
            var ushortResult = (ushort)BytesHelper.GetTypedObjectFromBytes(buffer, typeof(ushort), true);
            Assert.AreEqual(ushort.MaxValue, ushortResult);
            //小端
            buffer = new byte[] { 0x01, 0x00 };
            ushortResult = (ushort)BytesHelper.GetTypedObjectFromBytes(buffer, typeof(ushort), false);
            Assert.AreEqual((ushort)1, ushortResult);
            //小端 位数不够
            Assert.ThrowsException<ArgumentException>(() =>
            {
                buffer = new byte[] { 0x01, };
                ushortResult = (ushort)BytesHelper.GetTypedObjectFromBytes(buffer, typeof(ushort), false);
                Assert.AreEqual((ushort)1, ushortResult);
            });
            //小端 位数不够 补0
            buffer = new byte[] { 0x01, };
            buffer = BytesHelper.DeserializeAutoPaddingOrTruncate(buffer, typeof(ushort), false);
            ushortResult = (ushort)BytesHelper.GetTypedObjectFromBytes(buffer, typeof(ushort), false);
            Assert.AreEqual((ushort)1, ushortResult);
            //小端 位数 过多异常
            Assert.ThrowsException<ArgumentException>(() =>
            {
                buffer = new byte[] { 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 };
                ushortResult = (ushort)BytesHelper.GetTypedObjectFromBytes(buffer, typeof(ushort), false);
                Assert.AreEqual((ushort)1, ushortResult);

            });
            // 小端 位数过多 截断处理
            buffer = new byte[] { 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 };
            buffer = BytesHelper.DeserializeAutoPaddingOrTruncate(buffer, typeof(ushort), false);
            ushortResult = (ushort)BytesHelper.GetTypedObjectFromBytes(buffer, typeof(ushort), false);
            Assert.AreEqual((ushort)1, ushortResult);
            #endregion

            #region long类型
            //大端
            buffer = new byte[] { 0x7F, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };
            var longResult = (long)BytesHelper.GetTypedObjectFromBytes(buffer, typeof(long), true);
            Assert.AreEqual(long.MaxValue, longResult);
            //小端
            buffer = new byte[] { 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            longResult = (long)BytesHelper.GetTypedObjectFromBytes(buffer, typeof(long), false);
            Assert.AreEqual((long)1, longResult);
            //小端 位数不够
            Assert.ThrowsException<ArgumentException>(() =>
            {
                buffer = new byte[] { 0x01, 0x00, 0x00, 0x00 };
                longResult = (long)BytesHelper.GetTypedObjectFromBytes(buffer, typeof(long), false);
                Assert.AreEqual((long)1, longResult);
            });
            //小端 位数不够 补0
            buffer = new byte[] { 0x01, 0x00, 0x00 };
            buffer = BytesHelper.DeserializeAutoPaddingOrTruncate(buffer, typeof(long), false);
            longResult = (long)BytesHelper.GetTypedObjectFromBytes(buffer, typeof(long), false);
            Assert.AreEqual((long)1, longResult);
            //小端 位数 过多异常
            Assert.ThrowsException<ArgumentException>(() =>
            {
                buffer = new byte[] { 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 };
                longResult = (long)BytesHelper.GetTypedObjectFromBytes(buffer, typeof(long), false);
                Assert.AreEqual((long)1, longResult);

            });
            // 小端 位数过多 截断处理
            buffer = new byte[] { 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 };
            buffer = BytesHelper.DeserializeAutoPaddingOrTruncate(buffer, typeof(long), false);
            longResult = (long)BytesHelper.GetTypedObjectFromBytes(buffer, typeof(long), false);
            Assert.AreEqual((long)1, longResult);
            #endregion

            #region ulong类型
            //大端
            buffer = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };
            var ulongResult = (ulong)BytesHelper.GetTypedObjectFromBytes(buffer, typeof(ulong), true);
            Assert.AreEqual(ulong.MaxValue, ulongResult);
            //小端
            buffer = new byte[] { 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            ulongResult = (ulong)BytesHelper.GetTypedObjectFromBytes(buffer, typeof(ulong), false);
            Assert.AreEqual((ulong)1, ulongResult);
            //小端 位数不够
            Assert.ThrowsException<ArgumentException>(() =>
            {
                buffer = new byte[] { 0x01, 0x00, 0x00, 0x00 };
                ulongResult = (ulong)BytesHelper.GetTypedObjectFromBytes(buffer, typeof(ulong), false);
                Assert.AreEqual((ulong)1, ulongResult);
            });
            //小端 位数不够 补0
            buffer = new byte[] { 0x01, 0x00, 0x00 };
            buffer = BytesHelper.DeserializeAutoPaddingOrTruncate(buffer, typeof(ulong), false);
            ulongResult = (ulong)BytesHelper.GetTypedObjectFromBytes(buffer, typeof(ulong), false);
            Assert.AreEqual((ulong)1, ulongResult);
            //小端 位数 过多异常
            Assert.ThrowsException<ArgumentException>(() =>
            {
                buffer = new byte[] { 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 };
                ulongResult = (ulong)BytesHelper.GetTypedObjectFromBytes(buffer, typeof(ulong), false);
                Assert.AreEqual((ulong)1, ulongResult);

            });
            // 小端 位数过多 截断处理
            buffer = new byte[] { 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 };
            buffer = BytesHelper.DeserializeAutoPaddingOrTruncate(buffer, typeof(ulong), false);
            ulongResult = (ulong)BytesHelper.GetTypedObjectFromBytes(buffer, typeof(ulong), false);
            Assert.AreEqual((ulong)1, ulongResult);
            #endregion

            #region char类型
            //大端
            buffer = new byte[] { 0xFF, 0xFF };
            var charResult = (char)BytesHelper.GetTypedObjectFromBytes(buffer, typeof(char), true);
            Assert.AreEqual(char.MaxValue, charResult);
            //小端
            buffer = new byte[] { 0x01, 0x00};
            charResult = (char)BytesHelper.GetTypedObjectFromBytes(buffer, typeof(char), false);
            Assert.AreEqual((char)1, charResult);
            //小端 位数不够
            Assert.ThrowsException<ArgumentException>(() =>
            {
                buffer = new byte[] { 0x01 };
                charResult = (char)BytesHelper.GetTypedObjectFromBytes(buffer, typeof(char), false);
                Assert.AreEqual((char)1, charResult);
            });
            //小端 位数不够 补0
            buffer = new byte[] { 0x01 };
            buffer = BytesHelper.DeserializeAutoPaddingOrTruncate(buffer, typeof(char), false);
            charResult = (char)BytesHelper.GetTypedObjectFromBytes(buffer, typeof(char), false);
            Assert.AreEqual((char)1, charResult);
            //小端 位数 过多异常
            Assert.ThrowsException<ArgumentException>(() =>
            {
                buffer = new byte[] { 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 };
                charResult = (char)BytesHelper.GetTypedObjectFromBytes(buffer, typeof(char), false);
                Assert.AreEqual((char)1, charResult);

            });
            // 小端 位数过多 截断处理
            buffer = new byte[] { 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 };
            buffer = BytesHelper.DeserializeAutoPaddingOrTruncate(buffer, typeof(char), false);
            charResult = (char)BytesHelper.GetTypedObjectFromBytes(buffer, typeof(char), false);
            Assert.AreEqual((char)1, charResult);
            #endregion

            #region bool类型
            ////大端
            //buffer = new byte[] { 0xFF, 0xFF };
            //var boolResult = (bool)BytesHelper.GetTypedObjectFromBytes(buffer, typeof(bool), true);
            //Assert.AreEqual(bool.MaxValue, boolResult);
            ////小端
            //buffer = new byte[] { 0x01, 0x00 };
            //boolResult = (bool)BytesHelper.GetTypedObjectFromBytes(buffer, typeof(bool), false);
            //Assert.AreEqual((bool)1, boolResult);
            ////小端 位数不够
            //Assert.ThrowsException<ArgumentException>(() =>
            //{
            //    buffer = new byte[] { 0x01 };
            //    boolResult = (bool)BytesHelper.GetTypedObjectFromBytes(buffer, typeof(bool), false);
            //    Assert.AreEqual((bool)1, boolResult);
            //});
            ////小端 位数不够 补0
            //buffer = new byte[] { 0x01 };
            //buffer = BytesHelper.DeserializeAutoPaddingOrTruncate(buffer, typeof(bool), false);
            //boolResult = (bool)BytesHelper.GetTypedObjectFromBytes(buffer, typeof(bool), false);
            //Assert.AreEqual((bool)1, boolResult);
            ////小端 位数 过多异常
            //Assert.ThrowsException<ArgumentException>(() =>
            //{
            //    buffer = new byte[] { 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 };
            //    boolResult = (bool)BytesHelper.GetTypedObjectFromBytes(buffer, typeof(bool), false);
            //    Assert.AreEqual((bool)1, boolResult);

            //});
            //// 小端 位数过多 截断处理
            //buffer = new byte[] { 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 };
            //buffer = BytesHelper.DeserializeAutoPaddingOrTruncate(buffer, typeof(bool), false);
            //boolResult = (bool)BytesHelper.GetTypedObjectFromBytes(buffer, typeof(bool), false);
            //Assert.AreEqual((bool)1, boolResult);
            #endregion
        }




        [TestMethod]
        public void Test_BytesHelper_GetTypedObjectFromBytes_EachType() {
            
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
