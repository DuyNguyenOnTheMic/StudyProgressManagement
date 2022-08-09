using Microsoft.Ajax.Utilities;

namespace System.Web.Mvc
{
    public static class HtmlHelperExtensions
    {
        public static MvcHtmlString JsMinify(
            this HtmlHelper helper,
            Func<object, object> markup)
        {
            string notMinifiedJs =
                (markup.DynamicInvoke(helper.ViewContext) ?? "").ToString();

            var minifier = new Minifier();
            var minifiedJs = minifier.MinifyJavaScript(notMinifiedJs, new CodeSettings
            {
                EvalTreatment = EvalTreatment.MakeImmediateSafe,
                PreserveImportantComments = false
            });
            return new MvcHtmlString(minifiedJs);
        }

    }
}