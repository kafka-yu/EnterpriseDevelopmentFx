
using System;
using System.Data;
using System.Text;

namespace NkjSoft.ORM.Data
{
    using Common;
    using NkjSoft.ORM.Core;

    /// <summary>
    /// 
    /// </summary>
    public class DbTypeSystem : QueryTypeSystem
    {
        /// <summary>
        /// 将指定的数据类型转换成需要的数据类型.
        /// </summary>
        /// <param name="typeDeclaration">The type declaration.</param>
        /// <returns></returns>
        public override QueryType Parse(string typeDeclaration)
        {
            //TODO:设置查询参数类型信息
            string[] args = null;
            string typeName = null;
            string remainder = null;
            int openParen = typeDeclaration.IndexOf('(');
            if (openParen >= 0)
            {
                typeName = typeDeclaration.Substring(0, openParen).Trim();

                int closeParen = typeDeclaration.IndexOf(')', openParen);
                if (closeParen < openParen) closeParen = typeDeclaration.Length;

                string argstr = typeDeclaration.Substring(openParen + 1, closeParen - (openParen + 1));
                args = argstr.Split(',');
                remainder = typeDeclaration.Substring(closeParen + 1);
                //TODO:测试代码.. 
            }
            else
            {
                int space = typeDeclaration.IndexOf(' ');
                if (space >= 0)
                {
                    typeName = typeDeclaration.Substring(0, space);
                    remainder = typeDeclaration.Substring(space + 1).Trim();
                }
                else
                {
                    typeName = typeDeclaration;
                }
            }

            bool isNotNull = (remainder != null) ? remainder.ToUpper().Contains("NOT NULL") : false;

            return this.GetQueryType(typeName, args, isNotNull);
        }

        /// <summary>
        /// 获取查询的数据类型。
        /// </summary>
        /// <param name="typeName">Name of the type.</param>
        /// <param name="args">The args.</param>
        /// <param name="isNotNull">if set to <c>true</c> [is not null].</param>
        /// <returns></returns>
        public virtual QueryType GetQueryType(string typeName, string[] args, bool isNotNull)
        {
            if (String.Compare(typeName, "rowversion", StringComparison.OrdinalIgnoreCase) == 0)
            {
                typeName = "Timestamp";
            }

            if (String.Compare(typeName, "numeric", StringComparison.OrdinalIgnoreCase) == 0)
            {
                typeName = "Decimal";
            }

            if (String.Compare(typeName, "sql_variant", StringComparison.OrdinalIgnoreCase) == 0)
            {
                typeName = "Variant";
            }

            SqlDbType dbType = this.GetSqlType(typeName);

            int length = 0;
            short precision = 0;
            short scale = 0;

            switch (dbType)
            {
                case SqlDbType.Binary:
                case SqlDbType.Char:
                case SqlDbType.Image:
                case SqlDbType.NChar:
                case SqlDbType.NVarChar:
                case SqlDbType.VarBinary:
                case SqlDbType.VarChar:
                    if (args == null || args.Length < 1)
                    {
                        //TODO:默认字段长度
                        length = 80;
                    }
                    else if (string.Compare(args[0], "max", true) == 0)
                    {
                        length = Int32.MaxValue;
                    }
                    else
                    {
                        length = Int32.Parse(args[0]);
                    }
                    break;
                case SqlDbType.Money:
                    if (args == null || args.Length < 1)
                    {
                        precision = 29;
                    }
                    else
                    {
                        precision = Int16.Parse(args[0]);
                    }
                    if (args == null || args.Length < 2)
                    {
                        scale = 4;
                    }
                    else
                    {
                        scale = Int16.Parse(args[1]);
                    }
                    break;
                case SqlDbType.Decimal:
                    if (args == null || args.Length < 1)
                    {
                        precision = 29;
                    }
                    else
                    {
                        precision = Int16.Parse(args[0]);
                    }
                    if (args == null || args.Length < 2)
                    {
                        scale = 0;
                    }
                    else
                    {
                        scale = Int16.Parse(args[1]);
                    }
                    break;
                case SqlDbType.Float:
                case SqlDbType.Real:
                    if (args == null || args.Length < 1)
                    {
                        precision = 29;
                    }
                    else
                    {
                        precision = Int16.Parse(args[0]);
                    }
                    break;
                case SqlDbType.DateTime:
                    length = 8;
                    break;
                case SqlDbType.Int:
                    length = 4;
                    break;

            }

            return NewType(dbType, isNotNull, length, precision, scale);
        }

        /// <summary>
        /// 从SqlDbType 创建一个 <see cref="NkjSoft.ORM.Data.Common.QueryType"/>类型。
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="isNotNull">if set to <c>true</c> [is not null].</param>
        /// <param name="length">The length.</param>
        /// <param name="precision">The precision.</param>
        /// <param name="scale">The scale.</param>
        /// <returns></returns>
        public virtual QueryType NewType(SqlDbType type, bool isNotNull, int length, short precision, short scale)
        {
            return new DbQueryType(type, isNotNull, length, precision, scale);
        }

        /// <summary>
        /// 从指定的类型名称中获取 SQL 数据类型。
        /// </summary>
        /// <param name="typeName">Name of the type.</param>
        /// <returns></returns>
        public virtual SqlDbType GetSqlType(string typeName)
        {
            return (SqlDbType)Enum.Parse(typeof(SqlDbType), typeName, true);
        }

        /// <summary>
        /// 获取 String 类型数据的默认长度.
        /// </summary>
        /// <value>The default size of the string.</value>
        public virtual int StringDefaultSize
        {
            get { return Int32.MaxValue; }
        }

        /// <summary>
        /// 获取二进制值类型的最大长度。
        /// </summary>
        /// <value>The default size of the binary.</value>
        public virtual int BinaryDefaultSize
        {
            get { return Int32.MaxValue; }
        }

        /// <summary>
        /// 获取字段的数据类型.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public override QueryType GetColumnType(Type type)
        {
            bool isNotNull = type.IsValueType && !TypeHelper.IsNullableType(type);
            type = TypeHelper.GetNonNullableType(type);
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Boolean:
                    return NewType(SqlDbType.Bit, isNotNull, 0, 0, 0);
                case TypeCode.SByte:
                case TypeCode.Byte:
                    return NewType(SqlDbType.TinyInt, isNotNull, 0, 0, 0);
                case TypeCode.Int16:
                case TypeCode.UInt16:
                    return NewType(SqlDbType.SmallInt, isNotNull, 0, 0, 0);
                case TypeCode.Int32:
                case TypeCode.UInt32:
                    return NewType(SqlDbType.Int, isNotNull, 0, 0, 0);
                case TypeCode.Int64:
                case TypeCode.UInt64:
                    return NewType(SqlDbType.BigInt, isNotNull, 0, 0, 0);
                case TypeCode.Single:
                case TypeCode.Double:
                    return NewType(SqlDbType.Float, isNotNull, 0, 0, 0);
                case TypeCode.String:
                    return NewType(SqlDbType.NVarChar, isNotNull, this.StringDefaultSize, 0, 0);
                case TypeCode.Char:
                    return NewType(SqlDbType.NChar, isNotNull, 1, 0, 0);
                case TypeCode.DateTime:
                    return NewType(SqlDbType.DateTime, isNotNull, 0, 0, 0);
                case TypeCode.Decimal:
                    return NewType(SqlDbType.Decimal, isNotNull, 0, 29, 4);
                default:
                    if (type == typeof(byte[]))
                        return NewType(SqlDbType.VarBinary, isNotNull, this.BinaryDefaultSize, 0, 0);
                    else if (type == typeof(Guid))
                        return NewType(SqlDbType.UniqueIdentifier, isNotNull, 0, 0, 0);
                    else if (type == typeof(DateTimeOffset))
                        return NewType(SqlDbType.DateTimeOffset, isNotNull, 0, 0, 0);
                    else if (type == typeof(TimeSpan))
                        return NewType(SqlDbType.Time, isNotNull, 0, 0, 0);
                    return null;
            }
        }

        /// <summary>
        /// 获取SQL类型的DbType数据。
        /// </summary>
        /// <param name="dbType">Type of the db.</param>
        /// <returns></returns>
        public static DbType GetDbType(SqlDbType dbType)
        {
            switch (dbType)
            {
                case SqlDbType.BigInt:
                    return DbType.Int64;
                case SqlDbType.Binary:
                    return DbType.Binary;
                case SqlDbType.Bit:
                    return DbType.Boolean;
                case SqlDbType.Char:
                    return DbType.AnsiStringFixedLength;
                case SqlDbType.Date:
                    return DbType.Date;
                case SqlDbType.DateTime:
                case SqlDbType.SmallDateTime:
                    return DbType.DateTime;
                case SqlDbType.DateTime2:
                    return DbType.DateTime2;
                case SqlDbType.DateTimeOffset:
                    return DbType.DateTimeOffset;
                case SqlDbType.Decimal:
                    return DbType.Decimal;
                case SqlDbType.Float:
                case SqlDbType.Real:
                    return DbType.Double;
                case SqlDbType.Image:
                    return DbType.Binary;
                case SqlDbType.Int:
                    return DbType.Int32;
                case SqlDbType.Money:
                case SqlDbType.SmallMoney:
                    return DbType.Currency;
                case SqlDbType.NChar:
                    return DbType.StringFixedLength;
                case SqlDbType.NText:
                case SqlDbType.NVarChar:
                    return DbType.String;
                case SqlDbType.SmallInt:
                    return DbType.Int16;
                case SqlDbType.Text:
                    return DbType.AnsiString;
                case SqlDbType.Time:
                    return DbType.Time;
                case SqlDbType.Timestamp:
                    return DbType.Binary;
                case SqlDbType.TinyInt:
                    return DbType.SByte;
                case SqlDbType.Udt:
                    return DbType.Object;
                case SqlDbType.UniqueIdentifier:
                    return DbType.Guid;
                case SqlDbType.VarBinary:
                    return DbType.Binary;
                case SqlDbType.VarChar:
                    return DbType.AnsiString;
                case SqlDbType.Variant:
                    return DbType.Object;
                case SqlDbType.Xml:
                    return DbType.String;
                default:
                    throw new InvalidOperationException(string.Format("Unhandled sql type: {0}", dbType));
            }
        }

        /// <summary>
        /// Determines whether [is variable length] [the specified db type].
        /// </summary>
        /// <param name="dbType">Type of the db.</param>
        /// <returns>
        /// 	<c>true</c> if [is variable length] [the specified db type]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsVariableLength(SqlDbType dbType)
        {
            switch (dbType)
            {
                case SqlDbType.Image:
                case SqlDbType.NText:
                case SqlDbType.NVarChar:
                case SqlDbType.Text:
                case SqlDbType.VarBinary:
                case SqlDbType.VarChar:
                case SqlDbType.Xml:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// 获取变量定义的数据类型。
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="suppressSize">if set to <c>true</c> [suppress size].</param>
        /// <returns></returns>
        public override string GetVariableDeclaration(QueryType type, bool suppressSize)
        {
            var sqlType = (DbQueryType)type;
            StringBuilder sb = new StringBuilder();
            sb.Append(sqlType.SqlDbType.ToString().ToUpper());
            if (sqlType.Length > 0 && !suppressSize)
            {
                if (sqlType.Length == Int32.MaxValue)
                {
                    sb.Append("(max)");
                }
                else
                {
                    sb.AppendFormat("({0})", sqlType.Length);
                }
            }
            else if (sqlType.Precision != 0)
            {
                if (sqlType.Scale != 0)
                {
                    sb.AppendFormat("({0},{1})", sqlType.Precision, sqlType.Scale);
                }
                else
                {
                    sb.AppendFormat("({0})", sqlType.Precision);
                }
            }
            return sb.ToString();
        }
    }

    /// <summary>
    /// 表示查询类型。
    /// </summary>
    public class DbQueryType : QueryType
    {
        SqlDbType dbType;
        bool notNull;
        int length;
        short precision;
        short scale;


        /// <summary>
        /// Initializes a new instance of the <see cref="DbQueryType"/> class.
        /// </summary>
        /// <param name="dbType">Type of the db.</param>
        /// <param name="notNull">if set to <c>true</c> [not null].</param>
        /// <param name="length">The length.</param>
        /// <param name="precision">The precision.</param>
        /// <param name="scale">The scale.</param>
        public DbQueryType(SqlDbType dbType, bool notNull, int length, short precision, short scale)
        {
            this.dbType = dbType;
            this.notNull = notNull;
            this.length = length;
            this.precision = precision;
            this.scale = scale;
        }

        /// <summary>
        /// Gets the type of the db.
        /// </summary>
        /// <value>The type of the db.</value>
        public DbType DbType
        {
            get { return DbTypeSystem.GetDbType(this.dbType); }
        }

        public SqlDbType SqlDbType
        {
            get { return this.dbType; }
        }

        /// <summary>
        /// Gets the length.
        /// </summary>
        /// <value>The length.</value>
        public override int Length
        {
            get { return this.length; }
        }

        public override bool NotNull
        {
            get { return this.notNull; }
        }

        public override short Precision
        {
            get { return this.precision; }
        }

        public override short Scale
        {
            get { return this.scale; }
        }

        public override string DataType
        {
            get { return dbType.ToString(); }
        }
    }
}