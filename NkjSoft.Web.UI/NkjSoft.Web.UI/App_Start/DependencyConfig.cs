using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NkjSoft.Core.Data.Migrations;
using NkjSoft.Framework.IoC;

namespace NkjSoft.Web.UI.App_Start
{
    public static class DependencyConfig
    {
        public static void RegisterDependency()
        {
            // Use LocalDB for Entity Framework by default
            Database.DefaultConnectionFactory = new SqlConnectionFactory(@"Data Source=(localdb)\v11.0; Integrated Security=True; MultipleActiveResultSets=True");

            // ControllerBuilder.Current.SetControllerFactory(new NkjSoft.Web.MVC.NinjectControllerFactory());

            // KernelManager.Initialize(System.Configuration.ConfigurationManager
            //.AppSettings["NinjectConfig"]);

            //EngineContext.Current.Initialize();

            //设置MEF依赖注入容器
            DirectoryCatalog catalog = new DirectoryCatalog(AppDomain.CurrentDomain.SetupInformation.PrivateBinPath);
            MefDependencySolver solver = new MefDependencySolver(catalog);
            DependencyResolver.SetResolver(solver);

            MefDependencySolver.Current = solver;
            DatabaseInitializer.Initialize();
        }
    }
}