// Copyright (c) Microsoft Corporation.  All rights reserved.
// This source code is made available under the terms of the Microsoft Public License (MS-PL)

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using NkjSoft.ORM.Core;

namespace NkjSoft.ORM.Data.Common
{
    /// <summary>
    /// 提供将Lambda查询表达式转换成普通T-SQL表达式的能力。
    /// </summary>
    public class SqlFormatter : DbExpressionVisitor
    {
        QueryLanguage language;
        StringBuilder sb;
        int indent = 2;
        int depth;
        Dictionary<TableAlias, string> aliases;
        bool hideColumnAliases;
        bool hideTableAliases;
        bool isNested;
        bool forDebug;

        private SqlFormatter(QueryLanguage language, bool forDebug)
        {
            this.language = language;
            this.sb = new StringBuilder();
            this.aliases = new Dictionary<TableAlias, string>();
            this.forDebug = forDebug;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlFormatter"/> class.
        /// </summary>
        /// <param name="language">The language.</param>
        protected SqlFormatter(QueryLanguage language)
            : this(language, false)
        {
        }

        /// <summary>
        /// 格式化指定的 Lambda查询表达式成T-SQL语句。
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <param name="forDebug">if set to <c>true</c> [for debug].</param>
        /// <returns></returns>
        public static string Format(Expression expression, bool forDebug)
        {
            var formatter = new SqlFormatter(null, forDebug);
            formatter.Visit(expression);
            return formatter.ToString();
        }

        /// <summary>
        ///  格式化指定的 Lambda查询表达式成T-SQL语句。
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        public static string Format(Expression expression)
        {
            var formatter = new SqlFormatter(null, false);
            formatter.Visit(expression);
            return formatter.ToString();
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return this.sb.ToString();
        }

        /// <summary>
        /// 获取当前查询提供程序使用的 Lambda -> T-SQL 语句的翻译器对象。
        /// </summary>
        /// <value>The language.</value>
        protected virtual QueryLanguage Language
        {
            get { return this.language; }
        }

        /// <summary> 
        /// 获取或设置一个值，该值指示是否隐藏列的别名。
        /// </summary>
        /// <value><c>true</c> if [hide column aliases]; otherwise, <c>false</c>.</value>
        protected bool HideColumnAliases
        {
            get { return this.hideColumnAliases; }
            set { this.hideColumnAliases = value; }
        }

        /// <summary>
        /// 获取或设置一个值，该值指示是否隐藏表的别名。 
        /// </summary>
        /// <value><c>true</c> if [hide table aliases]; otherwise, <c>false</c>.</value>
        protected bool HideTableAliases
        {
            get { return this.hideTableAliases; }
            set { this.hideTableAliases = value; }
        }

        /// <summary>
        /// 获取或设置一个值，该值指示当前实例是否是内联的表达式。
        /// </summary>
        /// <value><c>true</c> if this instance is nested; otherwise, <c>false</c>.</value>
        protected bool IsNested
        {
            get { return this.isNested; }
            set { this.isNested = value; }
        }

        /// <summary>
        /// 获取或设置一个值，该值指示是否进行Debug查询。
        /// </summary>
        /// <value><c>true</c> if [for debug]; otherwise, <c>false</c>.</value>
        protected bool ForDebug
        {
            get { return this.forDebug; }
        }

        /// <summary>
        /// 定义一组值，每个值表示关联查询到方式。
        /// </summary>
        protected enum Indentation
        {
            /// <summary>
            /// 相同
            /// </summary>
            Same,
            /// <summary>
            /// 内部
            /// </summary>
            Inner,
            /// <summary>
            /// 外部
            /// </summary>
            Outer
        }

        /// <summary>
        /// 获取或设置契约的宽度。
        /// </summary>
        /// <value>The width of the indentation.</value>
        public int IndentationWidth
        {
            get { return this.indent; }
            set { this.indent = value; }
        }

        /// <summary>
        /// 将指定的值写入到格式化器。
        /// </summary>
        /// <param name="value">The value.</param>
        protected void Write(object value)
        {
            this.sb.Append(value);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="name">The name.</param>
        protected virtual void WriteParameterName(string name)
        {
            this.Write("@" + name);
        }

        /// <summary>
        ///  将变量名字写入到当前翻译实例。
        /// </summary>
        /// <param name="name">The name.</param>
        protected virtual void WriteVariableName(string name)
        {
            this.WriteParameterName(name);
        }

        /// <summary>
        /// 写入对象别名。
        /// </summary>
        /// <param name="aliasName">Name of the alias.</param>
        protected virtual void WriteAsAliasName(string aliasName)
        {
            this.Write("AS ");
            this.WriteAliasName(aliasName);
        }

        /// <summary>
        ///  写入对象别名。
        /// </summary>
        /// <param name="aliasName">Name of the alias.</param>
        protected virtual void WriteAliasName(string aliasName)
        {
            this.Write(aliasName);
        }

        /// <summary>
        /// 写入列别名。
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        protected virtual void WriteAsColumnName(string columnName)
        {
            this.Write("AS ");
            this.WriteColumnName(columnName);
        }

        /// <summary>
        /// 写入列名
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        protected virtual void WriteColumnName(string columnName)
        {
            string name = (this.Language != null) ? this.Language.Quote(columnName) : columnName;
            this.Write(name);
        }

        /// <summary>
        /// 写入表名。
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        protected virtual void WriteTableName(string tableName)
        {
            string name = (this.Language != null) ? this.Language.Quote(tableName) : tableName;
            this.Write(name);
        }

        protected void WriteLine(Indentation style)
        {
            sb.AppendLine();
            this.Indent(style);
            for (int i = 0, n = this.depth * this.indent; i < n; i++)
            {
                this.Write(" ");
            }
        }

        /// <summary>
        /// 根据缩进方式缩进当前表达式
        /// </summary>
        /// <param name="style">The style.</param>
        protected void Indent(Indentation style)
        {
            if (style == Indentation.Inner)
            {
                this.depth++;
            }
            else if (style == Indentation.Outer)
            {
                this.depth--;
                System.Diagnostics.Debug.Assert(this.depth >= 0);
            }
        }

        /// <summary>
        /// 获取表表名表示
        /// </summary>
        /// <param name="alias">The alias.</param>
        /// <returns></returns>
        protected virtual string GetAliasName(TableAlias alias)
        {
            string name;
            if (!this.aliases.TryGetValue(alias, out name))
            {
                name = "A" + alias.GetHashCode() + "?";
                this.aliases.Add(alias, name);
            }
            return name;
        }

        /// <summary>
        /// 添加表别名.
        /// </summary>
        /// <param name="alias">The alias.</param>
        protected void AddAlias(TableAlias alias)
        {
            //TODO : 重要 Tag
            string name;
            if (!this.aliases.TryGetValue(alias, out name))
            {
                name = "t" + this.aliases.Count;
                this.aliases.Add(alias, name);
            }
        }

        /// <summary>
        /// 添加表达式里面的需要使用别名的字段
        /// </summary>
        /// <param name="expr">The expr.</param>
        protected virtual void AddAliases(Expression expr)
        {
            AliasedExpression ax = expr as AliasedExpression;
            if (ax != null)
            {
                this.AddAlias(ax.Alias);
            }
            else
            {
                JoinExpression jx = expr as JoinExpression;
                if (jx != null)
                {
                    this.AddAliases(jx.Left);
                    this.AddAliases(jx.Right);
                }
            }
        }

        /// <summary>
        /// Visits the specified exp.
        /// </summary>
        /// <param name="exp">The exp.</param>
        /// <returns></returns>
        protected override Expression Visit(Expression exp)
        {
            if (exp == null) return null;

            // check for supported node types first 
            // non-supported ones should not be visited (as they would produce bad SQL)
            switch (exp.NodeType)
            {
                case ExpressionType.Negate:
                case ExpressionType.NegateChecked:
                case ExpressionType.Not:
                case ExpressionType.Convert:
                case ExpressionType.ConvertChecked:
                case ExpressionType.UnaryPlus:
                case ExpressionType.Add:
                case ExpressionType.AddChecked:
                case ExpressionType.Subtract:
                case ExpressionType.SubtractChecked:
                case ExpressionType.Multiply:
                case ExpressionType.MultiplyChecked:
                case ExpressionType.Divide:
                case ExpressionType.Modulo:
                case ExpressionType.And:
                case ExpressionType.AndAlso:
                case ExpressionType.Or:
                case ExpressionType.OrElse:
                case ExpressionType.LessThan:
                case ExpressionType.LessThanOrEqual:
                case ExpressionType.GreaterThan:
                case ExpressionType.GreaterThanOrEqual:
                case ExpressionType.Equal:
                case ExpressionType.NotEqual:
                case ExpressionType.Coalesce:
                case ExpressionType.RightShift:
                case ExpressionType.LeftShift:
                case ExpressionType.ExclusiveOr:
                case ExpressionType.Power:
                case ExpressionType.Conditional:
                case ExpressionType.Constant:
                case ExpressionType.MemberAccess:
                case ExpressionType.Call:
                case ExpressionType.New:
                case (ExpressionType)DbExpressionType.Table:
                case (ExpressionType)DbExpressionType.Column:
                case (ExpressionType)DbExpressionType.Select:
                case (ExpressionType)DbExpressionType.Join:
                case (ExpressionType)DbExpressionType.Aggregate:
                case (ExpressionType)DbExpressionType.Scalar:
                case (ExpressionType)DbExpressionType.Exists:
                case (ExpressionType)DbExpressionType.In:
                case (ExpressionType)DbExpressionType.AggregateSubquery:
                case (ExpressionType)DbExpressionType.IsNull:
                case (ExpressionType)DbExpressionType.Between:
                case (ExpressionType)DbExpressionType.RowCount:
                case (ExpressionType)DbExpressionType.Projection:
                case (ExpressionType)DbExpressionType.NamedValue:
                case (ExpressionType)DbExpressionType.Insert:
                case (ExpressionType)DbExpressionType.Update:
                case (ExpressionType)DbExpressionType.Delete:
                case (ExpressionType)DbExpressionType.Block:
                case (ExpressionType)DbExpressionType.If:
                case (ExpressionType)DbExpressionType.Declaration:
                case (ExpressionType)DbExpressionType.Variable:
                case (ExpressionType)DbExpressionType.Function:
                    return base.Visit(exp);

                case ExpressionType.ArrayLength:
                case ExpressionType.Quote:
                case ExpressionType.TypeAs:
                case ExpressionType.ArrayIndex:
                case ExpressionType.TypeIs:
                case ExpressionType.Parameter:
                case ExpressionType.Lambda:
                case ExpressionType.NewArrayInit:
                case ExpressionType.NewArrayBounds:
                case ExpressionType.Invoke:
                case ExpressionType.MemberInit:
                case ExpressionType.ListInit:
                default:
                    if (!forDebug)
                    {
                        //throw new NotSupportedException(string.Format("The LINQ expression node of type {0} is not supported", exp.NodeType));
                        return exp;
                    }
                    else
                    {
                        this.Write(string.Format("?{0}?(", exp.NodeType));
                        base.Visit(exp);
                        this.Write(")");
                        return exp;
                    }
            }
        }

        protected override Expression VisitMemberAccess(MemberExpression m)
        {
            if (this.forDebug)
            {
                this.Visit(m.Expression);
                this.Write(".");
                this.Write(m.Member.Name);
                return m;
            }
            else
            {
                throw new NotSupportedException(string.Format("The member access '{0}' is not supported", m.Member));
            }
        }

        protected override Expression VisitMethodCall(MethodCallExpression m)
        {
            if (m.Method.DeclaringType == typeof(Decimal))
            {
                switch (m.Method.Name)
                {
                    case "Add":
                    case "Subtract":
                    case "Multiply":
                    case "Divide":
                    case "Remainder":
                        this.Write("(");
                        this.VisitValue(m.Arguments[0]);
                        this.Write(" ");
                        this.Write(GetOperator(m.Method.Name));
                        this.Write(" ");
                        this.VisitValue(m.Arguments[1]);
                        this.Write(")");
                        return m;
                    case "Negate":
                        this.Write("-");
                        this.Visit(m.Arguments[0]);
                        this.Write("");
                        return m;
                    case "Compare":
                        this.Visit(Expression.Condition(
                            Expression.Equal(m.Arguments[0], m.Arguments[1]),
                            Expression.Constant(0),
                            Expression.Condition(
                                Expression.LessThan(m.Arguments[0], m.Arguments[1]),
                                Expression.Constant(-1),
                                Expression.Constant(1)
                                )));
                        return m;
                }
            }
            else if (m.Method.Name == "ToString" && m.Object.Type == typeof(string))
            {
                return this.Visit(m.Object);  // no op
            }
            else if (m.Method.Name == "Equals")
            {
                if (m.Method.IsStatic && m.Method.DeclaringType == typeof(object))
                {
                    this.Write("(");
                    this.Visit(m.Arguments[0]);
                    this.Write(" = ");
                    this.Visit(m.Arguments[1]);
                    this.Write(")");
                    return m;
                }
                else if (!m.Method.IsStatic && m.Arguments.Count == 1 && m.Arguments[0].Type == m.Object.Type)
                {
                    this.Write("(");
                    this.Visit(m.Object);
                    this.Write(" = ");
                    this.Visit(m.Arguments[0]);
                    this.Write(")");
                    return m;
                }
            }
            if (this.forDebug)
            {
                if (m.Object != null)
                {
                    this.Visit(m.Object);
                    this.Write(".");
                }
                this.Write(string.Format("?{0}?", m.Method.Name));
                this.Write("(");
                for (int i = 0; i < m.Arguments.Count; i++)
                {
                    if (i > 0)
                        this.Write(", ");
                    this.Visit(m.Arguments[i]);
                }
                this.Write(")");
                return m;
            }
            else
            {
                throw new NotSupportedException(string.Format("The method '{0}' is not supported", m.Method.Name));
            }
        }

        /// <summary>
        /// 判断指定的类型是否是整型。
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        /// 	<c>true</c> if the specified type is integer; otherwise, <c>false</c>.
        /// </returns>
        protected virtual bool IsInteger(Type type)
        {
            return TypeHelper.IsInteger(type);
        }

        /// <summary>
        /// 访问新表达式。
        /// </summary>
        /// <param name="nex">The nex.</param>
        /// <returns></returns>
        protected override NewExpression VisitNew(NewExpression nex)
        {
            if (this.forDebug)
            {
                this.Write("?new?");
                this.Write(nex.Type.Name);
                this.Write("(");
                for (int i = 0; i < nex.Arguments.Count; i++)
                {
                    if (i > 0)
                        this.Write(", ");
                    this.Visit(nex.Arguments[i]);
                }
                this.Write(")");
                return nex;
            }
            else
            {
                throw new NotSupportedException(string.Format("The construtor for '{0}' is not supported", nex.Constructor.DeclaringType));
            }
        }

        /// <summary>
        /// Visits the unary.
        /// </summary>
        /// <param name="u">The u.</param>
        /// <returns></returns>
        protected override Expression VisitUnary(UnaryExpression u)
        {
            string op = this.GetOperator(u);
            switch (u.NodeType)
            {
                case ExpressionType.Not:
                    if (IsBoolean(u.Operand.Type) || op.Length > 1)
                    {
                        this.Write(op);
                        this.Write(" ");
                        this.VisitPredicate(u.Operand);
                    }
                    else
                    {
                        this.Write(op);
                        this.VisitValue(u.Operand);
                    }
                    break;
                case ExpressionType.Negate:
                case ExpressionType.NegateChecked:
                    this.Write(op);
                    this.VisitValue(u.Operand);
                    break;
                case ExpressionType.UnaryPlus:
                    this.VisitValue(u.Operand);
                    break;
                case ExpressionType.Convert:
                    // ignore conversions for now
                    this.Visit(u.Operand);
                    break;
                default:
                    if (this.forDebug)
                    {
                        this.Write(string.Format("?{0}?", u.NodeType));
                        this.Write("(");
                        this.Visit(u.Operand);
                        this.Write(")");
                        return u;
                    }
                    else
                    {
                        throw new NotSupportedException(string.Format("The unary operator '{0}' is not supported", u.NodeType));
                    }
            }
            return u;
        }

        protected override Expression VisitBinary(BinaryExpression b)
        {
            string op = this.GetOperator(b);
            Expression left = b.Left;
            Expression right = b.Right;

            this.Write("(");
            switch (b.NodeType)
            {
                case ExpressionType.And:
                case ExpressionType.AndAlso:
                case ExpressionType.Or:
                case ExpressionType.OrElse:
                    if (this.IsBoolean(left.Type))
                    {
                        this.VisitPredicate(left);
                        this.Write(" ");
                        this.Write(op);
                        this.Write(" ");
                        this.VisitPredicate(right);
                    }
                    else
                    {
                        this.VisitValue(left);
                        this.Write(" ");
                        this.Write(op);
                        this.Write(" ");
                        this.VisitValue(right);
                    }
                    break;
                case ExpressionType.Equal:
                    if (right.NodeType == ExpressionType.Constant)
                    {
                        ConstantExpression ce = (ConstantExpression)right;
                        if (ce.Value == null)
                        {
                            this.Visit(left);
                            this.Write(" IS NULL");
                            break;
                        }
                    }
                    else if (left.NodeType == ExpressionType.Constant)
                    {
                        ConstantExpression ce = (ConstantExpression)left;
                        if (ce.Value == null)
                        {
                            this.Visit(right);
                            this.Write(" IS NULL");
                            break;
                        }
                    }
                    goto case ExpressionType.LessThan;
                case ExpressionType.NotEqual:
                    if (right.NodeType == ExpressionType.Constant)
                    {
                        ConstantExpression ce = (ConstantExpression)right;
                        if (ce.Value == null)
                        {
                            this.Visit(left);
                            this.Write(" IS NOT NULL");
                            break;
                        }
                    }
                    else if (left.NodeType == ExpressionType.Constant)
                    {
                        ConstantExpression ce = (ConstantExpression)left;
                        if (ce.Value == null)
                        {
                            this.Visit(right);
                            this.Write(" IS NOT NULL");
                            break;
                        }
                    }
                    goto case ExpressionType.LessThan;
                case ExpressionType.LessThan:
                case ExpressionType.LessThanOrEqual:
                case ExpressionType.GreaterThan:
                case ExpressionType.GreaterThanOrEqual:
                    // check for special x.CompareTo(y) && type.Compare(x,y)
                    if (left.NodeType == ExpressionType.Call && right.NodeType == ExpressionType.Constant)
                    {
                        MethodCallExpression mc = (MethodCallExpression)left;
                        ConstantExpression ce = (ConstantExpression)right;
                        if (ce.Value != null && ce.Value.GetType() == typeof(int) && ((int)ce.Value) == 0)
                        {
                            if (mc.Method.Name == "CompareTo" && !mc.Method.IsStatic && mc.Arguments.Count == 1)
                            {
                                left = mc.Object;
                                right = mc.Arguments[0];
                            }
                            else if (
                                (mc.Method.DeclaringType == typeof(string) || mc.Method.DeclaringType == typeof(decimal))
                                  && mc.Method.Name == "Compare" && mc.Method.IsStatic && mc.Arguments.Count == 2)
                            {
                                left = mc.Arguments[0];
                                right = mc.Arguments[1];
                            }
                        }
                    }
                    goto case ExpressionType.Add;
                case ExpressionType.Add:
                case ExpressionType.AddChecked:
                case ExpressionType.Subtract:
                case ExpressionType.SubtractChecked:
                case ExpressionType.Multiply:
                case ExpressionType.MultiplyChecked:
                case ExpressionType.Divide:
                case ExpressionType.Modulo:
                case ExpressionType.ExclusiveOr:
                case ExpressionType.LeftShift:
                case ExpressionType.RightShift:
                    this.VisitValue(left);
                    this.Write(" ");
                    this.Write(op);
                    this.Write(" ");
                    this.VisitValue(right);
                    break;
                default:
                    if (this.forDebug)
                    {
                        this.Write(string.Format("?{0}?", b.NodeType));
                        this.Write("(");
                        this.Visit(b.Left);
                        this.Write(", ");
                        this.Visit(b.Right);
                        this.Write(")");
                        return b;
                    }
                    else
                    {
                        throw new NotSupportedException(string.Format("The binary operator '{0}' is not supported", b.NodeType));
                    }
            }
            this.Write(")");
            return b;
        }

        /// <summary>
        /// 根据调用方法的名字返回对应的操作符。
        /// </summary>
        /// <param name="methodName">调用方法的名字.</param>
        /// <returns></returns>
        protected virtual string GetOperator(string methodName)
        {
            switch (methodName)
            {
                case "Add": return "+";
                case "Subtract": return "-";
                case "Multiply": return "*";
                case "Divide": return "/";
                case "Negate": return "-";
                case "Remainder": return "%";
                default: return null;
            }
        }

        protected virtual string GetOperator(UnaryExpression u)
        {
            switch (u.NodeType)
            {
                case ExpressionType.Negate:
                case ExpressionType.NegateChecked:
                    return "-";
                case ExpressionType.UnaryPlus:
                    return "+";
                case ExpressionType.Not:
                    return IsBoolean(u.Operand.Type) ? "NOT" : "~";
                default:
                    return string.Empty;
            }
        }

        /// <summary>
        /// 根据二元表达式获取SQL操作符.
        /// </summary>
        /// <param name="b">The b.</param>
        /// <returns></returns>
        protected virtual string GetOperator(BinaryExpression b)
        {
            switch (b.NodeType)
            {
                case ExpressionType.And:
                case ExpressionType.AndAlso:
                    return (IsBoolean(b.Left.Type)) ? "AND" : "&";
                case ExpressionType.Or:
                case ExpressionType.OrElse:
                    return (IsBoolean(b.Left.Type) ? "OR" : "|");
                case ExpressionType.Equal:
                    return "=";
                case ExpressionType.NotEqual:
                    return "<>";
                case ExpressionType.LessThan:
                    return "<";
                case ExpressionType.LessThanOrEqual:
                    return "<=";
                case ExpressionType.GreaterThan:
                    return ">";
                case ExpressionType.GreaterThanOrEqual:
                    return ">=";
                case ExpressionType.Add:
                case ExpressionType.AddChecked:
                    return "+";
                case ExpressionType.Subtract:
                case ExpressionType.SubtractChecked:
                    return "-";
                case ExpressionType.Multiply:
                case ExpressionType.MultiplyChecked:
                    return "*";
                case ExpressionType.Divide:
                    return "/";
                case ExpressionType.Modulo:
                    return "%";
                case ExpressionType.ExclusiveOr:
                    return "^";
                case ExpressionType.LeftShift:
                    return "<<";
                case ExpressionType.RightShift:
                    return ">>";
                default:
                    return "";
            }
        }

        protected virtual bool IsBoolean(Type type)
        {
            return type == typeof(bool) || type == typeof(bool?);
        }

        protected virtual bool IsPredicate(Expression expr)
        {
            switch (expr.NodeType)
            {
                case ExpressionType.And:
                case ExpressionType.AndAlso:
                case ExpressionType.Or:
                case ExpressionType.OrElse:
                    return IsBoolean(((BinaryExpression)expr).Type);
                case ExpressionType.Not:
                    return IsBoolean(((UnaryExpression)expr).Type);
                case ExpressionType.Equal:
                case ExpressionType.NotEqual:
                case ExpressionType.LessThan:
                case ExpressionType.LessThanOrEqual:
                case ExpressionType.GreaterThan:
                case ExpressionType.GreaterThanOrEqual:
                case (ExpressionType)DbExpressionType.IsNull:
                case (ExpressionType)DbExpressionType.Between:
                case (ExpressionType)DbExpressionType.Exists:
                case (ExpressionType)DbExpressionType.In:
                    return true;
                case ExpressionType.Call:
                    return IsBoolean(((MethodCallExpression)expr).Type);
                default:
                    return false;
            }
        }

        /// <summary>
        /// Visits the predicate.
        /// </summary>
        /// <param name="expr">The expr.</param>
        /// <returns></returns>
        protected virtual Expression VisitPredicate(Expression expr)
        {
            this.Visit(expr);
            if (!IsPredicate(expr))
            {
                this.Write(" <> 0");
            }
            return expr;
        }

        protected virtual Expression VisitValue(Expression expr)
        {
            return this.Visit(expr);
        }

        protected override Expression VisitConditional(ConditionalExpression c)
        {
            if (this.forDebug)
            {
                this.Write("?iff?(");
                this.Visit(c.Test);
                this.Write(", ");
                this.Visit(c.IfTrue);
                this.Write(", ");
                this.Visit(c.IfFalse);
                this.Write(")");
                return c;
            }
            else
            {
                throw new NotSupportedException(string.Format("Conditional expressions not supported"));
            }
        }

        /// <summary>
        /// Visits the constant.
        /// </summary>
        /// <param name="c">The c.</param>
        /// <returns></returns>
        protected override Expression VisitConstant(ConstantExpression c)
        {
            this.WriteValue(c.Value);
            return c;
        }

        protected virtual void WriteValue(object value)
        {
            if (value == null)
            {
                this.Write("NULL");
            }
            else if (value.GetType().IsEnum)
            {
                this.Write(Convert.ChangeType(value, Enum.GetUnderlyingType(value.GetType())));
            }
            else
            {
                switch (Type.GetTypeCode(value.GetType()))
                {
                    case TypeCode.Boolean:
                        this.Write(((bool)value) ? 1 : 0);
                        break;
                    case TypeCode.String:
                        this.Write("'");
                        this.Write(value);
                        this.Write("'");
                        break;
                    case TypeCode.Object:
                        throw new NotSupportedException(string.Format("The constant for '{0}' is not supported", value));
                    case TypeCode.Single:
                    case TypeCode.Double:
                        string str = value.ToString();
                        if (!str.Contains('.'))
                        {
                            str += ".0";
                        }
                        this.Write(str);
                        break;
                    default:
                        this.Write(value);
                        break;
                }
            }
        }

        protected override Expression VisitColumn(ColumnExpression column)
        {
            if (column.Alias != null && !this.HideColumnAliases)
            {
                this.WriteAliasName(GetAliasName(column.Alias));
                this.Write(".");
            }
            this.WriteColumnName(column.Name);
            return column;
        }

        protected override Expression VisitProjection(ProjectionExpression proj)
        {
            // treat these like scalar subqueries
            if ((proj.Projector is ColumnExpression) || this.forDebug)
            {
                this.Write("(");
                this.WriteLine(Indentation.Inner);
                this.Visit(proj.Select);
                this.Write(")");
                this.Indent(Indentation.Outer);
            }
            else
            {
                throw new NotSupportedException("Non-scalar projections cannot be translated to SQL.");
            }
            return proj;
        }

        protected override Expression VisitSelect(SelectExpression select)
        {
            this.AddAliases(select.From);
            this.Write("SELECT ");
            if (select.IsDistinct)
            {
                this.Write("DISTINCT ");
            }
            if (select.Take != null)
            {
                this.WriteTopClause(select.Take);
            }
            this.WriteColumns(select.Columns);
            if (select.From != null)
            {
                this.WriteLine(Indentation.Same);
                this.Write("FROM ");
                this.VisitSource(select.From);
            }
            if (select.Where != null)
            {
                this.WriteLine(Indentation.Same);
                this.Write("WHERE ");
                this.VisitPredicate(select.Where);
            }
            if (select.GroupBy != null && select.GroupBy.Count > 0)
            {
                this.WriteLine(Indentation.Same);
                this.Write("GROUP BY ");
                for (int i = 0, n = select.GroupBy.Count; i < n; i++)
                {
                    if (i > 0)
                    {
                        this.Write(", ");
                    }
                    this.VisitValue(select.GroupBy[i]);
                }
            }
            if (select.OrderBy != null && select.OrderBy.Count > 0)
            {
                this.WriteLine(Indentation.Same);
                this.Write("ORDER BY ");
                for (int i = 0, n = select.OrderBy.Count; i < n; i++)
                {
                    OrderExpression exp = select.OrderBy[i];
                    if (i > 0)
                    {
                        this.Write(", ");
                    }
                    this.VisitValue(exp.Expression);
                    if (exp.OrderType != OrderType.Ascending)
                    {
                        this.Write(" DESC");
                    }
                }
            }
            return select;
        }

        /// <summary>
        /// Writes the top clause.
        /// </summary>
        /// <param name="expression">The expression.</param>
        protected virtual void WriteTopClause(Expression expression)
        {
            this.Write("TOP (");
            this.Visit(expression);
            this.Write(") ");
        }

        protected virtual void WriteColumns(ReadOnlyCollection<ColumnDeclaration> columns)
        {
            if (columns.Count > 0)
            {
                for (int i = 0, n = columns.Count; i < n; i++)
                {
                    ColumnDeclaration column = columns[i];
                    if (i > 0)
                    {
                        this.Write(", ");
                    }
                    ColumnExpression c = this.VisitValue(column.Expression) as ColumnExpression;
                    if (!string.IsNullOrEmpty(column.Name) && (c == null || c.Name != column.Name))
                    {
                        this.Write(" ");
                        this.WriteAsColumnName(column.Name);
                    }
                }
            }
            else
            {
                this.Write("NULL ");
                if (this.isNested)
                {
                    this.WriteAsColumnName("tmp");
                    this.Write(" ");
                }
            }
        }

        protected override Expression VisitSource(Expression source)
        {
            bool saveIsNested = this.isNested;
            this.isNested = true;
            switch ((DbExpressionType)source.NodeType)
            {
                case DbExpressionType.Table:
                    TableExpression table = (TableExpression)source;
                    this.WriteTableName(table.Name);
                    if (!this.HideTableAliases)
                    {
                        this.Write(" ");
                        this.WriteAsAliasName(GetAliasName(table.Alias));
                    }
                    break;
                case DbExpressionType.Select:
                    SelectExpression select = (SelectExpression)source;
                    this.Write("(");
                    this.WriteLine(Indentation.Inner);
                    this.Visit(select);
                    this.WriteLine(Indentation.Same);
                    this.Write(") ");
                    this.WriteAsAliasName(GetAliasName(select.Alias));
                    this.Indent(Indentation.Outer);
                    break;
                case DbExpressionType.Join:
                    this.VisitJoin((JoinExpression)source);
                    break;
                default:
                    throw new InvalidOperationException("Select source is not valid type");
            }
            this.isNested = saveIsNested;
            return source;
        }

        protected override Expression VisitJoin(JoinExpression join)
        {
            this.VisitJoinLeft(join.Left);
            this.WriteLine(Indentation.Same);
            switch (join.Join)
            {
                case JoinType.CrossJoin:
                    this.Write("CROSS JOIN ");
                    break;
                case JoinType.InnerJoin:
                    this.Write("INNER JOIN ");
                    break;
                case JoinType.CrossApply:
                    this.Write("CROSS APPLY ");
                    break;
                case JoinType.OuterApply:
                    this.Write("OUTER APPLY ");
                    break;
                case JoinType.LeftOuter:
                case JoinType.SingletonLeftOuter:
                    this.Write("LEFT OUTER JOIN ");
                    break;
            }
            this.VisitJoinRight(join.Right);
            if (join.Condition != null)
            {
                this.WriteLine(Indentation.Inner);
                this.Write("ON ");
                this.VisitPredicate(join.Condition);
                this.Indent(Indentation.Outer);
            }
            return join;
        }

        protected virtual Expression VisitJoinLeft(Expression source)
        {
            return this.VisitSource(source);
        }

        protected virtual Expression VisitJoinRight(Expression source)
        {
            return this.VisitSource(source);
        }

        protected virtual void WriteAggregateName(string aggregateName)
        {
            switch (aggregateName)
            {
                case "Average":
                    this.Write("AVG");
                    break;
                case "LongCount":
                    this.Write("COUNT");
                    break;
                default:
                    this.Write(aggregateName.ToUpper());
                    break;
            }
        }

        protected virtual bool RequiresAsteriskWhenNoArgument(string aggregateName)
        {
            return aggregateName == "Count" || aggregateName == "LongCount";
        }

        protected override Expression VisitAggregate(AggregateExpression aggregate)
        {
            this.WriteAggregateName(aggregate.AggregateName);
            this.Write("(");
            if (aggregate.IsDistinct)
            {
                this.Write("DISTINCT ");
            }
            if (aggregate.Argument != null)
            {
                this.VisitValue(aggregate.Argument);
            }
            else if (RequiresAsteriskWhenNoArgument(aggregate.AggregateName))
            {
                this.Write("*");
            }
            this.Write(")");
            return aggregate;
        }

        protected override Expression VisitIsNull(IsNullExpression isnull)
        {
            this.VisitValue(isnull.Expression);
            this.Write(" IS NULL");
            return isnull;
        }

        protected override Expression VisitBetween(BetweenExpression between)
        {
            this.VisitValue(between.Expression);
            this.Write(" BETWEEN ");
            this.VisitValue(between.Lower);
            this.Write(" AND ");
            this.VisitValue(between.Upper);
            return between;
        }

        protected override Expression VisitRowNumber(RowNumberExpression rowNumber)
        {
            throw new NotSupportedException();
        }

        protected override Expression VisitScalar(ScalarExpression subquery)
        {
            this.Write("(");
            this.WriteLine(Indentation.Inner);
            this.Visit(subquery.Select);
            this.WriteLine(Indentation.Same);
            this.Write(")");
            this.Indent(Indentation.Outer);
            return subquery;
        }

        protected override Expression VisitExists(ExistsExpression exists)
        {
            this.Write("EXISTS(");
            this.WriteLine(Indentation.Inner);
            this.Visit(exists.Select);
            this.WriteLine(Indentation.Same);
            this.Write(")");
            this.Indent(Indentation.Outer);
            return exists;
        }

        protected override Expression VisitIn(InExpression @in)
        {
            if (@in.Values != null)
            {
                if (@in.Values.Count == 0)
                {
                    this.Write("0 <> 0");
                }
                else
                {
                    this.VisitValue(@in.Expression);
                    this.Write(" IN (");
                    for (int i = 0, n = @in.Values.Count; i < n; i++)
                    {
                        if (i > 0) this.Write(", ");
                        this.VisitValue(@in.Values[i]);
                    }
                    this.Write(")");
                }
            }
            else
            {
                this.VisitValue(@in.Expression);
                this.Write(" IN (");
                this.WriteLine(Indentation.Inner);
                this.Visit(@in.Select);
                this.WriteLine(Indentation.Same);
                this.Write(")");
                this.Indent(Indentation.Outer);
            }
            return @in;
        }

        protected override Expression VisitNamedValue(NamedValueExpression value)
        {
            this.WriteParameterName(value.Name);
            return value;
        }

        protected override Expression VisitInsert(InsertCommand insert)
        {
            this.Write("INSERT INTO ");
            this.WriteTableName(insert.Table.Name);
            this.Write("(");
            for (int i = 0, n = insert.Assignments.Count; i < n; i++)
            {
                ColumnAssignment ca = insert.Assignments[i];
                if (i > 0) this.Write(", ");
                this.WriteColumnName(ca.Column.Name);
            }
            this.Write(")");
            this.WriteLine(Indentation.Same);
            this.Write("VALUES (");
            for (int i = 0, n = insert.Assignments.Count; i < n; i++)
            {
                ColumnAssignment ca = insert.Assignments[i];
                if (i > 0) this.Write(", ");
                this.Visit(ca.Expression);
            }
            this.Write(")");
            return insert;
        }

        protected override Expression VisitUpdate(UpdateCommand update)
        {
            this.Write("UPDATE ");
            this.WriteTableName(update.Table.Name);
            this.WriteLine(Indentation.Same);
            bool saveHide = this.HideColumnAliases;
            this.HideColumnAliases = true;
            this.Write("SET ");
            for (int i = 0, n = update.Assignments.Count; i < n; i++)
            {
                ColumnAssignment ca = update.Assignments[i];
                if (i > 0) this.Write(", ");
                this.Visit(ca.Column);
                this.Write(" = ");
                this.Visit(ca.Expression);
            }
            if (update.Where != null)
            {
                this.WriteLine(Indentation.Same);
                this.Write("WHERE ");
                this.VisitPredicate(update.Where);
            }
            this.HideColumnAliases = saveHide;
            return update;
        }

        protected override Expression VisitDelete(DeleteCommand delete)
        {
            this.Write("DELETE FROM ");
            bool saveHideTable = this.HideTableAliases;
            bool saveHideColumn = this.HideColumnAliases;
            this.HideTableAliases = true;
            this.HideColumnAliases = true;
            this.VisitSource(delete.Table);
            if (delete.Where != null)
            {
                this.WriteLine(Indentation.Same);
                this.Write("WHERE ");
                this.VisitPredicate(delete.Where);
            }
            this.HideTableAliases = saveHideTable;
            this.HideColumnAliases = saveHideColumn;
            return delete;
        }

        protected override Expression VisitIf(IFCommand ifx)
        {
            throw new NotSupportedException();
        }

        protected override Expression VisitBlock(BlockCommand block)
        {
            throw new NotSupportedException();
        }

        protected override Expression VisitDeclaration(DeclarationCommand decl)
        {
            throw new NotSupportedException();
        }

        protected override Expression VisitVariable(VariableExpression vex)
        {
            this.WriteVariableName(vex.Name);
            return vex;
        }

        protected virtual void VisitStatement(Expression expression)
        {
            var p = expression as ProjectionExpression;
            if (p != null)
            {
                this.Visit(p.Select);
            }
            else
            {
                this.Visit(expression);
            }
        }

        protected override Expression VisitFunction(FunctionExpression func)
        {
            this.Write(func.Name);
            if (func.Arguments.Count > 0)
            {
                this.Write("(");
                for (int i = 0, n = func.Arguments.Count; i < n; i++)
                {
                    if (i > 0) this.Write(", ");
                    this.Visit(func.Arguments[i]);
                }
                this.Write(")");
            }
            return func;
        }
    }
}
