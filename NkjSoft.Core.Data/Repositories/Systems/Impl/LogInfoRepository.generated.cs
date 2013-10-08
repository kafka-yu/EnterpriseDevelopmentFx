

using NkjSoft.Core.Models.Systems;
using NkjSoft.Framework.Data;
using System;
using System.ComponentModel.Composition;
using System.Linq;


namespace NkjSoft.Core.Data.Repositories.Systems.Impl
{
    /// <summary>
    ///   仓储操作层实现——日志信息
    /// </summary>
    [Export(typeof(ILogInfoRepository))]
    public partial class LogInfoRepository : EFRepositoryBase<LogInfo, Guid>, ILogInfoRepository
    { }
}
