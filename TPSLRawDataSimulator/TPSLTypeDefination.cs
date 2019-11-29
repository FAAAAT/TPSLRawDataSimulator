using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using TPSLRawDataSimulator;

namespace TPSLRawDataSimulator.TypeDefinations
{
    public class TPSLDeserializationDataContext
    {
        public RawBinaryFormatter Formatter;
        public TPSLWrapper ContextObject;
    }

    [StructToRaw(Endian = Endian.BigEndian)]
    public class TPSLWrapper
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
        public TPSLDataBody DataBody;

        public TPSLWrapper() { }
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
            var context = new TPSLDeserializationDataContext() { ContextObject = this,Formatter = formatter };
            this.DataBody = new TPSLDataBody(this.Data);
            this.DataBody.FillObject(context);
        }
    }

    public class TPSLDataBody
    {
        public TPSLDataSegment_Type1 DataBodyType1;
        public TPSLDataSegment_Type2 DataBodyType2;
        public TPSLDataSegment_Type3or4 DataBodyType3;
        public TPSLDataSegment_Type3or4 DataBodyType4;
        // type 5 is software update data.
        public TPSLDataSegment_Type6 DataBodyType6;
        public byte[] Datas;
        public TPSLDataBody(byte[] datas)
        {
            this.DataBodyType1 = null;
            this.DataBodyType2 = null;
            this.DataBodyType3 = null;
            this.DataBodyType4 = null;
            this.DataBodyType6 = null;
            this.Datas = datas;
        }
        public void FillObject(TPSLDeserializationDataContext context) {
            var type = context.ContextObject.Type;
            var formatter = context.Formatter;
            if (type == FrameType.Data)
            {
                this.DataBodyType1 = (TPSLDataSegment_Type1)formatter.Deserialize(new MemoryStream(this.Datas), typeof(TPSLDataSegment_Type1));
                this.DataBodyType1.FillObject(context);

            }
            else if (type == FrameType.GetConfig)
            {
            }
            else if (type == FrameType.ResponseConfig)
            {
            }
            else if (type == FrameType.Result)
            {

                this.DataBodyType6 = (TPSLDataSegment_Type6)formatter.Deserialize(new MemoryStream(this.Datas), typeof(TPSLDataSegment_Type6));
            }
            else
                throw new Exception("Unknown frame type " + type);

        }
    }

    public class TPSLDataUnit
    {
        public TPSLSingleChannelDeviceSampleDataUnit SingleChannelDataUnit;
        public TPSLMultiChannelDeviceSampleDataUnit MultiChannelDataUnit;
        public TPSLSmokeSensor SmokeSensor;
        public TPSLBallValveDevice BallValveDevice;
        public TPSLMultiSwitcher SwitcherDataUnit;
        public byte[] Datas;

        public TPSLDataUnit( byte[] datas)
        {
            this.SingleChannelDataUnit = null;
            this.MultiChannelDataUnit = null;
            this.SmokeSensor = null;
            this.BallValveDevice = null;
            this.SwitcherDataUnit = null;
            this.Datas = datas;
        }

        public void FillObject(TPSLDeserializationDataContext context) {
            var type = context.ContextObject.DataBody.DataBodyType1.DeviceType;
            if (type == DeviceType.WireLessPressure || type == DeviceType.WireLessLquidLevel || type == DeviceType.WireLessTemperature || type == DeviceType.WireLessFireControl)
            {
                this.SingleChannelDataUnit = new TPSLSingleChannelDeviceSampleDataUnit(this.Datas);
            }
            else if (type == DeviceType.WireLessTempAndHumidity || type == DeviceType.WireLessMultiChannel)
            {
                this.MultiChannelDataUnit = new TPSLMultiChannelDeviceSampleDataUnit(this.Datas);
                this.MultiChannelDataUnit.FillObject(context);
            }
            else if (type == DeviceType.WireLessAmbientMonitor)
            {
                this.SmokeSensor = (TPSLSmokeSensor)context.Formatter.Deserialize(new MemoryStream(this.Datas), typeof(TPSLSmokeSensor));
            }
            else if (type == DeviceType.WireLessValve)
            {
                this.BallValveDevice = new TPSLBallValveDevice(this.Datas[0]);
            }
            else if (type == DeviceType.WireLessDigit)
            {
                this.SwitcherDataUnit = new TPSLMultiSwitcher(this.Datas);
            }
        }
    }


    /// <summary>
    /// type 0x01 data_body
    /// </summary>
    [StructToRaw(Endian = Endian.LittleEndian)]
    public class TPSLDataSegment_Type1
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

        public TPSLDataSegment_Type1() { }
        public void FillObject(TPSLDeserializationDataContext context)
        {
            this.DataUnit = new TPSLDataUnit(this.Sample_Data);
            this.FillSendTime();
            this.DataUnit.FillObject(context);
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
                year = (DateTime.Now.Year / 100 - 1)*100 + results[0];
            }
            else
            {
                year = (DateTime.Now.Year / 100)*100 + results[0];
            }
            this.SendTime = new DateTime(year, results[1], results[2], results[3], results[4], results[5]);
        }
    }

    /// <summary>
    /// for device_type 0x01,0x02,0x03,0x04
    /// </summary>
    public class TPSLSingleChannelDeviceSampleDataUnit
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
                            // I think this protocal is bullshit! Does the author do not know the half precision float It can resolve this problem with just 3 bytes and no fucking BCD encode!
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
                    dataData = dataData.Insert(dataData.Length - fractionalPartCount, ".");
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
    /// this implement is temporary.
    /// </summary>
    public class TPSLMultiChannelDeviceSampleDataUnit
    {
        public byte ChannelCounts;
        public byte[] Datas;
        public DataUnitPacket[] Packets;
        public TPSLMultiChannelDeviceSampleDataUnit(byte[] bytes)
        {
            this.ChannelCounts = bytes[0];
            this.Datas = bytes[1..bytes.Length];
            Packets = null;
        }
        public void FillObject(TPSLDeserializationDataContext context) {
            List<DataUnitPacket> result = new List<DataUnitPacket>();
            Span<byte> span = new Span<byte>(this.Datas);
            while (span.Length > 0)
            {
                var data = span[0..span[0]];
                result.Add(new DataUnitPacket(data.ToArray(), context));
                span = span[(span[0]+1)..];
            }
            this.Packets = result.ToArray();
        }
    }

    public class DataUnitPacket
    {
        public byte Length;
        public byte ChannelId;
        public MultiChannelDataUnit ValueUnit;
        public bool Signed;
        public byte FloatPrecision;
        /// <summary>
        /// this state has value.
        /// </summary>
        public DeviceWarningState? WarningState;
        /// <summary>
        /// this state has no value.
        /// </summary>
        public SmokeWarningState? SmokeDeviceWarningState;
        /// <summary>
        /// this state has no value.
        /// </summary>
        public DeviceBallValveOpenState? BallValveDeviceOpenState;
        public double? Value;
        public DataUnitPacket(byte[] bytes, TPSLDeserializationDataContext context)
        {
            this.Length = bytes[0];
            this.ChannelId = bytes[1];
            this.ValueUnit = (MultiChannelDataUnit)bytes[2];
            if (bytes[3] > 1)
                throw new Exception($"corruption data detected. need bool , actually is: {bytes[3]}");
            this.Signed = bytes[3] == 0;
            this.FloatPrecision = bytes[4];
            var contextDeviceType = context.ContextObject.DataBody.DataBodyType1.DeviceType;
            this.BallValveDeviceOpenState = null;
            this.SmokeDeviceWarningState = null;
            this.Value = null;
            this.WarningState = null;
            if (contextDeviceType == DeviceType.WireLessValve)
            {
                this.BallValveDeviceOpenState = (DeviceBallValveOpenState)bytes[0];
            }
            else if (contextDeviceType == DeviceType.WireLessAmbientMonitor)
            {
                this.SmokeDeviceWarningState = (SmokeWarningState)bytes[0];
            }
            else
            {
                this.WarningState = (DeviceWarningState)bytes[5];
                var remainLength = bytes.Length - 6;
                var valueStr = BytesHelper.BCDToInt32(bytes[6..bytes.Length]).ToString("0000000000000");
                this.Value = double.Parse(valueStr.Insert(valueStr.Length - this.FloatPrecision, "."));
            }

        }
    }

    [StructToRaw(Endian = Endian.LittleEndian)]
    /// <summary>
    /// for device_type 0x07
    /// </summary>
    public class TPSLSmokeSensor
    {
        [MemberIndex(Index = 0)]
        [MarshalAs(UnmanagedType.U1)]
        public SmokeWarningState WarningState;
        [MarshalAs(UnmanagedType.U1)]
        public SmokeDevicePowerState PowerState;
        public TPSLSmokeSensor() { }
    }

    /// <summary>
    /// for device_type 0x08
    /// </summary>
    public class TPSLBallValveDevice
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
    public class TPSLMultiSwitcher
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
    public class TPSLDataSegment_Type2
    {

    }

    /// <summary>
    /// type 3 or type 4 data_body
    /// </summary>
    [StructToRaw(Endian = Endian.LittleEndian)]
    public class TPSLDataSegment_Type3or4
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

        public TPSLDataSegment_Type3or4() { }
    }

    public class ParamUnit
    {
        public ParamType ParamType;
        public int ParamValueLen;
        public byte[] ParamValue;
    }

    /// <summary>
    /// this object for paramunits values container
    /// </summary>
    public class ParamUnitsData
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
    public class TPSLDataSegment_Type6
    {
        [MemberIndex(Index = 0)]
        public ExecResult EXEC_RESULT;
        public TPSLDataSegment_Type6() { }
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

    /// <summary>
    /// MultiChannel and SingleChannel both use this.
    /// </summary>
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

    /// <summary>
    /// MultiChannel and SingleChannel both use this.
    /// </summary>
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

    /// <summary>
    /// MultiChannel and SingleChannel both use this.
    /// </summary>
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
