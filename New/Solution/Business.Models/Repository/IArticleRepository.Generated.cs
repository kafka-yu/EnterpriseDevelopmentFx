using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;

using NkjSoft.Framework.Data;
using NkjSoft.DAL.Business;
namespace NkjSoft.DAL.Business.Repository
{
    /// <summary>
    /// 实体类-数据表映射——登录记录信息
    /// </summary>    
    public interface IArticleRepository : IRepository<Article, Guid>
    {
        
    }
}
	