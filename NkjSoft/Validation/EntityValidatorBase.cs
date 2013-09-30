using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NkjSoft.Validation
{
    /// <summary>
    /// 定义一个封装提供用于对指定类类型的属性进行验证能力的 abstract 类。
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = true)]
    public abstract class EntityValidatorBase : Attribute, IValidator
    {

        /// <summary>
        /// 
        /// </summary>
        protected TypeCode _originalTypeCode;
        /// <summary>
        /// 获取或设置值的类型码.
        /// </summary>
        public TypeCode OriginalTypeCode
        {
            get { return _originalTypeCode; }
            set { _originalTypeCode = value; }
        }

        /// <summary>
        /// 获取或设置验证值的类型名称。
        /// </summary>
        public string OriginalTypeName
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityValidatorBase"/> class.
        /// </summary>
        /// <param name="keyName">Name of the key.</param>
        /// <param name="message">The message.</param>
        public EntityValidatorBase(string keyName, string message)
        {
            this.KeyName = keyName;
            this.ErrorMessage = message;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityValidatorBase"/> class.
        /// </summary>
        public EntityValidatorBase()
        {

        }


        #region IEntityValidator 成员


        /// <summary>
        /// 获取或设置表示验证目标 的值。
        /// </summary>
        public object Target { get; set; }

        /// <summary>
        /// 获取或设置表示验证目标的类型代码。
        /// </summary>
        public MatchTypeCode TargetType { get; set; }

        /// <summary>
        /// 获取或设置用于进行自定义正则表达式验证的正则表达式语句。
        /// </summary>
        /// <value></value>
        public string RegexExpression { get; set; }

        /// <summary>
        /// 进行具体的验证操作，返回一个值，表示验证是否通过。
        /// </summary>
        /// <returns></returns>
        public abstract bool Validate();
        /// <summary>
        /// 进行验证的外部调用方法。
        /// </summary>
        /// <param name="instance">具体验证器对象</param>
        /// <returns></returns>
        public bool Validate(Action<EntityValidatorBase> instance)
        {
            //TODO:测试..2010.12.7 15:23 
            _isValidated = Validate();
            if (_isValidated == false)
                instance(this);
            return _isValidated;
        }

        /// <summary>
        /// 返回一个值,表示当前验证是否通过了。
        /// </summary>
        public bool IsValidate
        {
            get { return Validate(); }
        }
        /// <summary>
        /// 
        /// </summary>
        protected bool _isValidated = true;
        /// <summary>
        /// 获取或设置验证错误的时候的提示描述..
        /// </summary>
        protected string _errorMessage = "";
        /// <summary>
        /// 获取默认的错误提示描述信息
        /// </summary>
        protected virtual string _defaultMessage { get; set; }
        /// <summary>
        ///  获取或设置对验证信息的格式包装信息。
        /// </summary>
        public string ErrorMessage
        {
            get
            {
                if (string.IsNullOrEmpty(_errorMessage))
                {
                    if (string.IsNullOrEmpty(_defaultMessage))
                    {
                        return string.Empty;
                    }
                    else return _defaultMessage;
                }
                else
                    return _errorMessage;
            }
            //return string.IsNullOrEmpty(_errorMessage) ?  string.IsNullOrEmpty(_defaultMessage) ? "" : _errorMessage; }
            set { _errorMessage = value; }
        }


        /// <summary>
        /// 获取或设置 验证的键 名.
        /// </summary>
        public string KeyName
        {
            get;
            set;
        }

        #endregion

        /// <summary>
        /// 返回经过格式化的验证错误提示描述。
        /// </summary>
        /// <returns></returns>
        public virtual string BuildErrorMessage()
        {
            return string.Format(this.ErrorMessage, this.KeyName);
        }
    }


    /// <summary>
    /// 定义提供验证功能的接口。
    /// </summary>
    public interface IValidator
    {
        /// <summary>
        /// 进行验证。
        /// </summary> 
        /// <param name="valToValidate">进行验证的值。</param>
        /// <returns></returns>
        bool Validate();

        /// <summary>
        /// 进行验证,并提供一个对验证器进行处理的方法。
        /// </summary>
        /// <param name="entityValidatorBase">一个对验证器进行处理的方法</param>
        /// <returns></returns>
        bool Validate(Action<EntityValidatorBase> entityValidatorBase);

        /// <summary>
        /// 获取一个值，表示验证是否通过。
        /// </summary>
        bool IsValidate { get; }


        /// <summary>
        /// 获取或设置对验证信息的格式包装信息。
        /// </summary>
        string ErrorMessage { get; set; }

        /// <summary>
        /// 获取或设置一个值，表示当前需要进行验证的属性（字段）的友好名称。
        /// </summary>
        string KeyName { get; set; }

        /// <summary>
        /// 返回经过格式化的验证错误提示描述。
        /// </summary>
        /// <returns></returns>
        string BuildErrorMessage();



        /// <summary>
        /// 获取或设置表示验证目标 的值。
        /// </summary>
        object Target { get; set; }

        /// <summary>
        /// 获取或设置表示验证目标的类型代码。
        /// </summary>
        MatchTypeCode TargetType { get; set; }

        /// <summary>
        /// 获取或设置用于进行自定义正则表达式验证的正则表达式语句。
        /// </summary>
        string RegexExpression { get; set; }
    }
}
