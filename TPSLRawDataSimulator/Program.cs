using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using TPSLRawDataSimulator.TypeDefinations;

namespace TPSLRawDataSimulator
{
    class Program
    {
        static void Main(string[] args)
        {
            var a = new List<uint> { 0xFFFFFFF0, 0xFFFFFFF1, 0xFFFFFFF2, 0xFFFFFFF3 };

            var b = BytesHelper.IEnumerableToBytes<uint>(a);
            foreach (var data in b)
            {
                Console.Write(Convert.ToString(data, 2) + ',');
            }
            Console.WriteLine();

            Console.WriteLine("------------------------");
            test t = new test();
            t.I1 = 1;
            t.U2 = 2;
            var stream = new MemoryStream();
            new RawBinaryFormatter().Serialize(stream, t);
            var buffer = stream.ToArray();
            foreach (var data in buffer)
            {
                Console.Write(Convert.ToString(data, 2) + ',');

            }
            Console.WriteLine("------------------------");



            foreach (var memberInfo in typeof(test).GetMembers(BindingFlags.Public | BindingFlags.Instance))
            {
                if (((int)(memberInfo.MemberType & (MemberTypes.Field | MemberTypes.Property))) != 0)
                    Console.WriteLine(memberInfo.Name + "|" + memberInfo.DeclaringType);
            }
            Console.WriteLine("------------------------");


            testArray ta = new testArray();
            ta.IA1 = new[] { 1, 2, 3, 4, 5, 6, int.MaxValue };
            ta.UA2 = new uint[] { 1, 2, 3, 4, 5, 6, uint.MaxValue };

            stream = new MemoryStream();
            new RawBinaryFormatter().Serialize(stream, ta);
            buffer = stream.ToArray();
            foreach (var data in buffer)
            {
                Console.Write(Convert.ToString(data, 2) + ',');

            }
            Console.WriteLine();
            Console.WriteLine("------------------------");

            var realData = new byte[] {
                0x74,0x70,0x73,0x6C,0x03,0x00,0x67,0x02,0x38,0x36,0x38,0x34,0x37,0x34,0x30,0x34,0x32,0x38,0x33,0x37,0x33,0x32,0x36,0x30,0x01,0x04,0x00,0x00,0x00,0x3C,0x02,0x04,0x00,0x00,0x01,0x2C,0x03,0x0E,0x31,0x31,0x37,0x2E,0x36,0x30,0x2E,0x31,0x35,0x37,0x2E,0x31,0x33,0x37,0x04,0x04,0x35,0x36,0x38,0x33,0x06,0x02,0x00,0x00,0x07,0x02,0x00,0x00,0x08,0x02,0x00,0x00,0x09,0x01,0x00,0x0A,0x04,0x01,0x00,0x00,0x18,0x0B,0x04,0x01,0x03,0x02,0x01,0x0C,0x10,0x34,0x36,0x30,0x30,0x36,0x35,0x35,0x34,0x34,0x31,0x38,0x30,0x34,0x34,0x31,0x30,0x0D,0x05,0x63,0x74,0x6E,0x65,0x74,0xA6,0xE8,0x69,0x6F,0x74
            };
            var realData1 = new byte[] {
                0x74,0x70,0x73,0x6C,0x01,0x00,0x1D,0x02,0x38,0x36,0x38,0x34,0x37,0x34,0x30,0x34,0x32,0x38,0x33,0x37,0x33,0x32,0x36,0x30,0x19,0x11,0x28,0x11,0x59,0x16,0x64,0x0E,0x04,0x40,0x00,0x00,0x27,0x16,0x69,0x6F,0x74
            };

            RawBinaryFormatter formatter = new RawBinaryFormatter();
            var wrapper = (TPSLWrapper)formatter.Deserialize(new MemoryStream(realData1), typeof(TPSLWrapper));
            wrapper.FillObject(formatter);


            // this is the example
            // to avoid nagle issue.
            var realData2 = new byte[] {
                0x74,0x70,0x73,0x6C,0x01,0x00,0x1D,0x02,0x38,0x36,0x38,0x34,0x37,0x34,0x30,0x34,0x32,0x38,0x33,0x37,0x33,0x32,0x36,0x30,0x19,0x11,0x28,0x11,0x59,0x16,0x64,0x0E,0x04,0x40,0x00,0x00,0x27,0x16,0x69,0x6F,0x74
                ,0x74,0x70,0x73,0x6C,0x01,0x00,0x1D,0x02,0x38
            };
            BoundAutoStreamTransfers transfers = new BoundAutoStreamTransfers();
            transfers.addBytes(realData2);
            while(transfers.TryGetAnObject(typeof(TPSLWrapper), (Encoding.ASCII.GetBytes("tpsl"),Encoding.ASCII.GetBytes("iot")), out var wrapper2))
            {
                ((TPSLWrapper)wrapper2).FillObject(formatter);
            }

            var multiChannelData = new byte[] {
                0x74,0x70,0x73,0x6C,0x01,0x00,0x2B,0x05,0x38,0x36,0x38,0x34,0x37,0x34,0x30,0x34,0x36,0x31,0x36,0x34,0x36,0x36,0x39,0x30,0x19,0x11,0x29,0x09,0x43,0x44,0x64,0x16,0x02,0x08,0x01,0x05,0x00,0x01,0x02,0x00,0x02,0x38,0x07,0x02,0x06,0x00,0x01,0x01,0x02,0x98,0xBD,0x18,0x69,0x6F,0x74,
            };
            transfers.addBytes(multiChannelData);
            while (transfers.TryGetAnObject(typeof(TPSLWrapper), (Encoding.ASCII.GetBytes("tpsl"), Encoding.ASCII.GetBytes("iot")), out var wrapper3)) {
                ((TPSLWrapper)wrapper3).FillObject(formatter);
            }


            Console.ReadLine();
        }
     
    }

    public struct test
    {
        [MemberIndex(Index = 0)]
        public int I1 { get; set; }
        [MemberIndex(Index = 1)]
        public uint U2;

    }

    [StructToRaw(Endian = Endian.BigEndian)]
    public struct testArray
    {
        [MemberIndex(Index = 0)]
        public int[] IA1 { get; set; }
        [MemberIndex(Index = 1)]
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1)]
        public uint[] UA2;
    }
    
}


