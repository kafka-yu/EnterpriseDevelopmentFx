using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Ninject;
using NkjSoft.ServiceContracts.Core.Account;
namespace NkjSoft.Web.UI.Lib
{
    public class PermissionControlFilter : ActionFilterAttribute
    {
        private IUserAccountService service = KernelManager.Kernel.Get<IUserAccountService>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }


            var path = filterContext.HttpContext.Request.Path.ToLower();
            if (path == "/" || path == "/Account/LogOn".ToLower() || path == "/Account/LogOn".ToLower())
                return;//忽略对Login登录页的权限判定

            object[] attrs = filterContext.ActionDescriptor.GetCustomAttributes(typeof(ActionPermissionAttribute), true);
            var isViewPage = attrs.Length == 1;//当前Action请求是否为具体的功能页

            ActionPermissionAttribute actionPermissionAttr = attrs.FirstOrDefault() as ActionPermissionAttribute;

            if (this.AuthorizeCore(filterContext, isViewPage, actionPermissionAttr, path) == false)//根据验证判断进行处理
            {
                //注：如果未登录直接在URL输入功能权限地址提示不是很友好；如果登录后输入未维护的功能权限地址，那么也可以访问，这个可能会有安全问题
                if (isViewPage == true)
                {
                    filterContext.Result = new HttpUnauthorizedResult();//直接URL输入的页面地址跳转到登陆页
                }
                else
                {
                    filterContext.Result = new ContentResult { Content = @"'抱歉,你不具有当前操作的权限！" };//功能权限弹出提示框
                }
            }
        }


        //权限判断业务逻辑
        protected virtual bool AuthorizeCore(ActionExecutingContext filterContext, bool isViewPage, ActionPermissionAttribute actionPermission, string pageUrl)
        {
            if (filterContext.HttpContext == null)
            {
                throw new ArgumentNullException("httpContext");
            }

            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                return false;//判定用户是否登录
            }

            //var user = new CurrentUser()
            //{
            //    UserName = filterContext.HttpContext.User.Identity.Name,
            //};//获取当前用户信息

            //var buttons = service.GetAllActionPermission();

            //var controllerName = filterContext.RouteData.Values["controller"].ToString();
            //var actionName = filterContext.RouteData.Values["action"].ToString();

            //var helper = new UserLogonHelper(user);

            //var hasMenuPermission =
            //    helper.IsUserHasMenuPermission(actionName, controllerName, pageUrl);

            return true;
        }

    }

}