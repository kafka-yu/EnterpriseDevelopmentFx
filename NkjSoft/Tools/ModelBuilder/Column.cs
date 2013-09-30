using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NkjSoft.Extensions;
using System.ComponentModel;

namespace NkjSoft.Tools.ModelBuilder
{
    /// <summary>
    /// 表示一个表中一列的结构信息。无法继承此类。
    /// </summary>
    public sealed class Column
    {
        /// <summary>
        /// 所在表名
        /// </summary>
        [Description("获取所在的“表”名称。")]
        [ReadOnly(true),
        DisplayName("所属表"),
        Category("数据库")]
        public string TableName { get; set; }

        /// <summary>
        /// 列索引
        /// </summary>
        [Description("获取成员所在表中的位置索引。")]
        [ReadOnly(true),
        DisplayName("列索引"),
        Category("数据库")]
        public int ColumnIndex { get; set; }

        /// <summary>
        /// 列名
        /// </summary> 
        [Description("获取成员名称。")]
        [ReadOnly(true),
        DisplayName("名称"),
        Category("数据库")]
        public string ColumnName { get; set; }

        /// <summary>
        /// 是否是自动增长列
        /// </summary>
        [Description("获取一个值，表示字段是否是自动增长列。")]
        [ReadOnly(true),
        DisplayName("自动增长"),
        Category("数据库")]
        public string IsIdentity { get; set; }

        /// <summary>
        /// 是否是计算列
        /// </summary>
        [Description("获取一个值，表示字段是否是表达式列。")]
        [ReadOnly(true),
        DisplayName("表达式列"),
        Category("数据库")]
        public string IsComputed { get; set; }
        /// <summary>
        /// 是否是主键
        /// </summary>
        [Description("获取一个值，表示字段是否是主键列。")]
        [ReadOnly(true),
        DisplayName("主键"),
        Category("数据库")]
        public string PrimaryKey { get; set; }

        /// <summary>
        /// 数据库类型信息
        /// </summary>
        [Description("获取字段的数据库类型。")]
        [ReadOnly(true),
        DisplayName("类型"),
        Category("数据库")]
        public string DataType { get; set; }

        /// <summary>
        /// 字段值的长度
        /// </summary>
        [Description("获取字段的长度。")]
        [ReadOnly(true),
        DisplayName("长度"),
        Category("数据库")]
        public int Length { get; set; }

        /// <summary>
        /// 字节数长度
        /// </summary>
        [Description("获取字段的字节长度。")]
        [ReadOnly(true),
        DisplayName("字节长度"),
        Category("数据库")]
        public int ByteLength { get; set; }

        /// <summary>
        /// 小数位数
        /// </summary>
        [Description("获取字段的小数位数。")]
        [ReadOnly(true),
        DisplayName("小数位数"),
        Category("数据库")]
        public int DotLength { get; set; }

        /// <summary>
        /// 是否可以为 DbNull
        /// </summary>
        [Description("获取一个值，表示字段是否允许 NULL 。")]
        [ReadOnly(true),
        DisplayName("可空"),
        Category("数据库")]
        public string Nullable { get; set; }

        /// <summary>
        /// 默认值
        /// </summary>
        [Description("获取或设置字段映射后的默认值 。")]
        [DisplayName("默认值"),
Category("映射")]
        public string DefaultValue { get; set; }

        /// <summary>
        /// 描述信息
        /// </summary>
        [Description("获取或设置字段映射后的描述信息 ，默认为字段名。")]
        [DisplayName("描述信息"),
Category("映射")]
        public string Remark { get; set; }

        /// <summary>
        /// 获取或设置是否参与 Insert or  Update 操作 默认 False
        /// </summary>
        [Description("获取或设置字段映射后的属性成员是否参与数据库的 Insert 、Update 操作 ，默认 TRUE。")]
        [DisplayName("执行行为"),
        DefaultValue(true),
Category("映射")]
        public bool InsertOrUpdatble { get; set; }

        private string[] notNeedLengthSytax = { "int", "datetime", "float", "decimal", "double", "timestamp" };

        public Column()
        {
            this.InsertOrUpdatble = true;
        }


        /// <summary>
        /// 返回字段类型 和长度信息。
        /// </summary>
        /// <returns></returns>
        public string BuildeLength()
        {
            if (notNeedLengthSytax.Contains(this.DataType.ToLowerInvariant()))
                return this.DataType;
            return string.Format("{0}({1})", this.DataType, this.Length.ToString());
        }


        /// <summary>
        /// 返回该字段是否参与 Insert Or Update 
        /// </summary>
        /// <returns></returns>
        public bool GetInsertOrUpdatble()
        {
            return
                //  this.IsComputed.Equals("false", StringComparison.InvariantCultureIgnoreCase) &&
                //this.IsIdentity.Equals("false", StringComparison.InvariantCultureIgnoreCase) &&
                //    this.PrimaryKey.Equals("false", StringComparison.InvariantCultureIgnoreCase) &&
                      !this.DataType.Equals("timestamp", StringComparison.InvariantCultureIgnoreCase);

        }


        /// <summary>
        /// Columns the mapping result.
        /// </summary>
        /// <returns></returns>
        public string ColumnMappingResult()
        {
            StringBuilder sb = new StringBuilder("[Column(");
            //if (this.Nullable.Equals("true", StringComparison.InvariantCultureIgnoreCase))
            //    sb.AppendFormat("");
            sb.AppendFormat("DbType = \"{0}\",", this.BuildeLength());
            sb.AppendFormat(" Length = {0},", this.Length.ToString());

            if (this.PrimaryKey.Equals("true", StringComparison.InvariantCultureIgnoreCase))
                sb.Append(" IsPrimaryKey = true ,");
            if (this.IsIdentity.Equals("true", StringComparison.InvariantCultureIgnoreCase))
                sb.Append(" IsIdentity = true ,");
            if (this.IsComputed.Equals("true", StringComparison.InvariantCultureIgnoreCase))
                sb.Append(" IsComputed= true,");
            if (!GetInsertOrUpdatble())
                sb.Append(" InsertOrUpdatable = false,");
            if (!this.DefaultValue.IsNullOrEmpty() && !this.DefaultValue.Equals("null", StringComparison.InvariantCultureIgnoreCase))
                sb.AppendFormat(" DefaultValue=\"{0}\",", this.DefaultValue);
            //new ORM.Data.Mapping.ColumnAttribute(){   
            sb.Remove(sb.Length - 1, 1).Append(")]");

            return sb.ToString();
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            var p = this;
            return @"
列索引:            {0}
字段名:            {1}
数据类型:         {2}
数据长度:         {3}
是否主键:         {4}
自动增长:         {5}
允许空值:         {6}
描述内容:         {7}
".FormatWith(p.ColumnIndex.ToString(),
    p.ColumnName, p.DataType, p.Length.ToString(),
    p.PrimaryKey.ToBoolean() ? "是" : "否",
    p.IsIdentity.ToBoolean() ? "是" : "否",
    p.Nullable.ToBoolean() ? "是" : "否",
    p.Remark);
        }
    }
}
