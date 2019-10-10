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
    public class RawBinaryFormatter : IFormatter
    {
        private ConcurrentDictionary<Type, Func<object, byte[]>> ExpressionStorage = new ConcurrentDictionary<Type, Func<object, byte[]>>();
        public object Deserialize(Stream serializationStream)
        {
            throw new NotImplementedException();
        }

        public void Serialize(Stream serializationStream, object graph)
        {
            var type = graph.GetType();
            var structToRaw = type.GetCustomAttribute<StructToRawAttribute>();

            Func<object, byte[]> func = null;
            if (!ExpressionStorage.TryGetValue(type, out func))
            {
                var pInfos = graph.GetType().GetMembers(BindingFlags.Public|BindingFlags.Instance).Where(x=>(x.MemberType & (MemberTypes.Field|MemberTypes.Property)) != 0).ToList();
                List<Expression> inBlockExpressions = new List<Expression>();
                var variableExp = Expression.Variable(typeof(List<>).MakeGenericType(typeof(byte)), "buffer");
                var unBoxedDataObjectExp = Expression.Variable(graph.GetType(), "unBoxedDataObject");
                //                inBlockExpressions.Add(Expression.Assign(unBoxedDataObjectExp,Expression.Convert(Expression.Parameter(typeof(object), "dataObject"),type)));
                inBlockExpressions.Add(Expression.Assign(unBoxedDataObjectExp, Expression.Constant(graph, type)));
                //                inBlockExpressions.Add(variableExp);
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

                        if (marshal != null)
                        {
                            if (marshal.Value == UnmanagedType.ByValArray)
                            {
                                inBlockExpressions.Add(Expression.Call(variableExp, typeof(List<>).MakeGenericType(typeof(byte)).GetMethod("AddRange")
                                    , Expression.Call(null, typeof(BytesHelper).GetMethod("ArrayToBytes", BindingFlags.Public | BindingFlags.Static, Type.DefaultBinder, new[] { typeof(Array), typeof(UnmanagedType), typeof(bool) }, null)
                                    , Expression.PropertyOrField(unBoxedDataObjectExp, memberInfo.Name)
                                    , Expression.Constant(marshal.ArraySubType)
                                    , Expression.Constant(structToRaw == null || structToRaw.Endian == Endian.BigEndian)
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
                                inBlockExpressions.Add(Expression.Call(variableExp, typeof(List<>).MakeGenericType(typeof(byte)).GetMethod("Add"), Expression.ConvertChecked(Expression.PropertyOrField(Expression.Parameter(type, "unBoxedDataObject"), memberInfo.Name), typeof(byte))));
                            }
                            if (marshal.Value == UnmanagedType.U2 || marshal.Value == UnmanagedType.I2)
                            {
                                var tempConvertType = typeof(short);
                                inBlockExpressions.Add(Expression.Call(variableExp, typeof(List<>).MakeGenericType(typeof(byte)).GetMethod("AddRange"), Expression.Call(null, typeof(BitConverter).GetMethod("GetBytes", BindingFlags.Static | BindingFlags.Public, Type.DefaultBinder, new[] { tempConvertType }, null), Expression.ConvertChecked(Expression.PropertyOrField(Expression.Parameter(type, "unBoxedDataObject"), memberInfo.Name), tempConvertType))));
                            }
                            if (marshal.Value == UnmanagedType.U4 || marshal.Value == UnmanagedType.I4)
                            {
                                var tempConvertType = typeof(int);
                                inBlockExpressions.Add(Expression.Call(variableExp, typeof(List<>).MakeGenericType(typeof(byte)).GetMethod("AddRange"), Expression.Call(null, typeof(BitConverter).GetMethod("GetBytes", BindingFlags.Static | BindingFlags.Public, Type.DefaultBinder, new[] { tempConvertType }, null), Expression.ConvertChecked(Expression.PropertyOrField(Expression.Parameter(type, "unBoxedDataObject"), memberInfo.Name), tempConvertType))));
                            }
                            if (marshal.Value == UnmanagedType.U8 || marshal.Value == UnmanagedType.I8)
                            {
                                var tempConvertType = typeof(long);
                                inBlockExpressions.Add(Expression.Call(variableExp, typeof(List<>).MakeGenericType(typeof(byte)).GetMethod("AddRange"), Expression.Call(null, typeof(BitConverter).GetMethod("GetBytes", BindingFlags.Static | BindingFlags.Public, Type.DefaultBinder, new[] { tempConvertType }, null), Expression.ConvertChecked(Expression.PropertyOrField(Expression.Parameter(type, "unBoxedDataObject"), memberInfo.Name), tempConvertType))));
                            }
                        }
                        else
                        {
                            if (memberReturnType == typeof(int))
                            {

                                inBlockExpressions.Add(
                                Expression.Call(
                                    variableExp
                                    , typeof(List<>).MakeGenericType(typeof(byte)).GetMethod("AddRange")
                                    , Expression.Call(
                                        null
                                        , typeof(BitConverter).GetMethod("GetBytes", BindingFlags.Static | BindingFlags.Public, Type.DefaultBinder, new[] { typeof(int) }, null)
                                        , Expression.PropertyOrField(unBoxedDataObjectExp, memberInfo.Name))
                                    )
                                );
                            }
                            if (memberReturnType == typeof(uint))
                            {
                                inBlockExpressions.Add(Expression.Call(variableExp, typeof(List<>).MakeGenericType(typeof(byte)).GetMethod("AddRange"), Expression.Call(null, typeof(BitConverter).GetMethod("GetBytes", BindingFlags.Static | BindingFlags.Public, Type.DefaultBinder, new[] { typeof(uint) }, null), Expression.PropertyOrField(unBoxedDataObjectExp, memberInfo.Name))));
                            }
                            if (memberReturnType == typeof(short))
                            {
                                inBlockExpressions.Add(Expression.Call(variableExp, typeof(List<>).MakeGenericType(typeof(byte)).GetMethod("AddRange"), Expression.Call(null, typeof(BitConverter).GetMethod("GetBytes", BindingFlags.Static | BindingFlags.Public, Type.DefaultBinder, new[] { typeof(short) }, null), Expression.PropertyOrField(unBoxedDataObjectExp, memberInfo.Name))));
                            }
                            if (memberReturnType == typeof(ushort))
                            {
                                inBlockExpressions.Add(Expression.Call(variableExp, typeof(List<>).MakeGenericType(typeof(byte)).GetMethod("AddRange"), Expression.Call(null, typeof(BitConverter).GetMethod("GetBytes", BindingFlags.Static | BindingFlags.Public, Type.DefaultBinder, new[] { typeof(ushort) }, null), Expression.PropertyOrField(unBoxedDataObjectExp, memberInfo.Name))));
                            }
                            if (memberReturnType == typeof(long))
                            {
                                inBlockExpressions.Add(Expression.Call(variableExp, typeof(List<>).MakeGenericType(typeof(byte)).GetMethod("AddRange"), Expression.Call(null, typeof(BitConverter).GetMethod("GetBytes", BindingFlags.Static | BindingFlags.Public, Type.DefaultBinder, new[] { typeof(long) }, null), Expression.PropertyOrField(unBoxedDataObjectExp, memberInfo.Name))));
                            }
                            if (memberReturnType == typeof(ulong))
                            {
                                inBlockExpressions.Add(Expression.Call(variableExp, typeof(List<>).MakeGenericType(typeof(byte)).GetMethod("AddRange"), Expression.Call(null, typeof(BitConverter).GetMethod("GetBytes", BindingFlags.Static | BindingFlags.Public, Type.DefaultBinder, new[] { typeof(ulong) }, null), Expression.PropertyOrField(unBoxedDataObjectExp, memberInfo.Name))));
                            }
                            if (memberReturnType == typeof(char))
                            {
                                inBlockExpressions.Add(Expression.Call(variableExp, typeof(List<>).MakeGenericType(typeof(byte)).GetMethod("AddRange"), Expression.Call(null, typeof(BitConverter).GetMethod("GetBytes", BindingFlags.Static | BindingFlags.Public, Type.DefaultBinder, new[] { typeof(char) }, null), Expression.PropertyOrField(unBoxedDataObjectExp, memberInfo.Name))));
                            }
                            if (memberReturnType == typeof(byte))
                            {
                                inBlockExpressions.Add(Expression.Call(variableExp, typeof(List<>).MakeGenericType(typeof(byte)).GetMethod("Add"), Expression.PropertyOrField(unBoxedDataObjectExp, memberInfo.Name)));
                            }
                            if (memberReturnType == typeof(float))
                            {
                                inBlockExpressions.Add(Expression.Call(variableExp, typeof(List<>).MakeGenericType(typeof(byte)).GetMethod("AddRange"), Expression.Call(null, typeof(BitConverter).GetMethod("GetBytes", BindingFlags.Static | BindingFlags.Public, Type.DefaultBinder, new[] { typeof(float) }, null), Expression.PropertyOrField(unBoxedDataObjectExp, memberInfo.Name))));
                            }
                            if (memberReturnType == typeof(double))
                            {
                                inBlockExpressions.Add(Expression.Call(variableExp, typeof(List<>).MakeGenericType(typeof(byte)).GetMethod("AddRange"), Expression.Call(null, typeof(BitConverter).GetMethod("GetBytes", BindingFlags.Static | BindingFlags.Public, Type.DefaultBinder, new[] { typeof(double) }, null), Expression.PropertyOrField(unBoxedDataObjectExp, memberInfo.Name))));
                            }
                            if (memberReturnType == typeof(decimal))
                            {
                                inBlockExpressions.Add(Expression.Call(variableExp, typeof(List<>).MakeGenericType(typeof(byte)).GetMethod("AddRange"), Expression.Call(null, typeof(BitConverter).GetMethod("GetBytes", BindingFlags.Static | BindingFlags.Public, Type.DefaultBinder, new[] { typeof(decimal) }, null), Expression.PropertyOrField(unBoxedDataObjectExp, memberInfo.Name))));
                            }
                            if (memberReturnType == typeof(bool))
                            {
                                inBlockExpressions.Add(Expression.Call(variableExp, typeof(List<>).MakeGenericType(typeof(byte)).GetMethod("AddRange"), Expression.Call(null, typeof(BitConverter).GetMethod("GetBytes", BindingFlags.Static | BindingFlags.Public, Type.DefaultBinder, new[] { typeof(bool) }, null), Expression.PropertyOrField(unBoxedDataObjectExp, memberInfo.Name))));
                            }
                        }

                    }

                }
                inBlockExpressions.Add(Expression.Call(variableExp, typeof(List<>).MakeGenericType(typeof(byte)).GetMethod("ToArray")));
                var exp = Expression.Lambda<Func<object, byte[]>>(Expression.Block(new ParameterExpression[] { variableExp, unBoxedDataObjectExp }, inBlockExpressions), Expression.Parameter(typeof(object), "dataObject"));
                func = exp.Compile();
                ExpressionStorage.TryAdd(type, func);
            }

            var resultBytes = func.Invoke(graph);
            serializationStream.Write(resultBytes);
        }

        public SerializationBinder Binder { get; set; }
        public StreamingContext Context { get; set; }
        public ISurrogateSelector SurrogateSelector { get; set; }



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
