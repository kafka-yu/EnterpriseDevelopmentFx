using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace NkjSoft.Core.Models.Business
{
    [ComplexType()]
    public class Address
    {
        /// <summary>
        /// 国家
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// 省
        /// </summary>
        public string Province { get; set; }

        /// <summary>
        /// 市
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// 区县
        /// </summary>
        public string Region { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string Detail { get; set; }
    }
}
