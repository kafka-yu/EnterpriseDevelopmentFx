
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
    internal partial class UsersConfiguration
    {
        /// <summary>
        /// 额外的数据映射
        /// </summary>
        partial void UsersConfigurationAppend()
        {

        }
    }
}
