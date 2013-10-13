using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;

using NkjSoft.Framework.Data;
using NkjSoft.DAL.Business;
namespace NkjSoft.DAL.Business.Repository
{
    /// <summary>
    /// 实体类 Repository 接口定义
    /// </summary>
    public interface IArticleCommentRepository : IRepository<ArticleComment, Guid>
    {

    }
}
	