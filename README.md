# TPSL Raw Data Simulator
## OverView
this project implement some methods to serialize struct to binary format or deserialize struct from binary format.

## Usage

Accept ` MemberIndex ` attribute to a field to indicated the order when serializing the object. All the fileds and properties **MUST** have one index or you will get an exception.

Accept ` StructToRaw ` attribute to a struct to indicated the serializer uses BigEndian or LittleEndian.

Accept ` MarshalAs ` attribute to a field, you can:
1. ` UnmanagedType.ByValArray ` and ` ArraySubType ` to a array field, will get trancation or padding when serializing. Types `I1,U1,I2,U2,I4,U4,I8,U8,R4,R8` are supported now.
2. ` UnmanagedType ` to a basic value field, will get trancation or padding when serializing. Types `I1,U1,I2,U2,I4,U4,I8,U8,R4,R8` are supported now.

Use ` RawBinaryFormatter ` to serialize or deserialize.