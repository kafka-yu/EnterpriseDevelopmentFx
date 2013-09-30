using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NkjSoft.ORM
{
    /// <summary>
    /// 提供对查询命令进行日志的能力。
    /// </summary>
    public interface IQueryLogable
    {
        /// <summary>
        /// 在查询生成了之后发生。
        /// </summary>
        event EventHandler QueryLog;
    }
}
