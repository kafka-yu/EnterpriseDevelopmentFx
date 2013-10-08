
using NkjSoft.Core.Models.Account;
using NkjSoft.Framework.Data;
using System;
using System.ComponentModel.Composition;
using System.Linq;

namespace NkjSoft.Core.Data.Repositories.Account.Impl
{
    /// <summary>
    ///   仓储操作层实现——用户扩展信息
    /// </summary>
    [Export(typeof(IProfilesRepository))]
    public partial class ProfilesRepository : EFRepositoryBase<Profiles, Guid>, IProfilesRepository
    { }
}
