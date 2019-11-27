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


            Console.ReadLine();
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
        [StructToRaw(Endian = Endian.LittleEndian)]
        public struct TPSLWrapper
        {
            [MemberIndex(Index = 0, SizeCount = 4)]
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1)]
            public char[] Header;
            [MemberIndex(Index = 10)]
            [MarshalAs(UnmanagedType.U1)]
            public FrameType Type;
            [MemberIndex(Index = 20, LengthTo = "Data")]
            public ushort Length;
            [MemberIndex(Index = 30)]
            public byte[] Data;
            [MemberIndex(Index = 40)]
            public ushort CRC16;
            [MemberIndex(Index = 50, SizeCount = 3)]
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1)]
            public char[] Tail;

            public TPSLWrapper(FrameType type, byte[] data)
            {
                this.Header = new char[4] { 't', 'p', 's', 'l' };
                this.Type = type;
                this.Length = (ushort)data.Length;
                this.Data = data;
                this.Tail = new char[3] { 'i', 'o', 't' };
                this.CRC16 = Calculator_CRC16(data, data.Length);
            }


        }

        /// <summary>
        /// data_body
        /// </summary>
        [StructToRaw(Endian = Endian.LittleEndian)]
        public struct TPSLDataSegement_Type1
        {
            [MemberIndex(Index = 0)]
            [MarshalAs(UnmanagedType.U1)]
            public DeviceType DeviceType;
            [MemberIndex(Index = 10, SizeCount = 15)]
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1)]
            public char[] IMEI;
            [MemberIndex(Index = 20)]
            [MarshalAs(UnmanagedType.U1)]
            public char IMEI_STATIC_TAIL; //0x30 '0'
            [MemberIndex(Index = 30, SizeCount = 6)]
            [MarshalAs(UnmanagedType.ByValArray)]
            public byte[] Send_Time;

            [MemberIndex(Index = 40)]
            public byte Battery_Level;
            [MemberIndex(Index = 50)]
            public byte Singal_Strength;
            [MemberIndex(Index = 60)]
            public byte[] Sample_Data;

            /// <summary>
            /// fix "one hundred years" bug.
            /// </summary>
            /// <returns></returns>
            public DateTime GetSendTime() {
                var results = new List<int>();
                foreach (byte bcd in this.Send_Time)
                {
                    var result = 0;
                    result *= 100;
                    result += (10 * (bcd >> 4));
                    result += bcd & 0xf;
                    results.Add(result);
                }
                var year = 0;
                if (results[0] - DateTime.Now.Year % 100 == 99)
                {
                    year = DateTime.Now.Year / 100 - 1 + results[0];
                }
                else 
                {
                    year = DateTime.Now.Year / 100 + results[0];
                }
                return new DateTime(year, results[1], results[2], results[3], results[4], results[5]);
            }
        }



        public struct TPSLDataSegement_Type3_4
        {
            [MarshalAs(UnmanagedType.U1)]
            public DeviceType DeviceType;
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = 15)]
            public char[] IMEI;

            public char IMEI_STATIC_TAIL; //0x30 '0'
        }



        public enum FrameType
        {
            Data = 0x01,
            GetConfig = 0x02,
            ResponseConfig = 0x03,
            SendConfig = 0x04,
            Update = 0x05,
            Result = 0x06
        }

        public enum DeviceType
        {
            WireLessPressure = 0x01,
            WireLessLqLevel,
            WireLessTemperature,
            WireLessFireControl,
            WireLessTempAndHumidity,
            WireLessMultiChannel,
            WireLessAmbientMonitor,
            WireLessValve,
            WireLessDigit

        }

        public static ushort Calculator_CRC16(byte[] buf, int length)
        {
            ushort value, crc_reg = 0xFFFF;
            for (ushort i = 0; i < length; i++)
            {
                value = (ushort)(buf[i] << 8);
                for (ushort j = 0; j < 8; j++)
                {
                    if ((ushort)(crc_reg ^ value) < 0)
                        crc_reg = (ushort)((crc_reg << 1) ^ 0x1021);
                    else
                        crc_reg <<= 1; value <<= 1;
                }
            }
            return crc_reg;
        }
    }
}


