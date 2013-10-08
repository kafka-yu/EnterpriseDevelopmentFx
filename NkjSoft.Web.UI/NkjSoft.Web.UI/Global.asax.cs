using Kooboo.CMS.Common.Runtime;
using NkjSoft.Framework;
using NkjSoft.Framework.IoC;
using NkjSoft.Model.Migrations;
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


        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                new string[] { "NkjSoft.Web.UI.Controllers" }
            );

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            ModelBinders.Binders.DefaultBinder = new DefaultModelBinder();

            ModelBinders.Binders.Add(new KeyValuePair<Type, IModelBinder>(typeof(IEnumerable<QueryParameter>), new QueryParameterModelBinder()));

            // Use LocalDB for Entity Framework by default
            Database.DefaultConnectionFactory = new SqlConnectionFactory(@"Data Source=(localdb)\v11.0; Integrated Security=True; MultipleActiveResultSets=True");

            // ControllerBuilder.Current.SetControllerFactory(new NkjSoft.Web.MVC.NinjectControllerFactory());

            // KernelManager.Initialize(System.Configuration.ConfigurationManager
            //.AppSettings["NinjectConfig"]);

            //EngineContext.Current.Initialize();


            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            //设置MEF依赖注入容器
            DirectoryCatalog catalog = new DirectoryCatalog(AppDomain.CurrentDomain.SetupInformation.PrivateBinPath);
            MefDependencySolver solver = new MefDependencySolver(catalog);
            DependencyResolver.SetResolver(solver);

            DatabaseInitializer.Initialize();
        }
    }
}