using NkjSoft.Framework.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace NkjSoft.DAL.Business
{
    public partial class ArticleType : IdBasedEntityBase<Guid>
    {
        [Display(Name = "名称", Order = 2)]
        public string Name { get; set; }

        [Display(Name = "值", Order = 3)]
        public int TypeValue { get; set; }

        [Display(Name = "描述", Order = 4)]
        public string Description { get; set; }

        public ArticleType()
        {
            this.Id = Guid.NewGuid();
            this.Articles = new HashSet<Article>();
            this.ArticleCategories = new HashSet<ArticleCategory>();
        }

        /// <summary>
        /// 此类型类别列表
        /// </summary>
        public virtual ICollection<ArticleCategory> ArticleCategories { get; set; }

        /// <summary>
        /// 此类型文章列表
        /// </summary>
        public virtual ICollection<Article> Articles { get; set; }

    }
}
