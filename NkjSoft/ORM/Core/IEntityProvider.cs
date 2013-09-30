// Copyright (c) Microsoft Corporation.  All rights reserved.
// This source code is made available under the terms of the Microsoft Public License (MS-PL)

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace NkjSoft.ORM.Core
{
    /// <summary>
    /// 定义对数据映射对象到数据库实体的基本操作的功能。
    /// </summary>
    public interface IEntityProvider : IQueryProvider
    {
        /// <summary>
        /// 通过映射表的获取强类型的 <see cref="NkjSoft.ORM.Core.IEntityTable"/>&lt;<typeparamref name="T"/>&gt;结果。
        /// </summary>
        /// <typeparam name="T">映射表的类型</typeparam>
        /// <param name="tableId">映射表的ID</param>
        /// <returns></returns>
        IEntityTable<T> GetTable<T>(string tableId);
        /// <summary>
        /// 指定需要获取的 <see cref="NkjSoft.ORM.Core.IEntityTable"/> 类型以及映射表的 <see cref="System.String"/> 表示，获取强类型的 <see cref="NkjSoft.ORM.Core.IEntityTable"/>&lt;<typeparamref name="T"/>&gt;
        /// </summary>
        /// <param name="type">需要获取的 <see cref="NkjSoft.ORM.Core.IEntityTable"/> 类型</param>
        /// <param name="tableId">映射表的 <see cref="System.String"/> 表示</param>
        /// <returns></returns>
        IEntityTable GetTable(Type type, string tableId);
        /// <summary>
        /// 判断指定的 Lambda 表达式实例是否能被映射到本地变量。
        /// </summary>
        /// <param name="expression">指定的表达式</param>
        /// <returns>
        /// 	<c>true</c> 表示表达式中的变量类型的参数可以被映射到本地变量，这样的好处是减少 DbParameter 的个数; otherwise, <c>false</c>.
        /// </returns>
        bool CanBeEvaluatedLocally(Expression expression);
        /// <summary>
        /// 判断指定的 Lambda 表达式是否能被过滤为参数化的表达式。
        /// </summary>
        /// <param name="expression">指定的表达式</param>
        /// <returns>
        /// 	<c>true</c> 表示能被参数化; otherwise, <c>false</c>.
        /// </returns>
        bool CanBeParameter(Expression expression);
    }

    /// <summary>
    /// 定义表示通用的对从数据库映射关联的一个未知 CLR 表对象的操作。
    /// </summary>
    public interface IEntityTable : IQueryable, IUpdatable
    {
        /// <summary>
        /// 获取当前实体操作绑定的数据查询提供程序。
        /// </summary>
        /// <value>The provider.</value>
        new IEntityProvider Provider { get; }
        /// <summary>
        /// 获取一个 <see cref="System.String"/> ，表示映射表对象的ID。
        /// </summary>
        /// <value></value>
        string TableId { get; }
        /// <summary>
        /// 通过映射表对象的ID获取一个 <see cref="System.Object"/> 对象。
        /// </summary>
        /// <param name="id">映射表对象的ID</param>
        /// <returns></returns>
        object GetById(object id);
        /// <summary>
        /// 向数据库插入一个实体。该实体信息来自通过映射的对象数据。
        /// </summary>
        /// <param name="instance">提供实体信息</param>
        /// <returns></returns>
        int Insert(object instance);
        /// <summary>
        /// 对数据库的一条记录进行更新，更新的数据来自映射的实体。
        /// </summary>
        /// <param name="instance">提供实体信息</param>
        /// <returns></returns>
        int Update(object instance);
        ///// <summary>
        ///// 对数据库进行删除一条记录的操作。
        ///// </summary>
        ///// <param name="instance">提供实体信息</param>
        ///// <returns></returns>
        //int Delete(object instance);
        [Obsolete("版本已更新,请使用具体实体的Save方法代替此方法!")]
        /// <summary>
        /// 进行插入或者更新数据记录操作。
        /// </summary>
        /// <param name="instance">提供实体信息</param>
        /// <returns></returns>
        int InsertOrUpdate(object instance);
    }

    /// <summary>
    /// 定义表示对从数据库映射关联的一个已知 CLR 特定类型表对象的操作的泛型版本。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IEntityTable<T> : IQueryable<T>, IEntityTable, IUpdatable<T>
    {
        /// <summary>
        /// 通过数据库表名ID获取对应的CLR类型对象 <typeparamref name="T"/>。
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        new T GetById(object id);
        /// <summary>
        /// 向数据库插入一条记录, 该记录信息来自通过映射的实体对象数据。
        /// </summary>
        /// <param name="instance">提供实体信息</param>
        /// <returns></returns>
        int Insert(T instance);
        /// <summary>
        /// 对数据库的一条记录进行更新，更新的数据来自映射的实体。
        /// </summary>
        /// <param name="instance">提供实体信息</param>
        /// <returns></returns>
        int Update(T instance);
        /// <summary>
        /// 对数据库进行删除一条记录的操作。
        /// </summary>
        /// <param name="instance">提供实体信息</param>
        /// <returns></returns>
        int Delete(T instance); 
        /// <summary>
        /// 进行插入或者更新数据记录操作。
        /// </summary>
        /// <param name="instance">提供实体信息</param>
        /// <returns></returns>
        int InsertOrUpdate(T instance);
    }
}