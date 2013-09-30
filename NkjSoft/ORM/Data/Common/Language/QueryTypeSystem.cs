// Copyright (c) Microsoft Corporation.  All rights reserved.
// This source code is made available under the terms of the Microsoft Public License (MS-PL)

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Data;

namespace NkjSoft.ORM.Data.Common
{
    public abstract class QueryType
    {
        public abstract bool NotNull { get; }
        public abstract int Length { get; }
        public abstract short Precision { get; }
        public abstract short Scale { get; }

        //public abstract DbType DbType { get; } 
        public abstract string DataType { get; }
    }

    /// <summary>
    /// 提供各个数据库类型直接数据类型的转换操作。
    /// </summary>
    public abstract class QueryTypeSystem
    {
        /// <summary>
        /// 将指定的数据类型转换成需要的数据类型.
        /// </summary>
        /// <param name="typeDeclaration">The type declaration.</param>
        /// <returns></returns>
        public abstract QueryType Parse(string typeDeclaration);
        /// <summary>
        /// 获取字段的数据类型.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public abstract QueryType GetColumnType(Type type);
        /// <summary>
        /// 获取变量定义的数据类型。
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="suppressSize">if set to <c>true</c> [suppress size].</param>
        /// <returns></returns>
        public abstract string GetVariableDeclaration(QueryType type, bool suppressSize);
    }
}