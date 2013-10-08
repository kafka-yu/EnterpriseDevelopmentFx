using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq;
using System.Text;

namespace NkjSoft.Core.Data.Configurations.Account
{
    partial class ProfilesConfiguration
    {
        partial void ProfilesConfigurationAppend()
        {
            HasRequired(m => m.Users).WithOptional(n => n.Profiles);
        }
    }
}
