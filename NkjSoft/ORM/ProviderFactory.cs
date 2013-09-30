using System;
using System.Data.Common;
using NkjSoft.ORM.Data;
using NkjSoft.ORM.Data.Common;
using NkjSoft.ORM.Data.Mapping;

namespace NkjSoft.ORM
{
    /// <summary>
    /// 数据提供程序工厂类。 无法继承此类。
    /// </summary>
    [Serializable]
    public sealed class ProviderFactory
    {
        #region --- 变量||字段 ---
        /// <summary>
        /// 在将要创建 Provider 的时候发生。
        /// </summary>
        private static Func<string, string> BeforeCreateProvider;
        #endregion

        #region --- 常量 ---

        #endregion


        #region --- 属性 ---

        //缓存

        private static bool autoGet = true;

        /// <summary>
        /// 获取或设置是否自动判断连接字符串的获取。在需要对连接字符串进行加密等处理得时候，请设置此为 False。默认 true;
        /// </summary>
        /// <value><c>true</c> if [auto get]; otherwise, <c>false</c>.</value>
        public static bool AutoGet
        {
            get { return ProviderFactory.autoGet; }
            set { autoGet = value; }
        }


        #endregion

        #region --- AppSettings KeyName ---
        /// <summary>
        /// 获取或设置应用程序 AppSettings 配置节点中，获取 Provider 类型的节点名称。该字段默认值为 "NkjSoft.Provider"
        /// </summary>
        public static string PROVIDER_NODE_NAME = "NkjSoft.Provider";
        /// <summary>
        /// 获取或设置应用程序 &lt;connectionStrings&gt; 配置节点中，用来获取数据库连接字符串的节点名称（默认与 PROVIDER_NODE_NAME 节点的值相同）。
        /// </summary>
        public static string CONNECTION_NODE_NAME = "NkjSoft.Connection";
        /// <summary>
        /// 获取或设置应用程序 AppSettings 配置节点中，用来获取对象映射方式的节点名称。该字段默认值为“NkjSoft.Mapping”。
        /// </summary>
        public static string MAPPING_NODE_NAME = "NkjSoft.Mapping";
        #endregion

        /// <summary>
        /// 从应用程序配置中获取实体查询提供程序类型、连接对象的连接字符串信息、映射方式类型，这些信息将用以创建一个新的 <see cref="NkjSoft.ORM.Data.DbEntityProvider"/> 对象。
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">没有设置应用程序配置文件。</exception>
        public static DbEntityProvider FromApplicationSettings()
        {
            //获取 Provider 类型限定名
            var providerName = NkjSoft.Common.AppHelper.GetAppSetting(PROVIDER_NODE_NAME);

            var connName = NkjSoft.Common.AppHelper.GetAppSetting(CONNECTION_NODE_NAME);
            if (string.IsNullOrEmpty(connName))
            {
                connName = providerName;
            }
            var connectionString = NkjSoft.Common.AppHelper.GetConnectionSetting(connName);
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentException("ConnectionString丢失");
            }
            if (BeforeCreateProvider != null)
                connectionString = BeforeCreateProvider(connectionString);
            return FromApplicationSettings(providerName, connectionString);
        }



        /// <summary>
        /// 从应用程序配置中获取实体查询提供程序类型、连接对象的连接字符串信息、映射方式类型，这些信息将用以创建一个新的 <see cref="NkjSoft.ORM.Data.DbEntityProvider"/> 对象。
        /// </summary>
        /// <param name="doWithConnectionString">对连接字符串进行处理(比如加密等)</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">没有设置应用程序配置文件。</exception>
        public static DbEntityProvider FromApplicationSettings(Func<string, string> doWithConnectionString)
        {
            if (doWithConnectionString != null)
                BeforeCreateProvider = doWithConnectionString;
            return FromApplicationSettings();
        }

        /// <summary>
        /// 从应用程序配置中的指定节点获取实体查询提供程序类型、连接对象的连接字符串信息、映射方式类型，以创建一个新的 <see cref="NkjSoft.ORM.Data.DbEntityProvider"/> 对象。
        /// </summary>
        /// <param name="providerTypeName">Name of the provider type.</param>
        /// <param name="connnectionString">连接字符串</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException"> 应用程序配置文件节点未找到。</exception>
        public static DbEntityProvider FromApplicationSettings(string providerTypeName, string connnectionString)
        {
            var dataType = (DataClient)Enum.Parse(typeof(DataClient), providerTypeName, true);
            return FromApplicationSettings(dataType, connnectionString);
        }

        /// <summary>
        /// 指定一个 <see cref="DataClient"/> 的其中之一的值、一个对应的连接字符串，以创建一个新的 <see cref="NkjSoft.ORM.Data.DbEntityProvider"/> 对象。
        /// </summary>
        /// <param name="providerType">实体查询提供程序类型</param>
        /// <param name="connnectionString">连接字符串</param>
        /// <param name="mappingKeyName">对象映射方式</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException"> 应用程序配置文件节点未找到。</exception>
        public static DbEntityProvider FromApplicationSettings(DataClient providerType, string connnectionString)
        {
            DbEntityProvider _temp = null;
            var mp = new ImplicitMapping();

            DbConnection conn = null;
            var t = typeof(NkjSoft.ORM.Data.SqlClient.SqlDbProvider);
            switch (providerType)
            {
                default:
                case DataClient.SqlDbProvider:
                    // m_provider = new Data.SqlClient.SqlDbProvider((System.Data.SqlClient.SqlConnection)conn, mp, QueryPolicy.Default); 
                    conn = System.Data.SqlClient.SqlClientFactory.Instance.CreateConnection();
                    break;
#if OtherSQLProvider
                case DataClient.MySqlDbProvider:
                    //m_provider = new Data.MySqlClient.MySqlDbProvider((MySql.Data.MySqlClient.MySqlConnection)conn, mp, QueryPolicy.Default);
                    t = typeof(NkjSoft.ORM.Data.MySqlClient.MySqlDbProvider);
                    conn = MySql.Data.MySqlClient.MySqlClientFactory.Instance.CreateConnection();
                    break;
                case DataClient.OracleProvider:
                    //m_provider = new Datarovider((System.Data.)conn, mp, QueryPolicy.Default);
                    //UNDONE : 暂时未实现 Oracle 版本
                    break;
                case DataClient.SQLiteDbProvider:
                    //m_provider = new Data.SQLite.SQLiteDbProvider((System.Data.SQLite.SQLiteConnection)conn, mp, QueryPolicy.Default);    
                    t = typeof(NkjSoft.ORM.Data.SQLite.SQLiteDbProvider);
                    conn = System.Data.SQLite.SQLiteFactory.Instance.CreateConnection();
                    break;
#endif
                case DataClient.OleDbProvider:
                    //m_provider = new Data.Access.OleDbProvider((System.Data.OleDb.OleDbConnection)conn, mp, QueryPolicy.Default);
                    t = typeof(NkjSoft.ORM.Data.Access.OleDbProvider);
                    conn = System.Data.OleDb.OleDbFactory.Instance.CreateConnection();
                    break;
            }
            conn.ConnectionString = connnectionString;

            _temp = Activator.CreateInstance(t, conn, mp, QueryPolicy.Default) as DbEntityProvider;

            return _temp;
        }


        //TODO:2010-12-16 添加..
        /// <summary>
        /// 指定一个连接字符串，模块内部根据连接字符串推断具体的 ProviderType ，以此进行 <see cref="DbEntityProvider"/> 的实例化。
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <returns></returns>
        public static DbEntityProvider From(string connectionString)
        {
            //根据 connection 推断 DbProviderType 
            var provider = DataClient.SqlDbProvider;
            var clower = connectionString.ToLower();
            if (clower.Contains(".mdb") || clower.Contains(".accdb"))
            {
                provider = DataClient.OleDbProvider;
            }
            else if (clower.Contains(".sl3") || clower.Contains(".db3") || clower.Contains("db"))
            {
                provider = DataClient.SQLiteDbProvider;
            }
            else if (clower.Contains(".mdf"))
            {
                provider = DataClient.SqlDbProvider;
            }
            else
            {
                throw new InvalidOperationException(string.Format("指定的连接字符串无法推断出具体的 DbProvider,建议调用FromApplicationSettings(DataClient,ConnectionString)重载"));
            }
            return FromApplicationSettings(provider, connectionString);
        }



        /// <summary>
        /// 获取ADO.Net 的 Connection 类型。
        /// </summary>
        /// <param name="providerType">提供程序类型</param>
        /// <returns></returns>
        private static Type GetAdoConnectionType(Type providerType)
        {
            // 效率...又低了..T_T
            foreach (var con in providerType.GetConstructors())
            {
                foreach (var arg in con.GetParameters())
                {
                    if (arg.ParameterType.IsSubclassOf(typeof(DbConnection)))
                        return arg.ParameterType;
                }
            }
            return null;
        }

    }

}
/// <summary>
/// 
/// </summary>
public class ProviderTypes
{
    /// <summary>
    /// 
    /// </summary>
    public const string MsSql = "System.Data.SqlClient";
    /// <summary>
    /// 
    /// </summary>
    public const string MsAccess = "System.Data.OleDb";
    /// <summary>
    /// 
    /// </summary>
    public const string MySql = "MySql.Data.MySqlClient";
    /// <summary>
    /// Oracle 数据库 .(暂时不支持..呼呼...)
    /// </summary>
    public const string Oracle = "System.Data.OracleClient";
    /// <summary>
    /// 
    /// </summary>
    public const string SqlLite = "System.Data.SQLite";
}

/// <summary>
/// 定义提供ORM查询的不同数据库类型表述。
/// </summary>
public enum DataClient
{
    /// <summary>
    /// Sql Server 查询提供程序
    /// </summary>
    SqlDbProvider,

    /// <summary>
    /// OleDbProvider 查询提供程序
    /// </summary>
    OleDbProvider,

    /// <summary>
    /// SQLite 查询提供程序
    /// </summary>
    SQLiteDbProvider,

    /// <summary>
    /// MySQL 查询提供程序
    /// </summary>
    MySqlDbProvider,

    /// <summary>
    /// Oracle 查询提供程序
    /// </summary>
    OracleProvider
}
