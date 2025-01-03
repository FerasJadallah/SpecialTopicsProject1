using System.Web;
using System.Web.Optimization;

namespace WebApplication2
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Layout/js")
            .Include("~/Scripts/Layout.js"));

            bundles.Add(new ScriptBundle("~/Index/js")
            .Include("~/Scripts/Index.js"));

            bundles.Add(new StyleBundle("~/Layout/css").Include(
                      "~/Content/Layout.css",
                      "~/Content/bootstrap.min.css"
                      ));
            bundles.Add(new StyleBundle("~/Setting/css").Include(
                      "~/Content/Setting.css"
                      ));
            bundles.Add(new StyleBundle("~/Index/css").Include(
                      "~/Content/Index.css"
                      ));
            bundles.Add(new StyleBundle("~/Contact/css").Include(
                     "~/Content/Contact.css"
                     ));
            bundles.Add(new StyleBundle("~/About/css").Include(
                    "~/Content/about.css"
                    ));
            bundles.Add(new StyleBundle("~/Login/css").Include(
                     "~/Content/Login.css"
                     ));
            bundles.Add(new StyleBundle("~/Create_account/css").Include(
                    "~/Content/creat_account.css"
                    ));
        }
    }
}
