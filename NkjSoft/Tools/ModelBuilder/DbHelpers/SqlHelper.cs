using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace NkjSoft.Tools.DBUtility
{
    /// <summary>
    /// 数据库访问组件。
    /// <para>
    ///     调用方式 ：  
    ///   SqlHelper.ExecuteNonQuery("select * from 数据库表名 ;",CommandType.Text,参数,如果无则为null);
    /// </para>
    /// </summary>
    public sealed class SqlHelper
    {
        /// <summary>
        /// 获取数据库连接字符串
        /// </summary>
        public static string ConnectionString = null;//System.Configuration.ConfigurationManager.ConnectionStrings["SqlConnectionString"].ConnectionString;

        /// <summary>
        /// 
        /// </summary>
        private static SqlConnection commonConn = null;

        public static bool IsOpen
        {
            get
            {
                if (commonConn == null)
                    commonConn = new SqlConnection(ConnectionString);
                Open();
                return commonConn.State == ConnectionState.Open;
            }
        }

        private static void Open()
        {
            commonConn.Open();
        }



        /// <summary>
        /// 返回单个表的查询.
        /// </summary>
        /// <param name="sqlExpressionOrSp_Name">sql语句或者存储过程名字</param>
        /// <param name="cmdType">命令类型：SQL语句，或者存储过程</param>
        /// <param name="parames">参数集合，可空</param>
        /// <returns>单个查询结果集</returns>
        public static DataTable ToDataTable(string sqlExpressionOrSp_Name, CommandType cmdType, params System.Data.SqlClient.SqlParameter[] parames)
        {
            DataTable result = null;
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sqlExpressionOrSp_Name, conn))
                {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();
                    cmd.CommandType = cmdType;

                    if (parames != null && parames.Length != 0)
                        cmd.Parameters.AddRange(parames);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        result = new DataTable();

                        result.Load(reader);
                    }
                }

            }
            return result;
        }



        /// <summary>
        /// 执行查询，返回查询受影响的行数.
        /// </summary>
        /// <param name="sqlExpressionOrSp_Name">sql语句或者存储过程名字</param>
        /// <param name="cmdType">命令类型：SQL语句，或者存储过程</param>
        /// <param name="parames">参数集合，可空</param>
        /// <returns></returns>
        public static int ExecuteNonQuery(string sqlExpressionOrSp_Name, CommandType cmdType, params System.Data.SqlClient.SqlParameter[] parames)
        {
            int result = -1;
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sqlExpressionOrSp_Name, conn))
                {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();
                    cmd.CommandType = cmdType;

                    if (parames != null && parames.Length != 0)
                        cmd.Parameters.AddRange(parames);
                    //SqlParameter d = new SqlParameter("@ReturnValue", "");
                    //d.Direction = ParameterDirection.ReturnValue;
                    //cmd.Parameters.Add(d);
                    result = cmd.ExecuteNonQuery();
                }

            }
            return result;
        }


        /// <summary>
        /// 执行查询，返回自动增长的值。
        /// </summary>
        /// <param name="sqlExpressionOrSp_Name">sql语句或者存储过程名字</param>
        /// <param name="cmdType">命令类型：SQL语句，或者存储过程</param>
        ///<param name="identity"></param>
        /// <param name="parames">参数集合，可空</param>
        /// <returns></returns>
        public static int ExecuteNonQuery(string sqlExpressionOrSp_Name, CommandType cmdType, out int identity, params System.Data.SqlClient.SqlParameter[] parames)
        {
            int result = -1;
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sqlExpressionOrSp_Name, conn))
                {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();
                    cmd.CommandType = cmdType;

                    if (parames != null && parames.Length != 0)
                        cmd.Parameters.AddRange(parames);
                    SqlParameter d = new SqlParameter("@ReturnValue", "");
                    d.Direction = ParameterDirection.ReturnValue;
                    cmd.Parameters.Add(d);
                    result = cmd.ExecuteNonQuery();

                    identity = d.Value == null ? -1 : Convert.ToInt32(d.Value.ToString());
                }

            }
            return result;
        }


        /// <summary>
        /// 执行查询，返回单个值的强类型结果.
        /// </summary>
        /// <typeparam name="TResult">返回值类型</typeparam>
        /// <param name="sqlExpressionOrSp_Name">sql语句或者存储过程名字</param>
        /// <param name="cmdType">命令类型：SQL语句，或者存储过程</param>
        /// <param name="parames">参数集合，可空</param>
        /// <returns>返回值</returns>
        public static object ExecuteScalar(string sqlExpressionOrSp_Name, CommandType cmdType, params System.Data.SqlClient.SqlParameter[] parames)
        {
            object result = null;
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sqlExpressionOrSp_Name, conn))
                {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();
                    cmd.CommandType = cmdType;

                    if (parames != null && parames.Length != 0)
                        cmd.Parameters.AddRange(parames);
                    result = cmd.ExecuteScalar();
                }
            }
            return result;
        }


        public static SqlDataReader ExecuteReader(string sql, params SqlParameter[] Parameters)
        {
            SqlConnection conn = new SqlConnection(ConnectionString);
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                if (Parameters != null && Parameters.Length != 0)
                    cmd.Parameters.AddRange(Parameters);
                return cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
        }
        private static SqlConnectionStringBuilder builder;
        /// <summary>
        /// 返回当前连接字符串连接到的数据库的名字。
        /// </summary>
        /// <param name="dbName">Name of the db.</param>
        /// <returns></returns>
        public static string BuildConnectionString(string dbName)
        {
            builder = new SqlConnectionStringBuilder(ConnectionString);

            builder.InitialCatalog = dbName;
            return builder.ToString();
        }
    }
}
