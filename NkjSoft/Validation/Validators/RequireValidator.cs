using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NkjSoft.Validation.Validators
{
    /// <summary>
    /// 提供对属性不能为 Null 的验证。
    /// </summary>
    public class RequireValidator : EntityValidatorBase
    {
        /// <summary>
        /// 获取默认的错误提示描述信息
        /// </summary>
        /// <value></value>
        protected override string _defaultMessage
        {
            get
            {
                return DefaultValidatorMessages.RequireMessage;
            }
            set
            {
                base._defaultMessage = value;
            }
        }


        /// <summary>
        /// 进行具体的验证操作，返回一个值，表示验证是否通过。
        /// </summary>
        /// <returns></returns>
        public override bool Validate()
        {
            if (Target == null || string.IsNullOrEmpty(Target.ToString()))
                return false;
            return true;
        }
 
 

    }
}
