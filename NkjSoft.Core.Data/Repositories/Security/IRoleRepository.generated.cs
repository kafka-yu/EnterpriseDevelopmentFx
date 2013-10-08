
using System;

using NkjSoft.Framework.Data;
using NkjSoft.Core.Models.Security;


namespace NkjSoft.Core.Data.Repositories.Security
{
    /// <summary>
    ///   仓储操作层接口——角色信息
    /// </summary>
    public partial interface IRoleRepository : IRepository<Roles, Guid>
    { }
}
