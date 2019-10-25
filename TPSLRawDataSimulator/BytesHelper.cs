using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.IO;
using System.Threading;

namespace TPSLRawDataSimulator
{
    public class BytesHelper
    {

        public static byte[] StructToBytes(object structObj)
        {
            //得到结构体的大小
            int size = Marshal.SizeOf(structObj);
            //创建byte数组
            byte[] bytes = new byte[size];
            //分配结构体大小的内存空间
            IntPtr structPtr = Marshal.AllocHGlobal(size);
            //将结构体拷到分配好的内存空间
            Marshal.StructureToPtr(structObj, structPtr, false);
            //从内存空间拷到byte数组
            Marshal.Copy(structPtr, bytes, 0, size);
            //释放内存空间
            Marshal.FreeHGlobal(structPtr);
            //返回byte数组
            return bytes;
        }

        /// <summary>
        /// this method 's endian is depending bitconverter.IsLittleEndian.
        /// </summary>
        /// <param name="arrayObject"></param>
        /// <returns></returns>
        public static byte[] ArrayToBytes(object arrayObject)
        {
            var size = -1;
            var length = -1;
            Array obj = null;
            byte[] bytes = null;
            IntPtr structPtr = default(IntPtr);
            //将结构体拷到分配好的内存空间
            if (arrayObject.GetType() == typeof(int[]))
            {
                obj = (int[])arrayObject;
                length = obj.Length;
                size = obj.Length * sizeof(int);
                //创建byte数组
                bytes = new byte[size];
                //分配结构体大小的内存空间
                structPtr = Marshal.AllocHGlobal(size);
                Marshal.Copy((int[])obj, 0, structPtr, length);

            }

            if (arrayObject.GetType() == typeof(uint[]))
            {
                obj = (uint[])arrayObject;
                length = obj.Length;
                size = obj.Length * sizeof(uint);
                //创建byte数组
                bytes = new byte[size];
                //分配结构体大小的内存空间
                structPtr = Marshal.AllocHGlobal(size);
                Marshal.Copy((int[])obj, 0, structPtr, length);

            }
            if (arrayObject.GetType() == typeof(double[]))
            {
                obj = (double[])arrayObject;
                length = obj.Length;
                size = obj.Length * sizeof(double);
                //创建byte数组
                bytes = new byte[size];
                //分配结构体大小的内存空间
                structPtr = Marshal.AllocHGlobal(size);
                Marshal.Copy((double[])obj, 0, structPtr, length);

            }
            if (arrayObject.GetType() == typeof(short[]))
            {
                obj = (short[])arrayObject;
                length = obj.Length;
                size = obj.Length * sizeof(short);
                //创建byte数组
                bytes = new byte[size];
                //分配结构体大小的内存空间
                structPtr = Marshal.AllocHGlobal(size);
                Marshal.Copy((short[])obj, 0, structPtr, length);

            }
            if (arrayObject.GetType() == typeof(ushort[]))
            {
                obj = (ushort[])arrayObject;
                length = obj.Length;
                size = obj.Length * sizeof(ushort);
                //创建byte数组
                bytes = new byte[size];
                //分配结构体大小的内存空间
                structPtr = Marshal.AllocHGlobal(size);
                Marshal.Copy((short[])obj, 0, structPtr, length);

            }
            if (arrayObject.GetType() == typeof(long[]))
            {
                obj = (long[])arrayObject;
                length = obj.Length;
                size = obj.Length * sizeof(long);
                //创建byte数组
                bytes = new byte[size];
                //分配结构体大小的内存空间
                structPtr = Marshal.AllocHGlobal(size);
                Marshal.Copy((long[])obj, 0, structPtr, length);

            }
            if (arrayObject.GetType() == typeof(ulong[]))
            {
                obj = (ulong[])arrayObject;
                length = obj.Length;
                size = obj.Length * sizeof(ulong);
                //创建byte数组
                bytes = new byte[size];
                //分配结构体大小的内存空间
                structPtr = Marshal.AllocHGlobal(size);
                Marshal.Copy((long[])obj, 0, structPtr, length);

            }
            if (arrayObject.GetType() == typeof(float[]))
            {
                obj = (float[])arrayObject;
                length = obj.Length;
                size = obj.Length * sizeof(float);
                //创建byte数组
                bytes = new byte[size];
                //分配结构体大小的内存空间
                structPtr = Marshal.AllocHGlobal(size);
                Marshal.Copy((float[])obj, 0, structPtr, length);

            }
            if (arrayObject.GetType() == typeof(char[]))
            {
                obj = (char[])arrayObject;
                length = obj.Length;
                size = obj.Length * sizeof(char);
                //创建byte数组
                bytes = new byte[size];
                //分配结构体大小的内存空间
                structPtr = Marshal.AllocHGlobal(size);
                Marshal.Copy((char[])obj, 0, structPtr, length);

            }
            if (arrayObject.GetType() == typeof(byte[]))
            {
                obj = (byte[])arrayObject;
                length = obj.Length;
                size = obj.Length * sizeof(byte);
                //创建byte数组
                bytes = new byte[size];
                //分配结构体大小的内存空间
                structPtr = Marshal.AllocHGlobal(size);
                Marshal.Copy((byte[])obj, 0, structPtr, length);

            }
            if (arrayObject.GetType() == typeof(bool[]))
            {
                bytes = ArrayToBytes((bool[])arrayObject, UnmanagedType.I1, !BitConverter.IsLittleEndian);
            }

            if (structPtr != default(IntPtr))
            {
                //从内存空间拷到byte数组
                Marshal.Copy(structPtr, bytes, 0, size);
                //释放内存空间
                Marshal.FreeHGlobal(structPtr);
            }
            //返回byte数组
            return bytes;
        }

        public static byte[] ArrayToBytes(Array arrayObject, UnmanagedType elementType, bool isResultBigEndian)
        {
            List<byte> result = new List<byte>();
            foreach (var element in arrayObject)
            {
                if (elementType == UnmanagedType.I1 || elementType == UnmanagedType.U1)
                    result.Add(BitConverter.IsLittleEndian ? BytesHelper.GetBytes(element, isResultBigEndian).First() : BytesHelper.GetBytes(element, isResultBigEndian).Last());
                //result.Add(unchecked(Convert.ToByte(element)));
                if (elementType == UnmanagedType.I2 || elementType == UnmanagedType.U2)
                    result.AddRange(GetBytesDependOnEndian(BytesHelper.GetBytes(element, isResultBigEndian), 2, !BitConverter.IsLittleEndian, isResultBigEndian));
                if (elementType == UnmanagedType.I4 || elementType == UnmanagedType.U4)
                    result.AddRange(GetBytesDependOnEndian(BytesHelper.GetBytes(element, isResultBigEndian), 4, !BitConverter.IsLittleEndian, isResultBigEndian));
                if (elementType == UnmanagedType.I8 || elementType == UnmanagedType.U8)
                    result.AddRange(GetBytesDependOnEndian(BytesHelper.GetBytes(element, isResultBigEndian), 8, !BitConverter.IsLittleEndian, isResultBigEndian));
                if (elementType == UnmanagedType.R4)
                    result.AddRange(GetBytesDependOnEndian(BytesHelper.GetBytes(element, isResultBigEndian), 4, !BitConverter.IsLittleEndian, isResultBigEndian));
                if (elementType == UnmanagedType.R8)
                    result.AddRange(GetBytesDependOnEndian(BytesHelper.GetBytes(element, isResultBigEndian), 8, !BitConverter.IsLittleEndian, isResultBigEndian));
            }
            return result.ToArray();
        }
        public static byte[] ArrayToBytes(Array arrayObject, bool isResultBigEndian)
        {
            List<byte> result = new List<byte>();
            foreach (var element in arrayObject)
            {
                result.AddRange(BytesHelper.GetBytes(element, isResultBigEndian));
            }
            return result.ToArray();
        }

        public static byte[] IEnumerableToBytes<T>(IEnumerable<T> obj)
        {
            var array = obj.ToArray();
            return ArrayToBytes(array);
        }

        public static int GetBytesOfType(Type t)
        {
            if (t == typeof(int))
                return sizeof(int);
            if (t == typeof(uint))
                return sizeof(uint);
            if (t == typeof(long))
                return sizeof(long);
            if (t == typeof(short))
                return sizeof(short);
            if (t == typeof(byte))
                return sizeof(byte);
            if (t == typeof(ushort))
                return sizeof(ushort);
            if (t == typeof(float))
                return sizeof(float);
            if (t == typeof(char))
                return sizeof(char);
            if (t == typeof(double))
                return sizeof(double);
            if (t == typeof(UInt16) || t == typeof(Int16))
                return sizeof(UInt16);
            if (t == typeof(UInt32) || t == typeof(Int32))
                return sizeof(UInt32);
            if (t == typeof(UInt64) || t == typeof(Int64))
                return sizeof(UInt64);
            if (t == typeof(bool) || t == typeof(Boolean))
                return sizeof(bool);
            
            return -1;
        }

        public static int GetBytesCountOfArray(Array array) {
            var elementType = array.GetType().GetElementType();
            var elementLengthInByte = GetBytesOfType(elementType);
            if (elementLengthInByte < 0)
            {
                throw new ArgumentException( $"Element type:{elementType} of this array is not a fix length type.","array");
            }
            return array.Length * elementLengthInByte;
        }

        public static int GetBytesOfType(UnmanagedType t)
        {
            switch (t)
            {
                case UnmanagedType.I1:
                case UnmanagedType.U1:
                    return 1;
                case UnmanagedType.I2:
                case UnmanagedType.U2:
                    return 2;
                case UnmanagedType.I4:
                case UnmanagedType.U4:
                case UnmanagedType.Bool:
                case UnmanagedType.R4:
                    return 4;
                case UnmanagedType.I8:
                case UnmanagedType.U8:
                case UnmanagedType.R8:
                    return 8;

                default:
                    return -1;
            }
        }

        /// <summary>
        /// Get the bytes of the given object.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static byte[] GetBytes(object obj, bool isTargetBigEndian)
        {
            var objType = obj.GetType();
            var shouldReverseResult = isTargetBigEndian == BitConverter.IsLittleEndian;
            byte[] result = null;
            if (objType == typeof(byte))
            {
                return new[] { (byte)obj };
            }
            if (objType == typeof(int))
            {
                result = BitConverter.GetBytes((int)obj);
            }
            if (objType == typeof(uint))
            {
                result = BitConverter.GetBytes((uint)obj);
            }
            if (objType == typeof(sbyte))
            {
                result = BitConverter.GetBytes((sbyte)obj);
            }
            if (objType == typeof(short))
            {
                result = BitConverter.GetBytes((short)obj);
            }
            if (objType == typeof(ushort))
            {
                result = BitConverter.GetBytes((ushort)obj);
            }
            if (objType == typeof(long))
            {
                result = BitConverter.GetBytes((long)obj);
            }
            if (objType == typeof(ulong))
            {
                result = BitConverter.GetBytes((ulong)obj);
            }
            if (objType == typeof(char))
            {
                result = BitConverter.GetBytes((char)obj);
            }
            if (objType == typeof(bool))
            {
                result = BitConverter.GetBytes((bool)obj);
            }
            if (objType == typeof(float))
                result = BitConverter.GetBytes((float)obj);
            if (objType == typeof(double))
                result = BitConverter.GetBytes((double)obj);
            if (result == null)
                throw new NotImplementedException();
            if (shouldReverseResult)
                result = result.Reverse().ToArray();
            return result;


        }

        /// <summary>
        /// no auto truncate or padding
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="objType"></param>
        /// <param name="isBigEndian"></param>
        /// <returns></returns>
        public static object GetTypedObjectFromBytes(byte[] bytes, Type objType, bool isBigEndian)
        {
            if (GetBytesOfType(objType) is int targetLength && bytes.Length != targetLength)
                throw new ArgumentException($"Target type expected lenth:{targetLength}, provided buffer length:{bytes.Length}");
            var bytesMustReverse = BitConverter.IsLittleEndian == isBigEndian;
            if (bytesMustReverse)
            {
                bytes = bytes.Reverse().ToArray();
            }
            if (objType == typeof(int))
            {
                return BitConverter.ToInt32(bytes);
            }
            if (objType == typeof(uint))
            {
                return BitConverter.ToUInt32(bytes);
            }
            if (objType == typeof(byte))
            {
                return bytes[0];
            }
            if (objType == typeof(sbyte))
            {
                return bytes[0];
            }
            if (objType == typeof(short))
            {
                return BitConverter.ToInt16(bytes);
            }
            if (objType == typeof(ushort))
            {
                return unchecked((ushort)BitConverter.ToUInt16(bytes));
            }
            if (objType == typeof(long))
            {
                return BitConverter.ToInt64(bytes);
            }
            if (objType == typeof(ulong))
            {
                return unchecked((ulong)BitConverter.ToUInt64(bytes));
            }
            if (objType == typeof(char))
            {
                return BitConverter.ToChar(bytes);
            }
            if (objType == typeof(bool))
            {
                return BitConverter.ToBoolean(bytes);
            }
            if (objType == typeof(float))
            {
                return BitConverter.ToSingle(bytes);
            }
            if (objType == typeof(double))
            {
                return BitConverter.ToDouble(bytes);
            }
            throw new NotImplementedException();

        }

        public static object GetTypedObjectFromStream(Stream stream, Type objType, bool isBigEndian)
        {
            var buffer = new byte[GetBytesOfType(objType)];
            stream.Read(buffer, 0, buffer.Length);
            return GetTypedObjectFromBytes(buffer, objType, isBigEndian);
        }

        /// <summary>
        /// auto truncate or padding
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="marshalAsType"></param>
        /// <param name="objType"></param>
        /// <param name="isBigEndian"></param>
        /// <returns></returns>
        public static object GetTypedObjectFromStream(Stream stream, UnmanagedType marshalAsType, Type objType, bool isBigEndian) {
            var marshalBytes = GetBytesOfType(marshalAsType);
            var typeDeclaredBytes = GetBytesOfType(objType);
            var buffer = new byte[marshalBytes];
            stream.Read(buffer, 0, buffer.Length);
            //padding zero
            buffer = DeserializeAutoPaddingOrTruncate(buffer, objType, isBigEndian);
            return GetTypedObjectFromBytes(buffer, objType, isBigEndian);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="elementType"></param>
        /// <param name="lengthInByte"></param>
        /// <param name="isBigEndian"></param>
        /// <returns></returns>
        public static Array GetArrayFromStream(Stream stream, Type elementType, int lengthInByte, bool isBigEndian)
        {
            var typeBytes = GetBytesOfType(elementType);
            if (lengthInByte % typeBytes != 0)
            {
                throw new ArgumentException($"Element type ${elementType.FullName} of array is not fit with the length of bytes ${lengthInByte}");
            }
            var arrayLength = lengthInByte / typeBytes;
            var result = Array.CreateInstance(elementType, arrayLength);
            for (var i = 0; i < arrayLength; i++)
            {
                result.SetValue(GetTypedObjectFromStream(stream, elementType, isBigEndian), i);
            }
            return result;
        }

        /// <summary>
        /// auto truncate or padding
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="marshalAsType"></param>
        /// <param name="objType"></param>
        /// <param name="lengthInByte"></param>
        /// <param name="isBigEndian"></param>
        /// <returns></returns>
        //some truncation problem
        public static Array GetArrayFromStream(Stream stream, UnmanagedType marshalAsType, Type objType, int lengthInByte, bool isBigEndian)
        {
            var marshalBytes = GetBytesOfType(marshalAsType);
            var typeDeclaredBytes = GetBytesOfType(objType);
            if (lengthInByte % marshalBytes != 0)
            {
                throw new ArgumentException($"Element type ${objType.FullName} of array is not fit with the length of bytes ${lengthInByte}");
            }
            var arrayLength = lengthInByte / marshalBytes;
            var byteResult = Array.CreateInstance(objType,arrayLength);
            //padding 0 for trancatedbytes.
            var readoffset = isBigEndian ? typeDeclaredBytes - marshalBytes : 0;
            
            for (var i = 0; i < arrayLength; i++)
            {
                var buffer = new byte[marshalBytes];
                var readedLength = stream.Read(buffer, 0, marshalBytes);
                if (readedLength < marshalBytes)
                    throw new Exception("stream ended unexpeced");
                buffer = DeserializeAutoPaddingOrTruncate(buffer,objType,isBigEndian);
                byteResult.SetValue(GetTypedObjectFromBytes(buffer,objType,isBigEndian), i);
            }
            return byteResult;
        }

        /// <summary>
        /// Get byte[Count] from source bytes depends on both side endian. 
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="byteCount"></param>
        /// <param name="sourceBigEndian"></param>
        /// <param name="targetBigEndian"></param>
        /// <returns></returns>
        public static byte[] GetBytesDependOnEndian(byte[] bytes, int byteCount, bool sourceBigEndian, bool targetBigEndian)
        {
            var result = new byte[byteCount];

            for (var i = 0; i < byteCount; i++)
            {
                if (sourceBigEndian && targetBigEndian)
                    result[byteCount - i - 1] = bytes[bytes.Length - i - 1];
                if (sourceBigEndian && !targetBigEndian)
                    result[i] = bytes[bytes.Length - i - 1];
                if (!sourceBigEndian && targetBigEndian)
                    result[byteCount - i - 1] = bytes[i];
                if (!sourceBigEndian && !targetBigEndian)
                    result[i] = bytes[i];
            }
            return result;
        }

        public static byte[] GetBytesDependOnEndian(byte[] bytes, bool sourceBigEndian, bool targetBigEndian)
        {
            return GetBytesDependOnEndian(bytes, bytes.Length, sourceBigEndian, targetBigEndian);
        }

        public static byte[] DeserializeAutoPaddingOrTruncate(byte[] bytes, Type objType, bool isBigEndian)
        {
            var typeBytes = GetBytesOfType(objType);
            //truncate
            if (typeBytes > bytes.Length)
            {
                if (isBigEndian)
                    return new byte[typeBytes - bytes.Length].Concat(bytes).ToArray();
                else
                    return bytes.Concat(new byte[typeBytes - bytes.Length]).ToArray();
            }
            //padding
            else if (typeBytes < bytes.Length)
            {
                if (isBigEndian)
                {
                    var result = new byte[typeBytes];
                    bytes.CopyTo(result, typeBytes - bytes.Length);
                    return result;
                }
                else {
                    var result = new Span<byte>(bytes).Slice(0, typeBytes);
                    return result.ToArray();
                }
            }
            else
            {
                return bytes;
            }
        }

        /// <summary>
        /// this method is for unboxing a object.
        /// if you unbox a object to another type instead of its origin type, you will get an exception.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        //public static object ConvertTo(object obj,Type originType,Type convertToType) {
        //    unchecked {
        //        if (originType == typeof(int))
        //            return (int)obj;
        //        if ()
        //    }
        //}
    }

    public enum Endian
    {
        BigEndian,
        LittleEndian
    }

    /// <summary>
    /// Serialization: Endian is for target buffer.
    /// Deserialization: Endian is for source buffer.
    /// </summary>
    [AttributeUsage(AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
    public class StructToRawAttribute : Attribute
    {
        public Endian Endian { get; set; }
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class MemberIndexAttribute : Attribute
    {
        public ushort Index { get; set; }
        public string LengthTo { get; set; }
    }
}
