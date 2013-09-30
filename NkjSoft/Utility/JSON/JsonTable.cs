using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
namespace MTearSoft.Common
{
    /// <summary>
    /// 表示提供进行 JSON 和 .Net 类型的映射的能力.
    /// </summary> 
    //[DataContract]
    public partial class JsonTable
    {
        /// <summary>
        /// 获取或设置表类型(或者表名)
        /// </summary>
        //[DataMember]
        public string TableType { get; set; }
        /// <summary>
        /// 获取或设置表的行数据集合。
        /// </summary>
        //[DataMember]
        public Dictionary<int, List<JsonColumn>> Rows { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonTable"/> class.
        /// </summary>
        public JsonTable()
        {
            this.Rows = new Dictionary<int, List<JsonColumn>>();
        }
    }

    /// <summary>
    /// 表示 <see cref="Table"/> 中的列集合.
    /// </summary>
    //[DataContract]
    public class JsonColumn
    {
        /// <summary>
        /// 获取或设置列名
        /// </summary>
        //[DataMember]
        public string ColName { get; set; }
        //[DataMember]
        public int ColIndex { get; set; }
        /// <summary>
        /// 获取或设置列值
        /// </summary>
        //[DataMember]
        public string Value { get; set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="JsonColumn"/> class.
        /// </summary>
        public JsonColumn()
        {
            this.Value = string.Empty;
            this.ColName = string.Empty;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="colName"></param>
        /// <param name="val"></param>
        public JsonColumn(string colName, string val)
        {
            this.ColName = colName;
            this.Value = val;
        }
    }


    /// <summary>
    /// 对 Table 的扩展
    /// </summary>
    public static class TableExtension
    {
        //private static System.Runtime.Serialization.Json.DataContractJsonSerializer jsoner =
        //new System.Runtime.Serialization.Json.DataContractJsonSerializer((typeof(JsonTable)));

        ///// <summary>
        ///// 将当前 Table 对象转换成 客户端可以使用的Json数据字符串.
        ///// </summary>
        ///// <param name="source"></param>
        ///// <returns></returns>
        //public static string ToJsonData(this JsonTable source)
        //{
        //    Stream ms = new MemoryStream();
        //    jsoner.WriteObject(ms, source);
        //    StreamReader sr = new StreamReader(ms);
        //    ms.Position = 0;
        //    string s = sr.ReadToEnd();
        //    ms.Close();
        //    sr.Close();
        //    ms.Close();
        //    return s;
        //}


        /// <summary>
        /// 将 <see cref="System.Data.DataTable"/> 转换成自定义的 <see cref="JsonTable"/> 对象.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static JsonTable ToJsonTable(this DataTable source)
        {
            if (source == null || source.Rows.Count == 0)
            {
                return new JsonTable();
            }
            //得到所有列名
            int[] colsIndex = source.Columns.OfType<DataColumn>().Select(p => p.Ordinal).ToArray();
            string[] colsNames = source.Columns.OfType<DataColumn>().Select(p => p.ColumnName).ToArray();
            JsonTable result = new JsonTable();
            result.TableType = source.TableName;
            int count = 0;
            foreach (DataRow row in source.Rows)
            {
                List<JsonColumn> cols = new List<JsonColumn>();
                foreach (int index in colsIndex)
                {
                    cols.Add(new JsonColumn(colsNames[index], Convert.IsDBNull(row[index]) || row[index] == null ? string.Empty : row[index].ToString()));
                }
                result.Rows.Add(count, cols);
                count += 1;
            }
            return result;
        }

        ///// <summary>
        ///// 根据DataTable获得Json字符串
        ///// </summary>
        ///// <param name="_DataTable"></param>
        ///// <returns></returns>
        //public static string GetJsonDateTable(DataTable _DataTable)
        //{
        //    return ToJsonData(ToJsonTable(_DataTable));
        //}



        ///// <summary>
        ///// 将当前 <see cref="System.String"/> 通过 JSON 解析方式转换成 <see cref="Table"/> 对象.转换失败返回NULL
        ///// </summary>
        ///// <param name="jsonString">有效的JSON字符串</param>
        ///// <returns></returns>
        //public static JsonTable ParseToTable(this string jsonString)
        //{
        //    byte[] bytes = Encoding.UTF8.GetBytes(jsonString);
        //    Stream ms = new MemoryStream(bytes);
        //    var target = jsoner.ReadObject(ms) as JsonTable;
        //    ms.Close();

        //    return target;
        //}
    }
}