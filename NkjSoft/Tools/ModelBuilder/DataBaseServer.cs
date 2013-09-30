using System.Collections.Generic;

namespace NkjSoft.Tools.ModelBuilder
{
    /// <summary>
    /// 提供对多重数据库服务器进行交互的能力。
    /// </summary>
    public abstract class DataBaseServer : IDataBaseService
    {
        /// <summary>
        ///实例化一个新的 <see cref="DataBaseServer"/> 对象。
        /// </summary>
        /// <param name="connString">The conn string.</param>
        protected DataBaseServer(string connString)
        {
            this.ConnectionString = connString;
        }

        /// <summary>
        /// 获取或设置用于访问数据库的连接字符串.
        /// </summary>
        /// <value></value>
        public virtual string ConnectionString { get; set; }

        /// <summary>
        /// 获取数据库列表
        /// </summary>
        /// <returns></returns>
        public abstract List<string> LoadDataBases();
        /// <summary>
        /// 获取数据库表的完整信息
        /// </summary>
        /// <param name="tableName">数据库表名</param>
        /// <returns></returns>
        public abstract List<Column> LoadDataTableInfo(string tableName);
        /// <summary>
        /// 获取表名列表
        /// </summary>
        /// <param name="dbName">数据库名</param>
        /// <returns></returns>
        public abstract List<Table> LoadTables(string dbName);


        /// <summary>
        /// 获取指定数据库源的连接字符串模板。
        /// </summary>
        /// <value></value>
        public abstract string ConnectionStringTemplate { get; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="DataBaseServer"/> is connected.
        /// </summary>
        /// <value>
        ///   <c>true</c> if connected; otherwise, <c>false</c>.
        /// </value>
        public bool Connected { get; set; }

        /// <summary>
        /// Gets or sets the type of the db provider.
        /// </summary>
        /// <value>
        /// The type of the db provider.
        /// </value>
        public DataClient DbProviderType { get; set; }

        #region IDataBaseService 成员

        #endregion
    }
}
