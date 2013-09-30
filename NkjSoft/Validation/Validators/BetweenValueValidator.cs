//--------------------------文档信息----------------------------
//       
//                 文件名: BetweenValueValidator                 
//                 CLR Version: 4.0.30319.1
//                 项目命名空间: NkjSoft.Validation.Validators
//
//                 作  者: 俞如凯 
//                 Q Q: 250820436　 yurukai@vip.qq.com
//                 E-Mail: yurukai@hotmail.com
//                 创建时间 : 2010/12/7 23:28:52
//                 Copyright (c) Yurukai , All rights reserved.
//----------------------------------------------------------------


using System;
using System.Collections.Generic;
using System.Text;

namespace NkjSoft.Validation.Validators
{
    /// <summary>
    /// 表示进行属性值必须介于指定的两个值之间的验证。
    /// </summary>
    public class BetweenValueValidator : EntityValidatorBase
    {
        /// <summary>
        /// 获取或设置下限.
        /// </summary>
        public object Low { get; set; }

        /// <summary>
        /// 获取或设置上限.
        /// </summary>
        public object Height { get; set; }

        /// <summary>
        /// 进行具体的验证操作，返回一个值，表示验证是否通过。
        /// </summary>
        /// <returns></returns>
        public override bool Validate()
        {
            return false;
        }

        /// <summary>
        /// 返回经过格式化的验证错误提示描述。
        /// </summary>
        /// <returns></returns>
        public override string BuildErrorMessage()
        {
            return string.Format("“{0}”必须介于 {1} ～ {2} 之间.", this.KeyName, this.Low.ToString(), this.Height);
        }
    }
}
