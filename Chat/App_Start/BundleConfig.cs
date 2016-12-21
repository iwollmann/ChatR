using System.Web;
using System.Web.Optimization;
using Microsoft.Ajax.Utilities;
using System;

namespace Chat
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-1.*"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui*"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/signalR").Include(
                        "~/Scripts/jquery.signalR-*"));

            bundles.Add(new ScriptBundle("~/bundles/pinify").Include(
                        "~/Scripts/jquery.pinify*"));

            bundles.Add(new ScriptBundle("~/bundles/js").Include("~/Content/js/bootstrap.js"));

            bundles.Add(new MotherFuckerStyleBundle("~/bundles/css").Include("~/Content/css/*.css"));
        }
    }

    public class MotherFuckerStyleBundle : Bundle
    {
        public MotherFuckerStyleBundle(string virtualPath)
            : base(virtualPath, new[] { new A() })
        {

        }
    }

    public class A : IBundleTransform
    {
        IBundleTransform Css = new CssMinify();
        internal static string CssContentType = "text/css";
        public void Process(BundleContext context, BundleResponse response)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            if (response == null)
            {
                throw new ArgumentNullException("response");
            }
            if (!context.EnableInstrumentation)
            {
                Minifier minifier = new Minifier();
                Minifier arg_3F_0 = minifier;
                string arg_3F_1 = response.Content;
                CssSettings cssSettings = new CssSettings();
                cssSettings.CommentMode = CssComment.None;
                string content = arg_3F_0.MinifyStyleSheet(arg_3F_1, cssSettings).Replace("~/", VirtualPathUtility.ToAbsolute("~/"));
                if (minifier.ErrorList.Count > 0)
                {
                    //JsMinify.GenerateErrorResponse(response, minifier.get_ErrorList());
                }
                else
                {
                    response.Content = content;
                }
            }
            response.ContentType = CssContentType;
        }
    }
}