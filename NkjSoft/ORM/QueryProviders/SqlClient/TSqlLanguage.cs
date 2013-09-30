// Copyright (c) Microsoft Corporation.  All rights reserved.
// This source code is made available under the terms of the Microsoft Public License (MS-PL)

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace NkjSoft.ORM.Data.SqlClient
{
    using NkjSoft.ORM.Data.Common;
    using NkjSoft.ORM.Core;

    /// <summary>
    /// TSQL specific QueryLanguage
    /// </summary>
    public class TSqlLanguage : QueryLanguage
    {
        DbTypeSystem typeSystem = new DbTypeSystem();

        /// <summary>
        /// Initializes a new instance of the <see cref="TSqlLanguage"/> class.
        /// </summary>
        public TSqlLanguage()
        { 
        }

        public override QueryTypeSystem TypeSystem
        {
            get { return this.typeSystem; }
        }

        /// <summary>
        /// 指定特定的一个名字。
        /// </summary>
        /// <param name="name">特定的一个名字可以是表名、字段名。</param>
        /// <returns></returns>
        public override string Quote(string name)
        {
            if (name.StartsWith("[") && name.EndsWith("]"))
            {
                return name;
            }
            else if (name.IndexOf('.') > 0)
            {
                return "[" + string.Join("].[", name.Split(splitChars, StringSplitOptions.RemoveEmptyEntries)) + "]";
            }
            else
            {
                return "[" + name + "]";
            }
        }

        private static readonly char[] splitChars = new char[] { '.' };

        /// <summary>
        /// 获取一个值，表示是否运行进行批量语句操作。
        /// </summary>
        /// <value><c>true</c>那么，运行进行批量语句操作，否则 <c>false</c>.</value>
        public override bool AllowsMultipleCommands
        {
            get { return true; }
        }

        public override bool AllowSubqueryInSelectWithoutFrom
        {
            get { return true; }
        }

        /// <summary>
        /// 获取一个值，表示当前数据查询提供程序是否运行进行 DISTINCT 查询命令。
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [allow distinct in aggregates]; otherwise, <c>false</c>.
        /// </value>
        public override bool AllowDistinctInAggregates
        {
            get { return true; }
        }

        public override Expression GetGeneratedIdExpression(MemberInfo member)
        {
            return new FunctionExpression(TypeHelper.GetMemberType(member), "SCOPE_IDENTITY()", null);
        }

        public override QueryLinguist CreateLinguist(QueryTranslator translator)
        {
            return new TSqlLinguist(this, translator);
        }

        class TSqlLinguist : QueryLinguist
        {
            public TSqlLinguist(TSqlLanguage language, QueryTranslator translator)
                : base(language, translator)
            {
            }

            public override Expression Translate(Expression expression)
            {
                // fix up any order-by's
                expression = OrderByRewriter.Rewrite(this.Language, expression);

                expression = base.Translate(expression);

                // convert skip/take info into RowNumber pattern
                expression = SkipToRowNumberRewriter.Rewrite(this.Language, expression);

                // fix up any order-by's we may have changed
                expression = OrderByRewriter.Rewrite(this.Language, expression);

                return expression;
            }

            public override string Format(Expression expression)
            {
                return TSqlFormatter.Format(expression, this.Language);
            }
        }

        private static TSqlLanguage _default;

        /// <summary>
        /// Gets the default.
        /// </summary>
        /// <value>The default.</value>
        public static TSqlLanguage Default
        {
            get
            {
                if (_default == null)
                {
                    System.Threading.Interlocked.CompareExchange(ref _default, new TSqlLanguage(), null);
                }
                return _default;
            }
        } 
    }
}