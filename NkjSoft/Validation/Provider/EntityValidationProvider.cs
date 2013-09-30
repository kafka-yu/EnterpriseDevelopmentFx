using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NkjSoft.Validation;

namespace NkjSoft.Validation.Provider
{
    /// <summary>
    /// 针对自定义的 Attribute 的 类类型的验证实现的验证 Provider 。
    /// </summary>
    public class EntityValidationProvider : IValidationProvider
    {

        /// <summary>
        /// 实例化一个 <see cref="EntityValidationProvider"/> 对象。
        /// </summary>
        /// <param name="target">设置一个被验证的对象.</param>
        /// <exception cref="ArgumentNullException">必须设置被验证的类对象</exception>
        public EntityValidationProvider(object target)
        {
            if (target == null)
                throw new ArgumentNullException("必须设置被验证的类对象。");
            this.Target = target;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityValidationProvider"/> class.
        /// </summary>
        public EntityValidationProvider()
        {

        }

        #region IValidationProvider 成员

        /// <summary>
        /// 获取当前对象属性中所有标记的 EntityValidatorBase 的特性标记对象序列。
        /// </summary>
        public IEnumerable<EntityValidatorBase> GetEntityValidators()
        {
            var pp = Target.GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);

            foreach (var item in pp)
            {
                var attributes = item.GetCustomAttributes(typeof(EntityValidatorBase), false);

                foreach (var validator in attributes)
                {
                    yield return validator as EntityValidatorBase;
                }
            }
        }

        /// <summary>
        /// 获取当前对象的指定属性中所有标记的 EntityValidatorBase 的特性标记对象序列。
        /// </summary>
        /// <param name="predicate">指定一个用于检索指定属性的方法</param>
        /// <returns></returns>
        public IEnumerable<EntityValidatorBase> GetEntityValidators(Func<System.Reflection.PropertyInfo, bool> predicate)
        {
            var pp = this.GetType().GetProperties().Where(predicate);

            foreach (var item in pp)
            {
                var attributes = item.GetCustomAttributes(typeof(EntityValidatorBase), false);

                foreach (var validator in attributes)
                {
                    yield return validator as EntityValidatorBase;
                }
            }
        }

        private bool _isValid = true;
        /// <summary>
        /// 对当前的实体进行默认验证，返回一个值，表示是否所有的验证通过检查。指定一个对验证结果的所有验证错误信息进行接收处理的方法。
        /// </summary>
        /// <param name="out_Action">指定一个方法，用于接收验证操作最后的全部验证信息列表</param>
        /// <param name="messageItemFormatter">指定一个字符格式化串，用于对验证信息列表的每一项进行格式化输出。</param>
        ///  <exception cref="System.ArgumentNullException">format 或 args 为 null。</exception>
        /// <exception cref="System.FormatException">format 无效。- 或 -格式项的索引小于零或大于等于 args 数组的长度。</exception>
        /// <returns></returns>
        public virtual bool Validate(Action<string> out_Action, string messageItemFormatter)
        {
            if (out_Action == null)
                throw new NullReferenceException("必须指定验证信息接收参数");
            bool isValid = false;
            messageItemFormatter = string.IsNullOrEmpty(messageItemFormatter) ? "{0}" : messageItemFormatter;
            StringBuilder sb = new StringBuilder();
            //UNDONE:要缓存呼??
            System.Reflection.PropertyInfo[] p = Target.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

            foreach (var item in p)
            {
                var attributes = item.GetCustomAttributes(typeof(EntityValidatorBase), false);
                if (attributes.Length == 0)
                    continue;
                var val = item.GetValue(Target, null);
                var val_TypeName = item.PropertyType.FullName;
                if (item.PropertyType.IsGenericType)
                {
                    int startIndex = val_TypeName.IndexOf("[[");

                    val_TypeName = val_TypeName.Substring(startIndex + 2, val_TypeName.IndexOf(",") - startIndex - 2);
                }
                //获取一些其他信息
                //var val_code = val == null ? TypeCode.Object : type.b.GetTypeCode(); 
                // var val_TypeName = typeFullName;// item.PropertyType.GetGenericTypeDefinition().FullName;  //"System.Nullable`1[[System.Double, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]"

                foreach (var validator in attributes)
                {
                    var _nameValidator = ((EntityValidatorBase)validator);
                    _nameValidator.KeyName = _nameValidator.KeyName ?? item.Name;
                    _nameValidator.Target = val;
                    //_nameValidator.OriginalTypeCode = val_code;
                    _nameValidator.OriginalTypeName = val_TypeName;
                    var isOk = _nameValidator.Validate(v =>
                    {
                        sb.AppendFormat(string.Format(messageItemFormatter, v.BuildErrorMessage()));
                    });
                }
            }
            if (sb.Length == 0)
                isValid = true;
            else
            {
                out_Action(sb.ToString());
            }

            _isValid = isValid;
            return _isValid;
        }
        /// <summary>
        /// 对当前的实体进行默认验证，返回一个值，表示是否所有的验证通过检查。指定一个对验证结果的所有验证错误信息进行接收处理的方法。
        /// </summary>
        /// <param name="out_Action">指定一个方法，用于接收验证操作最后的全部验证信息列表</param> 
        /// <exception cref="System.ArgumentNullException">format 或 args 为 null。</exception>
        /// <exception cref="System.FormatException">format 无效。- 或 -格式项的索引小于零或大于等于 args 数组的长度。</exception>
        public bool Validate(Action<string> out_Action)
        {
            return Validate(out_Action, string.Empty);

        }

        /// <summary>
        /// 对当前的实体进行默认验证，返回一个值，表示是否所有的验证通过检查。该重载不对验证结果的所有验证错误信息进行接收处理。
        /// </summary> 
        /// <exception cref="System.ArgumentNullException">format 或 args 为 null。</exception>
        /// <exception cref="System.FormatException">format 无效。- 或 -格式项的索引小于零或大于等于 args 数组的长度。</exception>
        public bool Validate()
        { return Validate((s) => { }); }

        /// <summary>
        /// 获取一个值,表示当前类型的验证是否通过全部检查。
        /// </summary>
        public bool IsValid
        {
            get { return _isValid; }
        }


        /// <summary>
        /// 获取或设置被验证的类对象。
        /// </summary>
        /// <value>The target.</value>
        public object Target
        {
            get;
            set;
        }

        #endregion
    }
}
