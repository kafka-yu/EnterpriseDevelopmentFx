using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NkjSoft.Tools.ModelBuilder
{
    /// <summary>
    /// 提供对数据库的操作。
    /// </summary>
    public interface IDataBaseService
    {
        /// <summary>
        /// 获取数据库列表
        /// </summary>
        /// <returns></returns>
        List<string> LoadDataBases();
        /// <summary>
        /// 获取数据库表的完整信息
        /// </summary>
        /// <param name="tableName">数据库表名</param>
        /// <returns></returns>
        List<Column> LoadDataTableInfo(string tableName);
        /// <summary>
        /// 获取表名列表
        /// </summary>
        /// <param name="dbName">数据库名</param>
        /// <returns></returns>
        List<Table> LoadTables(string dbName);


        /// <summary>
        /// 获取或设置用于访问数据库的连接字符串.
        /// </summary>
        string ConnectionString { get; set; }

        /// <summary>
        /// 获取指定数据库源的连接字符串模板。
        /// </summary>
        string ConnectionStringTemplate { get; }
    }
}
