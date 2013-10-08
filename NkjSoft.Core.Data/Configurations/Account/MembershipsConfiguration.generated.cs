
using NkjSoft.Core.Models.Account;
using NkjSoft.Framework.Data;
using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;

namespace NkjSoft.Core.Data.Configurations.Account
{
    internal partial class MembershipsConfiguration : EntityTypeConfiguration<Memberships>, IEntityMapper
    {
        /// <summary>
        /// 实体类-数据表映射构造函数——用户扩展信息
        /// </summary>
        public MembershipsConfiguration()
        {
            MembershipsConfigurationAppend();
        }

        /// <summary>
        /// 额外的数据映射
        /// </summary>
        partial void MembershipsConfigurationAppend();

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
