using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace TPSLRawDataSimulator
{
    class BytesHelper
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
                obj = (int[]) arrayObject;
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

            //从内存空间拷到byte数组
            Marshal.Copy(structPtr, bytes, 0, size);
            //释放内存空间
            Marshal.FreeHGlobal(structPtr);
            //返回byte数组
            return bytes;
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
            return -1;
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
    }
}
