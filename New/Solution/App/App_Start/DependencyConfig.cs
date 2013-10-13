using NkjSoft.DAL.Business.Migrations;
using NkjSoft.Framework.IoC;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NkjSoft.App.App_Start
{
    public class DependencyConfig
    {
        public static void RegisterDependency()
        {

            // Use LocalDB for Entity Framework by default
            //Database.DefaultConnectionFactory = new SqlConnectionFactory(@"Data Source=(localdb)\v11.0; Integrated Security=True; MultipleActiveResultSets=True");

            // ControllerBuilder.Current.SetControllerFactory(new NkjSoft.Web.MVC.NinjectControllerFactory());

            //设置MEF依赖注入容器
            DirectoryCatalog catalog = new DirectoryCatalog(AppDomain.CurrentDomain.SetupInformation.PrivateBinPath);
            MefDependencySolver solver = new MefDependencySolver(catalog);
            DependencyResolver.SetResolver(solver);

            MefDependencySolver.Current = solver;
            DatabaseInitializer.Initialize();
        }
    }
}