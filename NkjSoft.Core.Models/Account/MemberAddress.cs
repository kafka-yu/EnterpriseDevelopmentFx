using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace NkjSoft.Core.Models.Account
{
    /// <summary>
    /// 用户地址信息
    /// </summary>
    public class MemberAddress
    {
        [StringLength(20)]
        public string Province { get; set; }

        [StringLength(50)]
        public string City { get; set; }

        [StringLength(50)]
        public string County { get; set; }

        [StringLength(200, MinimumLength = 5)]
        public string Street { get; set; }
    }
}
