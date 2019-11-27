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
        /// type 0x01 data_body
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
            public byte Signal_Strength;
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
                    results.Add(BytesHelper.BCDToInt32(bcd));
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

        /// <summary>
        /// for device_type 0x01,0x02,0x03,0x04
        /// </summary>
        public struct TPSLSingleChannelDeviceSampleDataUnit {
            public DeviceWarningState DeviceState;   // Higher4Bits
            public DataUnit DataUnit; //Lower4Bits
            public double Data; // BCD Encode. 
            //
            // when DataUnit is not Centigrade.
            // first byte higher4bits is the significant digit count of the fractional part. lower4bits is the first digit of 5 significant digits. 
            // second byte higher4bits is the second digit of 5 significant digits. lower4bits is the third digit of 5 significant digits.
            // etc...
            //
            // when Dataunit is Centigrade.
            // first byte higher4bits is the sign of the float. others are the float data.
            // I think this protocal is bullshit! Does the author do not know the half precision float??? It can resolve this problem with just 3 bytes and no fucking BCD encode!
            //


            public void FillObjectFromBytes(byte[] bytes) 
            {
                if (bytes.Length != 4)
                    throw new ArgumentException($"Data must have 4 bytes. Actually have :{bytes.Length} bytes","bytes");
                var data = BytesHelper.BCDToInt32(bytes[0]);
                this.DataUnit = (DataUnit)(data % 10);
                this.DeviceState = (DeviceWarningState)(data / 10);

                if (DataUnit != DataUnit.Centigrade)
                {
                    var dataData = "";
                    data = BytesHelper.BCDToInt32(bytes[1]);
                    var fractionalPartCount = data / 10;
                    dataData += data % 10;
                    data = BytesHelper.BCDToInt32(bytes[2]);
                    dataData += data;
                    data = BytesHelper.BCDToInt32(bytes[3]);
                    dataData += data;
                    if (fractionalPartCount != 0)
                        dataData.Insert(dataData.Length - fractionalPartCount, ".");
                    if (double.TryParse(dataData, out var result))
                        this.Data = result;
                    else
                        throw new Exception($"The data bytes can not transform to double! See data:{dataData}");
                }
                else 
                {
                    data = BytesHelper.BCDToInt32(bytes[1]);
                    var fractionalPartCount = data / 10;
                    var signed = data % 10;
                    var dataData = "";
                    data = BytesHelper.BCDToInt32(bytes[2]);
                    dataData += data;
                    data = BytesHelper.BCDToInt32(bytes[3]);
                    dataData += data;
                    if (fractionalPartCount != 0)
                        dataData.Insert(dataData.Length - fractionalPartCount, ".");
                    if (signed == 1)
                        dataData = "-" + dataData;
                    else if (signed > 0)
                        throw new Exception("unknown data when convert centigrade data.");
                    if (double.TryParse(dataData, out var result))
                        this.Data = result;
                    else
                        throw new Exception($"The data bytes can not transform to double! See data:{dataData}");

                }
            }
        }

        [StructToRaw(Endian = Endian.LittleEndian)]
        /// <summary>
        /// for device_type 0x07
        /// </summary>
        public struct TPSLSmokeSensor {
            [MemberIndex(Index = 0)]
            [MarshalAs(UnmanagedType.U1)]
            public SmokeWarningState WarningState;
            [MarshalAs(UnmanagedType.U1)]
            public SmokeDevicePowerState PowerState;
        }
        
        /// <summary>
        /// for device_type 0x08
        /// </summary>
        public struct TPSLUnKnownDevice {
            public UnknownDeviceState ControlState;
            public UnknownDeviceBallValveState BallValveState;

            public void FillObjectFromByte(byte data) {
                var bcd = BytesHelper.BCDToInt32(data);
                this.ControlState = (UnknownDeviceState)(bcd / 10);
                this.BallValveState = (UnknownDeviceBallValveState)(bcd % 10);
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



        /// <summary>
        /// type 0x06 data_body
        /// </summary>
        [StructToRaw(Endian=Endian.LittleEndian)]
        public struct TPSLDataSegment_Type6 {
            [MemberIndex(Index=0)]
            public ExecResult EXEC_RESULT;
        }


        public enum ExecResult {
            Successed,
            Failed
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

        public enum DeviceWarningState {
            Normal,
            LowerLimitWarning,
            UpperLimitWarning,
            Error,
            DynamicThresholdWarning,
            CollisionWarning,
            LeaningWarning,
            FlowWarning,
            LeakageWarning,
            LowPowerWarning
        }

        public enum DataUnit {
            MPa=1,
            Bar,
            KPa,
            M, 
            Centigrade,
            CubicMetresPerHour,
            Degree
        }

        public enum SmokeWarningState
        {
            Normal,
            Warning,
        }

        public enum SmokeDevicePowerState
        {
            Normal,
            LowPower
        }

        public enum UnknownDeviceState
        {
            CanNotBeControled,
            CanControledByTemperatureSensor,
            CanControledByPressureSensor
        }
        public enum UnknownDeviceBallValveState {
            Closed,
            OpenedTenPercent,
            OpenedTwentyPercent,
            OpenedThirtyPercent,
            OpenedFourtyPercent,
            OpenedFiftyPercent,
            OpenedSixtyPercent,
            OpenedSeventyPercent,
            OpenedEightyPercent,
            OpenedNinetyPercent,
            OpenedMaximum
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


