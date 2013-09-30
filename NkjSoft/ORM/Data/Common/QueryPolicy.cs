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

namespace NkjSoft.ORM.Data.Common
{
    /// <summary>
    /// 表示一个查询政策，查询表达式将根据这个政策进行数据库访问。
    /// </summary>
    public class QueryPolicy
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QueryPolicy"/> class.
        /// </summary>
        public QueryPolicy()
        {
        }

        /// <summary>
        /// Determines if a relationship property is to be included in the results of the query
        /// 判断一个对象成员是否包含在查询结果中。该方式会进行延迟加载。
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        public virtual bool IsIncluded(MemberInfo member)
        {
            return false;
        }

        /// <summary> 
        /// 判断一个关系属性是否包含在查询中。查询对对象的加载是延迟方式的，即在对象被第一次访问的时候才会从数据库中加载到内存。
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        public virtual bool IsDeferLoaded(MemberInfo member)
        {
            return false;
        }

        /// <summary>
        /// 通过查询翻译器创建一个查询Police。
        /// </summary>
        /// <param name="translator">The translator.</param>
        /// <returns></returns>
        public virtual QueryPolice CreatePolice(QueryTranslator translator)
        {
            return new QueryPolice(this, translator);
        }

        /// <summary>
        /// 返回一个默认查询政策。该字段是只读的。
        /// </summary>
        public static readonly QueryPolicy Default = new QueryPolicy();
    }

    /// <summary>
    /// 
    /// </summary>
    public class QueryPolice
    {
        QueryPolicy policy;
        QueryTranslator translator;

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryPolice"/> class.
        /// </summary>
        /// <param name="policy">The policy.</param>
        /// <param name="translator">The translator.</param>
        public QueryPolice(QueryPolicy policy, QueryTranslator translator)
        {
            this.policy = policy;
            this.translator = translator;
        }

        public QueryPolicy Policy
        {
            get { return this.policy; }
        }

        /// <summary>
        /// Gets the translator.
        /// </summary>
        /// <value>The translator.</value>
        public QueryTranslator Translator
        {
            get { return this.translator; }
        }

        /// <summary>
        /// 应用查询策略。
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <param name="member">The member.</param>
        /// <returns></returns>
        public virtual Expression ApplyPolicy(Expression expression, MemberInfo member)
        {
            return expression;
        }

        /// <summary>
        /// Provides policy specific query translations.  This is where choices about inclusion of related objects and how
        /// heirarchies are materialized affect the definition of the queries. 
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public virtual Expression Translate(Expression expression)
        {
            // add included relationships to client projection
            var rewritten = RelationshipIncluder.Include(this.translator.Mapper, expression);
            if (rewritten != expression)
            {
                expression = rewritten;
                expression = UnusedColumnRemover.Remove(expression);
                expression = RedundantColumnRemover.Remove(expression);
                expression = RedundantSubqueryRemover.Remove(expression);
                expression = RedundantJoinRemover.Remove(expression);
            }

            // convert any singleton (1:1 or n:1) projections into server-side joins (cardinality is preserved)
            rewritten = SingletonProjectionRewriter.Rewrite(this.translator.Linguist.Language, expression);
            if (rewritten != expression)
            {
                expression = rewritten;
                expression = UnusedColumnRemover.Remove(expression);
                expression = RedundantColumnRemover.Remove(expression);
                expression = RedundantSubqueryRemover.Remove(expression);
                expression = RedundantJoinRemover.Remove(expression);
            }

            // convert projections into client-side joins
            rewritten = ClientJoinedProjectionRewriter.Rewrite(this.policy, this.translator.Linguist.Language, expression);
            if (rewritten != expression)
            {
                expression = rewritten;
                expression = UnusedColumnRemover.Remove(expression);
                expression = RedundantColumnRemover.Remove(expression);
                expression = RedundantSubqueryRemover.Remove(expression);
                expression = RedundantJoinRemover.Remove(expression);
            }

            return expression;
        }

        /// <summary> 
        /// 将一个 Lambda 查询表达式转换成一个执行计划，该计划是一个执行查询并创建查询结果对象的方法。
        /// </summary>
        /// <param name="query"> Lambda 表达式查询</param>
        /// <param name="provider">查询提供程序</param>
        /// <returns></returns>
        public virtual Expression BuildExecutionPlan(Expression query, Expression provider)
        {
            return ExecutionBuilder.Build(this.translator.Linguist, this.policy, query, provider);
        }
    }
}