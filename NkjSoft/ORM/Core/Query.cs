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
    /// Optional interface for IQueryProvider to implement Query&lt;T&gt;'s QueryText property.
    /// </summary>
    public interface IQueryText
    {
        /// <summary>
        /// Gets the query text.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        string GetQueryText(Expression expression);
    }

    /// <summary>
    /// A default implementation of IQueryable for use with QueryProvider
    /// </summary>
    public class Query<T> : IQueryable<T>, IQueryable, IEnumerable<T>, IEnumerable, IOrderedQueryable<T>, IOrderedQueryable
    {
        IQueryProvider provider;
        Expression expression;

        /// <summary>
        /// Initializes a new instance of the <see cref="Query&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="provider">The provider.</param>
        public Query(IQueryProvider provider)
            : this(provider, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Query&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <param name="staticType">Type of the static.</param>
        public Query(IQueryProvider provider, Type staticType)
        {
            if (provider == null)
            {
                throw new ArgumentNullException("Provider");
            }
            this.provider = provider;
            this.expression = staticType != null ? Expression.Constant(this, staticType) : Expression.Constant(this);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Query&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <param name="expression">The expression.</param>
        public Query(QueryProvider provider, Expression expression)
        {
            if (provider == null)
            {
                throw new ArgumentNullException("Provider");
            }
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }
            if (!typeof(IQueryable<T>).IsAssignableFrom(expression.Type))
            {
                throw new ArgumentOutOfRangeException("expression");
            }
            this.provider = provider;
            this.expression = expression;
        }

        /// <summary>
        /// 获取与 <see cref="T:System.Linq.IQueryable"/> 的实例关联的表达式目录树。
        /// </summary>
        /// <value></value>
        /// <returns>与 <see cref="T:System.Linq.IQueryable"/> 的此实例关联的 <see cref="T:System.Linq.Expressions.Expression"/>。</returns>
        public Expression Expression
        {
            get { return this.expression; }
        }

        /// <summary>
        /// 获取在执行与 <see cref="T:System.Linq.IQueryable"/> 的此实例关联的表达式目录树时返回的元素的类型。
        /// </summary>
        /// <value></value>
        /// <returns>一个 <see cref="T:System.Type"/>，表示在执行与之关联的表达式目录树时返回的元素的类型。</returns>
        public Type ElementType
        {
            get { return typeof(T); }
        }

        /// <summary>
        /// 获取与此数据源关联的查询提供程序。
        /// </summary>
        /// <value></value>
        /// <returns>与此数据源关联的 <see cref="T:System.Linq.IQueryProvider"/>。</returns>
        public IQueryProvider Provider
        {
            get { return this.provider; }
        }

        /// <summary>
        /// Gets the enumerator.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<T> GetEnumerator()
        {
            return ((IEnumerable<T>)this.provider.Execute(this.expression)).GetEnumerator();
        }

        /// <summary>
        /// 返回一个循环访问集合的枚举数。
        /// </summary>
        /// <returns>
        /// 可用于循环访问集合的 <see cref="T:System.Collections.IEnumerator"/> 对象。
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)this.provider.Execute(this.expression)).GetEnumerator();
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            //if (this.expression.NodeType == ExpressionType.Constant &&
            //    ((ConstantExpression)this.expression).Value == this)
            //{
            //    return "Query(" + typeof(T) + ")";
            //}
            //else
            //{
            //    return this.expression.ToString();
            //}
            return this.QueryText;
        }
        private string queryText = string.Empty;
        /// <summary>
        /// Gets the query text.
        /// </summary>
        /// <value>The query text.</value>
        public string QueryText
        {
            get
            {
                if (string.IsNullOrEmpty(queryText))
                {
                    IQueryText iqt = this.provider as IQueryText;
                    if (iqt != null)
                    {
                        queryText = iqt.GetQueryText(this.expression);
                    }
                }

                return queryText;
            }
        }
    }
}
