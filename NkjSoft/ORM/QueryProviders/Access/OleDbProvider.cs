
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.OleDb;

namespace NkjSoft.ORM.Data.Access
{
    using NkjSoft.ORM.Data.Common;
    using NkjSoft.ORM.Data.OleDb;

    /// <summary>
    /// 
    /// </summary>
    public class OleDbProvider : OleDb.OleDbQueryProvider
    {
        Dictionary<QueryCommand, OleDbCommand> commandCache = new Dictionary<QueryCommand, OleDbCommand>();

        /// <summary>
        /// Initializes a new instance of the <see cref="OleDbProvider"/> class.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="mapping">The mapping.</param>
        /// <param name="policy">The policy.</param>
        public OleDbProvider(OleDbConnection connection, QueryMapping mapping, QueryPolicy policy)
            : base(connection, AccessLanguage.Default, mapping, policy)
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
            return new OleDbProvider((OleDbConnection)connection, mapping, policy);
        }

        /// <summary>
        /// Gets the connection string.
        /// </summary>
        /// <param name="databaseFile">The database file.</param>
        /// <returns></returns>
        public static string GetConnectionString(string databaseFile)
        {
            string dbLower = databaseFile.ToLower();
            if (dbLower.Contains(".mdb"))
            {
                return GetConnectionString(AccessOleDbProvider2000, databaseFile);
            }
            else if (dbLower.Contains(".accdb"))
            {
                return GetConnectionString(AccessOleDbProvider2007, databaseFile);
            }
            else
            {
                throw new InvalidOperationException(string.Format("Unrecognized file extension on database file '{0}'", databaseFile));
            }
        }

        /// <summary>
        /// 获取连接字符串。
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <param name="databaseFile">The database file.</param>
        /// <returns></returns>
        private static string GetConnectionString(string provider, string databaseFile)
        {
            return string.Format("Provider={0};ole db services=0;Data Source={1}", provider, databaseFile);
        }

        /// <summary>
        /// 获取表示对Access 2007 之前的版本数据库的数据提供程序版本。
        /// </summary>
        public static readonly string AccessOleDbProvider2000 = "Microsoft.Jet.OLEDB.4.0";
        /// <summary>
        /// 获取表示对Access 2007 及以后的版本数据库的数据提供程序版本。
        /// </summary>
        public static readonly string AccessOleDbProvider2007 = "Microsoft.ACE.OLEDB.12.0";

        /// <summary>
        /// Creates the executor.
        /// </summary>
        /// <returns></returns>
        protected override QueryExecutor CreateExecutor()
        {
            return new Executor(this);
        }

        /// <summary>
        /// 表示 OleDb 的查询执行者
        /// </summary>
        public new class Executor : OleDbQueryProvider.Executor
        {
            OleDbProvider provider;

            /// <summary>
            /// Initializes a new instance of the <see cref="Executor"/> class.
            /// </summary>
            /// <param name="provider">The provider.</param>
            public Executor(OleDbProvider provider)
                : base(provider)
            {
                this.provider = provider;
            }

            /// <summary>
            /// 通过查询命令已经参数获取此命令对应的 DbCommand 对象。
            /// </summary>
            /// <param name="query">The query.</param>
            /// <param name="paramValues">The param values.</param>
            /// <returns></returns>
            protected override DbCommand GetCommand(QueryCommand query, object[] paramValues)
            {
#if false
                OleDbCommand cmd;
                if (!this.provider.commandCache.TryGetValue(query, out cmd))
                {
                    cmd = (OleDbCommand)this.provider.Connection.CreateCommand();
                    cmd.CommandText = query.CommandText;
                    this.SetParameterValues(query, cmd, paramValues);
                    if (this.provider.Transaction != null)
                        cmd.Transaction = (OleDbTransaction)this.provider.Transaction;
                    cmd.Prepare();
                    this.provider.commandCache.Add(query, cmd);
                }
                else
                {
                    cmd = (OleDbCommand)cmd.Clone();
                    if (this.provider.Transaction != null)
                        cmd.Transaction = (OleDbTransaction)this.provider.Transaction;
                    this.SetParameterValues(query, cmd, paramValues);
                }
#else
                var cmd = (OleDbCommand)this.provider.Connection.CreateCommand();
                cmd.CommandText = query.CommandText;
                this.SetParameterValues(query, cmd, paramValues);
                if (this.provider.Transaction != null)
                    cmd.Transaction = (OleDbTransaction)this.provider.Transaction;

#endif
                return cmd;
            }

            /// <summary>
            /// Gets the type of the OLE db.
            /// </summary>
            /// <param name="type">The type.</param>
            /// <returns></returns>
            protected override OleDbType GetOleDbType(QueryType type)
            {
                DbQueryType sqlType = type as DbQueryType;
                if (sqlType != null)
                {
                    return ToOleDbType(sqlType.SqlDbType);
                }
                return base.GetOleDbType(type);
            }
        }
    }
}