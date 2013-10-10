using NkjSoft.Framework.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NkjSoft.Core.Models.Business
{
    /// <summary>
    /// 网站图片资源
    /// </summary>
    public class SiteImage : IdBasedEntityBase<Guid>
    {
        public string Name { get; set; }

        public string BigFilePath { get; set; }

        public string SmallFilePath { get; set; }

        public string MiddleFilePath { get; set; }
    }
}
