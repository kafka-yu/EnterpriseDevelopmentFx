using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NkjSoft.Validation.Validators
{
    /// <summary>
    /// 定义进行指定类型的验证。
    /// </summary>
    public class TypeValidator : EntityValidatorBase
    {
        /// <summary>
        /// 进行具体的验证操作，返回一个值，表示验证是否通过。
        /// </summary>
        /// <returns></returns>
        public override bool Validate()
        {
            return ValidationTypeHelper.GetMatchTypeFromTypeName(OriginalTypeName) == TargetType;
        }

        /// <summary>
        /// 获取默认的错误提示描述信息
        /// </summary>
        /// <value></value>
        protected override string _defaultMessage
        {
            get
            {
                return DefaultValidatorMessages.DataTypeMessage;
            }
            set
            {
                base._defaultMessage = value;
            }
        }


        /// <summary>
        /// 返回经过格式化的验证错误提示描述。
        /// </summary>
        /// <returns></returns>
        public override string BuildErrorMessage()
        {
            return string.Format(ErrorMessage, this.KeyName, ValidationTypeHelper.GetNameFromTypeCode(TargetType));
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class DateTimeValidator : TypeValidator
    {

        /// <summary>
        /// 进行具体的验证操作，返回一个值，表示验证是否通过。
        /// </summary>
        /// <returns></returns>
        public override bool Validate()
        {
            return false;
        }
    }

}
