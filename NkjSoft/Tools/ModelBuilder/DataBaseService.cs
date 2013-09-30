using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NkjSoft.Tools.ModelBuilder
{
    /// <summary>
    /// 定义一组值，表示数据库服务的类型。
    /// </summary>
    public enum DataBaseService
    {
        /// <summary>
        /// SQL 服务器
        /// </summary>
        SqlDataBaseServer,

        /// <summary>
        /// Access 服务器
        /// </summary>
        AccessDataBaseServer,

        /// <summary>
        /// SQLite 服务器
        /// </summary>
        SQLiteDataBaseServer,

        /// <summary>
        /// MySQL 服务器
        /// </summary>
        MySQLDataBaseServer,

        /// <summary>
        /// Oracle 服务器
        /// </summary>
        OracleDataBaseServer,
    }
}
