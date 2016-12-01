using System.Web;
using System.Web.Optimization;

namespace ContactMgmtApp
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));
            bundles.Add(new ScriptBundle("~/bundles/angularjs").Include(
                      "~/Scripts/jquery-{version}.js").Include(
                      "~/Scripts/angular.min.js").Include(
                      "~/Scripts/angular-ui-router.min.js").Include(
                      "~/Scripts/kendo.all.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/ContactMgmtApp").Include(
                     "~/Scripts/app/ContactMgmtApp.js").Include(
                     "~/Scripts/app/ContactMgmtCtrl.js").Include(
                     "~/Scripts/app/routes.js",
                     "~/Scripts/app/multiSelectDrtv.js",
                     "~/Scripts/app/ContactMgmtSvc.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));//,
                      //"~/Content/kendo.common-material.min.css",
                    //"~/Content/kendo.material.min.css",
                //"~/Content/kendo.material.mobile.min.css"));
        }
    }
}
