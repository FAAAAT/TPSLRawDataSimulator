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
            transfers.TryGetAnObject(typeof(TPSLWrapper), (Encoding.ASCII.GetBytes("tpsl"),Encoding.ASCII.GetBytes("iot")), out var wrapper2);
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
    [StructToRaw(Endian = Endian.BigEndian)]
    public struct TPSLWrapper
    {
        [MemberIndex(Index = 0, SizeCount = 4)]
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1)]
        public char[] Header;
        [MemberIndex(Index = 10)]
        [MarshalAs(UnmanagedType.U1)]
        public FrameType Type;
        [MemberIndex(Index = 20, LengthTo = "Data")]
        [MarshalAs(UnmanagedType.U2)]
        public long Length;
        [MemberIndex(Index = 30)]
        public byte[] Data;
        [MemberIndex(Index = 40)]
        public ushort CRC16;
        [MemberIndex(Index = 50, SizeCount = 3)]
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1)]
        public char[] Tail;

        [RawIgnore]
        public TPSLDataBody? DataBody;
        public TPSLWrapper(FrameType type, byte[] data)
        {
            this.Header = new char[4] { 't', 'p', 's', 'l' };
            this.Type = type;
            this.Length = (ushort)data.Length;
            this.Data = data;
            this.Tail = new char[3] { 'i', 'o', 't' };
            this.CRC16 = BytesHelper.Calculator_CRC16(data, data.Length);
            this.DataBody = null;
        }
        public void FillObject(RawBinaryFormatter formatter)
        {
            this.DataBody = new TPSLDataBody(this.Type, formatter, this.Data);
        }
    }

    public struct TPSLDataBody
    {
        public TPSLDataSegment_Type1? DataBodyType1;
        public TPSLDataSegment_Type2? DataBodyType2;
        public TPSLDataSegment_Type3or4? DataBodyType3;
        public TPSLDataSegment_Type3or4? DataBodyType4;
        // type 5 is software update data.
        public TPSLDataSegment_Type6? DataBodyType6;

        public TPSLDataBody(FrameType type, RawBinaryFormatter formatter, byte[] datas)
        {
            this.DataBodyType1 = null;
            this.DataBodyType2 = null;
            this.DataBodyType3 = null;
            this.DataBodyType4 = null;
            this.DataBodyType6 = null;
            if (type == FrameType.Data)
            {
                var data = (TPSLDataSegment_Type1)formatter.Deserialize(new MemoryStream(datas), typeof(TPSLDataSegment_Type1));
                data.FillObject(data.DeviceType, formatter, data.Sample_Data);
                this.DataBodyType1 = data;
            }
            else if (type == FrameType.GetConfig)
            {
            }
            else if (type == FrameType.ResponseConfig)
            {
            }
            else if (type == FrameType.Result)
            {

                this.DataBodyType6 = (TPSLDataSegment_Type6)formatter.Deserialize(new MemoryStream(datas), typeof(TPSLDataSegment_Type6));
            }
            else
                throw new Exception("Unknown frame type " + type);

        }
    }

    public struct TPSLDataUnit
    {
        public TPSLSingleChannelDeviceSampleDataUnit? SingleChannelDataUnit;
        public TPSLMultiChannelDeviceSampleDataUnit? MultiChannelDataUnit;
        public TPSLSmokeSensor? SmokeSensor;
        public TPSLBallValveDevice? BallValveDevice;
        public TPSLMultiSwitcher? SwitcherDataUnit;

        public TPSLDataUnit(DeviceType type, RawBinaryFormatter formatter, byte[] datas)
        {
            this.SingleChannelDataUnit = null;
            this.MultiChannelDataUnit = null;
            this.SmokeSensor = null;
            this.BallValveDevice = null;
            this.SwitcherDataUnit = null;
            if (type == DeviceType.WireLessPressure || type == DeviceType.WireLessLquidLevel || type == DeviceType.WireLessTemperature || type == DeviceType.WireLessFireControl)
            {
                this.SingleChannelDataUnit = new TPSLSingleChannelDeviceSampleDataUnit(datas);
            }
            else if (type == DeviceType.WireLessTempAndHumidity || type == DeviceType.WireLessMultiChannel)
            {
                throw new NotImplementedException();
                //this.MultiChannelDataUnit = new MultiChannelDataUnit();
            }
            else if (type == DeviceType.WireLessAmbientMonitor)
            {
                this.SmokeSensor = (TPSLSmokeSensor)formatter.Deserialize(new MemoryStream(datas), typeof(TPSLSmokeSensor));
            }
            else if (type == DeviceType.WireLessValve)
            {
                this.BallValveDevice = new TPSLBallValveDevice(datas[0]);
            }
            else if (type == DeviceType.WireLessDigit)
            {
                this.SwitcherDataUnit = new TPSLMultiSwitcher(datas);
            }
        }
    }


    /// <summary>
    /// type 0x01 data_body
    /// </summary>
    [StructToRaw(Endian = Endian.LittleEndian)]
    public struct TPSLDataSegment_Type1
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
        public byte[] Send_Time;
        [RawIgnore]
        public DateTime SendTime;

        [MemberIndex(Index = 40)]
        public byte Battery_Level;
        [MemberIndex(Index = 50)]
        public byte Signal_Strength;
        [MemberIndex(Index = 60)]
        public byte[] Sample_Data;

        [RawIgnore]
        public TPSLDataUnit DataUnit;

        public void FillObject(DeviceType type, RawBinaryFormatter formatter, byte[] datas)
        {
            this.DataUnit = new TPSLDataUnit(type, formatter, datas);
            this.FillSendTime();
        }
        /// <summary>
        /// fix "one hundred years" bug.
        /// </summary>
        /// <returns></returns>
        public void FillSendTime()
        {
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
            this.SendTime = new DateTime(year, results[1], results[2], results[3], results[4], results[5]);
        }
    }

    /// <summary>
    /// for device_type 0x01,0x02,0x03,0x04
    /// </summary>
    public struct TPSLSingleChannelDeviceSampleDataUnit
    {
        public DeviceWarningState DeviceState;   // Higher4Bits
        public SingleChannelDataUnit DataUnit; //Lower4Bits
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

        public TPSLSingleChannelDeviceSampleDataUnit(byte[] bytes)
        {
            if (bytes.Length != 4)
                throw new ArgumentException($"Data must have 4 bytes. Actually have :{bytes.Length} bytes", "bytes");
            var data = BytesHelper.BCDToInt32(bytes[0]);
            this.DataUnit = (SingleChannelDataUnit)(data % 10);
            this.DeviceState = (DeviceWarningState)(data / 10);

            if (DataUnit != SingleChannelDataUnit.Centigrade)
            {
                var dataData = "";
                data = BytesHelper.BCDToInt32(bytes[1]);
                var fractionalPartCount = data / 10;
                dataData += data % 10;
                data = BytesHelper.BCDToInt32(bytes[2]);
                dataData += data.ToString("00");
                data = BytesHelper.BCDToInt32(bytes[3]);
                dataData += data.ToString("00");
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

    /// <summary>
    /// for device_type 0x05, 0x06
    /// I can not understand the protocal introducing exactlly.
    /// </summary>
    public struct TPSLMultiChannelDeviceSampleDataUnit
    {

    }

    [StructToRaw(Endian = Endian.LittleEndian)]
    /// <summary>
    /// for device_type 0x07
    /// </summary>
    public struct TPSLSmokeSensor
    {
        [MemberIndex(Index = 0)]
        [MarshalAs(UnmanagedType.U1)]
        public SmokeWarningState WarningState;
        [MarshalAs(UnmanagedType.U1)]
        public SmokeDevicePowerState PowerState;
    }

    /// <summary>
    /// for device_type 0x08
    /// </summary>
    public struct TPSLBallValveDevice
    {
        public DeviceBallValveControlState ControlState;
        public DeviceBallValveOpenState BallValveState;

        public TPSLBallValveDevice(byte data)
        {
            var bcd = BytesHelper.BCDToInt32(data);
            this.ControlState = (DeviceBallValveControlState)(bcd / 10);
            this.BallValveState = (DeviceBallValveOpenState)(bcd % 10);
        }
    }

    /// <summary>
    /// for device_type 0x09
    /// </summary>
    public struct TPSLMultiSwitcher
    {
        /// <summary>
        /// maximum is 16
        /// </summary>
        public int SwitcherCount;
        /// <summary>
        /// length is switcher count
        /// </summary>
        public bool[] SwitcherState;

        public TPSLMultiSwitcher(byte[] bytes)
        {
            this.SwitcherCount = bytes[0];
            if (this.SwitcherCount > 16)
                throw new ArgumentException($"Switcher count must equal or less then 16. Actually is {this.SwitcherCount}", "bytes[0]");
            BitArray bitArray = new BitArray(bytes[1..3]);
            SwitcherState = new bool[SwitcherCount];
            var i = 0;
            while (i < SwitcherCount)
            {
                SwitcherState[i] = bitArray[i];
                i++;
            }
        }
    }

    /// <summary>
    /// type2 doesn't have data_body
    /// </summary>
    public struct TPSLDataSegment_Type2
    {

    }

    /// <summary>
    /// type 3 or type 4 data_body
    /// </summary>
    [StructToRaw(Endian = Endian.LittleEndian)]
    public struct TPSLDataSegment_Type3or4
    {
        [MemberIndex(Index = 1)]
        [MarshalAs(UnmanagedType.U1)]
        public DeviceType DeviceType;
        [MemberIndex(Index = 10, SizeCount = 15)]
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1)]
        public char[] IMEI;
        [MemberIndex(Index = 20)]
        [MarshalAs(UnmanagedType.U1)]
        public char IMEI_STATIC_TAIL; //0x30 '0'
        [MemberIndex(Index = 30)]
        public byte[] Param_Data;
    }

    public struct ParamUnit
    {
        public ParamType ParamType;
        public int ParamValueLen;
        public byte[] ParamValue;
    }

    /// <summary>
    /// this object for paramunits values container
    /// </summary>
    public struct ParamUnitsData
    {
        /// <summary>
        /// 0x01：采样间隔 4 数值，网络字节序，单位为秒 
        /// </summary>
        public uint SampleInteral;
        /// <summary>
        /// 0x02：发送间隔 4 数值，网络字节序，单位为秒 
        /// </summary>
        public uint SendInterval;
        /// <summary>
        /// 0x03：服务器 IP 地址 N 字符型 
        /// </summary>
        public string ServerIP;
        /// <summary>
        /// 0x04：服务器端口 N 字符型
        /// </summary>
        public string ServerPort;
        /// <summary>
        /// 0x05：服务器 URL N 字符型
        /// </summary>
        public string ServerUrl;
        /// <summary>
        /// 0x06：单通道告警阈值下限 2 数值，网络字节序，数值单位参考《蓝牙配置使用手册.pdf》
        /// </summary>
        public byte[] SingleChannelThresholdWarningLowerLimit;
        /// <summary>
        /// 0x07：单通道告警阈值上限 2 数值，网络字节序，数值单位参考《蓝 牙配置使用手册.pdf》 
        /// </summary>
        public byte[] SingleChannelThresholdWarningUpperLimit;
        /// <summary>
        /// 0x08：单通道告警动态变化阈值 2 数值，网络字节序，数值单位参考《蓝 牙配置使用手册.pdf》 
        /// </summary>
        public byte[] SingleChannelDeltaThreshold;
        /// <summary>
        /// 0x09：停止上报 1 数值。0：正常，1：停止上报
        /// </summary>
        public bool StopSending;
        /// <summary>
        /// 0x0a：软件版本 4 主版本-子版本-修订版本-协议版本，详 见《版本定义规范》。
        /// </summary>
        public byte[] SoftwareVersion;
        /// <summary>
        /// 0x0b：硬件版本 4 设备类型-芯片型号-模组型号-传感器 型号，详见《版本定义规范》。 
        /// </summary>
        public byte[] HardwareVersion;
        /// <summary>
        /// 0x0c：卡号（IMSI） 16 字符型 
        /// </summary>
        public string IMSI;
        /// <summary>
        /// 0x0d：APN N 字符型
        /// </summary>
        public string APN;
        /// <summary>
        /// 0x0e：多通道告警阈值下限 4 数值，按照温度湿度、温度压力顺序， 数值单位参考《蓝牙配置使用手册.pdf》 
        /// </summary>
        public byte[] MultiChannelThresholdWarningLowerLimit;
        /// <summary>
        /// 0x0f：多通道告警阈值上限 4 数值，按照温度湿度、温度压力顺序， 数值单位参考《蓝牙配置使用手册.pdf》
        /// </summary>
        public byte[] MultiChannelThresholdWarningUpperLimit;
        /// <summary>
        /// 0x10：多通道告警动态变化阈值 4 数值，按照温度湿度、温度压力顺序， 数值单位参考《蓝牙配置使用手册.pdf》
        /// </summary>
        public byte[] MultiChannelDeltaThreshold;
        /// <summary>
        /// 0x11：控制阀状态 1 见 1.4.1.6.1 定义 
        /// </summary>
        public DeviceBallValveOpenState ValveState;
        /// <summary>
        /// 0x12：控制阀供暖季开始时间 6 见 1.4.1.3 定义
        /// </summary>
        public DateTime ValveWarmStartDate;
        /// <summary>
        /// 0x13：控制阀供暖季结束时间 6 见 1.4.1.3 定义 
        /// </summary>
        public DateTime ValveWarmEndDate;
        /// <summary>
        /// 0x14：控制阀调整期天数 1 数值。
        /// </summary>
        public byte ValveControlDurationDayCount;
        /// <summary>
        /// 0x15：开关量状态 2 16 路开关量，每个 bit 代表一路的开关 状态
        /// </summary>
        public bool[] SwitcherState;
    }

    /// <summary>
    /// type 0x06 data_body
    /// </summary>
    [StructToRaw(Endian = Endian.LittleEndian)]
    public struct TPSLDataSegment_Type6
    {
        [MemberIndex(Index = 0)]
        public ExecResult EXEC_RESULT;
    }


    public enum ExecResult
    {
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
        /// <summary>
        /// 无线远程压力
        /// </summary>
        WireLessPressure = 0x01,
        /// <summary>
        /// 无线远程液位
        /// </summary>
        WireLessLquidLevel,
        /// <summary>
        /// 无线远程温度
        /// </summary>
        WireLessTemperature,
        /// <summary>
        /// 无线智能消防栓检测
        /// </summary>
        WireLessFireControl,
        /// <summary>
        /// 无线远程温度湿度
        /// </summary>
        WireLessTempAndHumidity,
        /// <summary>
        /// 无线智能多通道采集
        /// </summary>
        WireLessMultiChannel,
        /// <summary>
        /// 无线远程智能环境监测
        /// </summary>
        WireLessAmbientMonitor,
        /// <summary>
        /// 无线智能控制阀
        /// </summary>
        WireLessValve,
        /// <summary>
        /// 无线数字量监测
        /// </summary>
        WireLessDigit

    }

    public enum DeviceWarningState
    {
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

    /// <summary>
    /// 单通道设备计量单位
    /// </summary>
    public enum SingleChannelDataUnit
    {
        /// <summary>
        /// 压力
        /// </summary>
        MPa = 1,
        /// <summary>
        /// 压力
        /// </summary>
        Bar,
        /// <summary>
        /// 压力
        /// </summary>
        KPa,
        /// <summary>
        /// 液位
        /// </summary>
        M,
        /// <summary>
        /// 温度 摄氏度
        /// </summary>
        Centigrade,
        /// <summary>
        /// 流量 立方米每小时
        /// </summary>
        CubicMetresPerHour,
        /// <summary>
        /// 角度
        /// </summary>
        Degree
    }

    /// <summary>
    /// 多通道设备计量单位
    /// </summary>
    public enum MultiChannelDataUnit
    {
        /// <summary>
        /// 压力
        /// </summary>
        MPa = 1,
        /// <summary>
        /// 压力
        /// </summary>
        Bar,
        /// <summary>
        /// 压力
        /// </summary>
        KPa,
        /// <summary>
        /// 液位
        /// </summary>
        M,
        /// <summary>
        /// 温度 摄氏度
        /// </summary>
        Centigrade,
        /// <summary>
        /// 湿度
        /// </summary>
        RH,
        /// <summary>
        /// 流量
        /// </summary>
        CubicMetrePerHour,
        /// <summary>
        /// 流量
        /// </summary>
        CubitMetrePerMiniute,
        /// <summary>
        /// 电压 volt
        /// </summary>
        V,
        /// <summary>
        /// 电流 ampere
        /// </summary>
        A,
        /// <summary>
        /// 功率
        /// </summary>
        kwh,
        /// <summary>
        /// 烟感设备报警状态 0 正常 1 报警
        /// </summary>
        SmokeSensorWarning,
        /// <summary>
        /// 球阀开启状态
        /// </summary>
        BallValveOpenState,
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

    public enum DeviceBallValveControlState
    {
        CanNotBeControled,
        CanControledByTemperatureSensor,
        CanControledByPressureSensor
    }
    public enum DeviceBallValveOpenState
    {
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

    public enum ParamType
    {
        /// <summary>
        /// 0x01：采样间隔 4 数值，网络字节序，单位为秒 
        /// </summary>
        SampleInteral = 1,
        /// <summary>
        /// 0x02：发送间隔 4 数值，网络字节序，单位为秒 
        /// </summary>
        SendInterval,
        /// <summary>
        /// 0x03：服务器 IP 地址 N 字符型 
        /// </summary>
        ServerIP,
        /// <summary>
        /// 0x04：服务器端口 N 字符型
        /// </summary>
        ServerPort,
        /// <summary>
        /// 0x05：服务器 URL N 字符型
        /// </summary>
        ServerUrl,
        /// <summary>
        /// 0x06：单通道告警阈值下限 2 数值，网络字节序，数值单位参考《蓝牙配置使用手册.pdf》
        /// </summary>
        SingleChannelThresholdWarningLowerLimit,
        /// <summary>
        /// 0x07：单通道告警阈值上限 2 数值，网络字节序，数值单位参考《蓝 牙配置使用手册.pdf》 
        /// </summary>
        SingleChannelThresholdWarningUpperLimit,
        /// <summary>
        /// 0x08：单通道告警动态变化阈值 2 数值，网络字节序，数值单位参考《蓝 牙配置使用手册.pdf》 
        /// </summary>
        SingleChannelDeltaThreshold,
        /// <summary>
        /// 0x09：停止上报 1 数值。0：正常，1：停止上报
        /// </summary>
        StopSending,
        /// <summary>
        /// 0x0a：软件版本 4 主版本-子版本-修订版本-协议版本，详 见《版本定义规范》。
        /// </summary>
        SoftwareVersion,
        /// <summary>
        /// 0x0b：硬件版本 4 设备类型-芯片型号-模组型号-传感器 型号，详见《版本定义规范》。 
        /// </summary>
        HardwareVersion,
        /// <summary>
        /// 0x0c：卡号（IMSI） 16 字符型 
        /// </summary>
        IMSI,
        /// <summary>
        /// 0x0d：APN N 字符型
        /// </summary>
        APN,
        /// <summary>
        /// 0x0e：多通道告警阈值下限 4 数值，按照温度湿度、温度压力顺序， 数值单位参考《蓝牙配置使用手册.pdf》 
        /// </summary>
        MultiChannelThresholdWarningLowerLimit,
        /// <summary>
        /// 0x0f：多通道告警阈值上限 4 数值，按照温度湿度、温度压力顺序， 数值单位参考《蓝牙配置使用手册.pdf》
        /// </summary>
        MultiChannelThresholdWarningUpperLimit,
        /// <summary>
        /// 0x10：多通道告警动态变化阈值 4 数值，按照温度湿度、温度压力顺序， 数值单位参考《蓝牙配置使用手册.pdf》
        /// </summary>
        MultiChannelDeltaThreshold,
        /// <summary>
        /// 0x11：控制阀状态 1 见 1.4.1.6.1 定义 
        /// </summary>
        ValveState,
        /// <summary>
        /// 0x12：控制阀供暖季开始时间 6 见 1.4.1.3 定义
        /// </summary>
        ValveWarmStartDate,
        /// <summary>
        /// 0x13：控制阀供暖季结束时间 6 见 1.4.1.3 定义 
        /// </summary>
        ValveWarmEndDate,
        /// <summary>
        /// 0x14：控制阀调整期天数 1 数值。
        /// </summary>
        ValveControlDurationDayCount,
        /// <summary>
        /// 0x15：开关量状态 2 16 路开关量，每个 bit 代表一路的开关 状态
        /// </summary>
        SwitcherState

    }


}


