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
    public partial class ArticleCategory : IdBasedEntityBase<Guid>
    {
        /// <summary>
        /// 名称
        /// </summary>
        [MaxLength(50, ErrorMessage = "长度必须小于等于50")]
        [Display(Name = "名称", Order = 2)]
        [Required(ErrorMessage = "不能为空")]
        public string Name { get; set; }

        [ForeignKey("Type")]
        [Display(Name = "类型", Order = 3)]
        public Guid TypeId { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [Display(Name = "描述", Order = 4), MaxLength(200, ErrorMessage = "长度必须小于等于200")]
        public string Description { get; set; }

        private ArticleType _Type;
        /// <summary>
        /// 所属类别（培训机构，用户，企业）
        /// </summary>
        [ForeignKey("TypeId")]
        public virtual ArticleType Type
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

        [ForeignKey("ParentNode")]
        [Display(Name = "父类别", Order = 5)]
        public Nullable<Guid> ParentId { get; set; }

        [ForeignKey("ParentId")]
        public virtual ArticleCategory ParentNode { get; set; }

        /// <summary>
        /// 所属文章列表
        /// </summary>
        public virtual ICollection<Article> Articles { get; set; }

        /// <summary>
        /// 所属的子类别。
        /// </summary>
        public virtual ICollection<ArticleCategory> ChildrenCategories { get; set; }

        public ArticleCategory()
            : base()
        {
            this.Id = Guid.NewGuid();
            this.Description = string.Empty;
            this.Articles = new HashSet<Article>();
            this.ChildrenCategories = new HashSet<ArticleCategory>();
        }
    }
}
