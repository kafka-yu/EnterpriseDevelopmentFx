using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Ninject;
using NkjSoft.Model.Common;
using NkjSoft.ServiceContracts.Common;

namespace NkjSoft.Web.UI.Lib
{

    public class UserLogonHelper
    {
        private static readonly string MenusKey = "___Menus";
        private static readonly string ButtonsKey = "___Buttons";

        private List<Menu> _Menus;

        /// <summary>
        /// 
        /// </summary>
        public List<Menu> Menus
        {
            get
            {
                //_Menus = Caches.Get("__Menus") as List<Menu>;

                //if (_Menus == null)
                //{
                //    _Menus = KernelManager.Kernel.Get<IUserAccountService>()
                //       .GetAllMenus();

                //    Caches.Set(MenusKey, _Menus, HeyCahcer2.Enums.SaveType.InPorc);
                //}

                return _Menus;
            }
            set { _Menus = value; }
        }

        private List<Menu> _Buttons;
        private Model.CurrentUser user;

        /// <summary>
        /// 
        /// </summary>
        public List<Menu> Buttons
        {
            get
            {
                //_Buttons = Caches.Get(ButtonsKey) as List<Menu>;

                //if (_Buttons == null)
                //{
                //    _Buttons = KernelManager.Kernel.Get<IUserAccountService>()
                //       .GetAllButtons();

                //    Caches.Set(ButtonsKey, _Buttons, HeyCahcer2.Enums.SaveType.InPorc);
                //}

                return _Menus;
            }
            set { _Buttons = value; }
        }

        public UserLogonHelper()
        {
        }

        public UserLogonHelper(Model.CurrentUser user)
        {
            // TODO: Complete member initialization
            this.user = user;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="permissionInfo"></param>
        /// <returns></returns>
        public bool IsUserHasMenuPermission(string actionName, string controller, string url)
        {
            return true;
        }

    }
}