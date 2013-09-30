//--------------------------版权信息----------------------------
//       
//                 文件名: Extensions                 
//                 CLR Version: 2.0.50727.4927
//                 项目命名空间: HC.Common
//
//                 作  者: 俞如凯 
//                 Q Q: 250820436　 yurukai@vip.qq.com
//                 E-Mail: yurukai@hotmail.com
//                 创建时间 : 2010/7/01 08:32:38
//                 Copyright (c) Yurukai , All rights reserved.
//----------------------------------------------------------------



using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NkjSoft.ORM.Data;

namespace NkjSoft.ORM
{
    using NkjSoft.Extensions;
    /// <summary>
    /// 提供对指定数据源进行ORM查询的基本功能。
    /// </summary>
    public abstract class QueryContext
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryContext"/> class.
        /// </summary>
        protected QueryContext()
        {

        }
        /// <summary>
        /// 指定一个 <see cref="NkjSoft.ORM.Data.DbEntityProvider"/> 实例创建一个新的 <see cref="NkjSoft.ORM.QueryContext"/> 实例。
        /// </summary>
        /// <param name="provider"></param>
        protected QueryContext(DbEntityProvider provider) { this._provider = provider; }

        /// <summary>
        /// 通过指定的连接字符串初始化一个 <see cref="QueryContext"/> 实例。
        /// </summary>
        /// <param name="connectionString">连接字符串.</param>
        protected QueryContext(string connectionString)
        {
            this._provider = ProviderFactory.From(connectionString);
            if (_provider == null)
            { throw new NullReferenceException("Provider"); }
        }


        private DbEntityProvider _provider;

        /// <summary>
        /// 获取或设置查询提供程序。
        /// </summary>
        public virtual DbEntityProvider Provider
        {
            //TODO: 3.1.0开始此属性 由子类 去赋值
            get { return _provider; }//if (_provider == null)_provider = ProviderFactory.FromApplicationSettings();
            set { _provider = value; }
        }

        /// <summary>
        /// 在目标数据源创建指定名称的数据库。[貌似只支持SQLServer、MySql]
        /// </summary>
        /// <param name="dbName">数据库名称.</param> 
        /// <exception cref="System.NullReferenceException">Provider为Null</exception>
        /// <exception cref="System.Exception">数据库已存在</exception>
        /// <exception cref="System.Exception">无法连接到数据源</exception>
        /// <returns>创建成功:true,否则:false</returns>
        public bool CreateDataBase()
        {
            if (Provider == null)
                throw new NullReferenceException("Provider");
            var dbName = Provider.Connection.Database;
            string sql = string.Format("CREATE DATABASE {0} ;", dbName);
            try
            {
                int rtv = Provider.ExecuteCommand(sql);
                return true;
            }
            catch (System.Data.Common.DbException ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 删除当前连接的数据库中指定的表。
        /// </summary>
        /// <typeparam name="T">表映射名字</typeparam>
        /// <returns></returns>
        /// <exception cref="System.NullReferenceException">Provider 为空!</exception>
        public bool DeleteTable<T>() where T : class
        {
            return DeleteTable(typeof(T).Name);
        }
        /// <summary>
        /// 删除当前连接的数据库中指定的表。
        /// </summary>
        /// <param name="tableName">指定的表名。</param>
        /// <exception cref="System.NullReferenceException">Provider 为空!</exception>
        /// <returns></returns>
        public bool DeleteTable(string tableName)
        {
            if (this.Provider == null)
                throw new NullReferenceException("Provider 为空!");
            string sql = "DROP TABLE {0};".FormatWith(tableName);
            this.Provider.ExecuteCommand(sql);
            return true;
        }
        /// <summary>
        /// 在目标数据源中创建某个表。将使用当前<see cref="NkjSoft.ORM.QueryContext"/> 中 QueryProvider 提供的数据库连接字符串。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="System.Exception">NkjSoft.ORM.QueryContext.Provider 为空</exception>
        public bool CreateTable<T>()
        {
            var type = typeof(T);
            return CreateTable(type);
        }
        /// <summary>
        /// 通过映射类型信息在目标数据源创建一个表。
        /// </summary>
        /// <param name="entityType">映射类型.</param>
        /// <returns></returns>
        protected bool CreateTable(Type entityType)
        {
            if (Provider == null)
                throw new NullReferenceException("NkjSoft.ORM.QueryContext.Provider");
            //获取所有成员MemberInfo[] 
            //遍历..  
            var properties = entityType.GetProperties(System.Reflection.BindingFlags.Public
                | System.Reflection.BindingFlags.Instance);//; context.Provider.Mapping.GetMappedMembers(me);
            StringBuilder tableBuilder = new StringBuilder("CREATE TABLE {0}(\r\n");
            Data.Mapping.ColumnAttribute ca = null;
            foreach (var property in properties)
            {
                ca = property.GetCustomAttribute<Data.Mapping.ColumnAttribute>(false).FirstOrDefault();
                if (ca != null)
                    tableBuilder.AppendLine(Provider.Language.BuildeColumnInfo(property, ca));
            }
            if (tableBuilder.Length <= 10)
                return false;
            tableBuilder = tableBuilder.Remove(tableBuilder.ToString().LastIndexOf(","), 1);
            tableBuilder.Append(");");
            string tableInfo = tableBuilder.ToString().FormatWith(entityType.Name);

            try
            {
                // Console.WriteLine(tableInfo);
                Provider.ExecuteCommand(tableInfo);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Provider.Connection.Close(); }
        }



        //静态成员..
        /// <summary>
        /// 获取或设置当前查询上下文环境。
        /// </summary>
        /// <value>The current context.</value>
        public static QueryContext CurrentContext
        {
            get;
            set;
        }
    }
}
