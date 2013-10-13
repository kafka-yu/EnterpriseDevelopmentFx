//--------------------------文档信息----------------------------
//       
//                 文件名: StringExtensions                 
//                 CLR Version: 4.0.30319.1
//                 项目命名空间: NkjSoft.Common.Extensions
//
//                 作  者: 俞如凯 
//                 Q Q: 250820436　 yurukai@vip.qq.com
//                 E-Mail: yurukai@hotmail.com
//                 创建时间 : 2010/8/1 9:19:34
//                 Copyright (c) Yurukai , All rights reserved.
//----------------------------------------------------------------


using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace NkjSoft.Framework.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class StringExtensions
    {
        #region --- 过滤SQL注入 ---
        static readonly string SqlStr = @"and|or|exec|execute|insert|select|delete|update|alter|create|drop|count|\*|chr|char|asc|mid|substring|master|truncate|declare|xp_cmdshell|restore|backup|net +user|net +localgroup +administrators";
        /// <summary>   
        /// 检测字符串中是否有SQL攻击代码   
        /// </summary>   
        /// <param name="sqlString">待检测的字符串</param>   
        /// <returns>True：不危险，False：危险</returns>   
        public static bool IsSQLSafe(this string sqlString)
        {

            if (string.IsNullOrEmpty(sqlString))
                return false;
            string str_Regex = @"\b(" + SqlStr + @")\b";
            Regex regex = new Regex(str_Regex, RegexOptions.IgnoreCase);

            if (regex.IsMatch(sqlString))

                return false;

            return true;

        }

        /// <summary>
        /// 过滤当前 <see cref="System.String"/> 中危险的SQL字符。
        /// </summary>
        /// <param name="sqlString">被过滤的字符串</param>
        /// <returns></returns>
        public static string FilterSQLCmd(this string sqlString)
        {

            if (string.IsNullOrEmpty(sqlString))

                return sqlString;

            string str_Regex = @"\b(" + SqlStr + @")\b";

            //替换危险字符串  

            sqlString = Regex.Replace(sqlString, str_Regex, string.Empty, RegexOptions.IgnoreCase);

            return sqlString;

        }
        #endregion

        #region --- For String ---

        #region --- 类型转换部分To ---
        /// <summary>
        /// 将时间字符串以 yyyy-MM-dd 格式输出。<para>碰到无法转换的时间格式字符串时,将默认输出 <see cref="System.String.Empty"/>。</para>
        /// </summary>
        /// <param name="source">包含时间信息的字符串。</param>
        /// <returns></returns>
        public static string ToDate(this string source)
        {
            return ToDateTime(source).ToString("yyyy-MM-dd");
        }
        /// <summary>
        /// 将时间字符串以 yyyy-MM-dd HH:mm:ss 格式输出
        /// <para>碰到无法转换的时间格式字符串时,将默认输出当前时间!</para>
        /// </summary>
        /// <param name="source">时间字符串</param>
        /// <param name="splitChar">分隔符,可以是: - ,/ ,: 之一 </param>
        /// <exception cref="System.Exception">碰到无法转换的时间格式字符串时,将默认输出当前时间!</exception>
        /// <returns>yyyy-MM-dd HH:mm:ss 格式输出的字符串</returns>
        public static string ToDateTimeString(this string source, string splitChar)
        {
            splitChar = splitChar.Length == 0 ? "-" : splitChar;

            DateTime time;
            if (!DateTime.TryParse(source, out time))
                time = DateTime.Now;

            return time.ToString(string.Format("yyyy{0}MM{0}dd HH:mm:ss", splitChar));

        }
        /// <summary>
        /// 将 时间字符串 以 yyyy-MM-dd HH:mm:ss 格式输出 
        /// <para>碰到无法转换的时间格式字符串时,将默认输出当前时间!</para>
        /// </summary>
        /// <param name="source">时间字符串</param> 
        /// <exception cref="System.Exception">碰到无法转换的时间格式字符串时,将默认输出当前时间!</exception>
        /// <returns>yyyy-MM-dd HH:mm:ss 格式输出的字符串</returns>
        public static string ToDateTimeString(this string source)
        {
            return ToDateTimeString(source, "-");
        }
        /// <summary>
        /// 将当前字符串转换成 <see cref="System.DateTime"/> 类型。无法转换的时候返回当前时间。
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this string source)
        {
            DateTime temp;

            if (!DateTime.TryParse(source, out temp))
                temp = DateTime.Now;
            return temp;
        }
        ///// <exception cref="System.ArgumentNullException">空 字符串 调用!</exception>
        /// <summary>
        /// 将当前字符串实例转换成 <see cref="System.Int32"/> 类型 ,转换失败的时候返回 int.Min。
        /// </summary>
        /// <param name="source">当前字符串实例</param>  
        /// <returns></returns>
        public static Int32 ToInt32(this string source)
        {
            return ToInt32(source, int.MinValue);
        }
        /// <summary>
        /// 将当前字符串实例转换成 <see cref="System.Int32"/> 类型 ,转换失败的时候返回指定 <paramref name="defaultIfFalse"/> 值。
        /// </summary>
        /// <param name="source">当前字符串实例</param>
        /// <param name="defaultIfFalse">失败的时候返回指定的值</param>
        /// <returns></returns>
        public static Int32 ToInt32(this string source, int defaultIfFalse)
        {
            //if (source.IsNullOrEmpty())
            //    throw new ArgumentNullException("空 字符串 调用!");
            int errorValue = defaultIfFalse;
            bool r = int.TryParse(source, out errorValue);
            if (!r)
                errorValue = defaultIfFalse;
            return errorValue;
        }


        ///// <exception cref="System.Exception">参数错误!格式无效等!</exception>
        ///// <exception cref="System.NullReferenceException">空 字符串 调用!</exception>
        /// <summary>
        /// 将 当前字符串实例 转换成 <see cref="System.Decimal"/> 类型 。
        /// </summary>
        /// <param name="source">当前字符串实例</param>
        /// <param name="dotLength">保留小数位数</param>
        /// <returns></returns>
        public static decimal ToDecimal(this string source, int dotLength)
        {
            decimal errorValue = 0.0M;
            bool r = decimal.TryParse(source, out errorValue);
            return r == true ? decimal.Round(errorValue, dotLength) : errorValue;
        }
        /// <summary>
        ///  将 当前字符串实例 转换成 <see cref="System.Decimal"/> 类型 。
        /// </summary>
        /// <param name="source">当前字符串实例</param>
        /// <exception cref="System.Exception">参数错误!格式无效等!</exception>
        /// <returns></returns>
        public static decimal ToDecimal(this string source)
        { return ToDecimal(source, 0); }

        /// <summary>
        /// 将 当前字符串实例 转换成 <see cref="System.Double"/> 类型 。
        /// </summary>
        /// <param name="source">当前字符串实例</param> 
        /// <returns></returns>
        public static double ToDouble(this string source)
        {
            double errorValue = 0.0D;
            bool isOk = double.TryParse(source, out errorValue);
            return errorValue;

        }

        /// <summary>
        ///  将 当前字符串实例 转换成 <see cref="System.Boolean"/>类型 
        /// </summary> 
        /// <param name="source">当前字符串实例</param>
        /// <exception cref="System.Exception">参数错误!格式无效等!</exception>
        /// <exception cref="System.NullReferenceException">空 字符串 调用!</exception>
        /// <returns></returns>
        public static bool ToBoolean(this string source)
        {
            bool errorValue = false;
            bool.TryParse(source, out errorValue);

            return errorValue;
        }

        /// <summary>
        /// 将 字符串文本 转换成 字节流 [字节编码序列]
        /// </summary>
        /// <param name="source">字符串文本</param>
        /// <param name="encoding">字节编码</param>
        /// <exception cref="System.Exception">参数错误!格式无效等!</exception>
        /// <exception cref="System.ArgumentNullException">空 字符串 调用!</exception>
        /// <excption cref="System.Text.EncoderFallbackException"> 发生回退（请参见了解编码以获得完整的解释）-并且-System.Text.Encoding.EncoderFallback 被设置为 System.Text.EncoderExceptionFallback</exception>
        /// <returns></returns>
        public static byte[] ToBytes(this string source, Encoding encoding)
        {
            return encoding.GetBytes(source);
        }

        /// <summary>
        /// 对文本进行HTML编码。
        /// </summary>
        /// <param name="source">普通文本</param>
        /// <returns></returns>
        public static string HtmlEncode(this string source)
        {
            StringBuilder sb = new StringBuilder(source);

            return sb.Replace("<", "&lt;").Replace(">", "&gt;").Replace("\n", "<br/>").ToString();
        }

        /// <summary>
        /// 对Html文本进行解码。
        /// </summary>
        /// <param name="source">包含Html编码的文本</param>
        /// <returns></returns>
        public static string HtmlDecode(this string source)
        {
            StringBuilder sb = new StringBuilder(source);

            return sb.Replace("&lt;", "<").Replace("&gt;", ">").ToString();
        }

        /// <summary>
        /// 将 <see cref="System.Data.Sql.SqlDbType"/> 类型的字符串值转换成 <see cref="System.Data.Common.DbType"/> 类型。
        /// </summary>
        /// <param name="source">需要被转换的 <see cref="System.Data.Sql.SqlDbType"/>  字符串值。</param>
        /// <param name="ignoreCase">是否忽略类型转换时候的大小写拼写，默认忽略。</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">参数Null错误.</exception>
        /// <exception cref="System.Exception">不支持的转换。</exception>
        public static DbType ToDbType(this string source, bool ignoreCase)
        {
            SqlDbType sdt = (SqlDbType)Enum.Parse(typeof(SqlDbType), source, ignoreCase);

            System.Data.SqlClient.SqlParameter temp = new System.Data.SqlClient.SqlParameter("@temp", sdt);

            return temp.DbType;
        }


        #endregion

        #region --- 获取部分Gets  ---

        /// <summary>
        /// 将固定长文本截取所需要的 长度返回 ,可以指定过长部分 使用 代替符 替换。
        /// </summary>
        /// <param name="source">固定长文本</param>
        /// <param name="length">需要的 长度</param>
        /// <param name="insteadStr">过长部分 使用 代替符</param>
        /// <returns></returns>
        public static string ToCutBack(this string source, int length, string insteadStr)
        {
            if (null == source)
                return string.Empty;
            if (source.Trim().Length > length)
                return string.Format("{0} {1}", source.Substring(0, length), insteadStr);
            else
                return source;
        }

        /// <summary>
        /// 通过需要的序列组合匹配,获取需要的字符串。
        /// </summary>
        /// <param name="source">当前字符串实例</param>
        /// <param name="parameters">参数序列</param>
        /// <exception cref="System.Exception">参数错误!格式无效等!</exception>
        /// <exception cref="System.NullReferenceException">空 字符串 调用!</exception>
        /// <example>
        ///    需要获取 比如: 0 正确 ;1 错误 ;2 空 这样的结果 
        ///    <para>这样调用: GetSpecialBy("0","正确","1","错误","2","空");</para>   
        /// </example>
        /// <remarks>
        ///   需要获取 比如: 0 正确 ;1 错误 ;2 空 这样的结果
        ///   
        ///   这样调用: GetSpecialBy("0","正确","1","错误","2","空");
        /// </remarks>
        /// <returns></returns>
        public static string GetSpecialBy(this string source, params string[] parameters)
        {
            if (source == null)
                return string.Empty;

            if (parameters == null)
                return "参数错误";
            int length = parameters.Length;

            if (length == 1)
                return parameters[0];

            for (int i = 0; i < length; i++)
            {
                if (i % 2 == 0) //偶数项 和 奇数项交换
                {
                    if (source.Equals(parameters[i]))
                    {
                        return parameters[i + 1];
                    }
                }
            }

            return parameters[0];
        }


        /// <summary>
        /// 指定格式化字符串序列格式化当前字符串实例。
        /// </summary>
        /// <param name="format">当前字符串实例</param>
        /// <param name="args">格式化字符串序列</param>
        /// <exception cref="System.ArgumentNullException">空参数引用</exception>
        /// <exception cref="System.FormatException">格式化错误</exception>
        /// <returns></returns>
        public static string FormatWith(this string format, params object[] args)
        {
            return string.Format(format, args);
        }

        /// <summary>
        /// 使用指定的格式化方式修饰当前 <see cref="System.String"/> 实例。
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="formatter">格式化器,包含有且仅有一个{0}字符位置。</param>
        /// <returns></returns>
        public static string SurroundWith(this string source, string formatter)
        {
            if (String.IsNullOrEmpty(source))
                return string.Empty;
            return string.Format(formatter, source);
        }

        #endregion


        #endregion

        #region --- 判断部分 ---
        /// <summary>
        /// 返回一个值，该值表示当前的 <see cref="System.String "/> 对象是否是 null 或者为 <see cref="System.String.Empty"/>  。
        /// </summary>
        /// <param name="source">一个  <see cref="System.String "/> 引用</param>
        /// <returns>如果 value 参数为 null 或空字符串 ("")，则为 true；否则为 false。</returns>
        public static bool IsNullOrEmpty(this string source)
        {
            return string.IsNullOrEmpty(source);
        }
        /// <summary>
        /// 判断当前字符串是否是 Empty Or Null，如果是则返回指定的替代字符串。
        /// </summary>
        /// <param name="source"></param>
        /// <param name="replacor">指定的替代字符串</param>
        /// <returns></returns>
        public static string IfEmptyReplace(this string source, string replacor)
        {
            if (string.IsNullOrEmpty(source))
                return replacor;
            return source;
        }

        /// <summary>
        /// 检验 当前字符串实例是否 匹配 目标 正则表达式!
        /// </summary>
        /// <param name="source">当前字符串实例</param>
        /// <param name="regExpression">标 正则表达式</param>
        /// <exception cref="System.Exception">[正则表达式]错误!参数错误!格式无效等!</exception>
        /// <exception cref="System.ArgumentNullException">空 字符串 调用!</exception>
        /// <returns></returns>
        public static bool IsMatch(this string source, string regExpression)
        {
            if (source == null || source.Trim().Length == 0)
                return false;
            return System.Text.RegularExpressions.Regex.IsMatch(source, regExpression);
        }


        /// <summary>
        /// 返回一个值,表示当前 <see cref="System.String"/> 是否是有效的 <see cref="System.DateTime"/> 值。
        /// </summary>
        /// <param name="dateString"> </param>
        /// <returns>
        /// 	<c>true</c> if [is data time] [the specified date string]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsDataTime(this string dateString)
        {
            DateTime t = DateTime.Now;
            return DateTime.TryParse(dateString, out t);
        }
        #endregion


        /// <summary>
        ///  指定一组分隔符，以指定的方法将当前 <see cref="System.String"/> 实例分隔成  <typeparamref name="T"/> 序列。
        /// </summary>
        /// <typeparam name="T">需要得到的序列的元素类型。</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="resultSelecor">指定一个方法,SplitTo 需要的类型数组.</param>
        /// <param name="splitBy">分隔符，默认以空格分隔.</param>
        /// <returns></returns>
        public static IEnumerable<T> SplitTo<T>(this string source, Func<string, T> resultSelecor, params string[] splitBy)
        {
            if (source.IsNullOrEmpty())
                return default(T[]);
            if (splitBy == null || splitBy.Length == 0)
                splitBy = new string[] { string.Empty };
            string[] arr = source.Split(splitBy, StringSplitOptions.None);

            if (arr.Length > 0 && resultSelecor != null)
            {
                return arr.Select(resultSelecor);
            }
            return default(T[]);
        }

        /// <summary>
        /// 获取字符串中第一个汉字的拼音首字母
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns></returns>
        public static string GetFirstLetterFromChinese(this string str)
        {
            if (str.CompareTo("吖") < 0)
            {
                string s = str.Substring(0, 1).ToUpper();
                if (char.IsNumber(s, 0))
                {
                    return "0";
                }
                else
                {
                    return s;
                }
            }
            else if (str.CompareTo("八") < 0)
            {
                return "A";
            }
            else if (str.CompareTo("嚓") < 0)
            {
                return "B";
            }
            else if (str.CompareTo("咑") < 0)
            {
                return "C";
            }
            else if (str.CompareTo("妸") < 0)
            {
                return "D";
            }
            else if (str.CompareTo("发") < 0)
            {
                return "E";
            }
            else if (str.CompareTo("旮") < 0)
            {
                return "F";
            }
            else if (str.CompareTo("哈") < 0)
            {
                return "G";
            }
            else if (str.CompareTo("讥") < 0)
            {
                return "H";
            }
            else if (str.CompareTo("咔") < 0)
            {
                return "J";
            }
            else if (str.CompareTo("垃") < 0)
            {
                return "K";
            }
            else if (str.CompareTo("嘸") < 0)
            {
                return "L";
            }
            else if (str.CompareTo("拏") < 0)
            {
                return "M";
            }
            else if (str.CompareTo("噢") < 0)
            {
                return "N";
            }
            else if (str.CompareTo("妑") < 0)
            {
                return "O";
            }
            else if (str.CompareTo("七") < 0)
            {
                return "P";
            }
            else if (str.CompareTo("亽") < 0)
            {
                return "Q";
            }
            else if (str.CompareTo("仨") < 0)
            {
                return "R";
            }
            else if (str.CompareTo("他") < 0)
            {
                return "S";
            }
            else if (str.CompareTo("哇") < 0)
            {
                return "T";
            }
            else if (str.CompareTo("夕") < 0)
            {
                return "W";
            }
            else if (str.CompareTo("丫") < 0)
            {
                return "X";
            }
            else if (str.CompareTo("帀") < 0)
            {
                return "Y";
            }
            else if (str.CompareTo("咗") < 0)
            {
                return "Z";
            }
            else
            {
                return "0";
            }
        }

        /// <summary>
        /// 获取字符串中汉字的拼音首字母的 <see cref="String"/> 表示。非汉字字符将原样输出。
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns></returns>
        public static string GetEachLettersFromChinese(this string str)
        {
            StringBuilder sb = new StringBuilder();
            string temp = string.Empty;
            foreach (var item in str)
            {
                temp = item.ToString().GetFirstLetterFromChinese();
                sb.Append(temp);
            }

            return sb.ToString();
        }

        /// <summary>
        /// 返回一个以当前 <see cref="String"/> 包装之后的 <see cref="StringBuilder"/> 实例。
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public static StringBuilder AsStringBuilder(this string source)
        {
            if (source.IsNullOrEmpty())
                return new StringBuilder();

            return new StringBuilder(source);
        }

    }

}
