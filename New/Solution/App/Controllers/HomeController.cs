using System.Web.Mvc;
using System.Collections.Generic;
using NkjSoft.DAL;
using NkjSoft.BLL;
using Common;
using NkjSoft.App.Models;
using NkjSoft.IBLL;
using Models;
namespace NkjSoft.App.Controllers
{
    public class HomeController : BaseController
    {

        public ActionResult Index()
        {
            return RedirectToAction("Index", "Account", new {area = "Admin"});
            Account account = GetCurrentAccount();
            if (account == null)
            {
                RedirectToAction("Index", "Account");
            }
            else
            {
                ViewData["Show"] = string.Format(@"登录总次数：{0} <br/>       本次登录IP：{1}     <br/>    本次登录时间：{2}      <br/>   上次登录IP：{3}     <br/>    上次登录时间：{4}<br/>", account.LogonNum, Common.IP.GetIP(), System.DateTime.Now, account.LastLogonIP, account.LastLogonTime);
                ViewData["PersonName"] = account.Name;
                IHomeBLL home = new HomeBLL();
                //在1.4版本中修改 
                ViewData["Menu"] = home.GetMenuByAccount(ref account);// 获取菜单
            }

            return View();
        }


        /// <summary>
        /// 根据父节点获取数据字典,绑定二级下拉框的时候使用
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult GetSysFieldByParent(string id, string parentid, string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }
            ISysFieldHander baseDDL = new SysFieldHander();
            return Json(new SelectList(baseDDL.GetSysFieldByParent(id, parentid, value), "MyTexts", "MyTexts"), JsonRequestBehavior.AllowGet);

        }
        /// <summary>
        /// 获取列表中的按钮导航
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult GetToolbar(string id)
        {
            if (string.IsNullOrWhiteSpace(id) && id == "undefined")
            {
                return null;
            }
            Account account = GetCurrentAccount();
            if (account == null)
            {
                return Content(" <script type='text/javascript'> window.top.location='Account'; </script>");

            }
            ISysMenuSysRoleSysOperationBLL sro = new SysMenuSysRoleSysOperationBLL();
            List<SysOperation> sysOperations = sro.GetByRefSysMenuIdAndSysRoleId(id, account.RoleIds);
            List<toolbar> toolbars = new List<toolbar>();
            foreach (SysOperation item in sysOperations)
            {
                toolbars.Add(new toolbar() { handler = item.Function, iconCls = item.Iconic, text = item.Name });
            }
            return Json(toolbars, JsonRequestBehavior.AllowGet);
        }
    }
}


