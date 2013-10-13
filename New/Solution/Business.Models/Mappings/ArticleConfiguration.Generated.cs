using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;

using NkjSoft.Framework.Data;
using NkjSoft.DAL.Business;
namespace NkjSoft.DAL.Business.Mappings
{
    /// <summary>
    /// 实体类-数据表映射
    /// </summary>    
    internal partial class ArticleConfiguration : EntityTypeConfigurationMapperBase<Article>
    {
        protected override void ConfigurationAppend()
        {
            // Do mapping here if necessary.
        }
    }
}
