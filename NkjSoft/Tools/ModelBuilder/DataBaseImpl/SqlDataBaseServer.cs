//--------------------------版权信息----------------------------
//       
//                 文件名: SqlDataBaseServer                 
//                 CLR Version: 2.0.50727.4927
//                 项目命名空间: NkjSoft.Tools.ModelBuilder.DataBaseImpl
//
//                 作  者: 俞如凯 
//                 Q Q: 250820436　 yurukai@vip.qq.com
//                 E-Mail: yurukai@hotmail.com 
//                 最后更新 : 2010/7/3 9:57:21
//                 Copyright (c) Yurukai , All rights reserved.
//----------------------------------------------------------------

using System.Collections.Generic;
using System.Data;
using NkjSoft.Extensions.Data;

namespace NkjSoft.Tools.ModelBuilder.DataBaseImpl
{

    /// <summary>
    /// 实现了针对 MS SQL Server 上的数据库和表的交互的方法。无法继承此类。
    /// </summary>
    public sealed class SqlDataBaseServer : DataBaseServer
    {
        #region --- MyRegion ---
        /// <summary>
        /// 获取SQL Server数据库中某张表的所有字段完整信息的T-SQL语句。该字段是只读的。
        /// </summary>
        public static readonly string sqlExpression = @"select 
                        d.name  TableName,  
                        a.colorder ColumnIndex, 
                        a.name ColumnName, 
                        ( case when COLUMNPROPERTY (a.id,a.name,'isidentity') = 1 then 'true' else 'false' end ) IsIdentity, 
                        (case when  COLUMNPROPERTY ( a.id,a.name ,'IsComputed' )=1 then 'true' else 'false' end)as IsComputed,
                        ( case when ( 
                        select count(*) from sysobjects 
                        where name in ( 
                        select name from sysindexes where (id = a.id ) and ( indid in 
                        (select indid from sysindexkeys where 
                        ( id = a.id ) and ( colid in ( 
                        select colid from syscolumns 
                        where ( id = a.id ) and ( name = a.name )))))) 
                        and ( xtype ='PK')) > 0 then 'true' else 'false' end ) PrimaryKey, 
                        b.name DataType, 
                        a.length Length, COLUMNPROPERTY ( a.id,a.name ,'PRECISION' ) as ByteLength, 
                        isnull ( COLUMNPROPERTY ( a.id,a.name ,'Scale'),0) as DotLength,  
                        (case when a.isnullable = 1 then 'true' else 'false' end ) Nullable, 
                        isnull ( e.text,'Null') DefaultValue,(select p.[value] from sys.extended_properties p 
where p.[major_id]=d.[id] and p.[minor_id]=a.colorder and p.[name]='MS_Description') as Remark 
                        from syscolumns a left join systypes b 
                        on a.xtype = b.xusertype 
                        inner join sysobjects d 
                        on a.id = d.id and d.xtype='U' and d.name <> 'dtproperties' 
                        left join syscomments e 
                        on a.cdefault = e.id 
                        where d.name = ('{0}')
                        order by a.id ,a.colorder ";
        #endregion


        /// <summary>
        /// 获取用于获取MSSQLServer 上所有表名列表的T-SQL 语句。该字段是只读的。
        /// </summary>
        public static readonly string getTablesExpression = @"SELECT Name FROM {0}..SysObjects Where XType='U'";
        /// <summary>
        /// 获取用于获取MSSQLServer 上所有数据库名字列表的T-SQL 语句。该字段是只读的。
        /// </summary>
        public static readonly string getDataBaseExpression = @"select name from master..sysdatabases where dbid>4 order by name";

        /// <summary>
        ///  
        /// </summary>
        private static string connectionStringTemplate = "Data Source=.\\sqlexpress;Initial Catalog=;Integrated Security=SSPI;";

        /// <summary>
        /// 通过有效的MSSQL连接字符串实例化一个新的 <see cref="SqlDataBaseServer"/> 对象。
        /// </summary>
        /// <param name="connectionString">连接字符串.</param>
        public SqlDataBaseServer(string connectionString)
            : base(connectionString)
        {
            // TODO: Complete member initialization 
            DBUtility.SqlHelper.ConnectionString = connectionString;
        }

        /// <summary>
        /// 实例化一个新的 <see cref="SqlDataBaseServer"/> 对象。
        /// </summary>
        public SqlDataBaseServer()
            : this(string.Empty)
        {

        }

        /// <summary>
        /// 获取当前数据库中所有的表。
        /// </summary>
        /// <value>The tables.</value>
        public System.Collections.ObjectModel.ReadOnlyCollection<string> Tables { get; set; }


        /// <summary>
        /// 获取所有数据库名称列表。
        /// </summary>
        /// <returns></returns>
        public override List<string> LoadDataBases()
        {
            DataTable temp = new DataTable();
            temp.Load(DBUtility.SqlHelper.ExecuteReader(getDataBaseExpression, null));
            return temp.ToList<string>(row => row[0].ToString());

        }

        /// <summary>
        /// 根据表名获取数据库某张表的完整信息。
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns></returns>
        public override List<Column> LoadDataTableInfo(string tableName)
        {
            string sql = string.Format(sqlExpression, tableName);
            DataTable temp = new DataTable();
            temp.Load(DBUtility.SqlHelper.ExecuteReader(sql, null));
            return temp.ToList<Column>();
        }

        /// <summary>
        /// 获取表名列表
        /// </summary>
        /// <param name="dbName">数据库名</param>
        /// <returns></returns>
        public override List<Table> LoadTables(string dbName)
        {
            DBUtility.SqlHelper.ConnectionString = DBUtility.SqlHelper.BuildConnectionString(dbName);
            string sql = string.Format(getTablesExpression, dbName);

            DataTable temp = new DataTable();
            temp.Load(DBUtility.SqlHelper.ExecuteReader(sql, null));
            return temp.ToList<Table>(r => new Table() { Name = r[0].ToString() });
        }

        /// <summary>
        /// 获取或设置用于访问数据库的连接字符串.
        /// </summary>
        /// <value></value>
        public override string ConnectionString
        {
            get
            {
                return DBUtility.SqlHelper.ConnectionString;
            }
            set
            {
                DBUtility.SqlHelper.ConnectionString = value;
            }
        }


        /// <summary>
        /// 获取指定数据库源的连接字符串模板。
        /// </summary>
        /// <value></value>
        public override string ConnectionStringTemplate
        {
            get { return connectionStringTemplate; }
        }
    }
}
