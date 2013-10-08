
using NkjSoft.Core.Models.Account;
using NkjSoft.Framework.Data;
using System;

namespace NkjSoft.Core.Data.Repositories.Account
{
    /// <summary>
    ///   仓储操作层接口——登录记录信息
    /// </summary>
    public partial interface ILoginLogRepository : IRepository<LoginLog, Guid>
    { }
}
