using System;
using System.ComponentModel.Composition;
using System.Linq;

using NkjSoft.Framework.Data;
using NkjSoft.Core.Models.Account;


namespace NkjSoft.Core.Data.Repositories.Account.Impl
{
	/// <summary>
    ///   仓储操作层实现——登录记录信息
    /// </summary>
    [Export(typeof(ILoginLogRepository))]
    public partial class LoginLogRepository : EFRepositoryBase<LoginLog, Guid>, ILoginLogRepository
    { }
}
