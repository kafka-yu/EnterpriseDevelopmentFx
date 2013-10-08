using NkjSoft.Core.Models.Account;
using NkjSoft.Framework.Data;
using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;


namespace NkjSoft.Core.Data.Configurations.Account
{
    /// <summary>
    /// 实体类-数据表映射——登录记录信息
    /// </summary>    
    public partial class LoginLogConfiguration : EntityTypeConfiguration<LoginLog>, IEntityMapper
    {
        /// <summary>
        /// 实体类-数据表映射构造函数——登录记录信息
        /// </summary>
        public LoginLogConfiguration()
        {
            LoginLogConfigurationAppend();
        }

        /// <summary>
        /// 额外的数据映射
        /// </summary>
        partial void LoginLogConfigurationAppend();

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
