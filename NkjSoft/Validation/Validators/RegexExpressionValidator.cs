//--------------------------文档信息----------------------------
//       
//                 文件名: RegexExpressionValidator                 
//                 CLR Version: 4.0.30319.1
//                 项目命名空间: NkjSoft.Validation.Validators
//
//                 作  者: 俞如凯 
//                 Q Q: 250820436　 yurukai@vip.qq.com
//                 E-Mail: yurukai@hotmail.com
//                 创建时间 : 2010/12/7 23:15:21
//                 Copyright (c) Yurukai , All rights reserved.
//----------------------------------------------------------------


using System;
using System.Collections.Generic;
using System.Text;

namespace NkjSoft.Validation.Validators
{
    /// <summary>
    /// 正则表达式验证器.
    /// </summary>
    public class RegexExpressionValidator : EntityValidatorBase
    {
        /// <summary>
        /// 进行具体的验证操作，返回一个值，表示验证是否通过。
        /// </summary>
        /// <returns></returns>
        public override bool Validate()
        {
            return System.Text.RegularExpressions.Regex.IsMatch(this.Target == null ? string.Empty : this.Target.ToString(), this.RegexExpression);
        }

    }
}
