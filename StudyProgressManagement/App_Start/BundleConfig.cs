using System.Web.Optimization;

namespace StudyProgressManagement
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            // Scripts bundles
            bundles.Add(new ScriptBundle("~/bundles/vendorJs").Include(
                "~/app-assets/vendors/js/core/jquery-3.6.0.min.js",
                "~/app-assets/vendors/js/core/popper.min.js",
                "~/app-assets/vendors/js/core/bootstrap.min.js",
                "~/app-assets/vendors/js/perfect-scrollbar.jquery.min.js",
                "~/app-assets/vendors/js/prism.min.js",
                "~/app-assets/vendors/js/jquery.matchHeight-min.js",
                "~/app-assets/vendors/js/jquery.validate.min.js",
                "~/app-assets/vendors/js/jquery.validate.unobtrusive.min.js",
                "~/app-assets/vendors/js/screenfull.min.js",
                "~/app-assets/vendors/js/pace/pace.min.js",
                "~/app-assets/vendors/js/toastr.min.js",
                "~/app-assets/vendors/js/sweetalert2.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/sidebarJs").Include(
               "~/app-assets/js/app-sidebar.min.js"));

            // Style Bundles
            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/app-assets/vendors/css/perfect-scrollbar.min.css",
                "~/app-assets/vendors/css/prism.min.css",
                "~/app-assets/vendors/css/toastr.min.css",
                "~/app-assets/vendors/css/sweetalert2.min.css",
                "~/app-assets/css/app.min.css"));

            BundleTable.EnableOptimizations = true;
        }
    }
}
