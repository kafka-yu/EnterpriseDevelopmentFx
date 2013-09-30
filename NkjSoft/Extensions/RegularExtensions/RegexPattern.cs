//--------------------------文档信息----------------------------
//       
//                 文件名: RegexPattern                 
//                 CLR Version: 4.0.30319.1
//                 项目命名空间: NkjSoft.Extensions.RegularExtensions
//
//                 作  者: 俞如凯 
//                 Q Q: 250820436　 yurukai@vip.qq.com
//                 E-Mail: yurukai@hotmail.com
//                 创建时间 : 2010/8/1 16:33:49
//                 Copyright (c) Yurukai , All rights reserved.
//----------------------------------------------------------------


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NkjSoft.Extensions.RegularExtensions
{
    /// <summary>
    /// 常用正则表达式收藏。
    /// </summary>
    public class RegexPattern
    {
        #region --- 常量2 ---

        /// <summary>
        /// 表示只匹配阿拉伯字母。
        /// </summary>
        public const string ALPHA = "[^a-zA-Z]";
        /// <summary>
        /// 表示只匹配阿拉伯字母+数字。
        /// </summary>
        public const string ALPHA_NUMERIC = "[^a-zA-Z0-9]";
        /// <summary>
        /// 表示只匹配阿拉伯字母+数字+空格。
        /// </summary>
        public const string ALPHA_NUMERIC_SPACE = @"[^a-zA-Z0-9\s]"; 
        /// <summary>
        /// 信用卡？
        /// </summary>
        public const string CREDIT_CARD_CARTE_BLANCHE = @"^(?:(?:[3](?:[0][0-5]|[6|8]))(?:\d{11,12}))$";
        /// <summary>
        /// 
        /// </summary>
        public const string CREDIT_CARD_DINERS_CLUB = @"^(?:(?:[3](?:[0][0-5]|[6|8]))(?:\d{11,12}))$";
        /// <summary>
        /// 
        /// </summary>
        public const string CREDIT_CARD_DISCOVER = @"^(?:(?:6011)(?:\d{12}))$";
        public const string CREDIT_CARD_EN_ROUTE = @"^(?:(?:[2](?:014|149))(?:\d{11}))$";
        public const string CREDIT_CARD_JCB = @"^(?:(?:(?:2131|1800)(?:\d{11}))$|^(?:(?:3)(?:\d{15})))$";
        public const string CREDIT_CARD_MASTER_CARD = @"^(?:(?:[5][1-5])(?:\d{14}))$";
        /// <summary>
        /// 不允许带数字。
        /// </summary>
        public const string CREDIT_CARD_STRIP_NON_NUMERIC = @"(\-|\s|\D)*";
        /// <summary>
        /// 银联卡号。
        /// </summary>
        public const string CREDIT_CARD_VISA = @"^(?:(?:[4])(?:\d{12}|\d{15}))$";
        /// <summary>
        /// Emial.
        /// </summary>
        public const string EMAIL = @"^([0-9a-zA-Z]+[-._+&])*[0-9a-zA-Z]+@([-0-9a-zA-Z]+[.])+[a-zA-Z]{2,6}$";
        /// <summary>
        /// 
        /// </summary>
        public const string EMBEDDED_CLASS_NAME_MATCH = "(?<=^_).*?(?=_)";
        /// <summary>
        /// 
        /// </summary>
        public const string EMBEDDED_CLASS_NAME_REPLACE = "^_.*?_";
        /// <summary>
        /// 
        /// </summary>
        public const string EMBEDDED_CLASS_NAME_UNDERSCORE_MATCH = "(?<=^UNDERSCORE).*?(?=UNDERSCORE)";
        /// <summary>
        /// 
        /// </summary>
        public const string EMBEDDED_CLASS_NAME_UNDERSCORE_REPLACE = "^UNDERSCORE.*?UNDERSCORE";
        /// <summary>
        /// 匹配 GUID。
        /// </summary>
        public const string GUID = "[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}";
        /// <summary>
        /// 匹配IP。
        /// </summary>
        public const string IP_ADDRESS = @"^(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$";
        /// <summary>
        /// 匹配小写字母。
        /// </summary>
        public const string LOWER_CASE = @"^[a-z]+$";
        /// <summary>
        /// 匹配数字。
        /// </summary>
        public const string NUMERIC = "[^0-9]";
        /// <summary>
        /// 
        /// </summary>
        public const string SOCIAL_SECURITY = @"^\d{3}[-]?\d{2}[-]?\d{4}$";
        /// <summary>
        /// 
        /// </summary>
        public const string SQL_EQUAL = @"\=";
        public const string SQL_GREATER = @"\>";
        public const string SQL_GREATER_OR_EQUAL = @"\>.*\=";
        public const string SQL_IS = @"\x20is\x20";
        public const string SQL_IS_NOT = @"\x20is\x20not\x20";
        public const string SQL_LESS = @"\<";
        public const string SQL_LESS_OR_EQUAL = @"\<.*\=";
        public const string SQL_LIKE = @"\x20like\x20";
        public const string SQL_NOT_EQUAL = @"\<.*\>";
        public const string SQL_NOT_LIKE = @"\x20not\x20like\x20";

        /// <summary>
        /// 匹配强密码文。
        /// </summary>
        public const string STRONG_PASSWORD =
            @"(?=^.{8,255}$)((?=.*\d)(?=.*[A-Z])(?=.*[a-z])|(?=.*\d)(?=.*[^A-Za-z0-9])(?=.*[a-z])|(?=.*[^A-Za-z0-9])(?=.*[A-Z])(?=.*[a-z])|(?=.*\d)(?=.*[A-Z])(?=.*[^A-Za-z0-9]))^.*";

        /// <summary>
        /// 匹配大写字母。
        /// </summary>
        public const string UPPER_CASE = @"^[A-Z]+$";
        /// <summary>
        /// 匹配URL。
        /// </summary>
        public const string URL = @"^^(ht|f)tp(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&%\$#_=]*)?$";
        public const string US_CURRENCY = @"^\$(([1-9]\d*|([1-9]\d{0,2}(\,\d{3})*))(\.\d{1,2})?|(\.\d{1,2}))$|^\$[0](.00)?$";
        public const string US_TELEPHONE = @"^[01]?[- .]?(\([2-9]\d{2}\)|[2-9]\d{2})[- .]?\d{3}[- .]?\d{4}$";
        public const string US_ZIPCODE = @"^\d{5}$";
        public const string US_ZIPCODE_PLUS_FOUR = @"^\d{5}((-|\s)?\d{4})$";
        public const string US_ZIPCODE_PLUS_FOUR_OPTIONAL = @"^\d{5}((-|\s)?\d{4})?$";

        #endregion
    }
}
