//--------------------------作者信息----------------------------
//       
//                 文件名: IValidationProvider                 
//                 CLR Version: 4.0.30319.1
//                 项目命名空间: NkjSoft.Validation
//
//                 作  者: 俞如凯 
//                 Q Q: 250820436　 yurukai@vip.qq.com
//                 E-Mail: yurukai@hotmail.com
//                 创建时间 : 2010/12/12 21:27:21
//                 Copyright (c) Yurukai , All rights reserved.
//----------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NkjSoft.Validation;

namespace NkjSoft.Validation
{
    /// <summary>
    ///  定义提供不同验证策略的验证功能接口契约。
    /// </summary>
    public interface IValidationProvider
    {
        /// <summary>
        /// 获取当前对象属性中所有标记的 EntityValidatorBase 的特性标记对象序列。
        /// </summary>
        IEnumerable<EntityValidatorBase> GetEntityValidators();
        /// <summary>
        /// 获取当前对象的指定属性中所有标记的 EntityValidatorBase 的特性标记对象序列。
        /// </summary>
        /// <param name="predicate">指定一个用于检索指定属性的方法</param>
        /// <returns></returns>
        IEnumerable<EntityValidatorBase> GetEntityValidators(Func<System.Reflection.PropertyInfo, bool> predicate);

        /// <summary>
        /// 获取或设置被验证的类对象。
        /// </summary>
        /// <value>The target.</value>
        object Target { get; set; }

        /// <summary>
        /// 获取一个值,表示当前类型的验证是否通过全部检查。
        /// </summary>
        bool IsValid { get; }


        /// <summary>
        /// 对当前的实体进行默认验证，返回一个值，表示是否所有的验证通过检查。指定一个对验证结果的所有验证错误信息进行接收处理的方法。
        /// </summary>
        /// <param name="out_Action">指定一个方法，用于接收验证操作最后的全部验证信息列表</param> 
        /// <exception cref="System.ArgumentNullException">format 或 args 为 null。</exception>
        /// <exception cref="System.FormatException">format 无效。- 或 -格式项的索引小于零或大于等于 args 数组的长度。</exception> 
        bool Validate(Action<string> out_Action);


        /// <summary>
        /// 对当前的实体进行默认验证，返回一个值，表示是否所有的验证通过检查。指定一个对验证结果的所有验证错误信息进行接收处理的方法。
        /// </summary>
        /// <param name="out_Action">指定一个方法，用于接收验证操作最后的全部验证信息列表</param>
        /// <param name="messageItemFormatter">指定一个字符格式化串，用于对验证信息列表的每一项进行格式化输出。</param>
        ///  <exception cref="System.ArgumentNullException">format 或 args 为 null。</exception>
        /// <exception cref="System.FormatException">format 无效。- 或 -格式项的索引小于零或大于等于 args 数组的长度。</exception>
        /// <returns></returns>
        bool Validate(Action<string> out_Action, string messageItemFormatter);

        /// <summary>
        /// 对当前的实体进行默认验证，返回一个值，表示是否所有的验证通过检查。该重载不对验证结果的所有验证错误信息进行接收处理。
        /// </summary> 
        /// <exception cref="System.ArgumentNullException">format 或 args 为 null。</exception>
        /// <exception cref="System.FormatException">format 无效。- 或 -格式项的索引小于零或大于等于 args 数组的长度。</exception> 
        bool Validate();
    }
}
