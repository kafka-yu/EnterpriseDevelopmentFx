using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NkjSoft.Model
{
    /// <summary>
    /// 表示当前用户
    /// </summary>
    public class CurrentUser
    {
        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>
        /// The name of the user.
        /// </value>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [remember me].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [remember me]; otherwise, <c>false</c>.
        /// </value>
        public bool RememberMe { get; set; }

        /// <summary>
        /// Gets or sets the menu permissions.
        /// </summary>
        /// <value>
        /// The menu permissions.
        /// </value>
        public List<MenuPermission> MenuPermissions { get; set; }

        /// <summary>
        /// Gets or sets the function permissions.
        /// </summary>
        /// <value>
        /// The function permissions.
        /// </value>
        public List<FunctionPermission> ActionPermission { get; set; }

        public CurrentUser()
        {
            this.MenuPermissions = new List<MenuPermission>();
            this.ActionPermission = new List<FunctionPermission>();
        }
    }

    /// <summary>
    /// 表示提供菜单（页面）的权限信息提供者。
    /// </summary>
    public class MenuPermission : IPermissionProvider
    {

        /// <summary>
        /// Gets or sets the name of the controller.
        /// </summary>
        /// <value>
        /// The name of the controller.
        /// </value>
        public string ControllerName { get; set; }

        /// <summary>
        /// Gets or sets the name of the action.
        /// </summary>
        /// <value>
        /// The name of the action.
        /// </value>
        public string ActionName { get; set; }
    }

    /// <summary>
    /// 表示功能点的权限信息提供者。
    /// </summary>
    public class FunctionPermission
    {
        /// <summary>
        /// Gets or sets the name of the controller.
        /// </summary>
        /// <value>
        /// The name of the controller.
        /// </value>
        public string ControllerName { get; set; }

        /// <summary>
        /// Gets or sets the name of the action.
        /// </summary>
        /// <value>
        /// The name of the action.
        /// </value>
        public string ActionName { get; set; }
    }

}
