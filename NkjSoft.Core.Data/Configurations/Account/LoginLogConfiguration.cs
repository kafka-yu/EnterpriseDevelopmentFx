using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;

namespace NkjSoft.Core.Data.Configurations.Account
{
    public partial class LoginLogConfiguration
    {
        partial void LoginLogConfigurationAppend()
        {
            //HasRequired(m => m.Member).WithMany(n => n.LoginLogs);
        }
    }
}