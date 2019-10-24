using System;
using System.Collections.Generic;
using System.Text;
using TPSLRawDataSimulator;
using System.Runtime.InteropServices;
using System.Linq;


namespace UnitTestProject1
{
    public struct TestArray
    {
        public int[] IA1 { get; set; }
        public uint[] UA2;
    }
    public struct NormalValueObject
    {
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

        public override bool Equals(object obj)
        {
            if (GetType() != obj.GetType())
            {
                return false;
            }

            var unboxedObj = (NormalValueObject)obj;
            if (!(fInt == unboxedObj.fInt))
            {
                return false;
            }

            if (!(fUInt == unboxedObj.fUInt))
            {
                return false;
            }

            if (!(fLong == unboxedObj.fLong))
            {
                return false;
            }

            if (!(fULong == unboxedObj.fULong))
            {
                return false;
            }

            if (!(fShort == unboxedObj.fShort))
            {
                return false;
            }

            if (!(fUShort == unboxedObj.fUShort))
            {
                return false;
            }

            if (!(fFloat == unboxedObj.fFloat))
            {
                return false;
            }

            if (!(fDouble == unboxedObj.fDouble))
            {
                return false;
            }

            if (!(fChar == unboxedObj.fChar))
            {
                return false;
            }

            if (!(fBool == unboxedObj.fBool))
            {
                return false;
            }

            if (!(pInt == unboxedObj.pInt))
            {
                return false;
            }

            if (!(pUInt == unboxedObj.pUInt))
            {
                return false;
            }

            if (!(pLong == unboxedObj.pLong))
            {
                return false;
            }

            if (!(pULong == unboxedObj.pULong))
            {
                return false;
            }

            if (!(pShort == unboxedObj.pShort))
            {
                return false;
            }

            if (!(pUShort == unboxedObj.pUShort))
            {
                return false;
            }

            if (!(pFloat == unboxedObj.pFloat))
            {
                return false;
            }

            if (!(pDouble == unboxedObj.pDouble))
            {
                return false;
            }

            if (!(pChar == unboxedObj.pChar))
            {
                return false;
            }

            if (!(pBool == unboxedObj.pBool))
            {
                return false;
            }

            return true;
        }
    }
    public struct ArrayValueObject
    {
        [MemberIndex(Index = 10)]
        public int[] fIntArray;
        [MemberIndex(Index = 20)]
        public uint[] fUIntArray;
        [MemberIndex(Index = 30)]
        public long[] fLongArray;
        [MemberIndex(Index = 40)]
        public ulong[] fULongArray;
        [MemberIndex(Index = 50)]
        public short[] fShortArray;
        [MemberIndex(Index = 60)]
        public ushort[] fUShortArray;
        [MemberIndex(Index = 70)]
        public float[] fFloatArray;
        [MemberIndex(Index = 80)]
        public double[] fDoubleArray;
        [MemberIndex(Index = 90)]
        public char[] fCharArray;
        [MemberIndex(Index = 100)]
        public bool[] fBoolArray;

        [MemberIndex(Index = 110)]
        public int[] pIntArray { get; set; }
        [MemberIndex(Index = 120)]
        public uint[] pUIntArray { get; set; }
        [MemberIndex(Index = 130)]
        public long[] pLongArray { get; set; }
        [MemberIndex(Index = 140)]
        public ulong[] pULongArray { get; set; }
        [MemberIndex(Index = 150)]
        public short[] pShortArray { get; set; }
        [MemberIndex(Index = 160)]
        public ushort[] pUShortArray { get; set; }
        [MemberIndex(Index = 170)]
        public float[] pFloatArray { get; set; }
        [MemberIndex(Index = 180)]
        public double[] pDoubleArray { get; set; }
        [MemberIndex(Index = 190)]
        public char[] pCharArray { get; set; }
        [MemberIndex(Index = 200)]
        public bool[] pBoolArray { get; set; }

        public override bool Equals(object obj)
        {
            if (GetType() != obj.GetType())
            {
                return false;
            }

            var unboxedObj = (ArrayValueObject)obj;
            if (!Enumerable.SequenceEqual(fIntArray, unboxedObj.fIntArray))
            {
                return false;
            }

            if (!Enumerable.SequenceEqual(fUIntArray, unboxedObj.fUIntArray))
            {
                return false;
            }

            if (!Enumerable.SequenceEqual(fLongArray, unboxedObj.fLongArray))
            {
                return false;
            }

            if (!Enumerable.SequenceEqual(fULongArray, unboxedObj.fULongArray))
            {
                return false;
            }

            if (!Enumerable.SequenceEqual(fShortArray, unboxedObj.fShortArray))
            {
                return false;
            }

            if (!Enumerable.SequenceEqual(fUShortArray, unboxedObj.fUShortArray))
            {
                return false;
            }

            if (!Enumerable.SequenceEqual(fFloatArray, unboxedObj.fFloatArray))
            {
                return false;
            }

            if (!Enumerable.SequenceEqual(fDoubleArray, unboxedObj.fDoubleArray))
            {
                return false;
            }

            if (!Enumerable.SequenceEqual(fCharArray, unboxedObj.fCharArray))
            {
                return false;
            }

            if (!Enumerable.SequenceEqual(fBoolArray, unboxedObj.fBoolArray))
            {
                return false;
            }

            if (!Enumerable.SequenceEqual(pIntArray, unboxedObj.pIntArray))
            {
                return false;
            }

            if (!Enumerable.SequenceEqual(pUIntArray, unboxedObj.pUIntArray))
            {
                return false;
            }

            if (!Enumerable.SequenceEqual(pLongArray, unboxedObj.pLongArray))
            {
                return false;
            }

            if (!Enumerable.SequenceEqual(pULongArray, unboxedObj.pULongArray))
            {
                return false;
            }

            if (!Enumerable.SequenceEqual(pShortArray, unboxedObj.pShortArray))
            {
                return false;
            }

            if (!Enumerable.SequenceEqual(pUShortArray, unboxedObj.pUShortArray))
            {
                return false;
            }

            if (!Enumerable.SequenceEqual(pFloatArray, unboxedObj.pFloatArray))
            {
                return false;
            }

            if (!Enumerable.SequenceEqual(pDoubleArray, unboxedObj.pDoubleArray))
            {
                return false;
            }

            if (!Enumerable.SequenceEqual(pCharArray, unboxedObj.pCharArray))
            {
                return false;
            }

            if (!Enumerable.SequenceEqual(pBoolArray, unboxedObj.pBoolArray))
            {
                return false;
            }

            return true;
        }
    }

    [StructToRaw(Endian = Endian.BigEndian)]
    public struct ArrayValueSpecifiedEndianObject
    {
        [MemberIndex(Index = 10)]
        public int[] fIntArray;
        [MemberIndex(Index = 20)]
        public uint[] fUIntArray;
        [MemberIndex(Index = 30)]
        public long[] fLongArray;
        [MemberIndex(Index = 40)]
        public ulong[] fULongArray;
        [MemberIndex(Index = 50)]
        public short[] fShortArray;
        [MemberIndex(Index = 60)]
        public ushort[] fUShortArray;
        [MemberIndex(Index = 70)]
        public float[] fFloatArray;
        [MemberIndex(Index = 80)]
        public double[] fDoubleArray;
        [MemberIndex(Index = 90)]
        public char[] fCharArray;
        [MemberIndex(Index = 100)]
        public bool[] fBoolArray;

        [MemberIndex(Index = 110)]
        public int[] pIntArray { get; set; }
        [MemberIndex(Index = 120)]
        public uint[] pUIntArray { get; set; }
        [MemberIndex(Index = 130)]
        public long[] pLongArray { get; set; }
        [MemberIndex(Index = 140)]
        public ulong[] pULongArray { get; set; }
        [MemberIndex(Index = 150)]
        public short[] pShortArray { get; set; }
        [MemberIndex(Index = 160)]
        public ushort[] pUShortArray { get; set; }
        [MemberIndex(Index = 170)]
        public float[] pFloatArray { get; set; }
        [MemberIndex(Index = 180)]
        public double[] pDoubleArray { get; set; }
        [MemberIndex(Index = 190)]
        public char[] pCharArray { get; set; }
        [MemberIndex(Index = 200)]
        public bool[] pBoolArray { get; set; }

        public override bool Equals(object obj)
        {
            if (GetType() != obj.GetType())
            {
                return false;
            }

            var unboxedObj = (ArrayValueSpecifiedEndianObject)obj;
            if (!Enumerable.SequenceEqual(fIntArray, unboxedObj.fIntArray))
            {
                return false;
            }

            if (!Enumerable.SequenceEqual(fUIntArray, unboxedObj.fUIntArray))
            {
                return false;
            }

            if (!Enumerable.SequenceEqual(fLongArray, unboxedObj.fLongArray))
            {
                return false;
            }

            if (!Enumerable.SequenceEqual(fULongArray, unboxedObj.fULongArray))
            {
                return false;
            }

            if (!Enumerable.SequenceEqual(fShortArray, unboxedObj.fShortArray))
            {
                return false;
            }

            if (!Enumerable.SequenceEqual(fUShortArray, unboxedObj.fUShortArray))
            {
                return false;
            }

            if (!Enumerable.SequenceEqual(fFloatArray, unboxedObj.fFloatArray))
            {
                return false;
            }

            if (!Enumerable.SequenceEqual(fDoubleArray, unboxedObj.fDoubleArray))
            {
                return false;
            }

            if (!Enumerable.SequenceEqual(fCharArray, unboxedObj.fCharArray))
            {
                return false;
            }

            if (!Enumerable.SequenceEqual(fBoolArray, unboxedObj.fBoolArray))
            {
                return false;
            }

            if (!Enumerable.SequenceEqual(pIntArray, unboxedObj.pIntArray))
            {
                return false;
            }

            if (!Enumerable.SequenceEqual(pUIntArray, unboxedObj.pUIntArray))
            {
                return false;
            }

            if (!Enumerable.SequenceEqual(pLongArray, unboxedObj.pLongArray))
            {
                return false;
            }

            if (!Enumerable.SequenceEqual(pULongArray, unboxedObj.pULongArray))
            {
                return false;
            }

            if (!Enumerable.SequenceEqual(pShortArray, unboxedObj.pShortArray))
            {
                return false;
            }

            if (!Enumerable.SequenceEqual(pUShortArray, unboxedObj.pUShortArray))
            {
                return false;
            }

            if (!Enumerable.SequenceEqual(pFloatArray, unboxedObj.pFloatArray))
            {
                return false;
            }

            if (!Enumerable.SequenceEqual(pDoubleArray, unboxedObj.pDoubleArray))
            {
                return false;
            }

            if (!Enumerable.SequenceEqual(pCharArray, unboxedObj.pCharArray))
            {
                return false;
            }

            if (!Enumerable.SequenceEqual(pBoolArray, unboxedObj.pBoolArray))
            {
                return false;
            }

            return true;
        }
    }


    [StructToRaw(Endian = Endian.BigEndian)]
    public struct MarshalAs
    {
        [MemberIndex(Index = 10)]
        [MarshalAs(UnmanagedType.U1)]
        public int fU1;
        [MemberIndex(Index = 20)]
        [MarshalAs(UnmanagedType.U2)]
        public int fU2;
        [MemberIndex(Index = 30)]
        [MarshalAs(UnmanagedType.U4)]
        public int fU4;
        [MemberIndex(Index = 40)]
        [MarshalAs(UnmanagedType.U8)]
        public int fU8;
        [MemberIndex(Index = 50)]
        [MarshalAs(UnmanagedType.R4)]
        public double fR4;
        [MemberIndex(Index = 60)]
        [MarshalAs(UnmanagedType.R8)]
        public double fR8;

        public override bool Equals(object obj)
        {
            if (GetType() != obj.GetType())
            {
                return false;
            }

            var unboxedObj = (MarshalAs)obj;
            if (!(fU1 == unboxedObj.fU1))
            {
                return false;
            }

            if (!(fU2 == unboxedObj.fU2))
            {
                return false;
            }

            if (!(fU4 == unboxedObj.fU4))
            {
                return false;
            }

            if (!(fU8 == unboxedObj.fU8))
            {
                return false;
            }

            if (!(fR4 == unboxedObj.fR4))
            {
                return false;
            }

            if (!(fR8 == unboxedObj.fR8))
            {
                return false;
            }

            return true;
        }
    }

    public sealed class ObjectsInitializer {

        public static NormalValueObject InitATestPureValueObject()
        {
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

        public static ArrayValueObject InitATestArrayValueObject()
        {
            var result = new ArrayValueObject();
            result.fIntArray = new int[] { 0x11111110, 0x22222220, 0x33333330, 0x44444440, 0x55555550 };
            result.fUIntArray = new uint[] { 0x11111111, 0x22222221, 0x33333331, 0x44444441, 0x55555551 };
            result.fLongArray = new long[] { 0x111111111111110, 0x2222222222222220, 0x3333333333333330, 0x4444444444444440, 0x5555555555555550 };
            result.fULongArray = new ulong[] { 0x111111111111111, 0x2222222222222221, 0x3333333333333331, 0x4444444444444441, 0x5555555555555551 };
            result.fShortArray = new short[] { 0x1110, 0x2220, 0x3330, 0x4440, 0x5550 };
            result.fUShortArray = new ushort[] { 0x1110, 0x2220, 0x3330, 0x4440, 0x5550 };
            result.fFloatArray = new float[] { 1235.5f };
            result.fDoubleArray = new double[] { 12345.5d };
            result.fCharArray = "abcde".ToArray();
            result.fBoolArray = new bool[] { true, false, true, false };

            result.pIntArray = new int[] { 0x11111110, 0x22222220, 0x33333330, 0x44444440, 0x55555550 };
            result.pUIntArray = new uint[] { 0x11111111, 0x22222221, 0x33333331, 0x44444441, 0x55555551 };
            result.pLongArray = new long[] { 0x111111111111110, 0x2222222222222220, 0x3333333333333330, 0x4444444444444440, 0x5555555555555550 };
            result.pULongArray = new ulong[] { 0x111111111111111, 0x2222222222222221, 0x3333333333333331, 0x4444444444444441, 0x5555555555555551 };
            result.pShortArray = new short[] { 0x1110, 0x2220, 0x3330, 0x4440, 0x5550 };
            result.pUShortArray = new ushort[] { 0x1110, 0x2220, 0x3330, 0x4440, 0x5550 };
            result.pFloatArray = new float[] { 1235.5f };
            result.pDoubleArray = new double[] { 12345.5d };
            result.pCharArray = "edcba".ToArray();
            result.pBoolArray = new bool[] { false, true, false, true };

            return result;
        }

        public static ArrayValueSpecifiedEndianObject InitATestArraySpecifiedEndianValueObject()
        {
            var result = new ArrayValueSpecifiedEndianObject();
            result.fIntArray = new int[] { 0x11111110, 0x22222220, 0x33333330, 0x44444440, 0x55555550 };
            result.fUIntArray = new uint[] { 0x11111111, 0x22222221, 0x33333331, 0x44444441, 0x55555551 };
            result.fLongArray = new long[] { 0x111111111111110, 0x2222222222222220, 0x3333333333333330, 0x4444444444444440, 0x5555555555555550 };
            result.fULongArray = new ulong[] { 0x111111111111111, 0x2222222222222221, 0x3333333333333331, 0x4444444444444441, 0x5555555555555551 };
            result.fShortArray = new short[] { 0x1110, 0x2220, 0x3330, 0x4440, 0x5550 };
            result.fUShortArray = new ushort[] { 0x1110, 0x2220, 0x3330, 0x4440, 0x5550 };
            result.fFloatArray = new float[] { 1235.5f };
            result.fDoubleArray = new double[] { 12345.5d };
            result.fCharArray = "abcde".ToArray();
            result.fBoolArray = new bool[] { true, false, true, false };

            result.pIntArray = new int[] { 0x11111110, 0x22222220, 0x33333330, 0x44444440, 0x55555550 };
            result.pUIntArray = new uint[] { 0x11111111, 0x22222221, 0x33333331, 0x44444441, 0x55555551 };
            result.pLongArray = new long[] { 0x111111111111110, 0x2222222222222220, 0x3333333333333330, 0x4444444444444440, 0x5555555555555550 };
            result.pULongArray = new ulong[] { 0x111111111111111, 0x2222222222222221, 0x3333333333333331, 0x4444444444444441, 0x5555555555555551 };
            result.pShortArray = new short[] { 0x1110, 0x2220, 0x3330, 0x4440, 0x5550 };
            result.pUShortArray = new ushort[] { 0x1110, 0x2220, 0x3330, 0x4440, 0x5550 };
            result.pFloatArray = new float[] { 1235.5f };
            result.pDoubleArray = new double[] { 12345.5d };
            result.pCharArray = "edcba".ToArray();
            result.pBoolArray = new bool[] { false, true, false, true };

            return result;
        }

        public static IEnumerable<byte> GetATestPureValueObjectMemoryLayout()
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

        public static IEnumerable<byte> GetATestArrayValueObjectMemeryLayout()
        {
            var obj = InitATestArrayValueObject();
            List<byte> result = new List<byte>();
            foreach (var i in obj.fIntArray.Select(x => BitConverter.GetBytes(x)))
                result.AddRange(i);
            foreach (var i in obj.fUIntArray.Select(x => BitConverter.GetBytes(x)))
                result.AddRange(i);
            foreach (var i in obj.fLongArray.Select(x => BitConverter.GetBytes(x)))
                result.AddRange(i);
            foreach (var i in obj.fULongArray.Select(x => BitConverter.GetBytes(x)))
                result.AddRange(i);
            foreach (var i in obj.fShortArray.Select(x => BitConverter.GetBytes(x)))
                result.AddRange(i);
            foreach (var i in obj.fUShortArray.Select(x => BitConverter.GetBytes(x)))
                result.AddRange(i);
            foreach (var i in obj.fFloatArray.Select(x => BitConverter.GetBytes(x)))
                result.AddRange(i);
            foreach (var i in obj.fDoubleArray.Select(x => BitConverter.GetBytes(x)))
                result.AddRange(i);
            foreach (var i in obj.fCharArray.Select(x => BitConverter.GetBytes(x)))
                result.AddRange(i);
            foreach (var i in obj.fBoolArray.Select(x => BitConverter.GetBytes(x)))
                result.AddRange(i);

            foreach (var i in obj.pIntArray.Select(x => BitConverter.GetBytes(x)))
                result.AddRange(i);
            foreach (var i in obj.pUIntArray.Select(x => BitConverter.GetBytes(x)))
                result.AddRange(i);
            foreach (var i in obj.pLongArray.Select(x => BitConverter.GetBytes(x)))
                result.AddRange(i);
            foreach (var i in obj.pULongArray.Select(x => BitConverter.GetBytes(x)))
                result.AddRange(i);
            foreach (var i in obj.pShortArray.Select(x => BitConverter.GetBytes(x)))
                result.AddRange(i);
            foreach (var i in obj.pUShortArray.Select(x => BitConverter.GetBytes(x)))
                result.AddRange(i);
            foreach (var i in obj.pFloatArray.Select(x => BitConverter.GetBytes(x)))
                result.AddRange(i);
            foreach (var i in obj.pDoubleArray.Select(x => BitConverter.GetBytes(x)))
                result.AddRange(i);
            foreach (var i in obj.pCharArray.Select(x => BitConverter.GetBytes(x)))
                result.AddRange(i);
            foreach (var i in obj.pBoolArray.Select(x => BitConverter.GetBytes(x)))
                result.AddRange(i);
            return result;
        }

        public static IEnumerable<byte> GetATestArrayValueObjectMemeryLayoutBigEndian()
        {
            var obj = InitATestArrayValueObject();
            List<byte> result = new List<byte>();
            foreach (var i in obj.fIntArray.Select(x => BytesHelper.GetBytes(x, true)))
                result.AddRange(i);
            foreach (var i in obj.fUIntArray.Select(x => BytesHelper.GetBytes(x, true)))
                result.AddRange(i);
            foreach (var i in obj.fLongArray.Select(x => BytesHelper.GetBytes(x, true)))
                result.AddRange(i);
            foreach (var i in obj.fULongArray.Select(x => BytesHelper.GetBytes(x, true)))
                result.AddRange(i);
            foreach (var i in obj.fShortArray.Select(x => BytesHelper.GetBytes(x, true)))
                result.AddRange(i);
            foreach (var i in obj.fUShortArray.Select(x => BytesHelper.GetBytes(x, true)))
                result.AddRange(i);
            foreach (var i in obj.fFloatArray.Select(x => BytesHelper.GetBytes(x, true)))
                result.AddRange(i);
            foreach (var i in obj.fDoubleArray.Select(x => BytesHelper.GetBytes(x, true)))
                result.AddRange(i);
            foreach (var i in obj.fCharArray.Select(x => BytesHelper.GetBytes(x, true)))
                result.AddRange(i);
            foreach (var i in obj.fBoolArray.Select(x => BytesHelper.GetBytes(x, true)))
                result.AddRange(i);

            foreach (var i in obj.pIntArray.Select(x => BytesHelper.GetBytes(x, true)))
                result.AddRange(i);
            foreach (var i in obj.pUIntArray.Select(x => BytesHelper.GetBytes(x, true)))
                result.AddRange(i);
            foreach (var i in obj.pLongArray.Select(x => BytesHelper.GetBytes(x, true)))
                result.AddRange(i);
            foreach (var i in obj.pULongArray.Select(x => BytesHelper.GetBytes(x, true)))
                result.AddRange(i);
            foreach (var i in obj.pShortArray.Select(x => BytesHelper.GetBytes(x, true)))
                result.AddRange(i);
            foreach (var i in obj.pUShortArray.Select(x => BytesHelper.GetBytes(x, true)))
                result.AddRange(i);
            foreach (var i in obj.pFloatArray.Select(x => BytesHelper.GetBytes(x, true)))
                result.AddRange(i);
            foreach (var i in obj.pDoubleArray.Select(x => BytesHelper.GetBytes(x, true)))
                result.AddRange(i);
            foreach (var i in obj.pCharArray.Select(x => BytesHelper.GetBytes(x, true)))
                result.AddRange(i);
            foreach (var i in obj.pBoolArray.Select(x => BytesHelper.GetBytes(x, true)))
                result.AddRange(i);
            return result;
        }
    }

}
