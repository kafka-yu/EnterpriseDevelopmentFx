using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace NkjSoft.Core.Models.Business
{
    /// <summary>
    /// 联系方式。
    /// </summary>
    [ComplexType]
    public class ContactInfo
    {
        public string Tel1 { get; set; }
        public string Tel2 { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Fox { get; set; }
        public string Site { get; set; }
    }
}
