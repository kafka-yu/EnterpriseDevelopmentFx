using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NkjSoft.Validation.Validators
{
    /// <summary>
    /// 提供对属性是否满足最大值的验证。
    /// </summary>
    /// <remarks>该验证器只对 String/直接值类型 到 值类型的验证,即,当属性是 String 类型/值类型(包括DateTime)的时候, 需要验证其值是在指定的 MaximizeValue 之上。</remarks>
    public class MaximizeValidator : EntityValidatorBase
    {
        TypeValidator tv;
        /// <summary>
        /// 获取或设置属性值必须满足的最小值。
        /// </summary>
        /// <value>The maximize value.</value>
        public object MaximizeValue { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MaximizeValidator"/> class.
        /// </summary>
        public MaximizeValidator()
        {
            this.tv = new TypeValidator();
        }


        //TODO: 为完成 .. 2010/12/14 ..
        /// <summary>
        /// 进行具体的验证操作，返回一个值，表示验证是否通过。
        /// </summary>
        /// <remarks>该验证会根据属性 TargetType 进行类型判断,只要值类型(int,double,datetime,等才能进行验证。</remarks>
        /// <returns></returns>
        public override bool Validate()
        {
            tv.Target = this.Target;
            tv.KeyName = this.KeyName;
            tv.TargetType = this.TargetType;
            tv.OriginalTypeCode = this.OriginalTypeCode;
            tv.OriginalTypeName = this.OriginalTypeName;

            if (tv.Validate() == false)
            {
                this.ErrorMessage = tv.BuildErrorMessage();
                return false;
            }

            //是否是指定的类型.
            //if (base.Validate() == false)
            //{
            //    this.ErrorMessage = base.BuildErrorMessage();
            //    return false;
            //}
            //1. 如果是之类型的..---> int ,double ,datetime .. 

            //2.如果是 string 类型..-->根据 TargetType 进行具体类型的验证.


            return false;
        }


        /// <summary>
        /// 
        /// </summary>
        protected override string _defaultMessage
        {
            get
            {
                return DefaultValidatorMessages.MaximizeMessage;
            }
            set
            {
                base._defaultMessage = value;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string BuildErrorMessage()
        {
            return string.Format(ErrorMessage, this.KeyName, this.MaximizeValue);
        }

        internal bool IsGreateThan()
        {
            return false;
        }
    }
}
