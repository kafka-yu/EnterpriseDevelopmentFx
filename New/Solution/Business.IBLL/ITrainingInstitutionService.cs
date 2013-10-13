using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Business.IBLL
{
    [ServiceContract(Namespace = "www.NkjSoft.com")]
    public interface ITrainingInstitutionService
    {
        [OperationContract]
        ICollection<NkjSoft.DAL.SysPerson> TestGetTrainingInstitutions();
    }
}
