//--------------------------文档信息----------------------------
//       
//                 文件名: ValidationTypeHelp                 
//                 CLR Version: 4.0.30319.1
//                 项目命名空间: NkjSoft.Validation
//
//                 作  者: 俞如凯 
//                 Q Q: 250820436　 yurukai@vip.qq.com
//                 E-Mail: yurukai@hotmail.com
//                 创建时间 : 2010/12/7 22:04:06
//                 Copyright (c) Yurukai , All rights reserved.
//----------------------------------------------------------------


using System;
using System.Collections.Generic;
using System.Text;

namespace NkjSoft.Validation
{
    /// <summary>
    /// 封装实体验证的类型帮助方法.
    /// </summary>
    internal class ValidationTypeHelper
    {
        /// <summary>
        /// Gets the name from type code.
        /// </summary>
        /// <param name="typeCode">The type code.</param>
        /// <returns></returns>
        public static string GetNameFromTypeCode(MatchTypeCode typeCode)
        {
            string rtv = string.Empty;
            switch (typeCode)
            {
                case MatchTypeCode.Nullable:
                    rtv = "允许空的";
                    break;
                case MatchTypeCode.Interger:
                    rtv = "整数";
                    break;
                case MatchTypeCode.UInterger: rtv = "正整数";
                    break;
                case MatchTypeCode.NInterger: rtv = "父整数";
                    break;
                case MatchTypeCode.Email: rtv = "电子邮件";
                    break;
                case MatchTypeCode.URL: rtv = "网址";
                    break;
                case MatchTypeCode.Date: rtv = "日期";
                    break;
                case MatchTypeCode.Time: rtv = "时间";
                    break;
                case MatchTypeCode.DateTime: rtv = "日期时间";
                    break;
                case MatchTypeCode.Double: rtv = "小数";
                    break;
                case MatchTypeCode.Telephone: rtv = "手机号码";
                    break;
                case MatchTypeCode.Phone: rtv = "电话号码";
                    break;
                case MatchTypeCode.RegexExpression:
                    rtv = "自定义类型";
                    break;
                default:
                    break;
            }
            return rtv;
        }

        /// <summary>
        /// Determines whether this instance [can be the match type].
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if this instance [can be the match type]; otherwise, <c>false</c>.
        /// </returns>
        public static bool CanBeTheMatchType()
        {
            return false;
        }

        /// <summary>
        /// Gets the match type from type code.
        /// </summary>
        /// <param name="originalTypeCode">The original type code.</param>
        /// <returns></returns>
        public static MatchTypeCode GetMatchTypeFromTypeCode(TypeCode originalTypeCode)
        {
            MatchTypeCode mtc = MatchTypeCode.String;
            switch (originalTypeCode)
            {
                case TypeCode.Boolean:
                case TypeCode.Byte:
                case TypeCode.Char:
                case TypeCode.DBNull:
                case TypeCode.Empty:
                case TypeCode.Object:
                case TypeCode.String:
                    break;
                case TypeCode.DateTime:
                    mtc = MatchTypeCode.DateTime;
                    break;
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    mtc = MatchTypeCode.Double;
                    break;
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                    mtc = MatchTypeCode.Interger;
                    break;
                case TypeCode.SByte:
                    mtc = MatchTypeCode.SmallInterger;
                    break;
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                    mtc = MatchTypeCode.UInterger;
                    break;
                default:
                    break;
            }
            return mtc;
        }

        internal static MatchTypeCode GetMatchTypeFromTypeName(string OriginalTypeName)
        {
            MatchTypeCode mtc = MatchTypeCode.String;
            switch (OriginalTypeName)
            {

                case "System.String":
                case "System.Object":
                case "System.Empty":
                default:
                    break;
                case "System.DateTime":
                    mtc = MatchTypeCode.DateTime;
                    break;
                case "System.Date":
                    mtc = MatchTypeCode.Date;
                    break;
                case "System.Int32":
                case "System.Int64":
                case "System.Int16":
                    mtc = MatchTypeCode.Interger;
                    break;
                case "System.Double":
                case "System.Single":
                case "System.Decimal":
                    mtc = MatchTypeCode.Double;
                    break;
                case "System.UInt16":
                case "System.UInt32":
                case "System.UInt64":
                    mtc = MatchTypeCode.UInterger;
                    break;
            }
            return mtc;
        }
    }
}
