using NkjSoft.Core.Models.Account;
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
    /// 资讯评论
    /// </summary>
    public class AdvisoryComment : IdBasedEntityBase<Guid>
    {
        [ForeignKey("Advisory")]
        [Required]
        public Guid AdvisoryId { get; set; }

        /// <summary>
        /// 评论时间
        /// </summary>
        public DateTime PostAt { get; set; }

        [ForeignKey("PostBy")]
        public Guid UserId { get; set; }

        public string Comments { get; set; }

        /// <summary>
        /// 获得的积分
        /// </summary>
        public int Credits { get; set; }

        /// <summary>
        /// 回复评论的评论
        /// </summary>
        [ForeignKey("FeedbackTo")]
        public Guid FeedbackToId { get; set; }

        [ForeignKey("FeedbackToId")]
        public AdvisoryComment FeedbackTo { get; set; }

        /// <summary>
        /// 评论人
        /// </summary>
        [ForeignKey("UserId")]
        public Users PostBy { get; set; }

        [ForeignKey("AdvisoryId")]
        public Advisory Advisory { get; set; }
    }
}
