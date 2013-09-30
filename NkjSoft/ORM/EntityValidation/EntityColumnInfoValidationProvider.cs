//--------------------------文档信息----------------------------
//       
//                 文件名: EntityColumnInfoValidationProvider                 
//                 CLR Version: 4.0.30319.1
//                 项目命名空间: NkjSoft.ORM.EntityValidation
//
//                 作  者: 俞如凯 
//                 Q Q: 250820436　 yurukai@vip.qq.com
//                 E-Mail: yurukai@hotmail.com
//                 创建时间 : 2010/12/12 23:00:46
//                 Copyright (c) Yurukai , All rights reserved.
//----------------------------------------------------------------


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NkjSoft.ORM.EntityValidation
{
    /// <summary>
    /// 定义针对 ORM.Entity 的验证实现 Provider 的泛型版本。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <remarks>该验证 Provider 的实现,可以根据 ORM.EntityBase 的子类的属性(字段)的自定义标记(Attribute)的 ColumnAttribute 对象获取验证规则和信息。 </remarks>
    public sealed class EntityColumnInfoValidationProvider<T> : NkjSoft.Validation.Provider.EntityValidationProvider where T : ORM.Data.EntityBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EntityColumnInfoValidationProvider&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public EntityColumnInfoValidationProvider(T entity)
            : base(entity)
        {

        }
    }
}
