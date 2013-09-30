//--------------------------文档信息----------------------------
//       
//                 文件名: MatchTypeCode                 
//                 CLR Version: 4.0.30319.1
//                 项目命名空间: NkjSoft.Validation
//
//                 作  者: 俞如凯 
//                 Q Q: 250820436　 yurukai@vip.qq.com
//                 E-Mail: yurukai@hotmail.com
//                 创建时间 : 2010/12/7 22:59:27
//                 Copyright (c) Yurukai , All rights reserved.
//----------------------------------------------------------------


using System;
using System.Collections.Generic;
using System.Text;

namespace NkjSoft.Validation
{
    /// <summary>
    /// 进行验证的目标类型
    /// </summary>
    public enum MatchTypeCode
    {
        /// <summary>
        /// 允许空值
        /// </summary>
        Nullable,

        /// <summary>
        /// 整数
        /// </summary>
        Interger,

        /// <summary>
        /// 正整数
        /// </summary>
        UInterger,

        /// <summary>
        /// 负整数
        /// </summary>
        NInterger,

        /// <summary>
        /// E-mail
        /// </summary>
        Email,

        /// <summary>
        /// URL
        /// </summary>
        URL,

        /// <summary>
        /// 日期
        /// </summary>
        Date,

        /// <summary>
        /// 时间
        /// </summary>
        Time,

        /// <summary>
        /// 日期时间
        /// </summary>
        DateTime,

        /// <summary>
        /// 小数
        /// </summary>
        Double,

        /// <summary>
        /// 手机号码(全网段)
        /// </summary>
        Telephone,

        /// <summary>
        /// 电话
        /// </summary>
        Phone,

        /// <summary>
        /// 自定义正则表达式
        /// </summary>
        RegexExpression,

        /// <summary>
        /// 字符串
        /// </summary>
        String,

        /// <summary>
        /// -127 --- 128 之间的整数
        /// </summary>
        SmallInterger,
    }
}
