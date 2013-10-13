using System.Web.Mvc;

namespace NkjSoft.App.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { controller = "Account", action = "Index", id = UrlParameter.Optional },
                new[] { "NkjSoft.App.Areas.Admin.Controllers" }
            );
        }
    }
}
