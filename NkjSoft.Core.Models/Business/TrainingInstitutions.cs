using NkjSoft.Framework.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace NkjSoft.Core.Models.Business
{
    /// <summary>
    /// 培训机构
    /// </summary>
    public partial class TrainingInstitutions : IdBasedEntityBase<Guid>
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string InstitutionName { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public Address Address { get; set; }

        public string Introduction { get; set; }

        public string MainImgs { get; set; }

        /// <summary>
        /// 联系方式
        /// </summary>
        public ContactInfo ContactInfo { get; set; }

        /// <summary>
        /// 所属行业
        /// </summary>
        public ICollection<Industry> Industries { get; set; }

        /// <summary>
        /// 证书。
        /// </summary>
        public ICollection<Certificate> Certificates { get; set; }
    }
}
