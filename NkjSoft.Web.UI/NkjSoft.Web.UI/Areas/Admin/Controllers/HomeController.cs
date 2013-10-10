using NkjSoft.Framework.Web.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NkjSoft.Web.UI.Areas.Admin.Controllers
{
    [RequirePermission("系统管理", Area = "Admin", ShowAsMenu = true)]
    public class HomeController : Controller
    {
        //
        // GET: /Admin/Home/

        [RequirePermission("用户管理")]
        public ActionResult Index()
        {
            return View();
        }
    }
}
