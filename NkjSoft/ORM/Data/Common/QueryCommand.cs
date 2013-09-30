// Copyright (c) Microsoft Corporation.  All rights reserved.
// This source code is made available under the terms of the Microsoft Public License (MS-PL)

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace NkjSoft.ORM.Data.Common
{
    using NkjSoft.ORM.Core;
    /// <summary>
    /// 描述一个查询命令的完整信息。
    /// </summary>
    public class QueryCommand
    {
        string commandText;
        /// <summary>
        /// 
        /// </summary>
        ReadOnlyCollection<QueryParameter> parameters;

        /// <summary>
        /// 通过 commandText 、parameters 实例化一个新 <see cref="QueryCommand"/> 实例。
        /// </summary>
        /// <param name="commandText">命令文本</param>
        /// <param name="parameters">命令参数序列.</param>
        public QueryCommand(string commandText, IEnumerable<QueryParameter> parameters)
        {
            this.commandText = commandText;
            this.parameters = parameters.ToReadOnly();
        }

        /// <summary>
        /// 获取命令的一份 <see cref="System.String"/> 表示。
        /// </summary>
        /// <value>一份 <see cref="System.String"/> 表示</value>
        public string CommandText
        {
            get { return this.commandText; }
        }

        /// <summary>
        /// Gets the parameters.
        /// </summary>
        /// <value>The parameters.</value>
        public ReadOnlyCollection<QueryParameter> Parameters
        {
            get { return this.parameters; }
        }
    }

    /// <summary>
    /// 描述查询命令的参数。
    /// </summary>
    public class QueryParameter
    {
        string name;
        Type type;
        QueryType queryType;

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryParameter"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="type">The type.</param>
        /// <param name="queryType">Type of the query.</param>
        public QueryParameter(string name, Type type, QueryType queryType)
        {
            this.name = name;
            this.type = type;
            this.queryType = queryType;
        }

        public string Name
        {
            get { return this.name; }
        }

        public Type Type
        {
            get { return this.type; }
        }

        public QueryType QueryType
        {
            get { return this.queryType; }
        }
    }
}
