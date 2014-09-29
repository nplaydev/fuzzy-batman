using BundleTransformer.Core.Bundles;
using System.Web;
using System.Web.Optimization;

namespace Prototype
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/lib/jquery/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/lib/jquery/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/lib/modernizr/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/lib/bootstrap/bootstrap.js",
                      "~/Scripts/lib/bootstrap/respond.js"));


            bundles.Add(new ScriptBundle("~/bundles/searchprototype").Include(
                      "~/Scripts/lib/jquery/jquery-{version}.js",
                      "~/Scripts/lib/bootstrap/bootstrap.js",
                      "~/Scripts/lib/bootstrap/respond.js",
                      "~/Scripts/lib/isotope/isotope.js",
                      "~/Scripts/lib/mustache/mustache.js",
                      "~/Scripts/app/search-prototype.js"));


            bundles.Add(new CustomStyleBundle("~/App/style").Include(
                      "~/Content/style/app/app.less"));            

            // Set EnableOptimizations to false for debugging. For more information,
            // visit http://go.microsoft.com/fwlink/?LinkId=301862
            BundleTable.EnableOptimizations = true;
        }
    }
}
