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
using NkjSoft.ORM.Core;

namespace NkjSoft.ORM.Data.Common
{
    using NkjSoft.Extensions;
    /// <summary> 
    /// 定义提供针对不同数据库提供程序建立统一TSQL的查询语言的能力。
    /// </summary>
    public abstract class QueryLanguage
    {
        /// <summary>
        /// 获取类型系统。
        /// </summary>
        /// <value>The type system.</value>
        public abstract QueryTypeSystem TypeSystem { get; }
        /// <summary>
        /// 获取生成的表达式。
        /// </summary>
        /// <param name="member">The member.</param>
        /// <returns></returns>
        public abstract Expression GetGeneratedIdExpression(MemberInfo member);

        /// <summary>
        /// 指定特定的一个名字。
        /// </summary>
        /// <param name="name">特定的一个名字可以是表名、字段名。</param>
        /// <returns></returns>
        public virtual string Quote(string name)
        {
            return name;
        }

        /// <summary> 
        /// 获取一个值，表示是否运行进行批量语句操作。
        /// </summary>
        /// <value>
        /// 	<c>true</c>那么，运行进行批量语句操作，否则 <c>false</c>.
        /// </value>
        public virtual bool AllowsMultipleCommands
        {
            get { return false; }
        }

        /// <summary> 
        /// 获取一个值，表示是否进行无需 From 子句的TSQL子查询。
        /// </summary>
        /// <value>
        /// 	<c>true</c>允许进行无需 From 子句的TSQL子查询；否则 <c>false</c>.
        /// </value>
        public virtual bool AllowSubqueryInSelectWithoutFrom
        {
            get { return false; }
        }

        /// <summary> 
        /// 获取一个值，表示当前数据查询提供程序是否运行进行 DISTINCT 查询命令。
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [allow distinct in aggregates]; otherwise, <c>false</c>.
        /// </value>
        public virtual bool AllowDistinctInAggregates
        {
            get { return false; }
        }

        /// <summary> 
        /// 获取 TSQL 结果查询命令影响的行数。
        /// </summary>
        /// <param name="command"> TSQL 结果查询命令语句表达式。</param>
        /// <returns></returns>
        public virtual Expression GetRowsAffectedExpression(Expression command)
        {
            return new FunctionExpression(typeof(int), "@@ROWCOUNT", null);
        }

        /// <summary>
        /// Determines whether [is rows affected expressions] [the specified expression].
        /// <para>判断一个表达式是否是执行影响行的查询。</para> 
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns>
        /// 	<c>true</c> if [is rows affected expressions] [the specified expression]; otherwise, <c>false</c>.
        /// </returns>
        public virtual bool IsRowsAffectedExpressions(Expression expression)
        {
            FunctionExpression fex = expression as FunctionExpression;
            return fex != null && fex.Name == "@@ROWCOUNT";
        }

        /// <summary> 
        /// 获 SELECT 语句部分中取外联结表达式文本。
        /// </summary>
        /// <param name="select"> SELECT 语句 .</param>
        /// <returns></returns>
        public virtual Expression GetOuterJoinTest(SelectExpression select)
        {
            // if the column is used in the join condition (equality test)
            // if it is null in the database then the join test won't match (null != null) so the row won't appear
            // we can safely use this existing column as our test to determine if the outer join produced a row

            // find a column that is used in equality test
            var aliases = DeclaredAliasGatherer.Gather(select.From);
            var joinColumns = JoinColumnGatherer.Gather(aliases, select).ToList();
            if (joinColumns.Count > 0)
            {
                // prefer one that is already in the projection list.
                foreach (var jc in joinColumns)
                {
                    foreach (var col in select.Columns)
                    {
                        if (jc.Equals(col.Expression))
                        {
                            return jc;
                        }
                    }
                }
                return joinColumns[0];
            }

            // fall back to introducing a constant
            return Expression.Constant(1, typeof(int?));
        }

        /// <summary>
        /// 将一个外连接查询表达式添加到当前对象。
        /// </summary>
        /// <param name="proj">The proj.</param>
        /// <returns></returns>
        public virtual ProjectionExpression AddOuterJoinTest(ProjectionExpression proj)
        {
            var test = this.GetOuterJoinTest(proj.Select);
            var select = proj.Select;
            ColumnExpression testCol = null;
            // look to see if test expression exists in columns already
            foreach (var col in select.Columns)
            {
                if (test.Equals(col.Expression))
                {
                    var colType = this.TypeSystem.GetColumnType(test.Type);
                    testCol = new ColumnExpression(test.Type, colType, select.Alias, col.Name);
                    break;
                }
            }
            //if (testCol == null)
            //{
            //    //TODO:Test?
            //    // add expression to projection
            //    testCol = test as ColumnExpression;
            //    string colName = (testCol != null) ? testCol.Name : "Test";
            //    colName = proj.Select.Columns.GetAvailableColumnName(colName);
            //    var colType = this.TypeSystem.GetColumnType(test.Type);
            //    select = select.AddColumn(new ColumnDeclaration(colName, test, colType));
            //    testCol = new ColumnExpression(test.Type, colType, select.Alias, colName);
            //}
            var newProjector = new OuterJoinedExpression(testCol, proj.Projector);
            return new ProjectionExpression(select, newProjector, proj.Aggregator);
        }

        /// <summary>
        /// 关联查询列收集器。
        /// </summary>
        class JoinColumnGatherer
        {
            HashSet<TableAlias> aliases;
            HashSet<ColumnExpression> columns = new HashSet<ColumnExpression>();

            private JoinColumnGatherer(HashSet<TableAlias> aliases)
            {
                this.aliases = aliases;
            }

            /// <summary>
            /// 从一个 SELECT 表达式中收集指定的别名集合。
            /// </summary>
            /// <param name="aliases">The aliases.</param>
            /// <param name="select">The select.</param>
            /// <returns></returns>
            public static HashSet<ColumnExpression> Gather(HashSet<TableAlias> aliases, SelectExpression select)
            {
                var gatherer = new JoinColumnGatherer(aliases);
                gatherer.Gather(select.Where);
                return gatherer.columns;
            }

            private void Gather(Expression expression)
            {
                BinaryExpression b = expression as BinaryExpression;
                if (b != null)
                {
                    switch (b.NodeType)
                    {
                        case ExpressionType.Equal:
                        case ExpressionType.NotEqual:
                            if (IsExternalColumn(b.Left) && GetColumn(b.Right) != null)
                            {
                                this.columns.Add(GetColumn(b.Right));
                            }
                            else if (IsExternalColumn(b.Right) && GetColumn(b.Left) != null)
                            {
                                this.columns.Add(GetColumn(b.Left));
                            }
                            break;
                        case ExpressionType.And:
                        case ExpressionType.AndAlso:
                            if (b.Type == typeof(bool) || b.Type == typeof(bool?))
                            {
                                this.Gather(b.Left);
                                this.Gather(b.Right);
                            }
                            break;
                    }
                }
            }

            private ColumnExpression GetColumn(Expression exp)
            {
                while (exp.NodeType == ExpressionType.Convert)
                    exp = ((UnaryExpression)exp).Operand;
                return exp as ColumnExpression;
            }

            private bool IsExternalColumn(Expression exp)
            {
                var col = GetColumn(exp);
                if (col != null && !this.aliases.Contains(col.Alias))
                    return true;
                return false;
            }
        }

        /// <summary> 
        /// 判断一个 CLR 类型是否和当前数据提供程序查询语言匹配，匹配则返回true。
        /// </summary>
        /// <param name="type">待判断的 CLR 类型</param>
        /// <returns></returns>
        public virtual bool IsScalar(Type type)
        {
            type = TypeHelper.GetNonNullableType(type);
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Empty:
                case TypeCode.DBNull:
                    return false;
                case TypeCode.Object:
                    return
                        type == typeof(DateTimeOffset) ||
                        type == typeof(TimeSpan) ||
                        type == typeof(Guid) ||
                        type == typeof(byte[]);
                default:
                    return true;
            }
        }

        /// <summary> 
        /// 判断一个 <see cref="System.Reflection.MemberInfo"/> 对象是否需要进行统计操作的成员。
        /// </summary>
        /// <param name="member">The member.</param>
        /// <returns>
        /// 	<c>true</c> if the specified member is aggregate; otherwise, <c>false</c>.
        /// </returns>
        public virtual bool IsAggregate(MemberInfo member)
        {
            var method = member as MethodInfo;
            if (method != null)
            {
                if (method.DeclaringType == typeof(Queryable)
                    || method.DeclaringType == typeof(Enumerable))
                {
                    switch (method.Name)
                    {
                        case "Count":
                        case "LongCount":
                        case "Sum":
                        case "Min":
                        case "Max":
                        case "Average":
                            return true;
                    }
                }
            }
            var property = member as PropertyInfo;
            if (property != null
                && property.Name == "Count"
                && typeof(IEnumerable).IsAssignableFrom(property.DeclaringType))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Aggregates the argument is predicate.
        /// <para> 判断一个操作参数是否是用于统计的。</para> 
        /// </summary>
        /// <param name="aggregateName">Name of the aggregate.</param>
        /// <returns></returns>
        public virtual bool AggregateArgumentIsPredicate(string aggregateName)
        {
            return aggregateName == "Count" || aggregateName == "LongCount";
        }

        /// <summary> 
        /// 判断给定的表达式是否能成为 SELECT 表达式中的一列。
        /// </summary>
        /// <param name="expression">给定的表达式</param>
        /// <returns></returns>
        public virtual bool CanBeColumn(Expression expression)
        {
            // by default, push all work in projection to client
            return this.MustBeColumn(expression);
        }

        /// <summary> 
        /// 返回一个值，该值表示给定的表达式是否是必须的列。
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        public virtual bool MustBeColumn(Expression expression)
        {
            switch (expression.NodeType)
            {
                case (ExpressionType)DbExpressionType.Column:
                case (ExpressionType)DbExpressionType.Scalar:
                case (ExpressionType)DbExpressionType.Exists:
                case (ExpressionType)DbExpressionType.AggregateSubquery:
                case (ExpressionType)DbExpressionType.Aggregate:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary> 
        /// 通过指定的语言翻译器创建查询语言。
        /// </summary>
        /// <param name="translator">The translator.</param>
        /// <returns></returns>
        public virtual QueryLinguist CreateLinguist(QueryTranslator translator)
        {
            return new QueryLinguist(this, translator);
        }


        /// <summary>
        /// 表示自动增长列的表示。
        /// </summary>
        protected string auto_increment_info = "IDENTITY(1,1)";
        /// <summary>
        /// 表示字段默认值的格式化字符串。
        /// </summary>
        protected string default_value_formatter = "DEFAULT({0})";
        protected string integer_Name = "int";
        /// <summary>
        /// 在创建表的时候,创建列信息。
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="ca">The ca.</param>
        /// <returns></returns>
        public virtual string BuildeColumnInfo(PropertyInfo item, Mapping.ColumnAttribute ca)
        {
            ca = ca == null ? new Data.Mapping.ColumnAttribute() : ca;
            string colName = string.Empty;
            string dbType = string.Empty;
            string length = string.Empty;
            string primaryKey = string.Empty;
            string nullable = string.Empty;
            string autoInCrement = string.Empty;
            string defaultValue = string.Empty;
            colName = ca.Alias.IfEmptyReplace(item.Name);
            dbType = ca.DbType;
            length = ca.Length.ToString();
            primaryKey = ca.IsPrimaryKey ? "PRIMARY KEY" : string.Empty;
            nullable = ca.IsNullable && !ca.IsPrimaryKey ? "NULL" : "NOT NULL";
            autoInCrement = ca.IsIdentity ? auto_increment_info : string.Empty;
            defaultValue = ca.DefaultValue.IsNullOrEmpty() ? string.Empty : default_value_formatter.FormatWith(ca.DefaultValue);

            return string.Format("\r\n{0} {1} {2} {3} {4} {5}  ,",
                colName, autoInCrement == "AUTOINCREMENT" ? string.Empty : dbType,  autoInCrement, nullable, primaryKey, defaultValue
                );
        }

        private string getGenericDbType(string genericTypeFullName)
        {

            if (genericTypeFullName.Contains("Int32"))
                genericTypeFullName = integer_Name;
            else if (genericTypeFullName.Contains("DateTime"))
                genericTypeFullName = "datetime";
            else if (genericTypeFullName.Contains("String"))
                genericTypeFullName = "varchar";
            else if (genericTypeFullName.Contains("Single"))
                genericTypeFullName = "float";
            else if (genericTypeFullName.Contains("Decimal"))
                genericTypeFullName = "decimal";

            return genericTypeFullName;
        }

        /// <summary>
        /// Exchanges the type of the dot net.
        /// </summary>
        /// <param name="sqlDbType">Type of the SQL db.</param>
        /// <returns></returns>
        public static string ExchangeDotNetType(string sqlDbType)
        {
            return sqlDbType;
        }
    }

    /// <summary>
    /// 表示一个进行语言查询的查询语言。
    /// </summary>
    public class QueryLinguist
    {
        QueryLanguage language;
        QueryTranslator translator;

        /// <summary>
        /// 实例化新的一个 <see cref="QueryLinguist"/> 类对象。
        /// </summary>
        /// <param name="language">The language.</param>
        /// <param name="translator">The translator.</param>
        public QueryLinguist(QueryLanguage language, QueryTranslator translator)
        {
            this.language = language;
            this.translator = translator;
        }

        /// <summary>
        /// Gets the language.
        /// </summary>
        /// <value>The language.</value>
        public QueryLanguage Language
        {
            get { return this.language; }
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
        /// 提供一种特殊的查询程序语言.使用这种语言可以将 Lambda 查询表达式重写成T-SQL语句表示。 
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public virtual Expression Translate(Expression expression)
        {
            // remove redundant layers again before cross apply rewrite
            expression = UnusedColumnRemover.Remove(expression);
            expression = RedundantColumnRemover.Remove(expression);
            expression = RedundantSubqueryRemover.Remove(expression);

            // convert cross-apply and outer-apply joins into inner & left-outer-joins if possible
            var rewritten = CrossApplyRewriter.Rewrite(this.language, expression);

            // convert cross joins into inner joins
            rewritten = CrossJoinRewriter.Rewrite(rewritten);

            if (rewritten != expression)
            {
                expression = rewritten;
                // do final reduction
                expression = UnusedColumnRemover.Remove(expression);
                expression = RedundantSubqueryRemover.Remove(expression);
                expression = RedundantJoinRemover.Remove(expression);
                expression = RedundantColumnRemover.Remove(expression);
            }

            return expression;
        }

        /// <summary>
        /// 将 Lambda 查询表达式转换成 T-SQL 表达式。
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public virtual string Format(Expression expression)
        {
            // use common SQL formatter by default
            return SqlFormatter.Format(expression);
        }

        /// <summary>
        ///判断指定的 Lambda 查询表达式(转换成T-SQL语句之后)是否需要对语句进行参数化。
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public virtual Expression Parameterize(Expression expression)
        {
            return Parameterizer.Parameterize(this.language, expression);
        }
    }
}