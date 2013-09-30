using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NkjSoft.Validation
{ 
    /// <summary>
    /// 封装默认错误验证信息的字符串描述信息。该类无法被继承。
    /// </summary>
    public sealed class DefaultValidatorMessages
    {
        /// <summary>
        /// 获取表示 必需 字段的错误信息描述.其中 {0} 表示字段名称占位符.
        /// </summary>
        public const string RequireMessage = "一个有效的“{0}”是必须的。";

        /// <summary>
        /// 获取表示 某个数据类型 字段的错误信息描述.其中 {0} 表示字段名称占位符,{1}表示数据类型中文名称.
        /// </summary>
        public const string DataTypeMessage = "“{0}”必须是一个有效的 {1} 值。";

        /// <summary>
        /// 获取表示 某个数据字段的值必须大于一个数值 的错误信息描述.其中 {0} 表示字段名称占位符,{1}表示数值最小值.
        /// </summary>
        public const string MaximizeMessage = "“{0}”的值必须大于 {1} 。";

        /// <summary>
        /// 获取表示 某个数据字段的值必须小于一个数值 的错误信息描述.其中 {0} 表示字段名称占位符,{1}表示数值最大值.
        /// </summary>
        public const string MinimizeMessage = "“{0}”的值必须小于 {1} 。";

        /// <summary>
        /// 获取表示 某个数据字段的值必须介于两个数值之间 的错误信息描述.其中 {0} 表示字段名称占位符,{1}表示数值最小值.{2}表示最大值。
        /// </summary>
        public const string BetweenValueMessage = "“{0}”的值必须介于 {1} ~ {2} 之间。";

        /// <summary>
        /// 获取表示 某个数据字段的字符长度必须小于某个值 的错误信息描述.其中 {0} 表示字段名称占位符, {1}表示最大值。
        /// </summary>
        public const string LengthErrorMessage = "“{0}”的长度必须在 0 ~ {1} 之间，您输入的字符过长了。";


    }
}
