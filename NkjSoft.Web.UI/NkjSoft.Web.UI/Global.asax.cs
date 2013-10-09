using System.Web.Http;
using System.Web.Optimization;
using Kooboo.CMS.Common.Runtime;
using NkjSoft.Core.Data.Migrations;
using NkjSoft.Framework;
using NkjSoft.Framework.IoC;
using NkjSoft.Web.UI.App_Start;
using NkjSoft.Web.UI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace NkjSoft.Web.UI
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public MvcApplication()
        {
            AuthorizeRequest += new EventHandler(MvcApplication_AuthorizeRequest);
        }

        void MvcApplication_AuthorizeRequest(object sender, EventArgs e)
        {
            IIdentity id = Context.User.Identity;
            if (id.IsAuthenticated)
            {
                var roles = new string[] { "Administrator" };//new UserAccountService().GetRoles(id.Name);
                Context.User = new GenericPrincipal(id, roles);
            }
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            ModelBinders.Binders.DefaultBinder = new DefaultModelBinder();

            ModelBinders.Binders.Add(new KeyValuePair<Type, IModelBinder>(typeof(IEnumerable<QueryParameter>), new QueryParameterModelBinder()));

            DependencyConfig.RegisterDependency();
        }
    }
}