using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;

using NkjSoft.Framework.Data;
using NkjSoft.DAL.Business;
using NkjSoft.DAL.Business.Repository;
using System.ComponentModel.Composition;
namespace NkjSoft.DAL.Business.Repository.Impl
{
    /// <summary>
    /// 实体类 ArticleCategoryRepository 实现
    /// </summary>
    [Export(typeof(IArticleCategoryRepository))]
    public partial class ArticleCategoryRepository : EFRepositoryBase<ArticleCategory, Guid>, IArticleCategoryRepository
    {

    }
}
	