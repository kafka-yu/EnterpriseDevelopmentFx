using NkjSoft.Framework.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace NkjSoft.DAL.Business
{
    public partial class ArticleComment : IdBasedEntityBase<Guid>
    {
        [ForeignKey("Article")]
        [Required]
        [Display(Name = "文章", Order = 2)]
        public Guid ArticleId { get; set; }

        /// <summary>
        /// 评论时间
        /// </summary>
        [Display(Name = "评论时间", Order = 3)]
        public DateTime PostAt { get; set; }

        //[ForeignKey("PostBy")]
        //[Display(Name = "评论人", Order = 4)]
        //public object UserId { get; set; }

        [Display(Name = "评论内容", Order = 5)]
        [Required(ErrorMessage = "不能为空")]
        public string Comments { get; set; }

        /// <summary>
        /// 获得的积分
        /// </summary>
        [Display(Name = "获得积分", Order = 6, Description = "可获得的积分")]
        public int Credits { get; set; }

        ///// <summary>
        ///// 评论人
        ///// </summary>
        //[ForeignKey("UserId")]
        //public SysPerson PostBy { get; set; }

        [ForeignKey("ArticleId")]
        public virtual Article Article { get; set; }
    }
}
