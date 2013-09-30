//--------------------------版权信息----------------------------
//       
//                 文件名: DataExtensions                 
//                 CLR Version: 2.0.50727.4927
//                 项目命名空间: NkjSoft.Extensions.Data
//
//                 作  者: 俞如凯 
//                 Q Q: 250820436　 yurukai@vip.qq.com
//                 E-Mail: yurukai@hotmail.com
//                 创建时间 : 2010/4/23 09:21:09
//                 Copyright (c) Yurukai , All rights reserved.
//----------------------------------------------------------------


using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using MTearSoft.Common;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;

namespace NkjSoft.Extensions.Data
{
    /// <summary>
    /// 为 原 System.Data 命名空间的一些类型的扩展。
    /// </summary>
    public static class DataExtensions
    {
        #region --- For System.Data.DataTable ---

        #region --- 求值 ---

        /// <summary>
        /// 判断当前 <see cref="System.Data.DataTable"/> 是否是 Null 或者包含空行。
        /// </summary>
        /// <param name="table">当前 <see cref="System.Data.DataTable"/> </param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this DataTable table)
        {
            return table == null || table.Rows.Count == 0;
        }

        /// <summary>
        /// 获取指定行列的强类型的值。当指定的单元为Null 时，返回指定列字段 <typeparamref name="T"/>类型默认值。
        /// </summary>
        /// <typeparam name="T">字段的类型</typeparam>
        /// <param name="table">表</param>
        /// <param name="rowIndex">行索引</param>
        /// <param name="colIndex">列索引</param> 
        public static T FieldAt<T>(this DataTable table, int rowIndex, int colIndex)
        {
            if (table.IsNullOrEmpty())
                return default(T);
            if (table.Rows.Count < rowIndex || (table.Columns.Count < colIndex))
                return default(T); ;
            return table.Rows[rowIndex].Field<T>(colIndex);
        }

        /// <summary>
        /// 获取指定行、列的强类型的值。遇到空值时，返回指定的默认值。
        /// </summary>
        /// <typeparam name="T">字段的类型</typeparam>
        /// <param name="table">表</param>
        /// <param name="rowIndex">行索引</param>
        /// <param name="colIndex">列索引</param>
        /// <param name="defaultValue">当指定的单元为Null 时，返回这个值。</param>
        /// <returns></returns>
        public static T FieldAt<T>(this DataTable table, int rowIndex, int colIndex, T defaultValue)
        {
            if (table.IsNullOrEmpty())
                return defaultValue;
            if (table.Rows.Count < rowIndex || (table.Columns.Count < colIndex))
                return defaultValue;
            return table.Rows[rowIndex].Field<T>(colIndex);
        }


        /// <summary>
        /// 获取指定行列的 <see cref="System.String"/> 值。
        /// </summary>
        /// <param name="table">表</param>
        /// <param name="rowIndex">行所引</param>
        /// <param name="colIndex">列所引</param>
        /// <returns></returns>
        public static string FieldAt(this DataTable table, int rowIndex, int colIndex)
        {
            if (table.IsNullOrEmpty())
                return string.Empty;

            if (table.Rows.Count < rowIndex || (table.Columns.Count < colIndex))
                return string.Empty;
            object o = null;

            o = table.Rows[rowIndex][colIndex];

            return o == null ? string.Empty : o.ToString();
        }

        /// <summary>
        /// 获取指定行列的 <see cref="System.String"/> 值。
        /// </summary>
        /// <param name="table">表</param>
        /// <param name="rowIndex">行所引</param>
        /// <param name="colName">列名</param>
        /// <returns></returns>
        public static string FieldAt(this DataTable table, int rowIndex, string colName)
        {
            if (table.IsNullOrEmpty())
                return string.Empty;

            if (table.Rows.Count < rowIndex || (!table.Columns.Contains(colName)))
                return string.Empty;
            object o = null;

            o = table.Rows[rowIndex][colName];

            return o == null ? string.Empty : o.ToString();
        }
        #endregion

        /// <summary>
        /// 将  <see cref="System.Data.DataTable"/> 对象以指定的方式遍历对象的 Rows(<see cref="System.Data.DataRow"/>) 属性并返回强类型 <see cref="System.Collections.Generic.List&lt;T&gt;"/> 结果集。
        /// </summary>
        /// <typeparam name="T">目标对象类型。</typeparam>
        /// <param name="source">System.Data.DataTable 查询结果</param>
        /// <param name="resultSelector">指定的方式，用于给强类型对象结果集指定添加方式。</param>
        /// <example>
        ///     //用例:
        ///     DataTable dt= new DataTable();
        ///     dt.Columns.Add("ID",typeof(int));
        ///     dt.Columns.Add("LoginID",typeof(string));
        ///     
        ///     dt.Rows.Add(1,"admin");
        ///     dt.Rows.Add(2,"guest");
        ///     
        ///     //得到一个结构为: 包含 ID, LoginID 两个属性的匿名类型 的List 集合.
        ///     var tt1 = dt.ToList(p => new { ID = p[0], LoginID = p["LoginID"] });
        ///     
        ///     tt1.ForEach(p => Console.WriteLine(p.LoginID));
        ///     
        ///     //或者
        ///     var tt2 = dt.ToList(p=>p[0].ToString()); //得到 ID列的值的集合.
        /// </example>
        /// <returns></returns>
        public static List<T> ToList<T>(this DataTable source, Func<DataRow, T> resultSelector)
        {
            if (source == null || source.Rows.Count == 0)
                return new List<T>();
            int count = source.Rows.Count;


            List<T> result = new List<T>(count);
            if (resultSelector != null)
            {
                for (int i = 0; i < count; i++)
                {
                    result.Add(resultSelector(source.Rows[i]));
                }
            }
            return result;
        }


        /// <summary>
        /// 将  <see cref="System.Data.DataTable"/> 对象以指定的方式遍历对象的 Rows(<see cref="System.Data.DataRow"/>) 属性并返回类型 <typeparamref name="T"/> 的数组。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="resultSelector">指定的方式，用于给强类型对象结果集指定添加方式。</param>
        /// <returns></returns>
        public static T[] ToArray<T>(this DataTable source, Func<DataRow, T> resultSelector)
        {
            if (source == null || source.Rows.Count == 0)
                return default(T[]);
            int count = source.Rows.Count;


            T[] array = new T[count];
            if (resultSelector != null)
            {
                for (int i = 0; i < count; i++)
                {
                    array[i] = (resultSelector(source.Rows[i]));
                }
            }
            return array;
        }

        /// <summary>
        /// 返回  <see cref="System.Data.DataTable"/> 对象到强类型 <see cref="System.Collections.Generic.List&lt;T&gt;"/> <typeparamref name="T"/> 序列。
        /// </summary>
        /// <typeparam name="T">指定转换的强类型 </typeparam>
        /// <param name="source"><see cref="System.Data.DataTable"/> 对象，源。</param>
        /// <exception cref="System.NullReferenceException">源为Null</exception>
        /// <returns></returns>
        public static List<TResult> ToList<TResult>(this DataTable source)
        {
            List<TResult> list = new List<TResult>();
            if (source == null) return list;
            DataTableEntityBuilder<TResult> eblist = DataTableEntityBuilder<TResult>.CreateBuilder(source.Rows[0]);
            foreach (DataRow info in source.Rows)
            {
                list.Add(eblist.Build(info));
            }

            source.Dispose();
            source = null;
            return list;
        }

        /// <summary>
        /// 返回  <see cref="System.Data.DataTable"/> 对象到强类型 <see cref="System.Collections.Generic.List&lt;T&gt;"/> <typeparamref name="T"/> 序列。
        /// </summary>
        /// <typeparam name="T">指定转换的强类型 </typeparam>
        /// <param name="dr"><see cref="System.Data.DataTable"/> 对象，源。</param>
        /// <exception cref="System.NullReferenceException">源为Null</exception>
        /// <returns></returns>
        public static List<TResult> ToList<TResult>(this IDataReader dr, bool isClose = true)
        {
            IDataReaderEntityBuilder<TResult> eblist = IDataReaderEntityBuilder<TResult>.CreateBuilder(dr);
            List<TResult> list = new List<TResult>();
            if (dr == null) return list;
            while (dr.Read()) list.Add(eblist.Build(dr));

            if (isClose)
            {
                dr.Close(); dr.Dispose(); dr = null;
            }
            return list;
        }

        /// <summary>
        /// 返回 <see cref="System.Data.DataTable"/> 第一行第一列对应到 <typeparamref name="T"/> 类型的对象。如果当前 <see cref="System.Data.DataTable"/> 不包含数据据，则返回默认值。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public static T FirstOrDefault<T>(this DataTable source)
        {
            return source.ToList<T>().FirstOrDefault();
        }

        /// <summary>
        /// 通过指定的筛选方式返回 <see cref="System.Data.DataTable"/> 符合筛选条件的第一行第一列对应到 <typeparamref name="T"/> 类型的对象。如果当前 <see cref="System.Data.DataTable"/> 不包含数据据，则返回默认值。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="predicate">通过指定的筛选方式.</param>
        /// <returns></returns>
        public static T FirstOrDefault<T>(this DataTable source, Func<DataRow, bool> predicate)
        {
            return source.Where(predicate).ToList<T>().FirstOrDefault();
        }

        /// <summary>
        /// 返回通过指定方式获取的 <see cref="System.Data.DataTable"/> 第一行第一列对应到 <typeparamref name="T"/> 类型的对象。如果当前 <see cref="System.Data.DataTable"/> 不包含数据据，则返回默认值。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="resultSelector">指定 DataTable数据到 T 类型的转换列.</param>
        /// <returns></returns>
        public static T FirstOrDefault<T>(this DataTable source, Func<DataRow, T> resultSelector)
        {
            if (source.IsNullOrEmpty())
                return default(T);

            T temp = resultSelector(source.Rows[0]);

            return temp;
        }



        /// <summary>
        /// 对当前 <see cref="System.Data.DataTable"/> 对象数据进行分页，返回指定页起始索引，页大小的 <see cref="System.Data.DataTable"/> 副本。
        /// </summary>
        /// <param name="source">数据源</param>
        /// <param name="pageIndex">页起始索引</param>
        /// <param name="pageSize">每页记录数.</param>
        /// <param name="totalRecords">总共记录数</param>
        /// <returns></returns>
        public static DataTable ToPage(this DataTable source, int pageIndex, int pageSize, out int totalRecords)
        {
            if (source.IsNullOrEmpty())
            {
                totalRecords = 0;
                return null;
            }
            totalRecords = source.Rows.Count;
            int startRow = (pageIndex - 1) * pageSize;
            int endRow = startRow + pageSize;
            if (startRow > totalRecords || startRow < 0)
            {
                startRow = 0; endRow = pageSize;
            }
            if (endRow > totalRecords + pageSize)
            {
                startRow = totalRecords - pageSize; endRow = totalRecords;
            }
            DataTable dt2 = source.Clone();
            for (int i = startRow; i < endRow; i++)
            {
                if (i >= totalRecords) break;
                dt2.Rows.Add(source.Rows[i].ItemArray);
            }
            return dt2;
        }

        /// <summary>
        /// 对当前 <see cref="System.Data.DataTable"/> 对象数据进行分页，返回 pageSize 数内数量的 <typeparamref name="T"/> 类型序列。当前不包含数据则返回 NULL。
        /// </summary>
        /// <typeparam name="T">返回的<typeparamref name="T"/> 类型</typeparam>
        /// <param name="source">数据源</param>
        /// <param name="pageIndex">页起始索引.</param>
        /// <param name="pageSize">每页记录数.</param>
        /// <param name="totalRecords">总共记录数</param>
        /// <returns></returns>
        public static List<T> ToPage<T>(this DataTable source, int pageIndex, int pageSize, out int totalRecords)
        {
            return source.ToPage(pageIndex, pageSize, out totalRecords).ToList<T>();
        }

        /// <summary>
        /// 对当前 <see cref="System.Data.DataTable"/> 数据集进行排序并返回指定的强类型序列。
        /// </summary>
        /// <param name="source"><see cref="System.Data.DataTable"/> 源.</param>
        /// <param name="orderByExpression">针对数据的排序语句。</param>
        /// <returns></returns>
        public static List<T> OrderBy<T>(this DataTable source, string orderByExpression)
        {
            return source.OrderBy(orderByExpression).ToList<T>();
        }

        /// <summary>
        /// 返回经过指定排序语句排序之后的当前 <see cref="System.Data.DataTable"/> 数据集副本。
        /// </summary>
        /// <param name="source"><see cref="System.Data.DataTable"/> 源.</param>
        /// <param name="orderByExpression">针对数据的排序语句。</param>
        /// <returns></returns>
        public static DataTable OrderBy(this DataTable source, string orderByExpression)
        {
            if (source == null)
                return null;
            source.DefaultView.Sort = orderByExpression;
            return source.DefaultView.ToTable();
        }

        /// <summary>
        /// 返回当前 <see cref="System.Data.DataTable"/> 指定需要的列的副本。
        /// </summary>
        /// <param name="source"><see cref="System.Data.DataTable"/> 源.</param>
        /// <param name="columnsToReturn">需要返回的并包含在当前 <see cref="System.Data.DataTable"/> 中的列。</param>
        /// <returns></returns>
        public static DataTable Select(this DataTable source, params string[] columnsToReturn)
        {
            if (source.IsNullOrEmpty())
                return null;
            return source.DefaultView.ToTable(false, columnsToReturn);
        }


        /// <summary>
        /// 遍历当前 <see cref="System.Data.DataTable"/> 的所有行。
        /// </summary>
        /// <param name="source">当前 <see cref="System.Data.DataTable"/>对象 。</param>
        /// <param name="repeatter">指定遍历的操作。</param>
        public static void ForEach(this DataTable source, Action<DataRow> repeatter)
        {
            if (source == null || source.Rows.Count == 0)
                return;//throw new NullReferenceException("source");

            if (repeatter != null)
            {
                foreach (DataRow item in source.Rows)
                {
                    repeatter(item);
                }
            }
        }


        /// <summary>
        /// 对 <see cref="System.Data.DataTable"/> 对象的Rows进行按指定谓词筛选的操作,并返回筛选结果的新的一个 <see cref="System.Data.DataTable"/> 实例。
        /// </summary>
        /// <param name="table">当前 <see cref="System.Data.DataTable"/> 对象。</param>
        /// <param name="match">对行自定义的筛选条件</param>
        /// <returns></returns>
        public static DataTable Where(this DataTable table, Func<DataRow, bool> match)
        {
            if (table == null || table.Rows.Count == 0)
            {
                return null;
            }
            var result = table.AsEnumerable().Where(match);

            if (result != null && table.Rows.Count > 0)
            {
                DataTable temp;
                try
                {
                    temp = result.CopyToDataTable();
                }
                catch (System.InvalidOperationException)
                {
                    temp = null;
                }
                return temp;
            }
            return null;
        }

        /// <summary>
        /// 对 <see cref="System.Data.DataTable"/> 对象的Rows进行按指定筛选语句筛选,并返回筛选结果的新的一个 <see cref="System.Data.DataTable"/> 实例。
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="rowFilter">结果选择语句(T-SQL)</param>
        /// <returns></returns>
        public static DataTable Where(this DataTable table, string rowFilter)
        {
            if (table.IsNullOrEmpty())
                return null;
            table.DefaultView.RowFilter = rowFilter;
            return table.DefaultView.ToTable();
        }


        /// <summary>
        /// 将当前 <see cref="System.Data.DataTable"/> 转换成指定列的新 <see cref="System.Data.DataTable"/>.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="columnBuilder">The column builder.</param>
        /// <param name="rowBuilder">The row builder.</param>
        /// <returns></returns>
        public static DataTable AsNewTable(this DataTable table, Func<DataColumn[]> columnBuilder, Func<DataRow, DataRow> rowBuilder)
        {
            DataTable newTable = new DataTable();
            if (columnBuilder != null)
                newTable.Columns.AddRange(columnBuilder());
            if (rowBuilder != null)
                foreach (DataRow row in table.Rows)
                {
                    newTable.Rows.Add(rowBuilder(row));
                }
            return newTable;
        }

        /// <summary>
        /// 将当前 <see cref="System.Data.DataTable"/> 转换成指定列的新 <see cref="System.Data.DataTable"/>.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="columnBuilder">The column builder.</param>
        /// <param name="rowBuilder">The row builder.</param>
        /// <returns></returns>
        public static DataTable AsNewTable(this DataTable table, Func<DataColumn[]> columnBuilder, Func<DataRow, object[]> rowBuilder)
        {
            DataTable newTable = new DataTable();
            if (columnBuilder != null)
                newTable.Columns.AddRange(columnBuilder());
            if (rowBuilder != null)
                foreach (DataRow row in table.Rows)
                {
                    newTable.Rows.Add(rowBuilder);
                }
            return newTable;
        }
        #endregion

        #region --- For DataSet ---

        /// <summary>
        /// 获取指定  <see cref="System.Data.DataSet"/> 对象的 Tables 属性索引的对象，如果目标为空,返回 Null。
        /// </summary>
        /// <param name="ds">DataSet 源</param>
        /// <param name="tableIndex">表索引</param>
        /// <returns></returns>
        public static DataTable GetTable(this DataSet ds, int tableIndex)
        {
            return ds == null || ds.Tables.Count == 0 ? null : ds.Tables[tableIndex];
        }

        /// <summary>
        /// 获取指定 <see cref="System.Data.DataSet"/> 对象的 Tables 属性索引的对象，如果目标为空,返回Null
        /// </summary>
        /// <param name="ds">DataSet 源</param>
        /// <param name="tableName">表名称</param>
        /// <returns></returns>
        public static DataTable GetTable(this DataSet ds, string tableName)
        {
            return ds == null || ds.Tables.Count == 0 ? null : ds.Tables[tableName];
        }

        /// <summary>
        /// 通过一个自定义筛选方法选择 DataSet 集中需要的第一个 DataTable 对象。
        /// </summary>
        /// <param name="source">DataSet 集</param>
        /// <param name="match">一个自定义筛选方法</param>
        /// <returns></returns>
        public static DataTable Where(this DataSet source, Func<DataTable, bool> match)
        {
            if (source == null || source.Tables.Count == 0)
                return null;

            return source.Tables.OfType<DataTable>().Where(match).FirstOrDefault();
        }
        #endregion

        /// <summary>
        /// 返回当前 DataTable 转换到T-SQL 的 Update 语句。
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <returns></returns>
        public static string BuildUpdateSyntax(List<JsonColumn> data, string tableName)
        {
            StringBuilder sb = new StringBuilder("UPDATE {0} SET ".FormatWith(tableName));
            data.ForEach(p =>
            {
                sb.AppendFormat("{0}='{1}' ,", p.ColName, p.Value);
            });
            sb.Remove(sb.Length - 2, 2);
            return sb.ToString();
        }

        /// <summary>
        /// 返回当前 <see cref="DataTable"/> 转换到T-SQL 的 Insert 语句。
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <returns></returns>
        public static string BuildInsertSyntax(List<JsonColumn> data, string tableName)
        {
            var cols = data.Select(p => p.ColName).ToList();

            StringBuilder sb = new StringBuilder("INSERT INTO {0}  ({1}) VALUES (".FormatWith(tableName, cols.ToStringLine(",")));

            sb.Append(data.Select(p => p.Value).ToStringLine("'", "'", ","));
            return sb.ToString();
        }
    }

    public class DataTableEntityBuilder<Entity>
    {
        private static readonly MethodInfo getValueMethod = typeof(DataRow).GetMethod("get_Item", new Type[] { typeof(int) });
        private static readonly MethodInfo isDBNullMethod = typeof(DataRow).GetMethod("IsNull", new Type[] { typeof(int) });
        private delegate Entity Load(DataRow dataRecord);

        private Load handler;
        private DataTableEntityBuilder() { }

        public Entity Build(DataRow dataRecord)
        {
            return handler(dataRecord);
        }

        /// <summary>
        /// Creates the builder.
        /// </summary>
        /// <param name="dataRecord">The data record.</param>
        /// <returns></returns>
        public static DataTableEntityBuilder<Entity> CreateBuilder(DataRow dataRecord)
        {
            DataTableEntityBuilder<Entity> dynamicBuilder = new DataTableEntityBuilder<Entity>();
            DynamicMethod method = new DynamicMethod("DynamicCreateEntity", typeof(Entity), new Type[] { typeof(DataRow) }, typeof(Entity), true);
            ILGenerator generator = method.GetILGenerator();
            LocalBuilder result = generator.DeclareLocal(typeof(Entity));
            generator.Emit(OpCodes.Newobj, typeof(Entity).GetConstructor(Type.EmptyTypes));
            generator.Emit(OpCodes.Stloc, result);

            for (int i = 0; i < dataRecord.ItemArray.Length; i++)
            {
                PropertyInfo propertyInfo = typeof(Entity).GetProperty(dataRecord.Table.Columns[i].ColumnName);
                Label endIfLabel = generator.DefineLabel();
                if (propertyInfo != null && propertyInfo.GetSetMethod() != null)
                {
                    generator.Emit(OpCodes.Ldarg_0);
                    generator.Emit(OpCodes.Ldc_I4, i);
                    generator.Emit(OpCodes.Callvirt, isDBNullMethod);
                    generator.Emit(OpCodes.Brtrue, endIfLabel);
                    generator.Emit(OpCodes.Ldloc, result);
                    generator.Emit(OpCodes.Ldarg_0);
                    generator.Emit(OpCodes.Ldc_I4, i);
                    generator.Emit(OpCodes.Callvirt, getValueMethod);
                    generator.Emit(OpCodes.Unbox_Any, propertyInfo.PropertyType);
                    generator.Emit(OpCodes.Callvirt, propertyInfo.GetSetMethod());
                    generator.MarkLabel(endIfLabel);
                }
            }
            generator.Emit(OpCodes.Ldloc, result);
            generator.Emit(OpCodes.Ret);
            dynamicBuilder.handler = (Load)method.CreateDelegate(typeof(Load));
            return dynamicBuilder;
        }
    }

    public class IDataReaderEntityBuilder<Entity>
    {
        private static readonly MethodInfo getValueMethod =
        typeof(IDataRecord).GetMethod("get_Item", new Type[] { typeof(int) });
        private static readonly MethodInfo isDBNullMethod =
            typeof(IDataRecord).GetMethod("IsDBNull", new Type[] { typeof(int) });
        private delegate Entity Load(IDataRecord dataRecord);

        private Load handler;
        private IDataReaderEntityBuilder() { }
        public Entity Build(IDataRecord dataRecord)
        {
            return handler(dataRecord);
        }
        public static IDataReaderEntityBuilder<Entity> CreateBuilder(IDataRecord dataRecord)
        {
            IDataReaderEntityBuilder<Entity> dynamicBuilder = new IDataReaderEntityBuilder<Entity>();
            DynamicMethod method = new DynamicMethod("DynamicCreateEntity", typeof(Entity),
                    new Type[] { typeof(IDataRecord) }, typeof(Entity), true);
            ILGenerator generator = method.GetILGenerator();
            LocalBuilder result = generator.DeclareLocal(typeof(Entity));
            generator.Emit(OpCodes.Newobj, typeof(Entity).GetConstructor(Type.EmptyTypes));
            generator.Emit(OpCodes.Stloc, result);
            for (int i = 0; i < dataRecord.FieldCount; i++)
            {
                PropertyInfo propertyInfo = typeof(Entity).GetProperty(dataRecord.GetName(i));
                Label endIfLabel = generator.DefineLabel();
                if (propertyInfo != null && propertyInfo.GetSetMethod() != null)
                {
                    generator.Emit(OpCodes.Ldarg_0);
                    generator.Emit(OpCodes.Ldc_I4, i);
                    generator.Emit(OpCodes.Callvirt, isDBNullMethod);
                    generator.Emit(OpCodes.Brtrue, endIfLabel);
                    generator.Emit(OpCodes.Ldloc, result);
                    generator.Emit(OpCodes.Ldarg_0);
                    generator.Emit(OpCodes.Ldc_I4, i);
                    generator.Emit(OpCodes.Callvirt, getValueMethod);
                    generator.Emit(OpCodes.Unbox_Any, dataRecord.GetFieldType(i));
                    generator.Emit(OpCodes.Callvirt, propertyInfo.GetSetMethod());
                    generator.MarkLabel(endIfLabel);
                }
            }
            generator.Emit(OpCodes.Ldloc, result);
            generator.Emit(OpCodes.Ret);
            dynamicBuilder.handler = (Load)method.CreateDelegate(typeof(Load));
            return dynamicBuilder;
        }
    }
}
