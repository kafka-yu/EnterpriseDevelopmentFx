using NkjSoft.Core.Models.Account;
using NkjSoft.Framework.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace NkjSoft.Core.Models.Business
{
    public class Certificate : IdBasedEntityBase<Guid>
    {
        /// <summary>
        /// 证书名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 颁发日期
        /// </summary>
        public DateTime AwardedTime { get; set; }

        [ForeignKey("FromTrainingInstitutions")]
        public Guid FromTrainingInstitutionsId { get; set; }

        /// <summary>
        /// 详细信息
        /// </summary>
        public string Introduction { get; set; }

        [ForeignKey("AwardedTo")]
        public Nullable<Guid> AwardedToId { get; set; }

        /// <summary>
        /// 颁发给的人
        /// </summary>
        [ForeignKey("AwardedToId")]
        public Users AwardedTo { get; set; }

        /// <summary>
        /// 主图片。
        /// </summary>
        public SiteImage MainImg { get; set; }

        /// <summary>
        /// 颁发机构
        /// </summary>
        [ForeignKey("FromTrainingInstitutionsId")]
        public TrainingInstitutions FromTrainingInstitutions{ get; set; }
    }
}
