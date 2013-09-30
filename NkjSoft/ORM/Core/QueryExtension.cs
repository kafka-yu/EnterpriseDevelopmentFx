using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using NkjSoft.ORM.Core;
using NkjSoft.Extensions;
using NkjSoft.ORM.Data.Common;


namespace NkjSoft.ORM
{
    /// <summary>
    /// 对Query 的扩展 ..
    /// </summary>
    public static class QueryExtension
    {
        /// <summary>
        /// 将当前 Linq To SQL  查询表达式通过ADO.Net 执行，并返回 <see cref="System.Data.DataTable"/> 对象。
        /// </summary>
        /// <param name="query">当前 Linq To SQL  查询表达式</param>
        /// <returns></returns>
        public static DataTable ToDataTable(this IQueryable query)
        {
            if (query == null || string.IsNullOrEmpty(query.ToString()))
                throw new ArgumentNullException("query", "查询表达式空!");

            NkjSoft.ORM.Data.DbEntityProvider provider = ((query.Provider) as NkjSoft.ORM.Data.DbEntityProvider);
            System.Data.Common.DbConnection conn = provider.Connection;
            if (conn.State == ConnectionState.Broken || conn.State == ConnectionState.Closed)
                conn.Open();


            //UNDONE: 参数 没有获取到 ..~~2010-7.12
            string commandText = query.ToString();

            //QueryCommand qc = new QueryCommand(commandText, namedValues.Select(v => new QueryParameter(v.Name, v.Type, v.QueryType)));
            System.Data.Common.DbCommand cmd = conn.CreateCommand();
            cmd.CommandText = commandText;

            //cmd.Prepare();

            DataTable temp = new DataTable();
            temp.Load(cmd.ExecuteReader());
            cmd.Connection.Close();
            cmd.Dispose();
            provider.LogQuery(commandText);
            return temp;
        }

        /// <summary>
        /// 通过 ADO.Net 执行 T-SQL  查询表达式，并返回 <see cref="System.Data.DataTable"/> 对象。无需引用类型参数的查询.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="commandText">The command text.</param>
        /// <returns></returns>
        public static DataTable ExecuteDataTable(this QueryContext context, string commandText)
        {
            System.Data.Common.DbCommand cmd = ((context.Provider) as NkjSoft.ORM.Data.DbEntityProvider).Connection.CreateCommand();
            cmd.CommandText = commandText;
            cmd.Prepare();

            if (cmd.Connection.State == ConnectionState.Broken || cmd.Connection.State == ConnectionState.Closed)
                cmd.Connection.Open();

            DataTable temp = new DataTable();
            temp.Load(cmd.ExecuteReader());

            cmd.Connection.Close();
            cmd.Dispose();
            return temp;
        }

        /// <summary>
        /// 通过 ADO.Net 执行 T-SQL  查询表达式，并返回 <see cref="System.Data.DataTable"/> 对象,暂时需要指定 T-SQL查询 中非值类型的参数.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="args">The args.</param>
        /// <returns></returns>
        public static DataTable ToDataTable(this IQueryable query, params  object[] args)
        {
            if (query == null || string.IsNullOrEmpty(query.ToString()))
                throw new ArgumentNullException("query", "查询表达式空!");

            NkjSoft.ORM.Data.DbEntityProvider provider = ((query.Provider) as NkjSoft.ORM.Data.DbEntityProvider);
            System.Data.Common.DbConnection conn = provider.Connection;
            if (conn.State == ConnectionState.Broken || conn.State == ConnectionState.Closed)
                conn.Open();
            //UNDONE: 参数 没有获取到 ..~~2010-7.12
            //UNDONE: 暂时 手动添加 ...
            string commandText = query.ToString();

            //QueryCommand qc = new QueryCommand(commandText, namedValues.Select(v => new QueryParameter(v.Name, v.Type, v.QueryType)));
            System.Data.Common.DbCommand cmd = conn.CreateCommand();
            cmd.CommandText = commandText;
            
            if (args != null && args.Length > 0)
            { 
                int length = args.Length;
                for (int i = 0; i < length; i++)
                {
                    System.Data.Common.DbParameter dp = cmd.CreateParameter();
                    dp.ParameterName = "p{0}".FormatWith(i.ToString());
                    dp.Value = args[i];
                    string d = args[i].GetType().Name;
                    dp.DbType = d == "String" ? DbType.String : DbType.DateTime;// Only those two 
                    // query argument need do this AddParameter ...so . ..
                    cmd.Parameters.Add(dp);
                }
            }
            //cmd.Prepare();

            DataTable temp = new DataTable();
            temp.Load(cmd.ExecuteReader());
            cmd.Connection.Close();
            cmd.Dispose();
            provider.LogQuery(commandText);
            return temp;
        }
        /// <summary>
        /// 通过 ADO.Net 执行 T-SQL  查询表达式，并返回查询执行后影响的行数。
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="commandText">The command text.</param>
        /// <returns></returns>
        public static int ExecuteCommand(this QueryContext context, string commandText)
        {
            IDbConnection conn = context.Provider.Connection;
            int rtv = -1;
            using (IDbCommand cmder = conn.CreateCommand())
            {
                cmder.CommandText = commandText;
                cmder.CommandType = CommandType.Text;

                if (cmder.Connection.State == ConnectionState.Closed || cmder.Connection.State == ConnectionState.Broken)
                    cmder.Connection.Open();
                rtv = cmder.ExecuteNonQuery();
                context.Provider.LogQuery(commandText);
            }
            return rtv;
        }

        /// <summary>
        /// 执行存储过程，并返回受影响的行数。(只对有存储过程特性的数据库源有效)
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="procName">存储过程名字.</param>
        /// <param name="parameters">参数.</param>
        /// <returns></returns>
        public static int RunProcedure(this QueryContext context, string procName, params System.Data.Common.DbParameter[] parameters)
        {
            System.Data.Common.DbConnection conn = context.Provider.Connection;
            int rtv = -1;
            using (System.Data.Common.DbCommand cmder = conn.CreateCommand())
            {
                cmder.CommandText = procName;
                cmder.CommandType = CommandType.StoredProcedure;
                if (parameters != null && parameters.Count() > 0)
                    cmder.Parameters.AddRange(parameters);
                if (cmder.Connection.State == ConnectionState.Closed || cmder.Connection.State == ConnectionState.Broken)
                    cmder.Connection.Open();
                rtv = cmder.ExecuteNonQuery();
                //context.Provider.LogQuery(cmder.CommandText);
            }
            return rtv;
        }

        /// <summary>
        /// 执行存储过程，并返回查询结果集的 <see cref="System.Data.DataTable"/> 对象。(只对有存储过程特性的数据库源有效)
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="procName">存储过程名字</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        public static DataTable RunProcedureToTable(this  QueryContext context, string procName, params System.Data.Common.DbParameter[] parameters)
        {
            System.Data.Common.DbCommand cmd = context.Provider.Connection.CreateCommand();
            cmd.CommandText = procName;
            cmd.CommandType = CommandType.StoredProcedure;
            if (parameters != null && parameters.Count() > 0)
                cmd.Parameters.AddRange(parameters);
            cmd.Prepare();

            if (cmd.Connection.State == ConnectionState.Broken || cmd.Connection.State == ConnectionState.Closed)
                cmd.Connection.Open();

            DataTable temp = new DataTable();
            temp.Load(cmd.ExecuteReader());
            cmd.Connection.Close();
            cmd.Dispose();
            return temp;
        }

        #region --- DDL ---
        #endregion



        /// <summary>
        /// 向当前已经注册的日志愿输出查询命令。
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <param name="command">本次查询命令.</param>
        /// <exception cref="System.NullReferenceException">Provider 空!</exception>
        public static void LogQuery(this NkjSoft.ORM.Data.DbEntityProvider provider, string command)
        {
            if (provider == null)
                throw new NullReferenceException("Provider 空!");
            if (provider.Log != null)
                provider.Log.WriteLine(command);
        }
    }
}
