using System;
using System.Collections.Generic;
using System.Text;
using TPSLRawDataSimulator;
using System.Runtime.InteropServices;
using System.Linq;
using System.Globalization;
using System.ComponentModel;

namespace UnitTestProject1
{
    public struct TestArray
    {
        public int[] IA1 { get; set; }
        public uint[] UA2;
    }
    public struct NormalValueObject
    {
        [MemberIndex(Index = 9)]
        public byte fByte;
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

        [MemberIndex(Index = 109)]
        public byte pByte { get; set; }
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
        [MemberIndex(Index = 7, LengthTo = "fByteArray")]
        public long fByteArrayLength;
        [MemberIndex(Index = 8)]
        public byte[] fByteArray;
        [MemberIndex(Index = 9, LengthTo = "fIntArray")]
        public long fIntArrayLength;
        [MemberIndex(Index = 10)]
        public int[] fIntArray;
        [MemberIndex(Index = 19, LengthTo = "fUIntArray")]
        public long fUIntArrayLength;
        [MemberIndex(Index = 20)]
        public uint[] fUIntArray;
        [MemberIndex(Index = 29, LengthTo = "fLongArray")]
        public long fLongArrayLength;
        [MemberIndex(Index = 30)]
        public long[] fLongArray;
        [MemberIndex(Index = 39, LengthTo = "fULongArray")]
        public long fULongArrayLength;
        [MemberIndex(Index = 40)]
        public ulong[] fULongArray;
        [MemberIndex(Index = 49, LengthTo = "fShortArray")]
        public long fShortArrayLength;
        [MemberIndex(Index = 50)]
        public short[] fShortArray;
        [MemberIndex(Index = 59, LengthTo = "fUShortArray")]
        public long fUShortArrayLength;
        [MemberIndex(Index = 60)]
        public ushort[] fUShortArray;
        [MemberIndex(Index = 69, LengthTo = "fFloatArray")]
        public long fFloatArrayLength;
        [MemberIndex(Index = 70)]
        public float[] fFloatArray;
        [MemberIndex(Index = 79, LengthTo = "fDoubleArray")]
        public long fDoubleArrayLength;
        [MemberIndex(Index = 80)]
        public double[] fDoubleArray;
        [MemberIndex(Index = 89, LengthTo = "fCharArray")]
        public long fCharArrayLength;
        [MemberIndex(Index = 90)]
        public char[] fCharArray;
        [MemberIndex(Index = 99, LengthTo = "fBoolArray")]
        public long fBoolArrayLength;
        [MemberIndex(Index = 100)]
        public bool[] fBoolArray;

        [MemberIndex(Index = 107, LengthTo = "pByteArray")]
        public long pByteArrayLength;
        [MemberIndex(Index = 108)]
        public byte[] pByteArray { get; set; }
        [MemberIndex(Index = 109, LengthTo = "pIntArray")]
        public long pIntArrayLength;
        [MemberIndex(Index = 110)]
        public int[] pIntArray { get; set; }
        [MemberIndex(Index = 119, LengthTo = "pUIntArray")]
        public long pUIntArrayLength;
        [MemberIndex(Index = 120)]
        public uint[] pUIntArray { get; set; }
        [MemberIndex(Index = 129, LengthTo = "pLongArray")]
        public long pLongArrayLength;
        [MemberIndex(Index = 130)]
        public long[] pLongArray { get; set; }
        [MemberIndex(Index = 139, LengthTo = "pULongArray")]
        public long pULongArrayLength;
        [MemberIndex(Index = 140)]
        public ulong[] pULongArray { get; set; }
        [MemberIndex(Index = 149, LengthTo = "pShortArray")]
        public long pShortArrayLength;
        [MemberIndex(Index = 150)]
        public short[] pShortArray { get; set; }
        [MemberIndex(Index = 159, LengthTo = "pUShortArray")]
        public long pUShortArrayLength;
        [MemberIndex(Index = 160)]
        public ushort[] pUShortArray { get; set; }
        [MemberIndex(Index = 169, LengthTo = "pFloatArray")]
        public long pFloatArrayLength;
        [MemberIndex(Index = 170)]
        public float[] pFloatArray { get; set; }
        [MemberIndex(Index = 179, LengthTo = "pDoubleArray")]
        public long pDoubleArrayLength;
        [MemberIndex(Index = 180)]
        public double[] pDoubleArray { get; set; }
        [MemberIndex(Index = 189, LengthTo = "pCharArray")]
        public long pCharArrayLength;
        [MemberIndex(Index = 190)]
        public char[] pCharArray { get; set; }
        [MemberIndex(Index = 199, LengthTo = "pBoolArray")]
        public long pBoolArrayLength;
        [MemberIndex(Index = 200)]
        public bool[] pBoolArray { get; set; }

        public override bool Equals(object obj)
        {
            if (GetType() != obj.GetType())
            {
                return false;
            }

            var unboxedObj = (ArrayValueObject)obj;
            if (!(fByteArrayLength == unboxedObj.fByteArrayLength))
            {
                return false;
            }

            if (!Enumerable.SequenceEqual(fByteArray, unboxedObj.fByteArray))
            {
                return false;
            }

            if (!(fIntArrayLength == unboxedObj.fIntArrayLength))
            {
                return false;
            }

            if (!Enumerable.SequenceEqual(fIntArray, unboxedObj.fIntArray))
            {
                return false;
            }

            if (!(fUIntArrayLength == unboxedObj.fUIntArrayLength))
            {
                return false;
            }

            if (!Enumerable.SequenceEqual(fUIntArray, unboxedObj.fUIntArray))
            {
                return false;
            }

            if (!(fLongArrayLength == unboxedObj.fLongArrayLength))
            {
                return false;
            }

            if (!Enumerable.SequenceEqual(fLongArray, unboxedObj.fLongArray))
            {
                return false;
            }

            if (!(fULongArrayLength == unboxedObj.fULongArrayLength))
            {
                return false;
            }

            if (!Enumerable.SequenceEqual(fULongArray, unboxedObj.fULongArray))
            {
                return false;
            }

            if (!(fShortArrayLength == unboxedObj.fShortArrayLength))
            {
                return false;
            }

            if (!Enumerable.SequenceEqual(fShortArray, unboxedObj.fShortArray))
            {
                return false;
            }

            if (!(fUShortArrayLength == unboxedObj.fUShortArrayLength))
            {
                return false;
            }

            if (!Enumerable.SequenceEqual(fUShortArray, unboxedObj.fUShortArray))
            {
                return false;
            }

            if (!(fFloatArrayLength == unboxedObj.fFloatArrayLength))
            {
                return false;
            }

            if (!Enumerable.SequenceEqual(fFloatArray, unboxedObj.fFloatArray))
            {
                return false;
            }

            if (!(fDoubleArrayLength == unboxedObj.fDoubleArrayLength))
            {
                return false;
            }

            if (!Enumerable.SequenceEqual(fDoubleArray, unboxedObj.fDoubleArray))
            {
                return false;
            }

            if (!(fCharArrayLength == unboxedObj.fCharArrayLength))
            {
                return false;
            }

            if (!Enumerable.SequenceEqual(fCharArray, unboxedObj.fCharArray))
            {
                return false;
            }

            if (!(fBoolArrayLength == unboxedObj.fBoolArrayLength))
            {
                return false;
            }

            if (!Enumerable.SequenceEqual(fBoolArray, unboxedObj.fBoolArray))
            {
                return false;
            }

            if (!(pByteArrayLength == unboxedObj.pByteArrayLength))
            {
                return false;
            }

            if (!Enumerable.SequenceEqual(pByteArray, unboxedObj.pByteArray))
            {
                return false;
            }

            if (!(pIntArrayLength == unboxedObj.pIntArrayLength))
            {
                return false;
            }

            if (!Enumerable.SequenceEqual(pIntArray, unboxedObj.pIntArray))
            {
                return false;
            }

            if (!(pUIntArrayLength == unboxedObj.pUIntArrayLength))
            {
                return false;
            }

            if (!Enumerable.SequenceEqual(pUIntArray, unboxedObj.pUIntArray))
            {
                return false;
            }

            if (!(pLongArrayLength == unboxedObj.pLongArrayLength))
            {
                return false;
            }

            if (!Enumerable.SequenceEqual(pLongArray, unboxedObj.pLongArray))
            {
                return false;
            }

            if (!(pULongArrayLength == unboxedObj.pULongArrayLength))
            {
                return false;
            }

            if (!Enumerable.SequenceEqual(pULongArray, unboxedObj.pULongArray))
            {
                return false;
            }

            if (!(pShortArrayLength == unboxedObj.pShortArrayLength))
            {
                return false;
            }

            if (!Enumerable.SequenceEqual(pShortArray, unboxedObj.pShortArray))
            {
                return false;
            }

            if (!(pUShortArrayLength == unboxedObj.pUShortArrayLength))
            {
                return false;
            }

            if (!Enumerable.SequenceEqual(pUShortArray, unboxedObj.pUShortArray))
            {
                return false;
            }

            if (!(pFloatArrayLength == unboxedObj.pFloatArrayLength))
            {
                return false;
            }

            if (!Enumerable.SequenceEqual(pFloatArray, unboxedObj.pFloatArray))
            {
                return false;
            }

            if (!(pDoubleArrayLength == unboxedObj.pDoubleArrayLength))
            {
                return false;
            }

            if (!Enumerable.SequenceEqual(pDoubleArray, unboxedObj.pDoubleArray))
            {
                return false;
            }

            if (!(pCharArrayLength == unboxedObj.pCharArrayLength))
            {
                return false;
            }

            if (!Enumerable.SequenceEqual(pCharArray, unboxedObj.pCharArray))
            {
                return false;
            }

            if (!(pBoolArrayLength == unboxedObj.pBoolArrayLength))
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
        [MemberIndex(Index = 7, LengthTo = "fByteArray")]
        public long fByteArrayLength;
        [MemberIndex(Index = 8)]
        public byte[] fByteArray;
        [MemberIndex(Index = 9, LengthTo = "fIntArray")]
        public long fIntArrayLength;
        [MemberIndex(Index = 10)]
        public int[] fIntArray;
        [MemberIndex(Index = 19, LengthTo = "fUIntArray")]
        public long fUIntArrayLength;
        [MemberIndex(Index = 20)]
        public uint[] fUIntArray;
        [MemberIndex(Index = 29, LengthTo = "fLongArray")]
        public long fLongArrayLength;
        [MemberIndex(Index = 30)]
        public long[] fLongArray;
        [MemberIndex(Index = 39, LengthTo = "fULongArray")]
        public long fULongArrayLength;
        [MemberIndex(Index = 40)]
        public ulong[] fULongArray;
        [MemberIndex(Index = 49, LengthTo = "fShortArray")]
        public long fShortArrayLength;
        [MemberIndex(Index = 50)]
        public short[] fShortArray;
        [MemberIndex(Index = 59, LengthTo = "fUShortArray")]
        public long fUShortArrayLength;
        [MemberIndex(Index = 60)]
        public ushort[] fUShortArray;
        [MemberIndex(Index = 69, LengthTo = "fFloatArray")]
        public long fFloatArrayLength;
        [MemberIndex(Index = 70)]
        public float[] fFloatArray;
        [MemberIndex(Index = 79, LengthTo = "fDoubleArray")]
        public long fDoubleArrayLength;
        [MemberIndex(Index = 80)]
        public double[] fDoubleArray;
        [MemberIndex(Index = 89, LengthTo = "fCharArray")]
        public long fCharArrayLength;
        [MemberIndex(Index = 90)]
        public char[] fCharArray;
        [MemberIndex(Index = 99, LengthTo = "fBoolArray")]
        public long fBoolArrayLength;
        [MemberIndex(Index = 100)]
        public bool[] fBoolArray;

        [MemberIndex(Index = 107, LengthTo = "pByteArray")]
        public long pByteArrayLength;
        [MemberIndex(Index = 108)]
        public byte[] pByteArray { get; set; }
        [MemberIndex(Index = 109, LengthTo = "pIntArray")]
        public long pIntArrayLength;
        [MemberIndex(Index = 110)]
        public int[] pIntArray { get; set; }
        [MemberIndex(Index = 119, LengthTo = "pUIntArray")]
        public long pUIntArrayLength;
        [MemberIndex(Index = 120)]
        public uint[] pUIntArray { get; set; }
        [MemberIndex(Index = 129, LengthTo = "pLongArray")]
        public long pLongArrayLength;
        [MemberIndex(Index = 130)]
        public long[] pLongArray { get; set; }
        [MemberIndex(Index = 139, LengthTo = "pULongArray")]
        public long pULongArrayLength;
        [MemberIndex(Index = 140)]
        public ulong[] pULongArray { get; set; }
        [MemberIndex(Index = 149, LengthTo = "pShortArray")]
        public long pShortArrayLength;
        [MemberIndex(Index = 150)]
        public short[] pShortArray { get; set; }
        [MemberIndex(Index = 159, LengthTo = "pUShortArray")]
        public long pUShortArrayLength;
        [MemberIndex(Index = 160)]
        public ushort[] pUShortArray { get; set; }
        [MemberIndex(Index = 169, LengthTo = "pFloatArray")]
        public long pFloatArrayLength;
        [MemberIndex(Index = 170)]
        public float[] pFloatArray { get; set; }
        [MemberIndex(Index = 179, LengthTo = "pDoubleArray")]
        public long pDoubleArrayLength;
        [MemberIndex(Index = 180)]
        public double[] pDoubleArray { get; set; }
        [MemberIndex(Index = 189, LengthTo = "pCharArray")]
        public long pCharArrayLength;
        [MemberIndex(Index = 190)]
        public char[] pCharArray { get; set; }
        [MemberIndex(Index = 199, LengthTo = "pBoolArray")]
        public long pBoolArrayLength;
        [MemberIndex(Index = 200)]
        public bool[] pBoolArray { get; set; }


        public override bool Equals(object obj)
        {
            if (GetType() != obj.GetType())
            {
                return false;
            }

            var unboxedObj = (ArrayValueSpecifiedEndianObject)obj;
            if (!(fByteArrayLength == unboxedObj.fByteArrayLength))
            {
                return false;
            }

            if (!Enumerable.SequenceEqual(fByteArray, unboxedObj.fByteArray))
            {
                return false;
            }

            if (!(fIntArrayLength == unboxedObj.fIntArrayLength))
            {
                return false;
            }

            if (!Enumerable.SequenceEqual(fIntArray, unboxedObj.fIntArray))
            {
                return false;
            }

            if (!(fUIntArrayLength == unboxedObj.fUIntArrayLength))
            {
                return false;
            }

            if (!Enumerable.SequenceEqual(fUIntArray, unboxedObj.fUIntArray))
            {
                return false;
            }

            if (!(fLongArrayLength == unboxedObj.fLongArrayLength))
            {
                return false;
            }

            if (!Enumerable.SequenceEqual(fLongArray, unboxedObj.fLongArray))
            {
                return false;
            }

            if (!(fULongArrayLength == unboxedObj.fULongArrayLength))
            {
                return false;
            }

            if (!Enumerable.SequenceEqual(fULongArray, unboxedObj.fULongArray))
            {
                return false;
            }

            if (!(fShortArrayLength == unboxedObj.fShortArrayLength))
            {
                return false;
            }

            if (!Enumerable.SequenceEqual(fShortArray, unboxedObj.fShortArray))
            {
                return false;
            }

            if (!(fUShortArrayLength == unboxedObj.fUShortArrayLength))
            {
                return false;
            }

            if (!Enumerable.SequenceEqual(fUShortArray, unboxedObj.fUShortArray))
            {
                return false;
            }

            if (!(fFloatArrayLength == unboxedObj.fFloatArrayLength))
            {
                return false;
            }

            if (!Enumerable.SequenceEqual(fFloatArray, unboxedObj.fFloatArray))
            {
                return false;
            }

            if (!(fDoubleArrayLength == unboxedObj.fDoubleArrayLength))
            {
                return false;
            }

            if (!Enumerable.SequenceEqual(fDoubleArray, unboxedObj.fDoubleArray))
            {
                return false;
            }

            if (!(fCharArrayLength == unboxedObj.fCharArrayLength))
            {
                return false;
            }

            if (!Enumerable.SequenceEqual(fCharArray, unboxedObj.fCharArray))
            {
                return false;
            }

            if (!(fBoolArrayLength == unboxedObj.fBoolArrayLength))
            {
                return false;
            }

            if (!Enumerable.SequenceEqual(fBoolArray, unboxedObj.fBoolArray))
            {
                return false;
            }

            if (!(pByteArrayLength == unboxedObj.pByteArrayLength))
            {
                return false;
            }

            if (!Enumerable.SequenceEqual(pByteArray, unboxedObj.pByteArray))
            {
                return false;
            }

            if (!(pIntArrayLength == unboxedObj.pIntArrayLength))
            {
                return false;
            }

            if (!Enumerable.SequenceEqual(pIntArray, unboxedObj.pIntArray))
            {
                return false;
            }

            if (!(pUIntArrayLength == unboxedObj.pUIntArrayLength))
            {
                return false;
            }

            if (!Enumerable.SequenceEqual(pUIntArray, unboxedObj.pUIntArray))
            {
                return false;
            }

            if (!(pLongArrayLength == unboxedObj.pLongArrayLength))
            {
                return false;
            }

            if (!Enumerable.SequenceEqual(pLongArray, unboxedObj.pLongArray))
            {
                return false;
            }

            if (!(pULongArrayLength == unboxedObj.pULongArrayLength))
            {
                return false;
            }

            if (!Enumerable.SequenceEqual(pULongArray, unboxedObj.pULongArray))
            {
                return false;
            }

            if (!(pShortArrayLength == unboxedObj.pShortArrayLength))
            {
                return false;
            }

            if (!Enumerable.SequenceEqual(pShortArray, unboxedObj.pShortArray))
            {
                return false;
            }

            if (!(pUShortArrayLength == unboxedObj.pUShortArrayLength))
            {
                return false;
            }

            if (!Enumerable.SequenceEqual(pUShortArray, unboxedObj.pUShortArray))
            {
                return false;
            }

            if (!(pFloatArrayLength == unboxedObj.pFloatArrayLength))
            {
                return false;
            }

            if (!Enumerable.SequenceEqual(pFloatArray, unboxedObj.pFloatArray))
            {
                return false;
            }

            if (!(pDoubleArrayLength == unboxedObj.pDoubleArrayLength))
            {
                return false;
            }

            if (!Enumerable.SequenceEqual(pDoubleArray, unboxedObj.pDoubleArray))
            {
                return false;
            }

            if (!(pCharArrayLength == unboxedObj.pCharArrayLength))
            {
                return false;
            }

            if (!Enumerable.SequenceEqual(pCharArray, unboxedObj.pCharArray))
            {
                return false;
            }

            if (!(pBoolArrayLength == unboxedObj.pBoolArrayLength))
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

    public sealed class ObjectsInitializer
    {

        public static NormalValueObject InitATestPureValueObject()
        {
            NormalValueObject obj = new NormalValueObject();
            obj.fByte = 0xFF;
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

            obj.pByte = 0x11;
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
            result.fByteArray = new byte[] { 0x11, 0x22, 0x33, 0x44, 0x55 };
            result.fByteArrayLength = BytesHelper.GetBytesCountOfArray(result.fByteArray);
            result.fIntArray = new int[] { 0x11111110, 0x22222220, 0x33333330, 0x44444440, 0x55555550 };
            result.fIntArrayLength = BytesHelper.GetBytesCountOfArray(result.fIntArray);
            result.fUIntArray = new uint[] { 0x11111111, 0x22222221, 0x33333331, 0x44444441, 0x55555551 };
            result.fUIntArrayLength = BytesHelper.GetBytesCountOfArray(result.fUIntArray);
            result.fLongArray = new long[] { 0x111111111111110, 0x2222222222222220, 0x3333333333333330, 0x4444444444444440, 0x5555555555555550 };
            result.fLongArrayLength = BytesHelper.GetBytesCountOfArray(result.fLongArray);
            result.fULongArray = new ulong[] { 0x111111111111111, 0x2222222222222221, 0x3333333333333331, 0x4444444444444441, 0x5555555555555551 };
            result.fULongArrayLength = BytesHelper.GetBytesCountOfArray(result.fULongArray);
            result.fShortArray = new short[] { 0x1110, 0x2220, 0x3330, 0x4440, 0x5550 };
            result.fShortArrayLength = BytesHelper.GetBytesCountOfArray(result.fShortArray);
            result.fUShortArray = new ushort[] { 0x1110, 0x2220, 0x3330, 0x4440, 0x5550 };
            result.fUShortArrayLength = BytesHelper.GetBytesCountOfArray(result.fUShortArray);
            result.fFloatArray = new float[] { 1235.5f };
            result.fFloatArrayLength = BytesHelper.GetBytesCountOfArray(result.fFloatArray);
            result.fDoubleArray = new double[] { 12345.5d };
            result.fDoubleArrayLength = BytesHelper.GetBytesCountOfArray(result.fDoubleArray);
            result.fCharArray = "abcde".ToArray();
            result.fCharArrayLength = BytesHelper.GetBytesCountOfArray(result.fCharArray);
            result.fBoolArray = new bool[] { true, false, true, false };
            result.fBoolArrayLength = BytesHelper.GetBytesCountOfArray(result.fBoolArray);

            result.pByteArray = new byte[] { 0x55, 0x44, 0x33, 0x22, 0x11 };
            result.pByteArrayLength = BytesHelper.GetBytesCountOfArray(result.pByteArray);
            result.pIntArray = new int[] { 0x11111110, 0x22222220, 0x33333330, 0x44444440, 0x55555550 };
            result.pIntArrayLength = BytesHelper.GetBytesCountOfArray(result.pIntArray);
            result.pUIntArray = new uint[] { 0x11111111, 0x22222221, 0x33333331, 0x44444441, 0x55555551 };
            result.pUIntArrayLength = BytesHelper.GetBytesCountOfArray(result.pUIntArray);
            result.pLongArray = new long[] { 0x111111111111110, 0x2222222222222220, 0x3333333333333330, 0x4444444444444440, 0x5555555555555550 };
            result.pLongArrayLength = BytesHelper.GetBytesCountOfArray(result.pLongArray);
            result.pULongArray = new ulong[] { 0x111111111111111, 0x2222222222222221, 0x3333333333333331, 0x4444444444444441, 0x5555555555555551 };
            result.pULongArrayLength = BytesHelper.GetBytesCountOfArray(result.pULongArray);
            result.pShortArray = new short[] { 0x1110, 0x2220, 0x3330, 0x4440, 0x5550 };
            result.pShortArrayLength = BytesHelper.GetBytesCountOfArray(result.pShortArray);
            result.pUShortArray = new ushort[] { 0x1110, 0x2220, 0x3330, 0x4440, 0x5550 };
            result.pUShortArrayLength = BytesHelper.GetBytesCountOfArray(result.pUShortArray);
            result.pFloatArray = new float[] { 1235.5f };
            result.pFloatArrayLength = BytesHelper.GetBytesCountOfArray(result.pFloatArray);
            result.pDoubleArray = new double[] { 12345.5d };
            result.pDoubleArrayLength = BytesHelper.GetBytesCountOfArray(result.pDoubleArray);
            result.pCharArray = "edcba".ToArray();
            result.pCharArrayLength = BytesHelper.GetBytesCountOfArray(result.pCharArray);
            result.pBoolArray = new bool[] { false, true, false, true };
            result.pBoolArrayLength = BytesHelper.GetBytesCountOfArray(result.pBoolArray);

            return result;
        }

        public static ArrayValueSpecifiedEndianObject InitATestArraySpecifiedEndianValueObject()
        {
            var result = new ArrayValueSpecifiedEndianObject();

            result.fByteArray = new byte[] { 0x11, 0x22, 0x33, 0x44, 0xFF };
            result.fByteArrayLength = BytesHelper.GetBytesCountOfArray(result.fByteArray);
            result.fIntArray = new int[] { 0x11111110, 0x22222220, 0x33333330, 0x44444440, 0x55555550 };
            result.fIntArrayLength = BytesHelper.GetBytesCountOfArray(result.fIntArray);
            result.fUIntArray = new uint[] { 0x11111111, 0x22222221, 0x33333331, 0x44444441, 0x55555551 };
            result.fUIntArrayLength = BytesHelper.GetBytesCountOfArray(result.fUIntArray);
            result.fLongArray = new long[] { 0x111111111111110, 0x2222222222222220, 0x3333333333333330, 0x4444444444444440, 0x5555555555555550 };
            result.fLongArrayLength = BytesHelper.GetBytesCountOfArray(result.fLongArray);
            result.fULongArray = new ulong[] { 0x111111111111111, 0x2222222222222221, 0x3333333333333331, 0x4444444444444441, 0x5555555555555551 };
            result.fULongArrayLength = BytesHelper.GetBytesCountOfArray(result.fULongArray);
            result.fShortArray = new short[] { 0x1110, 0x2220, 0x3330, 0x4440, 0x5550 };
            result.fShortArrayLength = BytesHelper.GetBytesCountOfArray(result.fShortArray);
            result.fUShortArray = new ushort[] { 0x1110, 0x2220, 0x3330, 0x4440, 0x5550 };
            result.fUShortArrayLength = BytesHelper.GetBytesCountOfArray(result.fUShortArray);
            result.fFloatArray = new float[] { 1235.5f };
            result.fFloatArrayLength = BytesHelper.GetBytesCountOfArray(result.fFloatArray);
            result.fDoubleArray = new double[] { 12345.5d };
            result.fDoubleArrayLength = BytesHelper.GetBytesCountOfArray(result.fDoubleArray);
            result.fCharArray = "abcde".ToArray();
            result.fCharArrayLength = BytesHelper.GetBytesCountOfArray(result.fCharArray);
            result.fBoolArray = new bool[] { true, false, true, false };
            result.fBoolArrayLength = BytesHelper.GetBytesCountOfArray(result.fBoolArray);

            result.pByteArray = new byte[] { 0xFF, 0x44, 0x33, 0x22, 0x11 };
            result.pByteArrayLength = BytesHelper.GetBytesCountOfArray(result.pByteArray);
            result.pIntArray = new int[] { 0x11111110, 0x22222220, 0x33333330, 0x44444440, 0x55555550 };
            result.pIntArrayLength = BytesHelper.GetBytesCountOfArray(result.pIntArray);
            result.pUIntArray = new uint[] { 0x11111111, 0x22222221, 0x33333331, 0x44444441, 0x55555551 };
            result.pUIntArrayLength = BytesHelper.GetBytesCountOfArray(result.pUIntArray);
            result.pLongArray = new long[] { 0x111111111111110, 0x2222222222222220, 0x3333333333333330, 0x4444444444444440, 0x5555555555555550 };
            result.pLongArrayLength = BytesHelper.GetBytesCountOfArray(result.pLongArray);
            result.pULongArray = new ulong[] { 0x111111111111111, 0x2222222222222221, 0x3333333333333331, 0x4444444444444441, 0x5555555555555551 };
            result.pULongArrayLength = BytesHelper.GetBytesCountOfArray(result.pULongArray);
            result.pShortArray = new short[] { 0x1110, 0x2220, 0x3330, 0x4440, 0x5550 };
            result.pShortArrayLength = BytesHelper.GetBytesCountOfArray(result.pShortArray);
            result.pUShortArray = new ushort[] { 0x1110, 0x2220, 0x3330, 0x4440, 0x5550 };
            result.pUShortArrayLength = BytesHelper.GetBytesCountOfArray(result.pUShortArray);
            result.pFloatArray = new float[] { 1235.5f };
            result.pFloatArrayLength = BytesHelper.GetBytesCountOfArray(result.pFloatArray);
            result.pDoubleArray = new double[] { 12345.5d };
            result.pDoubleArrayLength = BytesHelper.GetBytesCountOfArray(result.pDoubleArray);
            result.pCharArray = "edcba".ToArray();
            result.pCharArrayLength = BytesHelper.GetBytesCountOfArray(result.pCharArray);
            result.pBoolArray = new bool[] { false, true, false, true };
            result.pBoolArrayLength = BytesHelper.GetBytesCountOfArray(result.pBoolArray);

            return result;
        }

        public static IEnumerable<byte> GetATestPureValueObjectMemoryLayout()
        {
            List<byte> result = new List<byte>();
            var resultObj = InitATestPureValueObject();
            result.AddRange(BytesHelper.GetBytes(resultObj.fByte, !BitConverter.IsLittleEndian));
            result.AddRange(BytesHelper.GetBytes(resultObj.fInt, !BitConverter.IsLittleEndian));
            result.AddRange(BytesHelper.GetBytes(resultObj.fUInt, !BitConverter.IsLittleEndian));
            result.AddRange(BytesHelper.GetBytes(resultObj.fLong, !BitConverter.IsLittleEndian));
            result.AddRange(BytesHelper.GetBytes(resultObj.fULong, !BitConverter.IsLittleEndian));
            result.AddRange(BytesHelper.GetBytes(resultObj.fShort, !BitConverter.IsLittleEndian));
            result.AddRange(BytesHelper.GetBytes(resultObj.fUShort, !BitConverter.IsLittleEndian));
            result.AddRange(BytesHelper.GetBytes(resultObj.fFloat, !BitConverter.IsLittleEndian));
            result.AddRange(BytesHelper.GetBytes(resultObj.fDouble, !BitConverter.IsLittleEndian));
            result.AddRange(BytesHelper.GetBytes(resultObj.fChar, !BitConverter.IsLittleEndian));
            result.AddRange(BytesHelper.GetBytes(resultObj.fBool, !BitConverter.IsLittleEndian));

            result.AddRange(BytesHelper.GetBytes(resultObj.pByte, !BitConverter.IsLittleEndian));
            result.AddRange(BytesHelper.GetBytes(resultObj.pInt, !BitConverter.IsLittleEndian));
            result.AddRange(BytesHelper.GetBytes(resultObj.pUInt, !BitConverter.IsLittleEndian));
            result.AddRange(BytesHelper.GetBytes(resultObj.pLong, !BitConverter.IsLittleEndian));
            result.AddRange(BytesHelper.GetBytes(resultObj.pULong, !BitConverter.IsLittleEndian));
            result.AddRange(BytesHelper.GetBytes(resultObj.pShort, !BitConverter.IsLittleEndian));
            result.AddRange(BytesHelper.GetBytes(resultObj.pUShort, !BitConverter.IsLittleEndian));
            result.AddRange(BytesHelper.GetBytes(resultObj.pFloat, !BitConverter.IsLittleEndian));
            result.AddRange(BytesHelper.GetBytes(resultObj.pDouble, !BitConverter.IsLittleEndian));
            result.AddRange(BytesHelper.GetBytes(resultObj.pChar, !BitConverter.IsLittleEndian));
            result.AddRange(BytesHelper.GetBytes(resultObj.pBool, !BitConverter.IsLittleEndian));
            return result;

        }

        public static IEnumerable<byte> GetATestArrayValueObjectMemeryLayout()
        {
            var obj = InitATestArrayValueObject();
            List<byte> result = new List<byte>();
            result.AddRange(BitConverter.GetBytes(obj.fByteArrayLength));
            foreach (var i in obj.fByteArray)
                result.Add(i);
            result.AddRange(BitConverter.GetBytes(obj.fIntArrayLength));
            foreach (var i in obj.fIntArray.Select(x => BitConverter.GetBytes(x)))
                result.AddRange(i);
            result.AddRange(BitConverter.GetBytes(obj.fUIntArrayLength));
            foreach (var i in obj.fUIntArray.Select(x => BitConverter.GetBytes(x)))
                result.AddRange(i);
            result.AddRange(BitConverter.GetBytes(obj.fLongArrayLength));
            foreach (var i in obj.fLongArray.Select(x => BitConverter.GetBytes(x)))
                result.AddRange(i);
            result.AddRange(BitConverter.GetBytes(obj.fULongArrayLength));
            foreach (var i in obj.fULongArray.Select(x => BitConverter.GetBytes(x)))
                result.AddRange(i);
            result.AddRange(BitConverter.GetBytes(obj.fShortArrayLength));
            foreach (var i in obj.fShortArray.Select(x => BitConverter.GetBytes(x)))
                result.AddRange(i);
            result.AddRange(BitConverter.GetBytes(obj.fUShortArrayLength));
            foreach (var i in obj.fUShortArray.Select(x => BitConverter.GetBytes(x)))
                result.AddRange(i);
            result.AddRange(BitConverter.GetBytes(obj.fFloatArrayLength));
            foreach (var i in obj.fFloatArray.Select(x => BitConverter.GetBytes(x)))
                result.AddRange(i);
            result.AddRange(BitConverter.GetBytes(obj.fDoubleArrayLength));
            foreach (var i in obj.fDoubleArray.Select(x => BitConverter.GetBytes(x)))
                result.AddRange(i);
            result.AddRange(BitConverter.GetBytes(obj.fCharArrayLength));
            foreach (var i in obj.fCharArray.Select(x => BitConverter.GetBytes(x)))
                result.AddRange(i);
            result.AddRange(BitConverter.GetBytes(obj.fBoolArrayLength));
            foreach (var i in obj.fBoolArray.Select(x => BitConverter.GetBytes(x)))
                result.AddRange(i);

            result.AddRange(BitConverter.GetBytes(obj.pByteArrayLength));
            foreach (var i in obj.pByteArray)
                result.Add(i);
            result.AddRange(BitConverter.GetBytes(obj.pIntArrayLength));
            foreach (var i in obj.pIntArray.Select(x => BitConverter.GetBytes(x)))
                result.AddRange(i);
            result.AddRange(BitConverter.GetBytes(obj.pUIntArrayLength));
            foreach (var i in obj.pUIntArray.Select(x => BitConverter.GetBytes(x)))
                result.AddRange(i);
            result.AddRange(BitConverter.GetBytes(obj.pLongArrayLength));
            foreach (var i in obj.pLongArray.Select(x => BitConverter.GetBytes(x)))
                result.AddRange(i);
            result.AddRange(BitConverter.GetBytes(obj.pULongArrayLength));
            foreach (var i in obj.pULongArray.Select(x => BitConverter.GetBytes(x)))
                result.AddRange(i);
            result.AddRange(BitConverter.GetBytes(obj.pShortArrayLength));
            foreach (var i in obj.pShortArray.Select(x => BitConverter.GetBytes(x)))
                result.AddRange(i);
            result.AddRange(BitConverter.GetBytes(obj.pUShortArrayLength));
            foreach (var i in obj.pUShortArray.Select(x => BitConverter.GetBytes(x)))
                result.AddRange(i);
            result.AddRange(BitConverter.GetBytes(obj.pFloatArrayLength));
            foreach (var i in obj.pFloatArray.Select(x => BitConverter.GetBytes(x)))
                result.AddRange(i);
            result.AddRange(BitConverter.GetBytes(obj.pDoubleArrayLength));
            foreach (var i in obj.pDoubleArray.Select(x => BitConverter.GetBytes(x)))
                result.AddRange(i);
            result.AddRange(BitConverter.GetBytes(obj.pCharArrayLength));
            foreach (var i in obj.pCharArray.Select(x => BitConverter.GetBytes(x)))
                result.AddRange(i);
            result.AddRange(BitConverter.GetBytes(obj.pBoolArrayLength));
            foreach (var i in obj.pBoolArray.Select(x => BitConverter.GetBytes(x)))
                result.AddRange(i);
            return result;
        }

        public static IEnumerable<byte> GetATestArrayValueObjectMemeryLayoutBigEndian()
        {
            var obj = InitATestArraySpecifiedEndianValueObject();
            List<byte> result = new List<byte>();
            result.AddRange(BytesHelper.GetBytes(obj.fByteArrayLength, true));
            foreach (var i in obj.fByteArray)
                result.Add(i);
            result.AddRange(BytesHelper.GetBytes(obj.fIntArrayLength, true));
            foreach (var i in obj.fIntArray.Select(x => BytesHelper.GetBytes(x, true)))
                result.AddRange(i);
            result.AddRange(BytesHelper.GetBytes(obj.fUIntArrayLength, true));
            foreach (var i in obj.fUIntArray.Select(x => BytesHelper.GetBytes(x, true)))
                result.AddRange(i);
            result.AddRange(BytesHelper.GetBytes(obj.fLongArrayLength, true));
            foreach (var i in obj.fLongArray.Select(x => BytesHelper.GetBytes(x, true)))
                result.AddRange(i);
            result.AddRange(BytesHelper.GetBytes(obj.fULongArrayLength, true));
            foreach (var i in obj.fULongArray.Select(x => BytesHelper.GetBytes(x, true)))
                result.AddRange(i);
            result.AddRange(BytesHelper.GetBytes(obj.fShortArrayLength, true));
            foreach (var i in obj.fShortArray.Select(x => BytesHelper.GetBytes(x, true)))
                result.AddRange(i);
            result.AddRange(BytesHelper.GetBytes(obj.fUShortArrayLength, true));
            foreach (var i in obj.fUShortArray.Select(x => BytesHelper.GetBytes(x, true)))
                result.AddRange(i);
            result.AddRange(BytesHelper.GetBytes(obj.fFloatArrayLength, true));
            foreach (var i in obj.fFloatArray.Select(x => BytesHelper.GetBytes(x, true)))
                result.AddRange(i);
            result.AddRange(BytesHelper.GetBytes(obj.fDoubleArrayLength, true));
            foreach (var i in obj.fDoubleArray.Select(x => BytesHelper.GetBytes(x, true)))
                result.AddRange(i);
            result.AddRange(BytesHelper.GetBytes(obj.fCharArrayLength, true));
            foreach (var i in obj.fCharArray.Select(x => BytesHelper.GetBytes(x, true)))
                result.AddRange(i);
            result.AddRange(BytesHelper.GetBytes(obj.fBoolArrayLength, true));
            foreach (var i in obj.fBoolArray.Select(x => BytesHelper.GetBytes(x, true)))
                result.AddRange(i);

            result.AddRange(BytesHelper.GetBytes(obj.pByteArrayLength, true));
            foreach (var i in obj.pByteArray)
                result.Add(i);
            result.AddRange(BytesHelper.GetBytes(obj.pIntArrayLength, true));
            foreach (var i in obj.pIntArray.Select(x => BytesHelper.GetBytes(x, true)))
                result.AddRange(i);
            result.AddRange(BytesHelper.GetBytes(obj.pUIntArrayLength, true));
            foreach (var i in obj.pUIntArray.Select(x => BytesHelper.GetBytes(x, true)))
                result.AddRange(i);
            result.AddRange(BytesHelper.GetBytes(obj.pLongArrayLength, true));
            foreach (var i in obj.pLongArray.Select(x => BytesHelper.GetBytes(x, true)))
                result.AddRange(i);
            result.AddRange(BytesHelper.GetBytes(obj.pULongArrayLength, true));
            foreach (var i in obj.pULongArray.Select(x => BytesHelper.GetBytes(x, true)))
                result.AddRange(i);
            result.AddRange(BytesHelper.GetBytes(obj.pShortArrayLength, true));
            foreach (var i in obj.pShortArray.Select(x => BytesHelper.GetBytes(x, true)))
                result.AddRange(i);
            result.AddRange(BytesHelper.GetBytes(obj.pUShortArrayLength, true));
            foreach (var i in obj.pUShortArray.Select(x => BytesHelper.GetBytes(x, true)))
                result.AddRange(i);
            result.AddRange(BytesHelper.GetBytes(obj.pFloatArrayLength, true));
            foreach (var i in obj.pFloatArray.Select(x => BytesHelper.GetBytes(x, true)))
                result.AddRange(i);
            result.AddRange(BytesHelper.GetBytes(obj.pDoubleArrayLength, true));
            foreach (var i in obj.pDoubleArray.Select(x => BytesHelper.GetBytes(x, true)))
                result.AddRange(i);
            result.AddRange(BytesHelper.GetBytes(obj.pCharArrayLength, true));
            foreach (var i in obj.pCharArray.Select(x => BytesHelper.GetBytes(x, true)))
                result.AddRange(i);
            result.AddRange(BytesHelper.GetBytes(obj.pBoolArrayLength, true));
            foreach (var i in obj.pBoolArray.Select(x => BytesHelper.GetBytes(x, true)))
                result.AddRange(i);
            return result;
        }
    }
}
