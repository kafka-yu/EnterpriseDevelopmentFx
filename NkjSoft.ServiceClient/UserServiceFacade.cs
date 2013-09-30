using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NkjSoft.Model;
using NkjSoft.ServiceContracts.Common;
using NkjSoft.WCF;

namespace NkjSoft.ServiceClient
{
    /// <summary>
    /// 
    /// </summary>
    public class UserAccountServiceFacade : NkjSoftWCFClientBase<IUserAccountService>, IUserAccountService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public string[] GetRoles(string userName)
        {
            return this.Fn.GetRoles(userName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<FunctionPermission> GetAllActionPermission()
        {
            return this.Fn.GetAllActionPermission();
        }


        public List<Model.Common.Menu> GetAllMenus()
        {
            throw new NotImplementedException();
        }

        public List<Model.Common.Menu> GetAllButtons()
        {
            throw new NotImplementedException();
        }
    }
}
