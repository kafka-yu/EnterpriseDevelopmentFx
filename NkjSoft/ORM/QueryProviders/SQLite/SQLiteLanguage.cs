using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Reflection;

namespace NkjSoft.ORM.Data.SQLite
{
    using NkjSoft.ORM.Data.Common;
    using NkjSoft.ORM.Core;

    /// <summary>
    /// 表示针对 SQLite 数据库的查询语言类。无法继承此类。
    /// </summary>
    public sealed class SQLiteLanguage : QueryLanguage
    {

        public SQLiteLanguage()
        {
            base.auto_increment_info = "AUTOINCREMENT ";// string.Empty;
            base.integer_Name = "INTEGER";
        }

        private SQLiteTypeSystem _typeSystem = new SQLiteTypeSystem();

        /// <summary>
        /// Gets the type system.
        /// </summary>
        /// <value>The type system.</value>
        public override QueryTypeSystem TypeSystem
        {
            get { return _typeSystem; }
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
        /// Gets the generated id expression.
        /// </summary>
        /// <param name="member">The member.</param>
        /// <returns></returns>
        public override Expression GetGeneratedIdExpression(MemberInfo member)
        {
            return new FunctionExpression(TypeHelper.GetMemberType(member), "last_insert_rowid()", null);
        }

        /// <summary>
        /// 获取 TSQL 结果查询命令影响的行数。
        /// </summary>
        /// <param name="command">TSQL 结果查询命令语句表达式。</param>
        /// <returns></returns>
        public override Expression GetRowsAffectedExpression(Expression command)
        {
            return new FunctionExpression(typeof(int), "changes()", null);
        }

        /// <summary>
        /// Determines whether [is rows affected expressions] [the specified expression].
        /// <para>判断一个表达式是否是执行影响行的查询。</para>
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns>
        /// 	<c>true</c> if [is rows affected expressions] [the specified expression]; otherwise, <c>false</c>.
        /// </returns>
        public override bool IsRowsAffectedExpressions(Expression expression)
        {
            FunctionExpression fex = expression as FunctionExpression;
            return fex != null && fex.Name == "changes()";
        }

        /// <summary>
        /// 通过指定的语言翻译器创建查询语言。
        /// </summary>
        /// <param name="translator">The translator.</param>
        /// <returns></returns>
        public override QueryLinguist CreateLinguist(QueryTranslator translator)
        {
            return new SQLiteLinguist(this, translator);
        }

        /// <summary>
        /// 
        /// </summary>
        class SQLiteLinguist : QueryLinguist
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="SQLiteLinguist"/> class.
            /// </summary>
            /// <param name="language">The language.</param>
            /// <param name="translator">The translator.</param>
            public SQLiteLinguist(SQLiteLanguage language, QueryTranslator translator)
                : base(language, translator)
            {
            }

            /// <summary>
            /// Provides language specific query translation.  Use this to apply language specific rewrites or
            /// to make assertions/validations about the query.
            /// </summary>
            /// <param name="expression"></param>
            /// <returns></returns>
            public override Expression Translate(Expression expression)
            {
                // fix up any order-by's
                expression = OrderByRewriter.Rewrite(this.Language, expression);

                expression = base.Translate(expression);

                //expression = SkipToNestedOrderByRewriter.Rewrite(expression);
                expression = UnusedColumnRemover.Remove(expression);

                return expression;
            }

            /// <summary>
            /// Converts the query expression into text of this query language
            /// </summary>
            /// <param name="expression"></param>
            /// <returns></returns>
            public override string Format(Expression expression)
            {
                return SQLiteFormatter.Format(expression);
            }
        }

        /// <summary>
        /// 表示默认的查询语言对象。字段是只读的。
        /// </summary>
        public static readonly QueryLanguage Default = new SQLiteLanguage();

    }
}
