using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace NkjSoft.ORM.Data.MySqlClient
{
    using NkjSoft.ORM.Data.Common;
    using NkjSoft.ORM.Core;
    using NkjSoft.ORM.Data.MySqlClient;

    /// <summary>
    /// 
    /// </summary>
    public class MySqlLanguage : QueryLanguage
    {
        DbTypeSystem typeSystem = new DbTypeSystem();

        /// <summary>
        /// Initializes a new instance of the <see cref="MySqlLanguage"/> class.
        /// </summary>
        public MySqlLanguage()
        {
            base.auto_increment_info = "AUTO_INCREMENT";
            base.default_value_formatter = "";
        }

        /// <summary>
        /// Gets the type system.
        /// </summary>
        /// <value>The type system.</value>
        public override QueryTypeSystem TypeSystem
        {
            get { return this.typeSystem; }
        }

        /// <summary>
        /// Gets a value indicating whether [allows multiple commands].
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [allows multiple commands]; otherwise, <c>false</c>.
        /// </value>
        public override bool AllowsMultipleCommands
        {
            get { return false; }
        }

        /// <summary>
        /// Gets a value indicating whether [allow distinct in aggregates].
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [allow distinct in aggregates]; otherwise, <c>false</c>.
        /// </value>
        public override bool AllowDistinctInAggregates
        {
            get { return true; }
        }

        /// <summary>
        /// Quotes the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public override string Quote(string name)
        {
            return name;
        }

        /// <summary>
        /// 
        /// </summary>
        private static readonly char[] splitChars = new char[] { '.' };

        /// <summary>
        /// Gets the generated id expression.
        /// </summary>
        /// <param name="member">The member.</param>
        /// <returns></returns>
        public override Expression GetGeneratedIdExpression(MemberInfo member)
        {
            return new FunctionExpression(TypeHelper.GetMemberType(member), "LAST_INSERT_ID()", null);
        }

        /// <summary>
        /// Gets the rows affected expression.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns></returns>
        public override Expression GetRowsAffectedExpression(Expression command)
        {
            return new FunctionExpression(typeof(int), "ROW_COUNT()", null);
        }

        /// <summary>
        /// Determines whether [is rows affected expressions] [the specified expression].
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns>
        /// 	<c>true</c> if [is rows affected expressions] [the specified expression]; otherwise, <c>false</c>.
        /// </returns>
        public override bool IsRowsAffectedExpressions(Expression expression)
        {
            FunctionExpression fex = expression as FunctionExpression;
            return fex != null && fex.Name == "ROW_COUNT()";
        }

        /// <summary>
        /// Creates the linguist.
        /// </summary>
        /// <param name="translator">The translator.</param>
        /// <returns></returns>
        public override QueryLinguist CreateLinguist(QueryTranslator translator)
        {
            return new MySqlLinguist(this, translator);
        }

        /// <summary>
        /// 
        /// </summary>
        class MySqlLinguist : QueryLinguist
        {
            public MySqlLinguist(MySqlLanguage language, QueryTranslator translator)
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

                expression = UnusedColumnRemover.Remove(expression);

                //expression = DistinctOrderByRewriter.Rewrite(expression);

                return expression;
            }

            /// <summary>
            /// Converts the query expression into text of this query language
            /// </summary>
            /// <param name="expression"></param>
            /// <returns></returns>
            public override string Format(Expression expression)
            {
                return MySqlClient.MySqlFormatter.Format(expression, this.Language);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static readonly QueryLanguage Default = new MySqlLanguage();
    }
}