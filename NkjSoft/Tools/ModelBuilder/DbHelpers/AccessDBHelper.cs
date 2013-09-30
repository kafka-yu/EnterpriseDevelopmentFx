using System;
using System.Collections.Generic;
using System.Web;
using System.Data.OleDb;
using System.Collections;
using System.Data;

namespace NkjSoft.Tools.DBUtility
{
    /// <summary>
    /// Access 数据库 DBHelper
    /// </summary>
    public class AccessDBHelper
    {
        //
        /// <summary>
        /// 数据库连接字符串(web.config来配置)，可以动态更改connectionString支持多数据库.		
        /// </summary>
        public static string connectionString = "";
        /// <summary>
        /// Initializes a new instance of the <see cref="AccessDBHelper"/> class.
        /// </summary>
        public AccessDBHelper()
        {
        }

        #region  执行简单SQL语句
        /// <summary>
        /// 执行查询语句，返回OleDbDataReader
        /// </summary>
        /// <param name="strSQL">查询语句</param>
        /// <returns>OleDbDataReader</returns>
        public static OleDbDataReader ExecuteReader(string strSQL)
        {
            OleDbConnection connection = new OleDbConnection(connectionString);
            OleDbCommand cmd = new OleDbCommand(strSQL, connection);
            try
            {
                connection.Open();
                OleDbDataReader myReader = cmd.ExecuteReader();
                return myReader;
            }
            catch (System.Data.OleDb.OleDbException e)
            {
                throw new Exception(e.Message);
            }

        }

        #endregion

        #region 执行带参数的SQL语句

        /// <summary>
        /// 执行查询语句，返回OleDbDataReader
        /// </summary>
        /// <param name="strSQL">查询语句</param>
        /// <returns>OleDbDataReader</returns>
        public static OleDbDataReader ExecuteReader(string SQLString, params OleDbParameter[] cmdParms)
        {
            OleDbConnection connection = new OleDbConnection(connectionString);
            OleDbCommand cmd = new OleDbCommand();
            try
            {
                PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                OleDbDataReader myReader = cmd.ExecuteReader();
                cmd.Parameters.Clear();
                return myReader;
            }
            catch (System.Data.OleDb.OleDbException e)
            {
                throw new Exception(e.Message);
            }

        }



        private static void PrepareCommand(OleDbCommand cmd, OleDbConnection conn, OleDbTransaction trans, string cmdText, OleDbParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            if (trans != null)
                cmd.Transaction = trans;
            cmd.CommandType = CommandType.Text;//cmdType;
            if (cmdParms != null)
            {
                foreach (OleDbParameter parm in cmdParms)
                    cmd.Parameters.Add(parm);
            }
        }

        #endregion

        /// <summary>
        /// 获取当前连接的 Access 数据库中所有的表列表。
        /// </summary>
        /// <returns></returns>
        public static DataTable GetAllTable()
        {
            DataTable temp = null;
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                conn.Open();
                temp = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "Table" });
                conn.Close();
                conn.Dispose();
            }
            return temp;
        }


        /// <summary>
        /// 获取指定表的所有列的完整架构信息。
        /// </summary>
        /// <param name="tableName">指定表.</param>
        /// <returns></returns>
        public static DataTable GetColumns(string tableName)
        {
            DataTable temp = new DataTable();
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                conn.Open();
                temp = conn.GetSchema("Columns", new string[] { null, null, tableName, null });//OleDbSchemaGuid.Columns
                //temp = new OleDbCommand(string.Format("Select * from {0}", tableName), conn).ExecuteReader().GetSchemaTable(); // conn.(OleDbSchemaGuid.Columns, new object[] { null, null, tableName, null });
                conn.Close();
                conn.Dispose();
            }
            return temp;
        }
    }
}
