using NkjSoft.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NkjSoft.Web.UI.Areas.Admin.Controllers
{
    public class UsersController : Controller
    {
        //
        // GET: /Admin/Users/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddUser()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetUsers(int? page, int? rows, IEnumerable<QueryParameter> queryParams, IEnumerable<string> ids)
        {
            var testData = new List<NkjSoft.Model.Common.Menu>();
            testData.Add(new Model.Common.Menu() { ActionName = "test", Controller = "Home", Id = 1, Name = "Add", });
            testData.Add(new Model.Common.Menu() { ActionName = "test2", Controller = "Home", Id = 1, Name = "Add", });
            testData.Add(new Model.Common.Menu() { ActionName = "test3", Controller = "Home", Id = 1, Name = "Add", });
            testData.Add(new Model.Common.Menu() { ActionName = "test4", Controller = "Home", Id = 1, Name = "Add", });
            testData.Add(new Model.Common.Menu() { ActionName = "test5", Controller = "Home", Id = 1, Name = "Add", });
            testData.Add(new Model.Common.Menu() { ActionName = "test6", Controller = "Home", Id = 1, Name = "Add", });
            testData.Add(new Model.Common.Menu() { ActionName = "test7", Controller = "Home", Id = 1, Name = "Add", });
            testData.Add(new Model.Common.Menu() { ActionName = "test8", Controller = "Home", Id = 1, Name = "Add", });
            testData.Add(new Model.Common.Menu() { ActionName = "test9", Controller = "Home", Id = 1, Name = "Add", });
            testData.Add(new Model.Common.Menu() { ActionName = "test10", Controller = "Home", Id = 1, Name = "Add", });
            testData.Add(new Model.Common.Menu() { ActionName = "test11", Controller = "Home", Id = 1, Name = "Add", });
            testData.Add(new Model.Common.Menu() { ActionName = "test12", Controller = "Home", Id = 1, Name = "Add", });
            testData.Add(new Model.Common.Menu() { ActionName = "test13", Controller = "Home", Id = 1, Name = "Add", });
            testData.Add(new Model.Common.Menu() { ActionName = "test14", Controller = "Home", Id = 1, Name = "Add", });
            testData.Add(new Model.Common.Menu() { ActionName = "test15", Controller = "Home", Id = 1, Name = "Add", });
            testData.Add(new Model.Common.Menu() { ActionName = "test16", Controller = "Home", Id = 1, Name = "Add", });
            testData.Add(new Model.Common.Menu() { ActionName = "test17", Controller = "Home", Id = 1, Name = "Add", });
            testData.Add(new Model.Common.Menu() { ActionName = "test18", Controller = "Home", Id = 1, Name = "Add", });
            testData.Add(new Model.Common.Menu() { ActionName = "test19", Controller = "Home", Id = 1, Name = "Add", });
            testData.Add(new Model.Common.Menu() { ActionName = "test20", Controller = "Home", Id = 1, Name = "Add", });
            testData.Add(new Model.Common.Menu() { ActionName = "test21", Controller = "Home", Id = 1, Name = "Add", });
            testData.Add(new Model.Common.Menu() { ActionName = "test22", Controller = "Home", Id = 1, Name = "Add", });
            testData.Add(new Model.Common.Menu() { ActionName = "test23", Controller = "Home", Id = 1, Name = "Add", });
            testData.Add(new Model.Common.Menu() { ActionName = "test24", Controller = "Home", Id = 1, Name = "Add", });
            testData.Add(new Model.Common.Menu() { ActionName = "test25", Controller = "Home", Id = 1, Name = "Add", });

            var result = testData.Skip((page.GetValueOrDefault() - 1) * rows.GetValueOrDefault())
                .Take(rows.GetValueOrDefault())
                .ToList();

            return Json(new PageList<NkjSoft.Model.Common.Menu>(25, result), JsonRequestBehavior.AllowGet);
        }
    }
}
