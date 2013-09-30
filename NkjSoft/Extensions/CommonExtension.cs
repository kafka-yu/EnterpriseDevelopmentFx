//--------------------------版权信息----------------------------
//       
//                 文件名: CommonExtension                 
//                 CLR Version: 2.0.50727.4927
//                 项目命名空间: NkjSoft.Extensions
//
//                 作  者: 俞如凯 
//                 Q Q: 250820436　 yurukai@vip.qq.com
//                 E-Mail: yurukai@hotmail.com
//                 创建时间 : 2010/4/14 9:49:38
//                 Copyright (c) Yurukai , All rights reserved.
//----------------------------------------------------------------


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Data;
using System.IO;
using System.Collections;

namespace NkjSoft.Extensions
{

    /// <summary>
    /// 我的扩展方法集类库 。封装了常用的扩展方法。
    /// </summary>
    /// <remarks> 
    /// 注意此命名空间需要 引用 System.Core.dll 以及引用 <see cref="System.Linq"/> 命名空间。  
    /// <para>涉及到的类型有：
    /// <list type="System.String">字符串</list>
    /// <list type="System.ValueType">常用值类型</list>
    /// <list type="System.Collections.Generic">泛型集合</list>
    /// <list type="System.Data.DataTable">表</list>
    /// <list type="System.Data.DataSet">数据集</list> 
    /// </para>
    /// </remarks> 
    public static class CommonExtension
    {
        #region --- 加密解密 [未实现]
        #endregion

        #region --- For Int32 ---
        /// <summary>
        /// 将当前整数值转换成指定的具体 <typeparamref name="TResult"/> 枚举类型。
        /// </summary>
        /// <typeparam name="TResult"> 指定需要得到的具体 <typeparamref name="TResult"/> 类型。</typeparam>
        /// <param name="original">当前整数值</param> 
        /// <exception cref="ArgumentNullException">参数Null</exception>
        /// <exception cref="ArgumentException">无法进行转换</exception>
        /// <returns></returns>
        public static TResult ToEnum<TResult>(this int original) where TResult : struct
        {
            try
            {
                return (TResult)Enum.Parse(typeof(TResult), original.ToString(), true);
            }
            catch (ArgumentNullException ex1)
            {
                throw ex1;
            }
            catch (ArgumentException ex2)
            { throw ex2; }
        }
        #endregion

        #region --- For Array/IEnumerable ---

        /// <summary>
        /// 将 <see cref="System.Collections.Generic.IEnumerator&lt;TSource&gt;"/> 序列以指定的前缀、后缀以及分隔符输出为 <see cref="System.String"/> 的形式。
        /// </summary>
        /// <typeparam name="TSource">需要转换的序列类型</typeparam>
        /// <param name="source">需要转换数组</param> 
        /// <param name="startPrefix">前缀</param>
        /// <param name="endPrefix">后缀</param>
        /// <param name="splitChar">指定的相隔符号</param>
        /// <exception cref="System.Exception">参数错误!格式无效等!</exception>
        /// <exception cref="System.NullReferenceException">空 对象 调用!</exception>
        /// <returns>String 值 </returns>
        /// <returns></returns>
        /// <example>
        ///    List&gt;int&lt; list =new&gt;int&lt;() {1,2,3,4,5,6};
        ///    string result = list.ToStringLine("'","'",",");
        ///    结果为: '1','2','3','4','5','6' 
        /// </example>
        public static string ToStringLine<TSource>(this IEnumerable<TSource> source, string startPrefix, string endPrefix, string splitChar)
        {
            if (source == null || source.Count() == 0)
                return string.Empty;
            StringBuilder builder = new StringBuilder();

            foreach (TSource item in source)
            {
                if (null == item)
                    continue;
                builder.AppendFormat("{0}{1}{2}{3}", startPrefix, item.ToString(), endPrefix, splitChar);
            }
            if (builder.Length <= 0)
                return string.Empty;
            return builder.ToString(0, builder.Length - splitChar.Length);
        }

        /// <summary>
        /// 将 <see cref="System.Collections.Generic.IEnumerator&lt;TSource&gt;"/> 序列以指定的前缀分隔输出为 <see cref="System.String"/> 的形式。
        /// </summary>
        /// <typeparam name="TSource">需要转换的数组类型</typeparam>
        /// <param name="source">需要转换数组</param>
        /// <param name="splitChar">分隔符,用于分隔数组元素。当不提供此值时(为Null)，默认会使用  <see cref="System.Empty"/> 作为分隔符。</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">参数错误!格式无效等!</exception>
        /// <exception cref="System.NullReferenceException">空 对象 调用!</exception>
        public static string ToStringLine<TSource>(this IEnumerable<TSource> source, string splitChar)
        {
            if (source == null || source.Count() == 0)
                return string.Empty;

            if (splitChar.Length == 0 || String.IsNullOrEmpty(splitChar))
                splitChar = string.Empty;

            StringBuilder builder = new StringBuilder();

            foreach (TSource i in source)
            {
                if (null == i)
                    continue;
                builder.AppendFormat("{0}{1}", i.ToString(), splitChar);
            }
            if (builder.Length <= 0)
                return string.Empty;
            return builder.ToString(0, builder.Length - 1);
        }

        ///// <summary>
        ///// 将 <typeparamref name="TSource"/> 类型数组转换成以指定分隔符分隔元素的 <see cref="System.String"/> 形式的结果。
        ///// </summary>
        ///// <typeparam name="TSource">需要转换的数组类型</typeparam>
        ///// <param name="source">需要转换数组</param>
        ///// <param name="splitChar">分隔符,用于分隔数组元素。当不提供此值时，默认会使用“，”作为分隔符。</param>
        /////  <exception cref="System.Exception">参数错误!格式无效等!</exception>
        ///// <exception cref="System.NullReferenceException">空 对象 调用!</exception>
        ///// <returns></returns>
        //public static string ToStringLine<TSource>(this TSource[] source, string splitChar)
        //{
        //    if (source == null || source.Length == 0)
        //        return string.Empty;

        //    if (splitChar.Length == 0 || String.IsNullOrEmpty(splitChar))
        //        splitChar = ",";

        //    StringBuilder builder = new StringBuilder();

        //    foreach (TSource i in source)
        //    {
        //        if (null == i)
        //            continue;
        //        builder.AppendFormat("{0}{1}", i.ToString(), splitChar);
        //    }
        //    return builder.ToString(0, builder.Length - 1);
        //}

        /// <summary>
        /// 将 <typeparamref name="TSource"/> 类型数组转换成以指定分隔符分隔元素的 <see cref="System.String"/> 形式的结果。
        /// </summary>
        /// <typeparam name="TSource">需要转换的数组类型</typeparam>
        /// <param name="source">需要转换数组</param>
        /// <param name="splitChar">分隔符,用于分隔数组元素。当不提供此值时(为Null)，默认会使用  <see cref="System.Empty"/> 作为分隔符。</param>
        /// <param name="withEnd">是否包含结束位置的分隔符。（默认不包含）</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">参数错误!格式无效等!</exception>
        /// <exception cref="System.NullReferenceException">空 对象 调用!</exception>
        public static string ToStringLine<TSource>(this IEnumerable<TSource> source, string splitChar, bool withEnd)
        {
            if (source == null) return string.Empty;
            if (splitChar.Length == 0 || String.IsNullOrEmpty(splitChar))
                splitChar = string.Empty;

            StringBuilder builder = new StringBuilder();

            foreach (TSource i in source)
            {
                if (null == i)
                    continue;
                builder.AppendFormat("{0}{1}", i.ToString(), splitChar);
            }
            if (builder.Length <= 0)
                return string.Empty;
            if (!withEnd)
                return builder.ToString(0, builder.Length - 1);
            return builder.ToString();
        }

        /// <summary>
        /// 将 <typeparamref name="TSource"/> 类型数组转换成以指定分隔符分隔元素,并以指定的格式化方式格式每一项,生成一个 <see cref="System.String"/> 形式的结果。
        /// </summary>
        /// <typeparam name="TSource">需要转换的数组类型</typeparam>
        /// <param name="source">需要转换数组</param>
        /// <param name="itemStringFormatter">每一项的格式化方式。</param>
        /// <param name="splitChar">分隔符,用于分隔数组元素。当不提供此值时(为Null)，默认会使用  <see cref="System.Empty"/> 作为分隔符。</param>
        /// <param name="argsSelector">要格式化的 System.Object .个数由 参数 <paramref name="itemStringFormatter"/> 中的格式占位符个数决定。</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">参数错误!格式无效等!</exception>
        /// <exception cref="System.NullReferenceException">空 对象 调用!</exception>
        public static string ToStringLine<TSource>(this IEnumerable<TSource> source, string itemStringFormatter, string splitChar, Func<TSource, object[]> argsSelector)
        {
            //"".ToStringLine("<span>{0}</span>{1}", p => new object[] { p.,p..});

            if (source == null || source.Count() == 0)
                return string.Empty;

            if (String.IsNullOrEmpty(splitChar))
                splitChar = string.Empty;

            StringBuilder builder = new StringBuilder();

            foreach (TSource i in source)
            {
                if (null == i)
                    continue;
                builder.AppendFormat(itemStringFormatter, argsSelector(i));
                builder.Append(splitChar);
            }
            if (builder.Length <= 0)
                return string.Empty;
            return builder.ToString(0, builder.Length - 1);
        }

        /// <summary>
        /// 对 <typeparamref name="T"/>类型实例的 <paramref name="source"/> 数组进行指定的 foreach 遍历操作。
        /// </summary>
        /// <typeparam name="T">数组元素类型</typeparam>
        /// <param name="source">源数组</param>
        /// <param name="action">进行的指定遍历操作</param>
        public static void ForEach<T>(this T[] source, Action<T> action)
        {
            if (source != null && source.Length > 0 && action != null)
            {
                foreach (T item in source)
                {
                    action(item);
                }
            }
        }
        /// <summary>
        /// 使用 for 循环的方式遍历当前集合/数组。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">源.</param>
        /// <param name="action">操作.</param>
        public static void ForEach<T>(this IEnumerable<T> source, Action<T, int> action)
        {
            if (source == null)
                return;
            var enumerator = source.GetEnumerator();
            int length = 0;
            while (enumerator.MoveNext())
            {
                T t = enumerator.Current;
                action(t, length);
                length++;
            }

        }

        /// <summary>
        /// 以指定的方式 foreach 遍历当前集合。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="action">遍历方式</param>
        public static void ForEach<T>(this ICollection source, Action<T> action)
        {
            if (source != null && source.Count > 0 && action != null)
            {
                foreach (T item in source)
                {
                    T t = item;
                    action(t);
                }
            }
        }


        #endregion

        #region --- For Boolean ---
        /// <summary>
        /// 给定操作的名称,根据当前值(True、False )的到类似：“某某操作成功!” 的字符串。
        /// </summary>
        /// <param name="source">当前值</param>
        /// <param name="opeartion">一个值，表示给定操作的名称。</param>
        /// <returns></returns>
        /// <remarks>
        ///    给定操作的名称,根据当前值(True、False )的到类似：XX操作成功! 的字符串。
        /// </remarks>
        /// <example>
        ///    <code>
        ///    bool isOk=db.AddNews(entity);
        ///   </code>
        ///   <code>
        ///    MessageBox.Show(isOk.ToString("添加新闻"));  
        /// </code>
        /// </example>
        public static string ToString(this bool source, string opeartion)
        {
            return string.Format("{0}{1}!", opeartion, source ? "成功" : "失败");
        }


        /// <summary>
        /// 返回一个 <see cref="System.String"/> ,当当前 <see cref="System.Bool"/> 为 TRUE 时 返回 <paramref name="whenTrue"/> , 相反返回 <paramref name="whenFalse"/>。
        /// </summary>
        /// <param name="source">if set to <c>true</c> [source].</param>
        /// <param name="whenTrue">The when true.</param>
        /// <param name="whenFalse">The when false.</param>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public static string ToString(this bool source, string whenTrue, string whenFalse)
        {
            return source ? whenTrue : whenFalse;
        }


        #endregion

        #region --- For List ---
        /// <summary>
        /// 自定义一个操作，生成 <typeparam name="T">类型</typeparam>  序列。
        /// </summary> 
        /// <param name="source">存放生成结果序列的List泛型列表容器</param>
        /// <param name="start">遍历的下限</param>
        /// <param name="max">遍历的上限</param>
        /// <param name="addMethod">元素生成的操作</param>
        /// <returns></returns>
        public static List<T> Range<T>(this List<T> source, int start, int max, Func<int, T> addMethod)
        {
            List<T> temp = source;
            if (source == null)
                temp = new List<T>();
            for (int i = start; i < max; i++)
            {
                temp.Add(addMethod(i));
            }

            return temp;
        }

        #endregion

        #region --- ForReflection ---
        /// <summary>
        /// 获取某个对象成员指定特性标记的匹配属性的值。
        /// </summary>
        /// <typeparam name="TAttribute">Attribute 的 <see cref="System.Type"/> 信息.</typeparam>
        /// <typeparam name="TResult">返回属性的类型</typeparam>
        /// <param name="member"><see cref="System.Reflection.MemberInfo"/> 成员.</param>
        /// <param name="valueSelector">基于谓词的属性筛选器</param>
        /// <returns></returns>
        public static TResult GetAttributeValue<TAttribute, TResult>(this MemberInfo member, Func<TAttribute, TResult> valueSelector) where TAttribute : Attribute
        {
            object attr = member.GetCustomAttributes(typeof(TAttribute), false).FirstOrDefault();
            if (attr == null)
                return default(TResult);
            return valueSelector((TAttribute)attr);
        }

        /// <summary>
        /// 获取当前类型的某一个已知 Attribute 的指定类型属性的值。
        /// </summary>
        /// <typeparam name="TAttributeType">当前类型的 Attribute 类型</typeparam>
        /// <typeparam name="TResult">返回属性类型</typeparam>
        /// <param name="type">类型</param>
        /// <param name="valueSelector">属性筛选器</param>
        /// <returns></returns>
        public static TResult GetAttributeValue<TAttributeType, TResult>(this Type type, Func<TAttributeType, TResult> valueSelector) where TAttributeType : Attribute
        {
            object attr = type.GetCustomAttributes(typeof(TAttributeType), false).FirstOrDefault();
            if (attr == null)
                return default(TResult);
            return valueSelector((TAttributeType)attr);
        }
        /// <summary>
        /// 获取当前 <see cref="System.Reflection.MemberInfo"/> 中标记的指定的特性。
        /// </summary>
        /// <typeparam name="TResult">需要查获取的 Attribute 类型</typeparam>
        /// <param name="type">The type.</param>
        /// <param name="inherit">是否在继承层次中查找。</param>
        /// <returns></returns>
        public static IEnumerable<TResult> GetCustomAttribute<TResult>(this MemberInfo type, bool inherit) where TResult : class
        {
            var attr = type.GetCustomAttributes(typeof(TResult), inherit).Select(p => (TResult)p);
            return attr;
        }

        /// <summary>
        /// 推断当前 <see cref="System.Reflection.PropertyInfo"/> 属性的类型。并返回对应类型的 <see cref="System.Object"/> 对应的值。
        /// </summary>
        /// <param name="prop">The prop.</param>
        /// <param name="val">The val.</param>
        /// <returns></returns>
        public static object PredicateValue(this  System.Reflection.PropertyInfo prop, object val)
        {
            var pType = prop.PropertyType.FullName;
            var vType = val.GetType().Name;
            object rtv = null;
            //识别泛型
            if (prop.PropertyType.IsGenericType)
            {
                pType = prop.PropertyType.UnderlyingSystemType.FullName;
                int start = pType.IndexOf("[[");
                pType = pType.Substring(start + 2, pType.IndexOf(",") - start - 2);
            }
            switch (pType)
            {
                default:
                case "System.String":
                    rtv = val.IfNullDefault(string.Empty);
                    break;
                case "System.Int32":
                    rtv = val.IfNullDefault<int>(0);
                    break;
                case "System.Single":
                    rtv = val.IfNullDefault(0.0f);
                    break;
                case "System.Double":
                    rtv = val.IfNullDefault(0.0d);
                    break;
                case "System.Boolean":
                    rtv = Convert.ToBoolean(val);
                    break;
                case "System.DateTime":
                    if ((val == null || val.ToString().Trim().Length == 0))
                    {
                        if (prop.PropertyType.IsGenericType)
                            rtv = null;
                        else rtv = DateTime.MinValue;
                    }
                    else
                        rtv = val.ToString().ToDateTime();
                    break;
            }
            //Console.WriteLine(vType); 
            return rtv;
        }
        #endregion

        #region --- For Object ---
        /// <summary>
        /// 如果当前对象为空(或者DbNull)，返回指定的值。
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="obj">The obj.</param>
        /// <param name="replacer">指定的值</param>
        /// <returns></returns>
        public static TResult IfNullDefault<TResult>(this object obj, TResult replacer)
        {
            if (obj == null || Convert.IsDBNull(obj))
                return replacer;
            return (TResult)Convert.ChangeType(obj, typeof(TResult));
        }

        /// <summary>
        /// 将当前类型强制转换成目标类型。必须确保两个类型之间是可以通过强制类型转换变换的。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The obj.</param>
        /// <returns></returns>
        public static T AsType<T>(this object obj)
        {
            if (null == obj)
                return default(T);
            return (T)Convert.ChangeType(obj, typeof(T));
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="string">The type of the tring.</typeparam>
        /// <param name="obj">The obj.</param>
        /// <param name="switchMethod">The switch method.</param>
        /// <param name="trueFormatter">The true formatter.</param>
        /// <param name="falseFormatter">The false formatter.</param>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public static string ToString<T>(this T obj, Func<T, bool> switchMethod, string trueFormatter, string falseFormatter)
        {
            var isOk = switchMethod(obj);
            if (isOk)
                return trueFormatter.IfNullDefault("{0}").FormatWith(obj);
            else
                return falseFormatter.IfNullDefault("{0}").FormatWith(obj);
        }
        #endregion

        #region --- For Byte[] ---
        /// <summary>
        /// 将当前 Byte[] 转换成 Long 型。
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static long ToLong(this byte[] buffer)
        {
            if (buffer == null || buffer.Length == 0)
                return 0;
            return BitConverter.ToInt64(buffer, 0);
        }
        #endregion


    }

}
