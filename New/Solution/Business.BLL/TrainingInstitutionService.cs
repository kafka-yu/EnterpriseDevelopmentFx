using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;

namespace Business.BLL
{
    [Export(typeof(Business.IBLL.ITrainingInstitutionService))]
    public class TrainingInstitutionService : Business.IBLL.ITrainingInstitutionService
    {
        [Import]
        public NkjSoft.IBLL.ISysPersonBLL PersonBll { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ICollection<NkjSoft.DAL.SysPerson> TestGetTrainingInstitutions()
        {
            return PersonBll.GetAll();
        }
    }
}
