namespace NkjSoft.Common
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Security.Cryptography;
    using System.Text.RegularExpressions;
    using NkjSoft.Common.IO;

    /// <summary>
    /// 常用静态辅助方法封装。 
    /// </summary>
    public static class DataUtils
    {
        private static Regex keyReg = new Regex("[^a-zA-Z]", RegexOptions.Compiled);

        private static bool CheckStruct(Type type)
        {
            if (!type.IsValueType || type.IsEnum)
            {
                return false;
            }
            return (!type.IsPrimitive && !type.IsSerializable);
        }

        /// <summary>
        /// 将当前对象进行深复制一份。
        /// </summary>
        /// <param name="obj">需要被克隆的对象</param>
        /// <returns></returns>
        public static object Clone(object obj)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, obj);
                stream.Position = 0L;
                return formatter.Deserialize(stream);
            }
        }

        /// <summary>
        /// 将指定的类型实例转换成需要的类型实例。
        /// </summary>
        /// <typeparam name="TResult">转换的结果类型.</typeparam>
        /// <param name="value">被转换的值</param>
        /// <returns></returns>
        public static TResult ConvertValue<TResult>(object value)
        {
            return ConvertValue<TResult>(value, default(TResult));
        }


        /// <summary>
        /// 将指定的类型实例转换成需要的类型实例。
        /// </summary>
        /// <typeparam name="TResult">转换的结果类型.</typeparam>
        /// <param name="value">被转换的值</param>
        /// <param name="defaultIfFalse">转换失败或者传递的值为Null时返回的值</param>
        /// <returns></returns>
        public static TResult ConvertValue<TResult>(object value, TResult defaultIfFalse)
        {
            if (Convert.IsDBNull(value) || (value == null))
            {
                return defaultIfFalse;
            }

            object obj = null;
            try
            {
                obj = ConvertValue(typeof(TResult), value);
            }
            catch
            {
                return defaultIfFalse;
                //throw;
            }
            if (obj == null)
            {
                return defaultIfFalse;
            }
            return (TResult)obj;
        }


        /// <summary>
        /// 转换类型
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static object ConvertValue(Type type, object value)
        {
            if (Convert.IsDBNull(value) || (value == null))
            {
                return null;
            }
            if (CheckStruct(type))
            {
                string data = value.ToString();
                return SerializationManager.Deserialize(type, data);
            }
            Type type2 = value.GetType();
            if (type == type2)
            {
                return value;
            }
            if (((type == typeof(Guid)) || (type == typeof(Guid?))) && (type2 == typeof(string)))
            {
                if (string.IsNullOrEmpty(value.ToString()))
                {
                    return null;
                }
                return new Guid(value.ToString());
            }
            if (((type == typeof(DateTime)) || (type == typeof(DateTime?))) && (type2 == typeof(string)))
            {
                if (string.IsNullOrEmpty(value.ToString()))
                {
                    return null;
                }
                return Convert.ToDateTime(value);
            }
            if (type.IsEnum)
            {
                try
                {
                    return Enum.Parse(type, value.ToString(), true);
                }
                catch
                {
                    return Enum.ToObject(type, value);
                }
            }
            if ((type == typeof(bool)) || (type == typeof(bool?)))
            {
                bool tempbool = false;
                if (bool.TryParse(value.ToString(), out tempbool))
                {
                    return tempbool;
                }
                if (string.IsNullOrEmpty(value.ToString()))
                {
                    return false;
                }
                return true;
            }
            if (type.IsGenericType)
            {
                type = type.GetGenericArguments()[0];
            }
            return Convert.ChangeType(value, type);
        }

        /// <summary>
        /// Creates this instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Create<T>()
        {
            return (T)Create(typeof(T))();
        }

        /// <summary>
        /// 创建一个返回指定类型的快速调用的委托。
        /// </summary>
        /// <param name="type">返回类型</param>
        /// <returns></returns>
        public static FastCreateInstanceHandler Create(Type type)
        {
            return DynamicCalls.GetInstanceCreator(type);
        }

        /// <summary>
        /// 执行快速反射调用对象的指定方法。
        /// </summary>
        /// <param name="obj">对象j.</param>
        /// <param name="method">需要执行的方法 <see cref="System.Reflection.MethodInfo"/>.</param>
        /// <param name="parameters">方法参数.</param>
        /// <returns></returns>
        public static object FastMethodInvoke(object obj, MethodInfo method, params object[] parameters)
        {
            return DynamicCalls.GetMethodInvoker(method)(obj, parameters);
        }

        internal static string FormatSQL(string sql, char leftToken, char rightToken)
        {
            if (sql == null)
            {
                return string.Empty;
            }
            sql = sql.Replace("{0}", leftToken.ToString()).Replace("{1}", rightToken.ToString());
            return sql;
        }

        /// <summary>
        /// 格式化当前对象.
        /// </summary>
        /// <param name="val">The val.</param>
        /// <returns></returns>
        public static string FormatValue(object val)
        {
            if ((val == null) || (val == DBNull.Value))
            {
                return "null";
            }
            Type type = val.GetType();
            if ((type == typeof(DateTime)) || (type == typeof(Guid)))
            {
                return string.Format("'{0}'", val);
            }
            if (type == typeof(TimeSpan))
            {
                DateTime baseTime = new DateTime(0x76c, 1, 1);
                return string.Format("(cast('{0}' as datetime) - cast('{1}' as datetime))", baseTime + ((TimeSpan)val), baseTime);
            }
            if (type == typeof(bool))
            {
                if (!((bool)val))
                {
                    return "0";
                }
                return "1";
            }
            if (type.IsEnum)
            {
                return Convert.ToInt32(val).ToString();
            }
            if (type.IsValueType)
            {
                return val.ToString();
            }
            return ("N'" + val.ToString().Replace("'", "''") + "'");
        }

        /// <summary>
        /// 获取 CMD命令最后的一个方法名字。
        /// </summary>
        /// <param name="cmdText">The CMD text.</param>
        /// <param name="startIndexOfCharIndex">Start index of the index of char.</param>
        /// <returns></returns>
        public static int GetEndIndexOfMethod(string cmdText, int startIndexOfCharIndex)
        {
            int foundEnd = -1;
            for (int i = startIndexOfCharIndex; i < cmdText.Length; i++)
            {
                if (cmdText[i] == '(')
                {
                    foundEnd--;
                }
                else if (cmdText[i] == ')')
                {
                    foundEnd++;
                }
                if (foundEnd == 0)
                {
                    return i;
                }
            }
            return 0;
        }

        /// <summary>
        /// 获取指定类型的指定属性的值.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        public static object GetPropertyValue<TEntity>(TEntity entity, string propertyName)
        {
            PropertyInfo property = entity.GetType().GetProperty(propertyName);
            if (property != null)
            {
                return property.GetValue(entity, null);
            }
            return null;
        }

        /// <summary>
        /// 生成唯一键值
        /// </summary>
        /// <param name="prefix">前缀</param>
        /// <returns></returns>
        public static string MakeUniqueKey(this string prefix)
        {
            byte[] data = new byte[0x10];
            new RNGCryptoServiceProvider().GetBytes(data);
            string keystring = keyReg.Replace(Convert.ToBase64String(data).Trim(), string.Empty);
            if (keystring.Length > 0x10)
            {
                return keystring.Substring(0, 0x10).ToLower();
            }
            return keystring.ToLower();
        }

        /// <summary>
        /// 设置对象的某个属性的值
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="property"></param>
        /// <param name="value"></param>
        public static void SetPropertyValue(this object obj, PropertyInfo property, object value)
        {
            if (property.CanWrite)
            {
                FastPropertySetHandler propertySetter = DynamicCalls.GetPropertySetter(property);
                value = ConvertValue(property.PropertyType, value);
                propertySetter(obj, value);
            }
        }

        /// <summary>
        /// 设置对象的某个属性的值
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        public static void SetPropertyValue(object obj, string propertyName, object value)
        {
            SetPropertyValue(obj.GetType(), obj, propertyName, value);
        }

        /// <summary>
        /// 设置某个类型实例的指定属性的值。
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="obj">The obj.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="value">The value.</param>
        public static void SetPropertyValue(Type type, object obj, string propertyName, object value)
        {
            PropertyInfo property = type.GetProperty(propertyName);
            if (property != null)
            {
                //if (value == null && property.PropertyType.IsValueType)
                //    return;
                SetPropertyValue(obj, property, value);
            }
        }

        /// <summary>
        /// 通过T-SQL分隔方式分离语句体的两个方法.
        /// </summary>
        /// <param name="bodyText">The body text.</param>
        /// <returns></returns>
        public static string[] SplitTwoParamsOfMethodBody(string bodyText)
        {
            int colonIndex = 0;
            int foundEnd = 0;
            for (int i = 1; i < (bodyText.Length - 1); i++)
            {
                if (bodyText[i] == '(')
                {
                    foundEnd--;
                }
                else if (bodyText[i] == ')')
                {
                    foundEnd++;
                }
                if ((bodyText[i] == ',') && (foundEnd == 0))
                {
                    colonIndex = i;
                    break;
                }
            }
            return new string[] { bodyText.Substring(0, colonIndex), bodyText.Substring(colonIndex + 1) };
        }

        /// <summary>
        /// 在 Windows 资源管理器中指向并选择指定的文件。
        /// </summary>
        /// <param name="filePathToNavAndSelected">指定的文件</param>
        public static void MakeFileSelected(string filePathToNavAndSelected)
        {
            if (File.Exists(filePathToNavAndSelected))
                System.Diagnostics.Process.Start("explorer.exe", "/select," + filePathToNavAndSelected);
        }
    }
}

