using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq;
using System.Text;

namespace NkjSoft.Core.Data.Configurations.Security
{
    public partial class ActionDefinitionConfiguration
    {
        partial void ActionDefinitionConfigurationAppend()
        {
            this.HasKey(t => t.Id);
            this.ToTable("ActionDefinition");
            this.Property(t => t.IsDeleted).HasColumnName("IsDeleted");
            this.Property(t => t.ShowAsMenu).HasColumnName("ShowAsMenu");
            this.Property(t => t.Style).HasColumnName("Style");
            this.Property(t => t.Text).HasColumnName("Text");
            this.Property(t => t.Url).HasColumnName("Url");
            this.Property(t => t.Area).HasColumnName("Area");
            this.Property(t => t.ActionName).HasColumnName("ActionName").HasMaxLength(256);
            this.Property(t => t.Controller).HasColumnName("Controller").HasMaxLength(256);
            this.Property(t => t.Description).HasColumnName("Description").HasMaxLength(256);
        }
    }
}
