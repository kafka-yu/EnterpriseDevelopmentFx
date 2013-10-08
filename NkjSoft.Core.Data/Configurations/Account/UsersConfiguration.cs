
using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;

using NkjSoft.Core.Models.Account;
using NkjSoft.Framework.Data;


namespace NkjSoft.Core.Data.Configurations.Account
{
    /// <summary>
    /// 实体类-数据表映射——用户信息
    /// </summary>    
    public partial class UsersConfiguration
    {
        /// <summary>
        /// 额外的数据映射
        /// </summary>
        partial void UsersConfigurationAppend()
        {
            this.HasKey(t => t.UserId);
            this.ToTable("Users");
            this.Property(t => t.ApplicationId).HasColumnName("ApplicationId");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.UserName).HasColumnName("UserName").IsRequired().HasMaxLength(50);
            this.Property(t => t.IsAnonymous).HasColumnName("IsAnonymous");
            this.Property(t => t.LastActivityDate).HasColumnName("LastActivityDate");
            this.HasRequired(t => t.Applications).WithMany(t => t.Users).HasForeignKey(d => d.ApplicationId);
        }
    }
}
