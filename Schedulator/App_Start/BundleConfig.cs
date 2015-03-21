using System.Web;
using System.Web.Optimization;

namespace Schedulator
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //Jquery
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jQuery.prettyPhoto.js",
                        "~/Scripts/jQuery.js",
                        "~/Scripts/jQuery.isotope.min.js",
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            //BooStrap
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/wow.min.js",
                      "~/Scripts/respond.min.js",
                      "~/Scripts/main.js",
                      "~/Scripts/html5shiv.js",
                      "~/Scripts/boostrap.min.js",
                      "~/Scripts/respond.js"));
            //CSS
            bundles.Add(new StyleBundle("~/Content/css").Include(
<<<<<<< HEAD
                      "~/Content/animate.min.css",
                      "~/Content/bootstrap.min.css",
                      "~/Content/font-awesome.css",
                      "~/Content/font-awesome.min.css",
                      "~/Content/main.css",
                      "~/Content/prettyPhoto.css",
                      "~/Content/responsive.css",
                      "~/Content/Site.css"));
=======
                      "~/Content/bootstrap.css",
                      "~/Content/site.css", "~/Content/Schedulator.css"));
>>>>>>> 31964f91712150b40ce027582dbc15f34073b2e3
        }
    }
}
