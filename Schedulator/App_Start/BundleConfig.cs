using System.Web;
using System.Web.Optimization;

namespace Schedulator
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jQuery.prettyPhoto.js",
                        "~/Scripts/jQuery.js",
                        "~/Scripts/jQuery.isotope.min.js",
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/wow.min.js",
                      "~/Scripts/respond.min.js",
                      "~/Scripts/main.js",
                      "~/Scripts/html5shiv.js",
                      "~/Scripts/boostrap.min.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/animate.min.css",
                      "~/Content/bootstrap.min.css",
                      "~/Content/font-awesome.css",
                      "~/Content/font-awesome.min.css",
                      "~/Content/main.css",
                      "~/Content/prettyPhoto.css",
                      "~/Content/responsive.css",
                      "~/Content/Site.css"));
        }
    }
}
