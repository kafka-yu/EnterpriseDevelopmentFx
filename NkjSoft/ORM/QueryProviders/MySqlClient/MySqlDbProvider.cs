 

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

#if MySQLEnabled
namespace NkjSoft.ORM.Data.MySqlClient
{
    using NkjSoft.ORM.Data.Common;
    using MySql.Data.MySqlClient;

    /// <summary>
    /// MySQL 数据访问提供程序
    /// </summary>
    public class MySqlDbProvider : DbEntityProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MySqlDbProvider"/> class.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="mapping">The mapping.</param>
        /// <param name="policy">The policy.</param>
        public MySqlDbProvider(MySqlConnection connection, QueryMapping mapping, QueryPolicy policy)
            : base(connection, MySqlLanguage.Default, mapping, policy)
        {
        }

        /// <summary>
        /// 根据连接对象、映射信息、查询策略新建一个 <see cref="NkjSoft.ORM.Data.DbEntityProvider"/> 对象。
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="mapping">The mapping.</param>
        /// <param name="policy">The policy.</param>
        /// <returns></returns>
        public override DbEntityProvider New(DbConnection connection, QueryMapping mapping, QueryPolicy policy)
        {
            return new MySqlDbProvider((MySqlConnection)connection, mapping, policy);
        }

        /// <summary>
        /// Gets the connection string.
        /// </summary>
        /// <param name="databaseName">Name of the database.</param>
        /// <returns></returns>
        public static string GetConnectionString(string databaseName)
        {
            return string.Format(@"Server=127.0.0.1;Database={0}", databaseName);
        }

        /// <summary>
        /// 创建一个 <see cref="NkjSoft.ORM.Data.Common.QueryExecutor"/> 查询器对象。
        /// </summary>
        /// <returns></returns>
        protected override QueryExecutor CreateExecutor()
        {
            return new Executor(this);
        }

        new class Executor : DbEntityProvider.Executor
        {
            MySqlDbProvider provider;

            public Executor(MySqlDbProvider provider)
                : base(provider)
            {
                this.provider = provider;
            }

            protected override bool BufferResultRows
            {
                get { return true; }
            }

            protected override void AddParameter(DbCommand command, QueryParameter parameter, object value)
            {
                DbQueryType sqlType = (DbQueryType)parameter.QueryType;
                if (sqlType == null)
                    sqlType = (DbQueryType)this.provider.Language.TypeSystem.GetColumnType(parameter.Type);
                var p = ((MySqlCommand)command).Parameters.Add(parameter.Name, ToMySqlDbType(sqlType.SqlDbType), sqlType.Length);
                if (sqlType.Precision != 0)
                    p.Precision = (byte)sqlType.Precision;
                if (sqlType.Scale != 0)
                    p.Scale = (byte)sqlType.Scale;
                p.Value = value ?? DBNull.Value;
            }
        }

        public static MySqlDbType ToMySqlDbType(SqlDbType dbType)
        {
            switch (dbType)
            {
                case SqlDbType.BigInt:
                    return MySqlDbType.Int64;
                case SqlDbType.Binary:
                    return MySqlDbType.Binary;
                case SqlDbType.Bit:
                    return MySqlDbType.Bit;
                case SqlDbType.NChar:
                case SqlDbType.Char:
                    return MySqlDbType.Text;
                case SqlDbType.Date:
                    return MySqlDbType.Date;
                case SqlDbType.DateTime:
                case SqlDbType.SmallDateTime:
                    return MySqlDbType.DateTime;
                case SqlDbType.Decimal:
                    return MySqlDbType.Decimal;
                case SqlDbType.Float:
                    return MySqlDbType.Float;
                case SqlDbType.Image:
                    return MySqlDbType.LongBlob;
                case SqlDbType.Int:
                    return MySqlDbType.Int32;
                case SqlDbType.Money:
                case SqlDbType.SmallMoney:
                    return MySqlDbType.Decimal;
                case SqlDbType.NVarChar:
                case SqlDbType.VarChar:
                    return MySqlDbType.VarChar;
                case SqlDbType.SmallInt:
                    return MySqlDbType.Int16;
                case SqlDbType.NText:
                case SqlDbType.Text:
                    return MySqlDbType.LongText;
                case SqlDbType.Time:
                    return MySqlDbType.Time;
                case SqlDbType.Timestamp:
                    return MySqlDbType.Timestamp;
                case SqlDbType.TinyInt:
                    return MySqlDbType.Byte;
                case SqlDbType.UniqueIdentifier:
                    return MySqlDbType.Guid;
                case SqlDbType.VarBinary:
                    return MySqlDbType.VarBinary;
                case SqlDbType.Xml:
                    return MySqlDbType.Text;
                default:
                    throw new NotSupportedException(string.Format("The SQL type '{0}' is not supported", dbType));
            }
        }
    }
}
#endif