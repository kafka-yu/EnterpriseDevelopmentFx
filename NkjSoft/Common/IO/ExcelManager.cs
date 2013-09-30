//--------------------------文档信息----------------------------
//       
//                 文件名: ExcelManager                 
//                 CLR Version: 4.0.30319.1
//                 项目命名空间: NkjSoft.Common.IO
//
//                 作  者: 俞如凯 
//                 Q Q: 250820436　 yurukai@vip.qq.com
//                 E-Mail: yurukai@hotmail.com
//                 创建时间 : 2010/8/29 18:40:35
//                 Copyright (c) Yurukai , All rights reserved.
//----------------------------------------------------------------


using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;

namespace NkjSoft.Common.IO
{
    /// <summary>
    /// 用于对 Excel 文档进行简单处理的类。
    /// </summary>
    public class ExcelDataManager
    {
        #region --- 构造函数 ---
        /// <summary>
        /// 初始化一个新的 <see cref="ExcelDataManager"/> 类实例。
        /// </summary>
        public ExcelDataManager()
        {
            OleDbProvider = "Provider=Microsoft.ACE.OLEDB.12.0";
        }

        /// <summary>
        /// 初始化一个已知 Excel 文件路径的 <see cref="ExcelDataManager"/> 类对象。
        /// </summary>
        /// <param name="excelFileName">Excel 文件(全)路径</param>
        public ExcelDataManager(string excelFileName)
            : this()
        {
            this.ExcelFilename = excelFileName;
        }
        #endregion

        #region --- 属性 ---
        /// <summary>
        /// 获取或设置用于传输 Excel 数据和 ADO.NET  DataSet 数据的提供程序名称。
        /// </summary>
        /// <remarks>
        /// 默认为:
        ///     Provider=Microsoft.ACE.OLEDB.12.0 ;
        ///  可选:
        ///     Provider=Microsoft.Jet.OLEDB.4.0 ;
        /// </remarks>
        public string OleDbProvider { get; set; }

        /// <summary>
        /// 获取当前使用的连接 Excel 文件的连接字符串。
        /// </summary>
        public string ConnectionString
        {
            get
            {
                return string.Format("{0};Data Source={1};Extended Properties='Excel 12.0;HDR=Yes;IMEX=1'",
                    OleDbProvider, ExcelFilename);
            }
        }

        /// <summary>
        /// 获取或设置管理器连接的 Excel 文档路径。
        /// </summary>
        public string ExcelFilename { get; set; }
        #endregion

        #region --- 字段 ---
        /// <summary>
        /// 用于查询数据的 T-SQL 语句。
        /// </summary>
        private readonly string selectSqlString = "SELECT * FROM [{0}$]";

        #endregion

        #region --- 方法 ---
        /// <summary>
        /// 将当前连接的 Excel 文件中指定的 sheets 导出到 <see cref="DataSet"/> 中。
        /// </summary>
        /// <param name="sheetNames">需要导出的 Sheet 名字集。默认 Sheet1 </param>
        /// <exception cref="System.Data.OleDb.OleDbException">一般性数据库错误，或者指定的 Sheet 名字不存在对应的数据。</exception>
        /// <returns></returns>
        public DataSet ToDataSet(params string[] sheetNames)
        {
            if (sheetNames == null || sheetNames.Length == 0)
                sheetNames = new string[] { "sheet1" };
            string ConnStr = ConnectionString;
            //Conn 
            System.Data.OleDb.OleDbConnection conn = new System.Data.OleDb.OleDbConnection(ConnStr);

            //new DataSet 
            DataSet myDataSet = new DataSet();
            //Command
            System.Data.OleDb.OleDbCommand cmd = new System.Data.OleDb.OleDbCommand();

            cmd.Connection = conn;
            try
            {
                foreach (var p in sheetNames)
                {
                    cmd.CommandText = string.Format(selectSqlString, p);
                    DataTable dt = new DataTable(p);
                    conn.Open();
                    dt.Load(cmd.ExecuteReader(CommandBehavior.CloseConnection));
                    myDataSet.Tables.Add(dt);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
                cmd.Dispose();
            }
            return myDataSet;
        }

        /// <summary>
        /// 返回 Excel 文件默认的第一个 Sheet 对应的 <see cref="DataTable"/>
        /// </summary>
        /// <returns></returns>
        public DataTable ToDataTable()
        {
            return ToDataSet("sheet1").Tables[0];
        }


        #endregion


    }
}
