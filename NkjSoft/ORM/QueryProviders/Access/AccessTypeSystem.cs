using System;
using System.Data;
using System.Text;

namespace NkjSoft.ORM.Data.Access
{
    using NkjSoft.ORM.Data.Common;

    /// <summary>
    /// 表示提供给 Access 查询提供程序的数据类型系统.
    /// </summary>
    public class AccessTypeSystem : DbTypeSystem
    {
        /// <summary>
        /// Gets the default size of the string.
        /// </summary>
        /// <value>The default size of the string.</value>
        public override int StringDefaultSize
        {
            get { return 2000; }
        }

        /// <summary>
        /// 获取二进制值类型的最大长度。
        /// </summary>
        /// <value>The default size of the binary.</value>
        public override int BinaryDefaultSize
        {
            get { return 4000; }
        }

        /// <summary>
        /// 获取查询的数据类型。
        /// </summary>
        /// <param name="typeName">Name of the type.</param>
        /// <param name="args">The args.</param>
        /// <param name="isNotNull">if set to <c>true</c> [is not null].</param>
        /// <returns></returns>
        public override QueryType GetQueryType(string typeName, string[] args, bool isNotNull)
        {
            if (String.Compare(typeName, "Memo", StringComparison.OrdinalIgnoreCase) == 0)
            {
                return base.GetQueryType("varchar", new [] {"max"}, isNotNull);
            }
            return base.GetQueryType(typeName, args, isNotNull);
        }

        /// <summary>
        /// 从指定的类型名称中获取 SQL 数据类型。
        /// </summary>
        /// <param name="typeName">Name of the type.</param>
        /// <returns></returns>
        public override SqlDbType GetSqlType(string typeName)
        {
            if (string.Compare(typeName, "Memo", true) == 0)
            {
                return SqlDbType.VarChar;
            }
            else if (string.Compare(typeName, "Currency", true) == 0)
            {
                return SqlDbType.Decimal;
            }
            else if (string.Compare(typeName, "ReplicationID", true) == 0)
            {
                return SqlDbType.UniqueIdentifier;
            }
            else if (string.Compare(typeName, "YesNo", true) == 0)
            {
                return SqlDbType.Bit;
            }
            else if (string.Compare(typeName, "LongInteger", true) == 0)
            {
                return SqlDbType.BigInt;
            }
            else if (string.Compare(typeName, "VarWChar", true) == 0)
            {
                return SqlDbType.NVarChar;
            }
            else
            {
                return base.GetSqlType(typeName);
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
            StringBuilder sb = new StringBuilder();
            DbQueryType sqlType = (DbQueryType)type;
            SqlDbType sqlDbType = sqlType.SqlDbType;

            switch (sqlDbType)
            {
                case SqlDbType.BigInt:
                case SqlDbType.Bit:
                case SqlDbType.DateTime:
                case SqlDbType.Int:
                case SqlDbType.Money:
                case SqlDbType.SmallDateTime:
                case SqlDbType.SmallInt:
                case SqlDbType.SmallMoney:
                case SqlDbType.Timestamp:
                case SqlDbType.TinyInt:
                case SqlDbType.UniqueIdentifier:
                case SqlDbType.Variant:
                case SqlDbType.Xml:
                    sb.Append(sqlDbType);
                    break;
                case SqlDbType.Binary:
                case SqlDbType.Char:
                case SqlDbType.NChar:
                    sb.Append(sqlDbType);
                    if (type.Length > 0 && !suppressSize)
                    {
                        sb.Append("(");
                        sb.Append(type.Length);
                        sb.Append(")");
                    }
                    break;
                case SqlDbType.Image:
                case SqlDbType.NText:
                case SqlDbType.NVarChar:
                case SqlDbType.Text:
                case SqlDbType.VarBinary:
                case SqlDbType.VarChar:
                    sb.Append(sqlDbType);
                    if (type.Length > 0 && !suppressSize)
                    {
                        sb.Append("(");
                        sb.Append(type.Length);
                        sb.Append(")");
                    }
                    break;
                case SqlDbType.Decimal:
                    sb.Append("Currency");
                    break;
                case SqlDbType.Float:
                case SqlDbType.Real:
                    sb.Append(sqlDbType);  
                    if (type.Precision != 0)
                    {
                        sb.Append("(");
                        sb.Append(type.Precision);
                        if (type.Scale != 0)
                        {
                            sb.Append(",");
                            sb.Append(type.Scale);
                        }
                        sb.Append(")");
                    }
                    break;
            }
            return sb.ToString();
        }
    }
}