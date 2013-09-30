using NkjSoft.ORM.Data.Common;
using System;

namespace NkjSoft.ORM.Core
{
    /// <summary>
    /// 
    /// </summary>
    public class ColumnIndeedExpression : DbExpression
    {
        private ColumnsIndeed fieldsToBeUpdated;

        /// <summary>
        /// 获取需要更新的列数组。
        /// </summary>
        /// <value>The fields to be updated.</value>
        public ColumnsIndeed FieldsToBeUpdated
        {
            get { return fieldsToBeUpdated; }
        }


        /// <summary>
        /// 初始化新的<see cref="ColumnIndeedExpression"/> 对象.
        /// </summary>
        /// <param name="fieldsToBeHandle">需要操作的列.</param>
        public ColumnIndeedExpression(ColumnsIndeed fieldsToBeHandle)
            : base(DbExpressionType.ColumnIndeed, typeof(void))
        {
            // TODO: Complete member initialization
            this.fieldsToBeUpdated = fieldsToBeHandle;
        }

    }

    /// <summary>
    /// 表示在 ORM 操作中，指定 Insert、Update 操作时自定义的列。
    /// </summary>
    public class ColumnsIndeed
    {
        /// <summary>
        /// 获取或设置操作的自定义列集合。
        /// </summary>
        public System.Collections.ObjectModel.ReadOnlyCollection<string> ColumnsToBeHandled { get; set; }

        /// <summary>
        /// 初始化新的 <see cref="ColumnsIndeed"/> 对象。
        /// </summary>
        public ColumnsIndeed()
        {

        }

        /// <summary>
        /// 指定需要操作的列名,初始化新的 <see cref="ColumnsIndeed"/> 对象。
        /// </summary>
        /// <param name="fieldsToBeHandled">需要操作的列名列表，请确保所有的列都在目标表的定义中</param>
        public ColumnsIndeed(params string[] fieldsToBeHandled)
        {
            if (fieldsToBeHandled != null && fieldsToBeHandled.Length > 0)
                this.ColumnsToBeHandled = fieldsToBeHandled.ToReadOnly();
        }
    }
}
