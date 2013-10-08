
using NkjSoft.Core.Models.Systems;
using NkjSoft.Framework.Data;
using System;


namespace NkjSoft.Core.Data.Repositories.Systems
{
    /// <summary>
    ///   仓储操作层接口——日志信息
    /// </summary>
    public partial interface ILogInfoRepository : IRepository<LogInfo, Guid>
    { }
}
