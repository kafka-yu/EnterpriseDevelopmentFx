using Kooboo.CMS.Common.Runtime;
using NkjSoft.Core.Data.Repositories.Account;
using NkjSoft.Core.Models.Account;
using NkjSoft.Framework;
using NkjSoft.Framework.IoC;
using NkjSoft.Web.UI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NkjSoft.Web.UI.Areas.Admin.Controllers
{
    [Export]
    public class UsersController : Controller
    {
        [Import]
        public IUsersRepository AccountContract { get; set; }

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
            //var testData = new List<NkjSoft.Model.Common.Menu>();
            //testData.Add(new Model.Common.Menu() { Action = "test", Controller = "Home", Id = Guid.NewGuid(), Text = "Add", });
            //testData.Add(new Model.Common.Menu() { Action = "test2", Controller = "Home", Id = Guid.NewGuid(), Text = "Add", });
            //testData.Add(new Model.Common.Menu() { Action = "test3", Controller = "Home", Id = Guid.NewGuid(), Text = "Add", });
            //testData.Add(new Model.Common.Menu() { Action = "test4", Controller = "Home", Id = Guid.NewGuid(), Text = "Add", });
            //testData.Add(new Model.Common.Menu() { Action = "test5", Controller = "Home", Id = Guid.NewGuid(), Text = "Add", });
            //testData.Add(new Model.Common.Menu() { Action = "test6", Controller = "Home", Id = Guid.NewGuid(), Text = "Add", });
            //testData.Add(new Model.Common.Menu() { Action = "test7", Controller = "Home", Id = Guid.NewGuid(), Text = "Add", });
            //testData.Add(new Model.Common.Menu() { Action = "test8", Controller = "Home", Id = Guid.NewGuid(), Text = "Add", });
            //testData.Add(new Model.Common.Menu() { Action = "test9", Controller = "Home", Id = Guid.NewGuid(), Text = "Add", });
            //testData.Add(new Model.Common.Menu() { Action = "test10", Controller = "Home", Id = Guid.NewGuid(), Text = "Add", });
            //testData.Add(new Model.Common.Menu() { Action = "test11", Controller = "Home", Id = Guid.NewGuid(), Text = "Add", });
            //testData.Add(new Model.Common.Menu() { Action = "test12", Controller = "Home", Id = Guid.NewGuid(), Text = "Add", });
            //testData.Add(new Model.Common.Menu() { Action = "test13", Controller = "Home", Id = Guid.NewGuid(), Text = "Add", });
            //testData.Add(new Model.Common.Menu() { Action = "test14", Controller = "Home", Id = Guid.NewGuid(), Text = "Add", });
            //testData.Add(new Model.Common.Menu() { Action = "test15", Controller = "Home", Id = Guid.NewGuid(), Text = "Add", });
            //testData.Add(new Model.Common.Menu() { Action = "test16", Controller = "Home", Id = Guid.NewGuid(), Text = "Add", });
            //testData.Add(new Model.Common.Menu() { Action = "test17", Controller = "Home", Id = Guid.NewGuid(), Text = "Add", });
            //testData.Add(new Model.Common.Menu() { Action = "test18", Controller = "Home", Id = Guid.NewGuid(), Text = "Add", });
            //testData.Add(new Model.Common.Menu() { Action = "test19", Controller = "Home", Id = Guid.NewGuid(), Text = "Add", });
            //testData.Add(new Model.Common.Menu() { Action = "test20", Controller = "Home", Id = Guid.NewGuid(), Text = "Add", });
            //testData.Add(new Model.Common.Menu() { Action = "test21", Controller = "Home", Id = Guid.NewGuid(), Text = "Add", });
            //testData.Add(new Model.Common.Menu() { Action = "test22", Controller = "Home", Id = Guid.NewGuid(), Text = "Add", });
            //testData.Add(new Model.Common.Menu() { Action = "test23", Controller = "Home", Id = Guid.NewGuid(), Text = "Add", });
            //testData.Add(new Model.Common.Menu() { Action = "test24", Controller = "Home", Id = Guid.NewGuid(), Text = "Add", });
            //testData.Add(new Model.Common.Menu() { Action = "test25", Controller = "Home", Id = Guid.NewGuid(), Text = "Add", });

            //var repo = EngineContext.Current.Resolve<IUnitOfWork>()
            //    .GetRepository<NkjSoft.Repository.IUserRepository>();
            var repo =
                AccountContract;

            var data = repo.GetType();

            var tt = repo.Entities
            .Select(p => new
            {
                ApplicationId = p.ApplicationId,
                UserId = p.UserId,
                Email = p.Memberships.Email,
                Name = p.UserName
            })
            .ToList();

            //var result = testData.Skip((page.GetValueOrDefault() - 1) * rows.GetValueOrDefault())
            //    .Take(rows.GetValueOrDefault())
            //    .ToList();

            return Json(tt.AsPagedList(repo.Entities.Count()), JsonRequestBehavior.AllowGet);
        }
    }

    [Export]
    public class testDb : NkjSoft.Framework.Data.EFDbContext
    {
        protected override void OnModelCreating(System.Data.Entity.DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
