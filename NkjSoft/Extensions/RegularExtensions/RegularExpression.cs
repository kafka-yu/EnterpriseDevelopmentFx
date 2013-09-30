using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace NkjSoft.Extensions.RegularExpExtensions
{
    /// <summary>
    /// 常用正则表达式的扩展
    /// </summary>
    ///<remarks>这里的东西比较常用! </remarks>
    public static class RegExpression
    {
        //验证是否是电子邮件字符串
        /// <summary>
        /// 验证是否是电子邮件字符串
        /// </summary>
        /// <param name="source">被验证的字符串</param>
        /// <returns>true 是,false 否</returns>
        /// <exception cref="System.ArgumentNullException">空字符串错误</exception>
        public static bool IsEmail(this string source)
        {
            return source.IsMatch(EMailRegExp);
        }
        /// <summary>
        /// 验证是否是 Http 地址 
        /// </summary>
        /// <remarks>验证符合 : Http://  开头的 Http 地址 的字符串</remarks>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsURL(this string source)
        {
            return source.IsMatch(UrlRegExp);
        }
        /// <summary>
        /// 验证是否是 [15、18]位身份证号码
        /// </summary>
        /// <remarks>验证符合  15、18位的合法身份证号码 </remarks>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsIDCard(this string source)
        {
            return source.IsMatch(IDRegExp);
        }

        #region --- 常量 ---
        /// <summary>
        /// 获取验证是 电子邮件的 的正则表达式字符串
        /// </summary>
        [ReadOnly(true)]
        public static string EMailRegExp
        {
            get { return @"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$"; }
        }
        /// <summary>
        /// 获取验证是 Http地址的 的正则表达式字符串
        /// </summary>
        /// <remarks>
        /// 验证符合 : Http://  开头的 Http 地址
        /// </remarks>
        [ReadOnly(true)]
        public static string UrlRegExp
        {
            get { return @"^http://([\w-]+\.)+[\w-]+(/[\w-./?%&=]*)?$"; }
        }
         

        /// <summary>
        /// 获取验证是 15、18位身份证号码 的正则表达式字符串
        /// </summary>
        /// <remarks>
        /// 验证15、18位身份证!
        /// </remarks>
        [ReadOnly(true)]
        public static string IDRegExp
        {
            get { return @"^\d{15}|\d{18}$"; }
        }

        /// <summary>
        /// 获取验证是否含有^%&',;=?$\x22 这些符号的 的正则表达式字符串
        /// </summary>
        /// <remarks>是否含有^%&',;=?$\x22 这些符号</remarks>
        [ReadOnly(true)]
        public static string ContainNotAllowdChars
        {
            get
            { return @"[^%&',;=?$\x22]+"; }
        }
        /// <summary>
        ///  获取验证是 只能输入由数字、26个英文字母或者下划线组成的字符串 的正则表达式字符串
        /// </summary>
        /// <remarks>只能输入由数字、26个英文字母或者下划线组成的字符串</remarks>
        [ReadOnly(true)]
        public static string OnlyPassWord
        {
            get { return @"^\w+$"; }
        }
        #endregion
         
    }
}
