//--------------------------文档信息----------------------------
//       
//                 文件名: IEntity                 
//                 CLR Version: 4.0.30319.1
//                 项目命名空间: NkjSoft.ORM.Data
//
//                 作  者: 俞如凯 
//                 Q Q: 250820436　 yurukai@vip.qq.com
//                 E-Mail: yurukai@hotmail.com
//                 创建时间 : 2010/8/20 12:01:13
//                 Copyright (c) Yurukai , All rights reserved.
//----------------------------------------------------------------


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace NkjSoft.ORM.Data
{
    /// <summary>
    /// 表示NkjSoft.ORM 实体的公共基接口定义,实现检测属性的更新。
    /// </summary>
    public interface IEntity : INotifyPropertyChanged
    {
        /// <summary>
        /// 获取实际进行 Insert 或者 Update 的字段集合。
        /// </summary>
        System.Collections.Specialized.StringDictionary PropertysToInsertOrUpdate { get; }

    }
}
