using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NkjSoft.DAL.Business;
using NkjSoft.Framework.Data;
using Models;

using NkjSoft.Framework;
using System.ComponentModel.Composition;
using NkjSoft.Framework.Extensions;

namespace NkjSoft.App.Areas.Admin.Controllers
{
    [Export]
    public class ArticleTypeController : BaseController
    {
        [Import]
        private NkjSoft.DAL.Business.Repository.IArticleTypeRepository ArticleTypeService { get; set; }

        //
        // GET: /Admin/ArticleType/

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetArticleTypeListJson()
        {
            var result = ArticleTypeService.Entities.Select(p => new
            {
                Name = p.Name,
                Description = p.Description,
                Id = p.Id,
                TypeValue = p.TypeValue,
                AddDate = p.AddDate,
            })
            .ToList().AsPagedList(ArticleTypeService.Entities.Count());

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /Admin/ArticleType/Details/5

        public ActionResult Details(Nullable<Guid> id = null)
        {
            ArticleType articletype = ArticleTypeService.GetByKey(id.GetValueOrDefault());
            if (articletype == null)
            {
                return HttpNotFound();
            }
            return View(articletype);
        }

        //
        // GET: /Admin/ArticleType/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Admin/ArticleType/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ArticleType articletype)
        {
            if (ModelState.IsValid)
            {
                articletype.Id = Guid.NewGuid();
                ArticleTypeService.Insert(articletype);
                return RedirectToAction("Index");
            }

            return View(articletype);
        }

        //
        // GET: /Admin/ArticleType/Edit/5

        public ActionResult Edit(Nullable<Guid> id = null)
        {
            ArticleType articletype = ArticleTypeService.GetByKey(id.GetValueOrDefault());
            if (articletype == null)
            {
                return HttpNotFound();
            }
            return View(articletype);
        }

        //
        // POST: /Admin/ArticleType/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ArticleType articletype)
        {
            if (ModelState.IsValid)
            {
                ArticleTypeService.Update(articletype);
                return RedirectToAction("Index");
            }
            return View(articletype);
        }

        //
        // GET: /Admin/ArticleType/Delete/5

        public ActionResult Delete(Nullable<Guid> id = null)
        {
            ArticleType articletype = GetArticleTypeInstance(id);
            if (articletype == null)
            {
                return HttpNotFound();
            }
            return View(articletype);
        }

        private ArticleType GetArticleTypeInstance(Nullable<Guid> id)
        {
            ArticleType articletype = ArticleTypeService.GetByKey(id.GetValueOrDefault());
            return articletype;
        }

        //
        // POST: /Admin/ArticleType/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            ArticleType articletype = GetArticleTypeInstance(id);
            ArticleTypeService.Delete(articletype);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}