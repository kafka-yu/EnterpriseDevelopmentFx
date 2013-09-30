using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace NkjSoft.Web.UI.Lib
{
    /// <summary>
    /// 
    /// </summary>
    public class ActionPermissionAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        public string PermissionKey { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string[] PermissionKeys
        {
            get
            {
                return PermissionKey.Split(',');
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="permissionKeys"></param>
        public ActionPermissionAttribute(string permissionKeys)
        {
            this.PermissionKey = permissionKeys;
        }
    }
}
