using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace NkjSoft.Framework.Common
{
    /// <summary>
    ///     可持久到数据库的领域模型的基类。
    /// </summary>
    [Serializable]
    public abstract class EntityBase<TKey>
    {
        #region 构造函数

        /// <summary>
        ///     数据实体基类
        /// </summary>
        protected EntityBase()
        {
            AddDate = DateTime.Now;
            StatusNum = "";
        }

        #endregion

        #region 属性

        [NotMapped]
        public abstract TKey __KeyId { get; }

        /// <summary>
        ///     获取或设置 添加时间
        /// </summary>
        [DataType(System.ComponentModel.DataAnnotations.DataType.DateTime)]
        [Display(Name = "添加时间", Order = 50)]
        public Nullable<DateTime> AddDate { get; set; }

        [Column("Status")]
        [Display(Name = "状态", Order = 60)]
        public object StatusNum { get; set; }

        ///// <summary>
        ///// 获取或设置 角色类型
        ///// </summary>
        //[NotMapped]
        //public RecordStatus Status
        //{
        //    get { return (RecordStatus)StatusNum; }
        //    set { StatusNum = (int)value; }
        //}
        #endregion
    }

    public enum RecordStatus
    {
        Passed = 1,
        Failed = 2,
        Freezed = 3,
        Deleted = 4,
        Disabled = 5,
        Enabled = 6,
    }

    /// <summary>
    ///     可持久到数据库的领域模型的基类。
    /// </summary>
    [Serializable]
    public abstract class IdBasedEntityBase<TKey>
        : EntityBase<TKey>
    {
        [Key, Display(Name = "主键", Order = 1)]
        public TKey Id { get; set; }

        public override TKey __KeyId
        {
            get { return Id; }
        }
    }
}
