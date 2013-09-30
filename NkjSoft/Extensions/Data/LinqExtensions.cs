using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace NkjSoft.Extensions.Data
{ 
    //为 System.Data.Linq 的扩展
    namespace Linq
    {
        /// <summary>
        /// 为 <see cref="System.Data.Linq"/> 的一些扩展。
        /// </summary>
        public static class ExtensionForDataLinq
        {
            /// <summary>
            /// 将当前Linq To SQL 查询句法执行 并以 <see cref="System.Data.DataTable"/> 的形式返回数据集
            /// </summary>
            /// <param name="source"> 查询的 <see cref="System.Linq.IQueryable"/> 源</param>
            /// <param name="dataContext">
            /// 数据库DataContext上下文
            /// <para>提供数据库环境的上下文</para>
            /// </param> 
            /// <exception cref="System.Exception">未知异常</exception>
            /// <example>
            /// 例:  将 SQL 数据提取并以DataTabe显示
            /// <para>
            ///    <code>
            ///        NorthWindDataContext db=new NorthWindDataContext("连接字符串");
            ///        
            ///        var result= from  user in db.Users
            ///                        from role in db.Roles 
            ///                       where user.RoleID=role.ID
            ///                      select user ;
            ///         DataTable temp= result.ToDataTable(db);
            ///    </code>
            /// </para>
            /// </example>
            /// <returns>返回 <see cref="System.Data.DataTable"/> 结果。</returns>
            public static DataTable ToDataTable(this IQueryable source, System.Data.Linq.DataContext dataContext)
            {
                if (dataContext.Connection.State == ConnectionState.Closed)
                    dataContext.Connection.Open();

                DataTable result = new DataTable();

                try
                {
                    if (dataContext.Connection.State == ConnectionState.Closed)
                        dataContext.Connection.Open();
                    result.Load(dataContext.GetCommand(source).ExecuteReader());

                    dataContext.Connection.Close();
                    return result;

                }
                catch (Exception ex)
                { throw ex; }
                finally
                { dataContext.Connection.Close(); }
            }
        }
    }
}
