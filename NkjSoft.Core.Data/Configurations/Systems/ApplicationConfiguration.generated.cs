
using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;

using NkjSoft.Core.Models.Account;
using NkjSoft.Framework.Data;


namespace NkjSoft.Core.Data.Configurations.Systems
{
    /// <summary>
    /// 实体类-数据表映射——用户信息
    /// </summary>    
    public partial class ApplicationConfiguration : EntityTypeConfiguration<Applications>, IEntityMapper
    {
        /// <summary>
        /// 实体类-数据表映射构造函数——用户信息
        /// </summary>
        public ApplicationConfiguration()
        {
            ApplicationConfigurationAppend();
        }

        /// <summary>
        /// 额外的数据映射
        /// </summary>
        partial void ApplicationConfigurationAppend();

        /// <summary>
        /// 将当前实体映射对象注册到当前数据访问上下文实体映射配置注册器中
        /// </summary>
        /// <param name="configurations">实体映射配置注册器</param>
        public void RegistTo(ConfigurationRegistrar configurations)
        {
            configurations.Add(this);
        }
    }
}
