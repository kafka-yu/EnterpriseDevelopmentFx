using System.Web.Optimization;

using NkjSoft.Framework.Extensions;

namespace NkjSoft.Web.UI.App_Start
{
    public static class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            RegisterStyleBundles(bundles);
            RegisterJavascriptBundles(bundles);
        }

        private static void RegisterStyleBundles(BundleCollection bundles)
        {
            var theme = "metro";
            bundles.Add(new StyleBundle("~/Content/Admin")
                  .Include("~/Content/default.css")
                  .Include("~/Content/themes/icon.css")
                  .Include("~/Content/themes/{0}/easyui.css"
                  .FormatWith(theme))
                  .Include("~/Content/easyui.{0}.modify.css".FormatWith(theme)));

            bundles.Add(new StyleBundle("~/Content/LogOn")
                .Include("~/Content/bootstrap.css")
                .Include("~/Content/LogOn.css"));
        }

        private static void RegisterJavascriptBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Scripts/jquery").Include(
                       "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/Scripts/easyui").Include(
                "~/Scripts/jquery.easyui.min.js",
                "~/Scripts/locale/easyui-lang-zh_CN.js"));

            bundles.Add(new ScriptBundle("~/Scripts/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/Scripts/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/Scripts/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/Scripts/admin")
                .Include("~/Scripts/jquery.json-2.4.min.js")
                .Include("~/Scripts/searchBoxBackend.js")
                .Include("~/Scripts/wikmain.js")
                .Include("~/Scripts/wikmenu.js"));
        }
    }
}