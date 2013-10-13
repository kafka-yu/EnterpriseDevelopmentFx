using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq;
using System.Text;

namespace NkjSoft.Framework.Data
{
    public class EntityTypeConfigurationMapperBase<T> : EntityTypeConfiguration<T>, IEntityMapper where T : class
    {
        /// <summary>
        /// 实体类-数据表映射构造函数——登录记录信息
        /// </summary>
        public EntityTypeConfigurationMapperBase()
        {
            ConfigurationAppend();
        }

        /// <summary>
        /// 额外的数据映射
        /// </summary>
        protected virtual void ConfigurationAppend() { }

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
