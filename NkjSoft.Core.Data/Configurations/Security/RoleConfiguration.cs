using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq;
using System.Text;

namespace NkjSoft.Core.Data.Configurations.Security
{
    public partial class RoleConfiguration
    {
        partial void RoleConfigurationAppend()
        {
            this.HasKey(t => t.RoleId);
            this.ToTable("Roles");
            this.Property(t => t.ApplicationId).HasColumnName("ApplicationId");
            this.Property(t => t.RoleId).HasColumnName("RoleId");
            this.Property(t => t.RoleName).HasColumnName("RoleName").IsRequired().HasMaxLength(256);
            this.Property(t => t.Description).HasColumnName("Description").HasMaxLength(256);
            this.HasRequired(t => t.Applications).WithMany(t => t.Roles).HasForeignKey(d => d.ApplicationId);
            this.HasMany(t => t.Users).WithMany(t => t.Roles)
                .Map(m =>
                {
                    m.ToTable("UsersInRoles");
                    m.MapLeftKey("RoleId");
                    m.MapRightKey("UserId");
                });
        }
    }
}
