using NkjSoft.Framework.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NkjSoft.Core.Models.Business.AdvisoryShow
{
    /// <summary>
    /// 资讯所属类型：系统默认有：培训机构，用户，企业，站长，
    /// </summary>
    public partial class AdvisoryType : IdBasedEntityBase<Guid>
    {
        public string Name { get; set; }

        public int TypeValue { get; set; }

        public string Description { get; set; }
    }
}
