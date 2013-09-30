//--------------------------版权信息----------------------------
//       
//                 文件名: AccessDataBaseServer                 
//                 CLR Version: 2.0.50727.4927
//                 项目命名空间: NkjSoft.Tools.ModelBuilder.DataBaseImpl
//
//                 作  者: 俞如凯 
//                 Q Q: 250820436　 yurukai@vip.qq.com
//                 E-Mail: yurukai@hotmail.com 
//                 最后更新 : 2010/7/4 17:24:34
//                 Copyright (c) Yurukai , All rights reserved.
//----------------------------------------------------------------

using System.Collections.Generic;
using System.Data;
using System.Linq;
using NkjSoft.Extensions.Data;
namespace NkjSoft.Tools.ModelBuilder.DataBaseImpl
{
    using NkjSoft.Extensions;

    /// <summary>
    /// 实现了对Access 数据库服务的方法。无法继承此类。
    /// </summary>
    public sealed class AccessDataBaseServer : DataBaseServer
    {
        private System.Data.OleDb.OleDbConnectionStringBuilder connBuilder = new System.Data.OleDb.OleDbConnectionStringBuilder();
        /// <summary>
        /// 获取数据库列表
        /// </summary>
        /// <returns></returns>
        public override List<string> LoadDataBases()
        {
            connBuilder.ConnectionString = this.ConnectionString;
            string dbName = System.IO.Path.GetFileNameWithoutExtension(connBuilder.DataSource);
            return new List<string>() { dbName };

        }
        /// <summary>
        /// 通过有效的连接字符串实例化一个新的 <see cref="AccessDataBaseServer"/> 对象。
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public AccessDataBaseServer(string connectionString)
            : base(connectionString)
        {

        }
        /// <summary>
        /// 实例化一个新的 <see cref="AccessDataBaseServer"/> 对象。
        /// </summary>
        public AccessDataBaseServer()
            : this(string.Empty)
        {

        }


        /// <summary>
        /// 获取数据库表的完整信息
        /// </summary>
        /// <param name="tableName">数据库表名</param>
        /// <returns></returns>
        public override List<Column> LoadDataTableInfo(string tableName)
        {
            DataTable temp = DBUtility.AccessDBHelper.GetColumns(tableName);
            //System.Windows.Forms.Form f = new System.Windows.Forms.Form() { Width = 400, Height = 500 };
            //System.Windows.Forms.DataGridView g = new System.Windows.Forms.DataGridView();
            //g.DataSource = temp;

            //g.Dock = System.Windows.Forms.DockStyle.Fill;
            //f.Controls.Add(g);
            //f.ShowDialog(); 
            return temp.ToList<Column>(row =>
                new Column()
                {
                    TableName = tableName,
                    ColumnName = row[3].ToString(),
                    ColumnIndex = row[6].ToString().ToInt32(),
                    ByteLength = row[13].ToString().ToInt32(),
                    Length = row[13].ToString().ToInt32(0) == 0 ? (row[11].ToString() == "3" ? 4 : row[11].ToString() == "7" ? 8 : 255) : row[13].ToString().ToInt32(),
                    DataType = ExchangeDbType(row[11].ToString()),
                    DefaultValue = row[8].ToString().Replace("\"", string.Empty),
                    IsComputed = "false",
                    IsIdentity = (row[9].ToString().ToInt32() == 90 && row[10].ToString().ToBoolean() == false).ToString().ToLowerInvariant(),
                    Nullable = row[10].ToString().ToLowerInvariant(),
                    PrimaryKey = (row[9].ToString().ToInt32() == 90 && row[10].ToString().ToBoolean() == false).ToString().ToLowerInvariant(),
                    DotLength = 0,
                    Remark = row[27].ToString().IfEmptyReplace(row[3].ToString())
                }).OrderBy(p => p.ColumnIndex).ToList();
        }

        /// <summary>
        /// 获取Access数据库的所有表。
        /// </summary>
        /// <param name="dbName">Access数据库</param>
        /// <returns></returns>
        public override List<Table> LoadTables(string dbName)
        {
            return DBUtility.AccessDBHelper.GetAllTable().ToList<Table>(p => new Table() { Name = p[2].ToString() });
        }
        /// <summary>
        /// 获取或设置用于访问数据库的连接字符串.
        /// </summary>
        /// <value></value>
        public override string ConnectionString
        {
            get { return DBUtility.AccessDBHelper.connectionString; }
            set { DBUtility.AccessDBHelper.connectionString = value; }

        }

        /// <summary>
        /// Exchanges the type of the db.
        /// </summary>
        /// <param name="typeValue">The type value.</param>
        /// <returns></returns>
        private string ExchangeDbType(string typeValue)
        {
            string dbType = "varchar";
            switch (typeValue)
            {
                default:
                case "130":
                    break;
                case "3":
                    dbType = "int";
                    break;
                case "7":
                    dbType = "datetime";
                    break;
            }
            return dbType;
        }
        /// <summary>
        /// 表示指定数据库源的连接字符串模板。该字段是只读的。
        /// </summary>
        public static readonly string connectionStringTemplate = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=数据库文件地址;Persist Security Info=True;";
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
