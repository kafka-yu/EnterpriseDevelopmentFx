// Copyright (c) Microsoft Corporation.  All rights reserved.
// This source code is made available under the terms of the Microsoft Public License (MS-PL)

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace NkjSoft.ORM.Core
{
    /// <summary>
    /// A basic abstract LINQ query provider
    /// </summary>
    public abstract class QueryProvider : IQueryProvider, IQueryText//, IQueryLogable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QueryProvider"/> class.
        /// </summary>
        protected QueryProvider()
        {
        }

        /// <summary>
        /// Creates the query.
        /// </summary>
        /// <typeparam name="S"></typeparam>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        IQueryable<S> IQueryProvider.CreateQuery<S>(Expression expression)
        {
            return new Query<S>(this, expression);
        }

        IQueryable IQueryProvider.CreateQuery(Expression expression)
        {
            Type elementType = TypeHelper.GetElementType(expression.Type);
            try
            {
                return (IQueryable)Activator.CreateInstance(typeof(Query<>).MakeGenericType(elementType), new object[] { this, expression });
            }
            catch (TargetInvocationException tie)
            {
                throw tie.InnerException;
            }
        }

        S IQueryProvider.Execute<S>(Expression expression)
        {
            return (S)this.Execute(expression);
        }

        object IQueryProvider.Execute(Expression expression)
        {
            return this.Execute(expression);
        }

        /// <summary>
        /// Gets the query text.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        public abstract string GetQueryText(Expression expression);
        /// <summary>
        /// 执行指定表达式目录树所表示的查询。
        /// </summary>
        /// <param name="expression">表示 LINQ 查询的表达式目录树。</param>
        /// <returns>执行指定查询所生成的值。</returns>
        public abstract object Execute(Expression expression);

        ///// <summary>
        ///// 在查询发生之后发生。
        ///// </summary>
        //public abstract event EventHandler QueryLog;
    }
}
