
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
    public partial class ApplicationConfiguration
    {
        /// <summary>
        /// 额外的数据映射
        /// </summary>
        partial void ApplicationConfigurationAppend()
        {
            this.HasKey(t => t.ApplicationId);
            this.ToTable("Applications");
            this.Property(t => t.ApplicationName).HasColumnName("ApplicationName").IsRequired().HasMaxLength(235);
            this.Property(t => t.ApplicationId).HasColumnName("ApplicationId");
            this.Property(t => t.Description).HasColumnName("Description").HasMaxLength(256);
        }
    }
}
