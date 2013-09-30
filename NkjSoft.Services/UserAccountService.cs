using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using NkjSoft.ServiceContracts.Common;

namespace NkjSoft.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in both code and config file together.
    public class UserAccountService : IUserAccountService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public string[] GetRoles(string userName)
        {
            return new string[] { "admin", "poweruser" };
        }

        /// <summary>
        /// Gets all action permission.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public List<Model.FunctionPermission> GetAllActionPermission()
        {
            return new List<Model.FunctionPermission>();
        }


        public List<Model.Common.Menu> GetAllMenus()
        {
            return new List<Model.Common.Menu>();
        }

        public List<Model.Common.Menu> GetAllButtons()
        {
            return new List<Model.Common.Menu>();
        }
    }
}
