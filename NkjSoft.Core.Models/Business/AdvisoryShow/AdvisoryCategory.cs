using NkjSoft.Framework.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace NkjSoft.Core.Models.Business.AdvisoryShow
{
    /// <summary>
    /// 资讯类别
    /// </summary>
    public class AdvisoryCategory : IdBasedEntityBase<Guid>
    {
        /// <summary>
        /// 名称
        /// </summary>
        [MaxLength(50)]
        public string Name { get; set; }

        [ForeignKey("Type")]
        public Guid TypeId { get; set; }
        
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 所属类别（培训机构，用户，企业）
        /// </summary>
        [ForeignKey("TypeId")]
        public AdvisoryType Type { get; set; }

        [ForeignKey("ParentNode")]
        public Nullable<Guid> ParentId { get; set; }

        [ForeignKey("ParentId")]
        public AdvisoryCategory ParentNode { get; set; }
    }


}
