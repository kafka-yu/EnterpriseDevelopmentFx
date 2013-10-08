using NkjSoft.Framework.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace NkjSoft.Core.Models.Account
{
    public class LoginLog : IdBasedEntityBase<Guid>
    {
        [Required]
        [StringLength(15)]
        public string IpAddress { get; set; }

        /// <summary>
        /// 获取或设置 所属用户信息
        /// </summary>
        public virtual Users Member { get; set; }
    }
}
