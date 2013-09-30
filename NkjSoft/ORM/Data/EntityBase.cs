//--------------------------文档信息----------------------------
//       
//                 文件名: EntityBase                 
//                 CLR Version: 4.0.30319.1
//                 项目命名空间: NkjSoft.ORM.Data
//
//                 作  者: 俞如凯 
//                 Q Q: 250820436　 yurukai@vip.qq.com
//                 E-Mail: yurukai@hotmail.com
//                 创建时间 : 2010/8/20 11:32:13
//                 Copyright (c) Yurukai , All rights reserved.
//----------------------------------------------------------------


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NkjSoft.ORM.Data.Mapping;
using NkjSoft.Validation;
using NkjSoft.Validation.Provider;

namespace NkjSoft.ORM.Data
{
    /// <summary>
    /// 表示NkjSoft.ORM 实体的公共基类,实现检测属性的更新
    /// </summary>
    [Serializable]
    public abstract partial class EntityBase : IEntity
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityBase"/> class.
        /// </summary>
        protected EntityBase()
        {
            //this.ValidationProvider = new EntityValidationProvider(this);
            this.ValidationProvider =
                DefaultValidationProvider(this);

        }
        /// <summary>
        /// Initializes a new instance of the <see cref="EntityBase"/> class.
        /// </summary>
        /// <param name="useInsertOrUpdateNotify">if set to <c>true</c> [use insert or update notify].</param>
        public EntityBase(bool useInsertOrUpdateNotify)
            : this()
        {
            SetInsertOrUpdateNotify(useInsertOrUpdateNotify);
        }

        #region IEntity 成员

        /// <summary>
        /// 
        /// </summary>
        protected System.Collections.Specialized.StringDictionary _propertysToInsertOrUpdate = new System.Collections.Specialized.StringDictionary();
        /// <summary>
        /// 获取实际进行 Insert 或者 Update 的字段集合。
        /// </summary>
        /// <value></value>
        public System.Collections.Specialized.StringDictionary PropertysToInsertOrUpdate
        {
            get { return _propertysToInsertOrUpdate; }
        }
        #endregion

        #region INotifyPropertyChanged 成员

        /// <summary>
        /// 在属性值更改时发生。
        /// </summary>
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        #endregion


        /// <summary>
        /// Handles the PropertyChanged event of the MailBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.ComponentModel.PropertyChangedEventArgs"/> instance containing the event data.</param>
        protected virtual void PropertyChangedHandler(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (_propertysToInsertOrUpdate.ContainsKey(e.PropertyName))
                return;
            else
                _propertysToInsertOrUpdate[e.PropertyName] = e.PropertyName;
        }
        /// <summary>
        /// 类的属性值更改的时候的事件处理函数.
        /// </summary>
        /// <param name="propertyName">属性名</param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (null != handler)
                this.PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// 指定一个值。该值表示在 Insert Or Update 的时候是否自动判断只需要更新实体中属性值更改的字段。
        /// </summary> 
        public void SetInsertOrUpdateNotify(bool propertyChangedNotify)
        {
            if (propertyChangedNotify)
            {
                if (_propertysToInsertOrUpdate == null)
                    _propertysToInsertOrUpdate = new System.Collections.Specialized.StringDictionary();
                else
                    _propertysToInsertOrUpdate.Clear();
                if (this.PropertyChanged == null)
                    this.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(PropertyChangedHandler);
            }
            else
            {
                _propertysToInsertOrUpdate.Clear();

                this.PropertyChanged -= new System.ComponentModel.PropertyChangedEventHandler(PropertyChangedHandler);
            }
        }


        #region IValidationProvider 成员

        /// <summary>
        /// 对当前的实体进行默认验证，返回一个值，表示是否所有的验证通过检查。指定一个对验证结果的所有验证错误信息进行接收处理的方法。
        /// </summary>
        /// <param name="out_Action">指定一个方法，用于接收验证操作最后的全部验证信息列表</param>
        /// <param name="messageItemFormatter">指定一个字符格式化串，用于对验证信息列表的每一项进行格式化输出。</param>
        ///  <exception cref="System.ArgumentNullException">format 或 args 为 null。</exception>
        /// <exception cref="System.FormatException">format 无效。- 或 -格式项的索引小于零或大于等于 args 数组的长度。</exception>
        /// <returns></returns>
        public bool Validate(Action<string> out_Action, string messageItemFormatter)
        {
            return fetchTheValidationProvider().Validate(out_Action, messageItemFormatter);

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

        #endregion


        #region --- Validation ---
        /// <summary>
        /// 获取或设置进行属性验证的验证器对象。
        /// </summary>
        public IValidationProvider ValidationProvider
        { get; set; }


        /// <summary>
        /// 判断并返回当前对象的 IValidationProvider 属性的值,如果为Null ,抛出异常.
        /// </summary>
        /// <returns></returns>
        private IValidationProvider fetchTheValidationProvider()
        {
            if (this.ValidationProvider == null)
                throw new NullReferenceException("没有验证提供程序.");
            return this.ValidationProvider;
        }
        #endregion

    }


    /// <summary>
    /// 
    /// </summary>
    public partial class EntityBase
    {
        /// <summary>
        /// Defaults the validation provider.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns></returns>
        public static IValidationProvider DefaultValidationProvider(object obj)
        {
            //return new EntityValidation.EntityColumnInfoValidationProvider<EntityBase>(obj as EntityBase);
            return new EntityValidationProvider(obj);
        }

    }

}
