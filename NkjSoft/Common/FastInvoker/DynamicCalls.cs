namespace NkjSoft.Common
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Reflection.Emit;

    /// <summary>
    /// 执行动态反射调用。
    /// </summary>
    public static class DynamicCalls
    {
        private static Dictionary<Type, FastCreateInstanceHandler> dictCreator = new Dictionary<Type, FastCreateInstanceHandler>();
        private static Dictionary<PropertyInfo, FastPropertyGetHandler> dictGetter = new Dictionary<PropertyInfo, FastPropertyGetHandler>();
        private static Dictionary<MethodInfo, FastInvokeHandler> dictInvoker = new Dictionary<MethodInfo, FastInvokeHandler>();
        private static Dictionary<PropertyInfo, FastPropertySetHandler> dictSetter = new Dictionary<PropertyInfo, FastPropertySetHandler>();

        private static void EmitBoxIfNeeded(ILGenerator ilGenerator, Type type)
        {
            if (type.IsValueType)
            {
                ilGenerator.Emit(OpCodes.Box, type);
            }
        }

        private static void EmitCastToReference(ILGenerator ilGenerator, Type type)
        {
            if (type.IsValueType)
            {
                ilGenerator.Emit(OpCodes.Unbox_Any, type);
            }
            else
            {
                ilGenerator.Emit(OpCodes.Castclass, type);
            }
        }

        /// <summary>
        /// Emits the fast int.
        /// </summary>
        /// <param name="ilGenerator">The il generator.</param>
        /// <param name="value">The value.</param>
        private static void EmitFastInt(ILGenerator ilGenerator, int value)
        {
            switch (value)
            {
                case -1:
                    ilGenerator.Emit(OpCodes.Ldc_I4_M1);
                    return;

                case 0:
                    ilGenerator.Emit(OpCodes.Ldc_I4_0);
                    return;

                case 1:
                    ilGenerator.Emit(OpCodes.Ldc_I4_1);
                    return;

                case 2:
                    ilGenerator.Emit(OpCodes.Ldc_I4_2);
                    return;

                case 3:
                    ilGenerator.Emit(OpCodes.Ldc_I4_3);
                    return;

                case 4:
                    ilGenerator.Emit(OpCodes.Ldc_I4_4);
                    return;

                case 5:
                    ilGenerator.Emit(OpCodes.Ldc_I4_5);
                    return;

                case 6:
                    ilGenerator.Emit(OpCodes.Ldc_I4_6);
                    return;

                case 7:
                    ilGenerator.Emit(OpCodes.Ldc_I4_7);
                    return;

                case 8:
                    ilGenerator.Emit(OpCodes.Ldc_I4_8);
                    return;
            }
            if ((value > -129) && (value < 0x80))
            {
                ilGenerator.Emit(OpCodes.Ldc_I4_S, (sbyte) value);
            }
            else
            {
                ilGenerator.Emit(OpCodes.Ldc_I4, value);
            }
        }

        /// <summary>
        /// 获取某个类型的构造器。
        /// </summary>
        /// <param name="type">指定需要获取的类型</param>
        /// <returns></returns>
        public static FastCreateInstanceHandler GetInstanceCreator(Type type)
        {
            lock (dictCreator)
            {
                if (dictCreator.ContainsKey(type))
                {
                    return dictCreator[type];
                }
                DynamicMethod dynamicMethod = new DynamicMethod(string.Empty, type, new Type[0], typeof(DynamicCalls).Module);
                ILGenerator ilGenerator = dynamicMethod.GetILGenerator();
                ilGenerator.Emit(OpCodes.Newobj, type.GetConstructor(Type.EmptyTypes));
                ilGenerator.Emit(OpCodes.Ret);
                FastCreateInstanceHandler creator = (FastCreateInstanceHandler) dynamicMethod.CreateDelegate(typeof(FastCreateInstanceHandler));
                dictCreator.Add(type, creator);
                return creator;
            }
        }

        /// <summary>
        /// 获取某个方法
        /// </summary>
        /// <param name="methodInfo"></param>
        /// <returns></returns>
        public static FastInvokeHandler GetMethodInvoker(MethodInfo methodInfo)
        {
            lock (dictInvoker)
            {
                if (dictInvoker.ContainsKey(methodInfo))
                {
                    return dictInvoker[methodInfo];
                }
                DynamicMethod dynamicMethod = new DynamicMethod(string.Empty, typeof(object), new Type[] { typeof(object), typeof(object[]) }, methodInfo.DeclaringType.Module);
                ILGenerator ilGenerator = dynamicMethod.GetILGenerator();
                ParameterInfo[] parameters = methodInfo.GetParameters();
                Type[] paramTypes = new Type[parameters.Length];
                for (int i = 0; i < paramTypes.Length; i++)
                {
                    if (parameters[i].ParameterType.IsByRef)
                    {
                        paramTypes[i] = parameters[i].ParameterType.GetElementType();
                    }
                    else
                    {
                        paramTypes[i] = parameters[i].ParameterType;
                    }
                }
                LocalBuilder[] locals = new LocalBuilder[paramTypes.Length];
                for (int i = 0; i < paramTypes.Length; i++)
                {
                    locals[i] = ilGenerator.DeclareLocal(paramTypes[i], true);
                }
                for (int i = 0; i < paramTypes.Length; i++)
                {
                    ilGenerator.Emit(OpCodes.Ldarg_1);
                    EmitFastInt(ilGenerator, i);
                    ilGenerator.Emit(OpCodes.Ldelem_Ref);
                    EmitCastToReference(ilGenerator, paramTypes[i]);
                    ilGenerator.Emit(OpCodes.Stloc, locals[i]);
                }
                if (!methodInfo.IsStatic)
                {
                    ilGenerator.Emit(OpCodes.Ldarg_0);
                }
                for (int i = 0; i < paramTypes.Length; i++)
                {
                    if (parameters[i].ParameterType.IsByRef)
                    {
                        ilGenerator.Emit(OpCodes.Ldloca_S, locals[i]);
                    }
                    else
                    {
                        ilGenerator.Emit(OpCodes.Ldloc, locals[i]);
                    }
                }
                if (!methodInfo.IsStatic)
                {
                    ilGenerator.EmitCall(OpCodes.Callvirt, methodInfo, null);
                }
                else
                {
                    ilGenerator.EmitCall(OpCodes.Call, methodInfo, null);
                }
                if (methodInfo.ReturnType == typeof(void))
                {
                    ilGenerator.Emit(OpCodes.Ldnull);
                }
                else
                {
                    EmitBoxIfNeeded(ilGenerator, methodInfo.ReturnType);
                }
                for (int i = 0; i < paramTypes.Length; i++)
                {
                    if (parameters[i].ParameterType.IsByRef)
                    {
                        ilGenerator.Emit(OpCodes.Ldarg_1);
                        EmitFastInt(ilGenerator, i);
                        ilGenerator.Emit(OpCodes.Ldloc, locals[i]);
                        if (locals[i].LocalType.IsValueType)
                        {
                            ilGenerator.Emit(OpCodes.Box, locals[i].LocalType);
                        }
                        ilGenerator.Emit(OpCodes.Stelem_Ref);
                    }
                }
                ilGenerator.Emit(OpCodes.Ret);
                FastInvokeHandler invoker = (FastInvokeHandler) dynamicMethod.CreateDelegate(typeof(FastInvokeHandler));
                dictInvoker.Add(methodInfo, invoker);
                return invoker;
            }
        }

        /// <summary>
        /// 获取某个对象的属性
        /// </summary>
        /// <param name="propInfo">对象的属性</param>
        /// <returns></returns>
        public static FastPropertyGetHandler GetPropertyGetter(PropertyInfo propInfo)
        {
            lock (dictGetter)
            {
                if (dictGetter.ContainsKey(propInfo))
                {
                    return dictGetter[propInfo];
                }
                DynamicMethod dynamicMethod = new DynamicMethod(string.Empty, typeof(object), new Type[] { typeof(object) }, propInfo.DeclaringType.Module);
                ILGenerator ilGenerator = dynamicMethod.GetILGenerator();
                ilGenerator.Emit(OpCodes.Ldarg_0);
                ilGenerator.EmitCall(OpCodes.Callvirt, propInfo.GetGetMethod(), null);
                EmitBoxIfNeeded(ilGenerator, propInfo.PropertyType);
                ilGenerator.Emit(OpCodes.Ret);
                FastPropertyGetHandler getter = (FastPropertyGetHandler) dynamicMethod.CreateDelegate(typeof(FastPropertyGetHandler));
                dictGetter.Add(propInfo, getter);
                return getter;
            }
        }
        /// <summary>
        /// 获取某个对象的属性
        /// </summary>
        /// <param name="propInfo">对象的属性</param>
        /// <returns></returns>
        public static FastPropertySetHandler GetPropertySetter(PropertyInfo propInfo)
        {
            lock (dictSetter)
            {
                if (dictSetter.ContainsKey(propInfo))
                {
                    return dictSetter[propInfo];
                }
                DynamicMethod dynamicMethod = new DynamicMethod(string.Empty, null, new Type[] { typeof(object), typeof(object) }, propInfo.DeclaringType.Module);
                ILGenerator ilGenerator = dynamicMethod.GetILGenerator();
                ilGenerator.Emit(OpCodes.Ldarg_0);
                ilGenerator.Emit(OpCodes.Ldarg_1);
                EmitCastToReference(ilGenerator, propInfo.PropertyType);
                ilGenerator.EmitCall(OpCodes.Callvirt, propInfo.GetSetMethod(), null);
                ilGenerator.Emit(OpCodes.Ret);
                FastPropertySetHandler setter = (FastPropertySetHandler) dynamicMethod.CreateDelegate(typeof(FastPropertySetHandler));
                dictSetter.Add(propInfo, setter);
                return setter;
            }
        }
    }
}

