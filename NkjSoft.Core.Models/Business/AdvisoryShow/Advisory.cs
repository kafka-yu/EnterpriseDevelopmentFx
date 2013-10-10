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
    /// 资讯
    /// </summary>
    public partial class Advisory : IdBasedEntityBase<Guid>
    {
        /// <summary>
        /// 内容
        /// </summary>
        [MaxLength]
        public string Content { get; set; }

        /// <summary>
        /// 简介（200字内）
        /// </summary>
        [MaxLength(200)]
        public string BriefIntroduction { get; set; }

        /// <summary>
        /// 被浏览次数
        /// </summary>
        public int ViewCount { get; set; }

        /// <summary>
        /// 可用积分，浏览者浏览之后可以获得的积分数。
        /// </summary>
        public int AvailableCredits { get; set; }

        [ForeignKey("Category")]
        public Guid CategoryId { get; set; }

        [ForeignKey("Type")]
        public Guid TypeId { get; set; }

        [ForeignKey("OwnerOfThis")]
        public Guid UserId { get; set; }

        /// <summary>
        /// 发布者信息
        /// </summary>
        [ForeignKey("UserId")]
        public Account.Memberships OwnerOfThis { get; set; }

        /// <summary>
        /// 资讯类型：比如 来自培训机构，企业，用户，站长等
        /// </summary>
        [ForeignKey("TypeId")]
        public AdvisoryType Type { get; set; }

        /// <summary>
        /// 资讯类别：招聘，培训教程，等等。
        /// </summary>
        [ForeignKey("CategoryId")]
        public AdvisoryCategory Category { get; set; }

        /// <summary>
        /// 资讯评论列表
        /// </summary>
        public ICollection<AdvisoryComment> AdvisoryComments { get; set; }

        public Advisory()
        {
            this.AdvisoryComments = new HashSet<AdvisoryComment>();
            this.ViewCount = 0;
            this.AvailableCredits = 0;
        }
    }
}
