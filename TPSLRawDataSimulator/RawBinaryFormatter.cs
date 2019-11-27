using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;

namespace TPSLRawDataSimulator
{
    public class RawBinaryFormatter
    {
        private ConcurrentDictionary<Type, Func<object, byte[]>> ExpressionStorage = new ConcurrentDictionary<Type, Func<object, byte[]>>();
        private ConcurrentDictionary<Type, Func<Stream, object>> DeserializeExpressionStorage = new ConcurrentDictionary<Type, Func<Stream, object>>();
        public object Deserialize(Stream serializationStream, Type type)
        {
            var structToRaw = type.GetCustomAttribute<StructToRawAttribute>();
            Func<Stream, object> func = null;
            if (!this.DeserializeExpressionStorage.TryGetValue(type, out func))
            {
                //sort the member info by member index.
                var mInfos = type.GetMembers(BindingFlags.Public | BindingFlags.Instance).Where(x => (x.MemberType & (MemberTypes.Field | MemberTypes.Property)) != 0).ToList();
                //var createInstanceMethodInfo = type.GetConstructor(Type.EmptyTypes);
                mInfos.Sort(new GenericComparer<MemberInfo, int>(x => x.GetCustomAttribute<MemberIndexAttribute>().Index));

                var getTypedObjFromStreamMethodInfo = typeof(BytesHelper).GetMethod("GetTypedObjectFromStream", BindingFlags.Static | BindingFlags.Public, Type.DefaultBinder, new[] { typeof(Stream), typeof(Type), typeof(bool) }, null);
                var getArrayFromStreamMethodInfo = typeof(BytesHelper).GetMethod("GetArrayFromStream", BindingFlags.Static | BindingFlags.Public, Type.DefaultBinder, new[] { typeof(Stream), typeof(Type), typeof(int), typeof(bool) }, null);
                var getArrayMarshalAsFromStreamMethodInfo = typeof(BytesHelper).GetMethod("GetArrayFromStream", BindingFlags.Static | BindingFlags.Public, Type.DefaultBinder, new[] { typeof(Stream), typeof(UnmanagedType), typeof(Type), typeof(int), typeof(bool) }, null);
                var getTypedObjMarshalAsFromStreamMethodInfo = typeof(BytesHelper).GetMethod("GetTypedObjectFromStream", BindingFlags.Static | BindingFlags.Public, Type.DefaultBinder, new[] { typeof(Stream), typeof(UnmanagedType), typeof(Type), typeof(bool) }, null);

                // block temporary variable expr
                List<Expression> inBlockExpressions = new List<Expression>();
                var streamVariableExpr = Expression.Variable(typeof(Stream), "stream");
                var returnObjExp = Expression.Variable(type, "returnObj");
                var variantLengthVariableParamExprList = new Dictionary<string, ParameterExpression>();

                // multi used variable expr
                var isBigEndianExpr = Expression.Constant(structToRaw == null ? !BitConverter.IsLittleEndian : structToRaw.Endian == Endian.BigEndian);

                inBlockExpressions.Add(Expression.Assign(returnObjExp, Expression.New(type)));

                foreach (MemberInfo mInfo in mInfos)
                {
                    Type mType = null;
                    PropertyInfo pInfo = null;
                    FieldInfo fInfo = null;
                    if (mInfo.MemberType == MemberTypes.Property)
                    {
                        pInfo = (mInfo as PropertyInfo);
                        mType = pInfo.PropertyType;
                    }
                    else if (mInfo.MemberType == MemberTypes.Field)
                    {
                        fInfo = (mInfo as FieldInfo);
                        mType = fInfo.FieldType;
                    }


                    //var mDeclareType = (pInfo != null ? pInfo.PropertyType : fInfo != null ? fInfo.FieldType : null);
                    var elementType = mType.GetElementType();
                    var marshalAs = mType.GetCustomAttribute<MarshalAsAttribute>();

                    if (mType.IsArray)
                    {
                        var keyPrefix = (pInfo != null ? pInfo.Name : fInfo != null ? fInfo.Name : "");
                        var variableKey = keyPrefix + "Length";
                        if (variantLengthVariableParamExprList.TryGetValue(variableKey, out var variableLengthExpr))
                        {
                            if (marshalAs == null)
                                inBlockExpressions.Add(Expression.Assign(Expression.MakeMemberAccess(returnObjExp, mInfo), Expression.Convert(Expression.Call(null, getArrayFromStreamMethodInfo, streamVariableExpr, Expression.Constant(elementType, typeof(Type)), variableLengthExpr, isBigEndianExpr), mType)));
                            else
                                inBlockExpressions.Add(Expression.Assign(Expression.MakeMemberAccess(returnObjExp, mInfo), Expression.Convert(Expression.Call(null, getArrayMarshalAsFromStreamMethodInfo, streamVariableExpr, Expression.Constant(marshalAs.MarshalType, typeof(UnmanagedType)), Expression.Constant(elementType, typeof(Type)), variableLengthExpr, isBigEndianExpr), mType)));
                        }
                        else if (mType.GetCustomAttribute<MemberIndexAttribute>() != null && mType.GetCustomAttribute<MemberIndexAttribute>().SizeCount > 0)
                        {
                            variableLengthExpr = Expression.Variable(typeof(uint), variableKey);
                            var variableLength = mType.GetCustomAttribute<MemberIndexAttribute>().SizeCount;    
                            variantLengthVariableParamExprList.Add(variableKey, variableLengthExpr);

                            if (marshalAs == null)
                                inBlockExpressions.Add(Expression.Assign(Expression.MakeMemberAccess(returnObjExp, mInfo), Expression.Convert(Expression.Call(null, getArrayFromStreamMethodInfo, streamVariableExpr, Expression.Constant(elementType, typeof(Type)), variableLengthExpr, isBigEndianExpr), mType)));
                            else
                                inBlockExpressions.Add(Expression.Assign(Expression.MakeMemberAccess(returnObjExp, mInfo), Expression.Convert(Expression.Call(null, getArrayMarshalAsFromStreamMethodInfo, streamVariableExpr, Expression.Constant(marshalAs.MarshalType, typeof(UnmanagedType)), Expression.Constant(elementType, typeof(Type)), variableLengthExpr, isBigEndianExpr), mType)));
                        }
                        else
                        {
                            throw new InvalidOperationException($"{variableKey} must defined index before {keyPrefix}");
                        }
                    }
                    else if (mInfo.GetCustomAttribute<MemberIndexAttribute>().LengthTo is string mName && !string.IsNullOrEmpty(mName))
                    {
                        var variablelength = Expression.Variable(mType, mName + "Length");
                        variantLengthVariableParamExprList.Add(mName + "Length", variablelength);
                        inBlockExpressions.Add(Expression.Assign(variablelength, Expression.Convert(Expression.Call(null, getTypedObjFromStreamMethodInfo, streamVariableExpr, Expression.Constant(mType), Expression.Constant(structToRaw == null ? !BitConverter.IsLittleEndian : structToRaw.Endian == Endian.BigEndian)), mType)));
                        inBlockExpressions.Add(Expression.Assign(Expression.MakeMemberAccess(returnObjExp, mInfo), variablelength));
                    }
                    else
                    {
                        if (marshalAs == null)
                            inBlockExpressions.Add(Expression.Assign(Expression.MakeMemberAccess(returnObjExp, mInfo), Expression.Convert(Expression.Call(null, getTypedObjFromStreamMethodInfo, streamVariableExpr, Expression.Constant(mType, typeof(Type)), isBigEndianExpr), mType)));
                        else
                            inBlockExpressions.Add(Expression.Assign(Expression.MakeMemberAccess(returnObjExp, mInfo), Expression.Convert(Expression.Call(null, getTypedObjMarshalAsFromStreamMethodInfo, streamVariableExpr, Expression.Constant(marshalAs.MarshalType, typeof(UnmanagedType)), Expression.Constant(mType, typeof(Type)), isBigEndianExpr), mType)));
                    }
                }
                var blockDefineContext = variantLengthVariableParamExprList.Values.ToArray().Concat(new[] { returnObjExp });
                inBlockExpressions.Add(Expression.Convert(returnObjExp, typeof(object)));
                var block = Expression.Block(blockDefineContext, inBlockExpressions);
                func = Expression.Lambda<Func<Stream, object>>(block, new[] { streamVariableExpr }).Compile();
                this.DeserializeExpressionStorage.TryAdd(type, func);
            }
            var result = func.Invoke(serializationStream);
            return result;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="serializationStream"></param>
        /// <param name="obj"></param>
        public void Serialize(Stream serializationStream, object obj)
        {
            var type = obj.GetType();
            var structToRaw = type.GetCustomAttribute<StructToRawAttribute>();

            Func<object, byte[]> func = null;
            if (!ExpressionStorage.TryGetValue(type, out func))
            {
                var pInfos = obj.GetType().GetMembers(BindingFlags.Public | BindingFlags.Instance).Where(x => (x.MemberType & (MemberTypes.Field | MemberTypes.Property)) != 0).ToList();
                List<Expression> inBlockExpressions = new List<Expression>();
                var variableExp = Expression.Variable(typeof(List<>).MakeGenericType(typeof(byte)), "buffer");
                var unBoxedDataObjectExp = Expression.Variable(obj.GetType(), "unBoxedDataObject");
                var objParam = Expression.Variable(typeof(object), "dataObject");

                inBlockExpressions.Add(Expression.Assign(unBoxedDataObjectExp, Expression.Convert(objParam, type)));

                inBlockExpressions.Add(Expression.Assign(variableExp, Expression.New(typeof(List<byte>).GetConstructor(Type.EmptyTypes))));
                pInfos.Sort(new GenericComparer<MemberInfo, int>(x => x.GetCustomAttribute<MemberIndexAttribute>().Index));
                foreach (var memberInfo in pInfos)
                {
                    Type memberReturnType = null;

                    if ((memberInfo.MemberType & MemberTypes.Field) == MemberTypes.Field)
                        memberReturnType = (memberInfo as FieldInfo).FieldType;
                    else if ((memberInfo.MemberType & MemberTypes.Property) == MemberTypes.Property)
                        memberReturnType = (memberInfo as PropertyInfo).PropertyType;
                    else
                        continue;

                    var marshal = memberInfo.GetCustomAttributes<MarshalAsAttribute>().SingleOrDefault();
                    if (memberReturnType.IsArray)
                    {
                        //for array memeber that indiciated the marshal value.
                        if (marshal != null)
                        {
                            if (marshal.Value == UnmanagedType.ByValArray)
                            {
                                inBlockExpressions.Add(Expression.Call(variableExp, typeof(List<>).MakeGenericType(typeof(byte)).GetMethod("AddRange")
                                    , Expression.Call(null, typeof(BytesHelper).GetMethod("ArrayToBytes", BindingFlags.Public | BindingFlags.Static, Type.DefaultBinder, new[] { typeof(Array), typeof(UnmanagedType), typeof(bool) }, null)
                                    , Expression.PropertyOrField(unBoxedDataObjectExp, memberInfo.Name)
                                    , Expression.Constant(marshal.ArraySubType)
                                    , Expression.Constant(structToRaw == null ? !BitConverter.IsLittleEndian : structToRaw.Endian == Endian.BigEndian)
                                    )
                                    ));

                                //if (marshal.ArraySubType == UnmanagedType.I1 || marshal.ArraySubType == UnmanagedType.U1)
                                //{
                                //    inBlockExpressions.Add(ExpressionHelper.ExpForEach(

                                //        , Expression.Parameter(propertyInfo.FieldType.GetElementType(), "current")
                                //        , Expression.Call(variableExp, typeof(List<>).MakeGenericType(typeof(byte)).GetMethod("Add"), Expression.Convert(Expression.Parameter(propertyInfo.FieldType.GetElementType(), "current"), typeof(byte)))));
                                //        , Expression.Call(variableExp, typeof(List<>).MakeGenericType(typeof(byte)).GetMethod("Add"), Expression.Call(null, typeof(BitConverter).GetMethod("GetBytes", BindingFlags.Static | BindingFlags.Public, Type.DefaultBinder, new[] { typeof(byte) }, null), Expression.Convert(Expression.Parameter(propertyInfo.FieldType.GetElementType(), "current"), typeof(byte))))));

                                //}
                            }
                        }
                        //for array member not indiciated the marshal value, convert them as define type, and the specified endian.
                        else if (structToRaw != null)
                        {
                            inBlockExpressions.Add(Expression.Call(variableExp, typeof(List<>).MakeGenericType(typeof(byte)).GetMethod("AddRange")
                                    , Expression.Call(null, typeof(BytesHelper).GetMethod("ArrayToBytes", BindingFlags.Public | BindingFlags.Static, Type.DefaultBinder, new[] { typeof(Array), typeof(bool) }, null)
                                    , Expression.PropertyOrField(unBoxedDataObjectExp, memberInfo.Name)
                                    , Expression.Constant(structToRaw == null ? !BitConverter.IsLittleEndian : structToRaw.Endian == Endian.BigEndian)
                                    )
                                    ));
                        }
                        //for array member not indiciated the marshal value, convert them as define type, and the local system endian.
                        else
                        {
                            inBlockExpressions.Add(Expression.Call(variableExp, typeof(List<>).MakeGenericType(typeof(byte)).GetMethod("AddRange"), Expression.Call(null, typeof(BytesHelper).GetMethod("ArrayToBytes", BindingFlags.Public | BindingFlags.Static, Type.DefaultBinder, new[] { typeof(object) }, null), Expression.PropertyOrField(unBoxedDataObjectExp, memberInfo.Name))));
                        }
                    }
                    else
                    {
                        //                        if (propertyInfo.PropertyType.GetInterfaces().Any(x=>x.GetGenericTypeDefinition() == typeof(IEnumerable<>)))
                        //                        {
                        //                            var ArrayToBytesMethod = typeof(BytesHelper).GetMethod("IEnumerableToBytes<>", BindingFlags.Static).MakeGenericMethod(propertyInfo.PropertyType.GenericTypeArguments[0]);
                        //                            
                        //                            inBlockExpressions.Add(Expression.Call(GetVarbleExpression(), typeof(List<>).MakeGenericType(typeof(byte)).GetMethod("AddRange"),Expression.Call(null, ArrayToBytesMethod)));
                        //                        }
                        if (marshal != null)
                        {
                            if (marshal.Value == UnmanagedType.U1 || marshal.Value == UnmanagedType.I1)
                            {
                                inBlockExpressions.Add(Expression.Call(variableExp, typeof(List<>).MakeGenericType(typeof(byte)).GetMethod("Add"), Expression.Convert(Expression.PropertyOrField(unBoxedDataObjectExp, memberInfo.Name), typeof(byte))));
                            }
                            if (marshal.Value == UnmanagedType.U2 || marshal.Value == UnmanagedType.I2)
                            {
                                var tempConvertType = typeof(short);
                                inBlockExpressions.Add(Expression.Call(variableExp, typeof(List<>).MakeGenericType(typeof(byte)).GetMethod("AddRange"), Expression.Call(null, typeof(BytesHelper).GetMethod("GetBytes", BindingFlags.Static | BindingFlags.Public, Type.DefaultBinder, new[] { typeof(object), typeof(bool) }, null), Expression.Convert(Expression.Convert(Expression.PropertyOrField(unBoxedDataObjectExp, memberInfo.Name), tempConvertType), typeof(object)), Expression.Constant(structToRaw == null ? !BitConverter.IsLittleEndian : structToRaw.Endian == Endian.BigEndian))));
                            }
                            if (marshal.Value == UnmanagedType.U4 || marshal.Value == UnmanagedType.I4)
                            {
                                var tempConvertType = typeof(int);
                                inBlockExpressions.Add(Expression.Call(variableExp, typeof(List<>).MakeGenericType(typeof(byte)).GetMethod("AddRange"), Expression.Call(null, typeof(BytesHelper).GetMethod("GetBytes", BindingFlags.Static | BindingFlags.Public, Type.DefaultBinder, new[] { typeof(object), typeof(bool) }, null), Expression.Convert(Expression.Convert(Expression.PropertyOrField(unBoxedDataObjectExp, memberInfo.Name), tempConvertType), typeof(object)), Expression.Constant(structToRaw == null ? !BitConverter.IsLittleEndian : structToRaw.Endian == Endian.BigEndian))));
                            }
                            if (marshal.Value == UnmanagedType.U8 || marshal.Value == UnmanagedType.I8)
                            {
                                var tempConvertType = typeof(long);
                                inBlockExpressions.Add(Expression.Call(variableExp, typeof(List<>).MakeGenericType(typeof(byte)).GetMethod("AddRange"), Expression.Call(null, typeof(BytesHelper).GetMethod("GetBytes", BindingFlags.Static | BindingFlags.Public, Type.DefaultBinder, new[] { typeof(object), typeof(bool) }, null), Expression.Convert(Expression.Convert(Expression.PropertyOrField(unBoxedDataObjectExp, memberInfo.Name), tempConvertType), typeof(object)), Expression.Constant(structToRaw == null ? !BitConverter.IsLittleEndian : structToRaw.Endian == Endian.BigEndian))));
                            }
                            if (marshal.Value == UnmanagedType.R4)
                            {
                                var tempConvertType = typeof(float);
                                inBlockExpressions.Add(Expression.Call(variableExp, typeof(List<>).MakeGenericType(typeof(byte)).GetMethod("AddRange"), Expression.Call(null, typeof(BytesHelper).GetMethod("GetBytes", BindingFlags.Static | BindingFlags.Public, Type.DefaultBinder, new[] { typeof(object), typeof(bool) }, null), Expression.Convert(Expression.Convert(Expression.PropertyOrField(unBoxedDataObjectExp, memberInfo.Name), tempConvertType), typeof(object)), Expression.Constant(structToRaw == null ? !BitConverter.IsLittleEndian : structToRaw.Endian == Endian.BigEndian))));
                            }
                            if (marshal.Value == UnmanagedType.R8)
                            {
                                var tempConvertType = typeof(double);
                                inBlockExpressions.Add(Expression.Call(variableExp, typeof(List<>).MakeGenericType(typeof(byte)).GetMethod("AddRange"), Expression.Call(null, typeof(BytesHelper).GetMethod("GetBytes", BindingFlags.Static | BindingFlags.Public, Type.DefaultBinder, new[] { typeof(object), typeof(bool) }, null), Expression.Convert(Expression.Convert(Expression.PropertyOrField(unBoxedDataObjectExp, memberInfo.Name), tempConvertType), typeof(object)), Expression.Constant(structToRaw == null ? !BitConverter.IsLittleEndian : structToRaw.Endian == Endian.BigEndian))));
                            }
                        }
                        else
                        {
                            inBlockExpressions.Add(Expression.Call(variableExp, typeof(List<>).MakeGenericType(typeof(byte)).GetMethod("AddRange"), Expression.Call(null, typeof(BytesHelper).GetMethod("GetBytes", BindingFlags.Static | BindingFlags.Public, Type.DefaultBinder, new[] { typeof(object), typeof(bool) }, null), Expression.Convert(Expression.PropertyOrField(unBoxedDataObjectExp, memberInfo.Name), typeof(object)), Expression.Constant(structToRaw == null ? !BitConverter.IsLittleEndian : structToRaw.Endian == Endian.BigEndian))));
                        }

                    }

                }
                inBlockExpressions.Add(Expression.Call(variableExp, typeof(List<>).MakeGenericType(typeof(byte)).GetMethod("ToArray")));
                var exp = Expression.Lambda<Func<object, byte[]>>(Expression.Block(new ParameterExpression[] { variableExp, unBoxedDataObjectExp }, inBlockExpressions), objParam);
                func = exp.Compile();
                ExpressionStorage.TryAdd(type, func);
            }

            var resultBytes = func.Invoke(obj);
            serializationStream.Write(resultBytes);
        }
    }


    public static class ExpressionHelper
    {
        public static Expression ExpForEach(Expression collection, ParameterExpression loopVar, Expression loopContent)
        {
            var elementType = loopVar.Type;
            var enumerableType = typeof(IEnumerable<>).MakeGenericType(elementType);
            var enumeratorType = typeof(IEnumerator<>).MakeGenericType(elementType);

            var enumeratorVar = Expression.Variable(enumeratorType, "enumerator");
            var getEnumeratorCall = Expression.Call(collection, enumerableType.GetMethod("GetEnumerator"));
            var enumeratorAssign = Expression.Assign(enumeratorVar, getEnumeratorCall);

            // The MoveNext method's actually on IEnumerator, not IEnumerator<T>
            var moveNextCall = Expression.Call(enumeratorVar, typeof(IEnumerator).GetMethod("MoveNext"));

            var breakLabel = Expression.Label("LoopBreak");

            var loop = Expression.Block(new[] { enumeratorVar },
                enumeratorAssign,
                Expression.Loop(
                    Expression.IfThenElse(
                        Expression.Equal(moveNextCall, Expression.Constant(true)),
                        Expression.Block(new[] { loopVar },
                            Expression.Assign(loopVar, Expression.Property(enumeratorVar, "Current")),
                            loopContent
                        ),
                        Expression.Break(breakLabel)
                    ),
                    breakLabel)
            );

            return loop;
        }
    }
}
