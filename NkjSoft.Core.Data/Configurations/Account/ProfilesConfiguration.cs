using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq;
using System.Text;

namespace NkjSoft.Core.Data.Configurations.Account
{
    public partial class ProfilesConfiguration
    {
        partial void ProfilesConfigurationAppend()
        {
            this.HasKey(t => t.UserId);
            this.ToTable("Profiles");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.PropertyNames).HasColumnName("PropertyNames").IsRequired().HasMaxLength(4000);
            this.Property(t => t.PropertyValueStrings).HasColumnName("PropertyValueStrings").IsRequired().HasMaxLength(4000);
            this.Property(t => t.PropertyValueBinary).HasColumnName("PropertyValueBinary").IsRequired();
            this.Property(t => t.LastUpdatedDate).HasColumnName("LastUpdatedDate");
            this.HasRequired(t => t.Users).WithOptional(t => t.Profiles);
        }
    }
}
