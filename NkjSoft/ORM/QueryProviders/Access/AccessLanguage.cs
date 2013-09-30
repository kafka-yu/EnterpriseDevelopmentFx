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

namespace NkjSoft.ORM.Data.Access
{
    using NkjSoft.ORM.Data.Common;
    using NkjSoft.ORM.Core;

    using NkjSoft.Extensions;
    /// <summary>
    /// TSQL specific QueryLanguage
    /// </summary>
    public class AccessLanguage : QueryLanguage
    {
        AccessTypeSystem typeSystem = new AccessTypeSystem();
        public AccessLanguage()
        {
            base.auto_increment_info = "AUTOINCREMENT";// string.Empty;
            base.default_value_formatter = "DEFAULT {0}";
        }
        /// <summary>
        /// 获取类型系统。
        /// </summary>
        /// <value>The type system.</value>
        public override QueryTypeSystem TypeSystem
        {
            get { return this.typeSystem; }
        }

        /// <summary>
        /// 指定特定的一个名字，该名字将被包装成当前数据查询程序合法的查询变量表示.
        /// </summary>
        /// <param name="name">特定的一个名字可以是表名、字段名。</param>
        /// <returns></returns>
        public override string Quote(string name)
        {
            if (name.StartsWith("[") && name.EndsWith("]"))
            {
                return name;
            }
            else
            {
                return "[" + name + "]";
            }
        }

        /// <summary>
        /// 获取生成的表达式。
        /// </summary>
        /// <param name="member">The member.</param>
        /// <returns></returns>
        public override Expression GetGeneratedIdExpression(MemberInfo member)
        {
            return new FunctionExpression(TypeHelper.GetMemberType(member), "@@IDENTITY", null);
        }

        /// <summary>
        /// 通过指定的语言翻译器创建查询语言。
        /// </summary>
        /// <param name="translator">The translator.</param>
        /// <returns></returns>
        public override QueryLinguist CreateLinguist(QueryTranslator translator)
        {
            return new AccessLinguist(this, translator);
        }

        /// <summary>
        /// 查询语言家。
        /// </summary>
        class AccessLinguist : QueryLinguist
        {
            public AccessLinguist(AccessLanguage language, QueryTranslator translator)
                : base(language, translator)
            {
            }

            /// <summary>
            /// 提供一种特殊的查询程序语言.使用这种语言可以将 Lambda 查询表达式重写成T-SQL语句表示。
            /// </summary>
            /// <param name="expression"></param>
            /// <returns></returns>
            public override Expression Translate(Expression expression)
            {
                // fix up any order-by's
                expression = OrderByRewriter.Rewrite(this.Language, expression);

                expression = base.Translate(expression);

                expression = CrossJoinIsolator.Isolate(expression);
                expression = SkipToNestedOrderByRewriter.Rewrite(this.Language, expression);
                expression = OrderByRewriter.Rewrite(this.Language, expression);
                expression = UnusedColumnRemover.Remove(expression);
                expression = RedundantSubqueryRemover.Remove(expression);

                return expression;
            }

            /// <summary>
            /// 将 Lambda 查询表达式转换成 T-SQL 表达式。
            /// </summary>
            /// <param name="expression"></param>
            /// <returns></returns>
            public override string Format(Expression expression)
            {
                return AccessFormatter.Format(expression);
            }
        }

        private static AccessLanguage _default;

        /// <summary>
        /// 获取当前查询提供程序需要的默认查询语言翻译器。
        /// </summary>
        /// <value>The default.</value>
        public static AccessLanguage Default
        {
            get
            {
                if (_default == null)
                {
                    System.Threading.Interlocked.CompareExchange(ref _default, new AccessLanguage(), null);
                }
                return _default;
            }
        }
    }
}