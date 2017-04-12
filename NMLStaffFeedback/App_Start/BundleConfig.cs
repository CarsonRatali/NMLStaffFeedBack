using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Optimization;

namespace NMLStaffFeedback
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                "~/Scripts/jquery.unobtrusive*",
                "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/knockout").Include(
                "~/Scripts/knockout-{version}.js",
                "~/Scripts/knockout.validation.js"));

            bundles.Add(new ScriptBundle("~/bundles/app").Include(
                "~/Scripts/sammy-{version}.js",
                "~/Scripts/app/common.js",
                "~/Scripts/app/app.datamodel.js",
                "~/Scripts/app/app.viewmodel.js",
                "~/Scripts/app/home.viewmodel.js",
                "~/Scripts/app/_run.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                "~/Scripts/bootstrap.js",
                "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                 "~/Content/bootstrap.css",
                 "~/Content/Site.css"));

            bundles.Add(new ScriptBundle("~/bundles/kendo/2015.3.930/js").Include(
                                      "~/Scripts/kendo/2015.3.930/kendo.all.min.js",
                                      // "~/Scripts/kendo/kendo.timezones.min.js", // uncomment if using the Scheduler
                                      "~/Scripts/kendo/2015.3.930/kendo.aspnetmvc.min.js"));

            bundles.Add(new StyleBundle("~/Content/kendo/2015.3.930/css").Include(
                                        "~/Content/kendo/2015.3.930/kendo.common-bootstrap.min.css",
                                        "~/Content/kendo/2015.3.930/kendo.bootstrap.min.css"));
            bundles.IgnoreList.Clear();
        }
    }
}
