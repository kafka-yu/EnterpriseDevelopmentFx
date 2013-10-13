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
    /// <summary>
    /// 资讯
    /// </summary>
    public partial class Article : IdBasedEntityBase<Guid>
    {
        [Required(ErrorMessage = "不能为空")]
        [Display(Name = "标题", Order = 1)]
        public string Title { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        [Display(Name = "内容", Order = 3)]
        [Required(ErrorMessage = "内容不能为空")]
        public string Content { get; set; }

        /// <summary>
        /// 简介（200字内）
        /// </summary>
        [Display(Name = "简介", Order = 2)]
        [MaxLength(200, ErrorMessage = "长度必须小于等于200")]
        public string BriefIntroduction { get; set; }

        /// <summary>
        /// 被浏览次数
        /// </summary>
        [Display(Name = "浏览次数", Order = 4)]
        public int ViewCount { get; set; }

        /// <summary>
        /// 可用积分，浏览者浏览之后可以获得的积分数。
        /// </summary>
        [Display(Name = "可用积分", Description = "浏览者浏览之后可以获得的积分数。", Order = 5)]
        public int AvailableCredits { get; set; }

        [ForeignKey("Category")]
        [Display(Name = "类别", Order = 6)]
        [Required(ErrorMessage = "类别不能为空")]
        public Guid CategoryId { get; set; }

        [ForeignKey("ArticleType")]
        [Display(Name = "类型", Order = 7)]
        public Guid TypeId { get; set; }

        //[ForeignKey("Author")]
        //[Display(Name = "作者", Order = 8)]
        //[Required(ErrorMessage = "不能为空")]
        //public object AuthorId { get; set; }

        ///// <summary>
        ///// 发布者信息
        ///// </summary>
        //[ForeignKey("AuthorId")]
        //public NkjSoft.DAL.SysPerson Author { get; set; }

        private ArticleType _Type;
        /// <summary>
        /// 所属类别（培训机构，用户，企业）
        /// </summary>
        [ForeignKey("TypeId")]
        public virtual ArticleType ArticleType
        {
            get
            {
                return _Type;
            }
            set
            {
                if (value != null)
                {
                    TypeId = value.Id;
                }

                _Type = value;
            }
        }

        private ArticleCategory _category;
        /// <summary>
        /// 资讯类别：招聘，培训教程，等等。
        /// </summary>
        [ForeignKey("CategoryId")]
        public virtual ArticleCategory Category
        {
            get
            {
                return _category;
            }
            set
            {
                if (value != null)
                {
                    CategoryId = value.Id;
                }

                _category = value;
            }
        }

        /// <summary>
        /// 资讯评论列表
        /// </summary>
        public virtual ICollection<ArticleComment> Comments { get; set; }

        public Article()
        {
            this.Id = Guid.NewGuid();
            this.Comments = new HashSet<ArticleComment>();
            this.ViewCount = 0;
            this.AvailableCredits = 0;
        }
    }
}
