using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text;
using System.Web;

namespace Manager.WebApp.Helpers
{
    public static class Extensions
    {
        //public static HtmlString RawHtmlCustom(this IHtmlHelper htmlHelper, string text)
        //{
        //    if (string.IsNullOrEmpty(text))
        //        return HtmlString.NewLine(text);
        //    else
        //    {
        //        StringBuilder builder = new StringBuilder();
        //        string[] lines = text.Split('\n');
        //        for (int i = 0; i < lines.Length; i++)
        //        {
        //            if (i > 0)
        //                builder.Append("<br/>\n");
        //            builder.Append(HttpUtility.HtmlEncode(lines[i]));
        //        }
        //        return HtmlString.Create(builder.ToString());
        //    }
        //}

        public static IHtmlContent RawHtmlCustom(this IHtmlHelper htmlHelper, string text)
        {
            if (string.IsNullOrEmpty(text))
                return new HtmlString(text);
            else
            {
                StringBuilder builder = new StringBuilder();
                string[] lines = text.Split('\n');
                for (int i = 0; i < lines.Length; i++)
                {
                    if (i > 0)
                        builder.Append("<br/>\n");
                    builder.Append(HttpUtility.HtmlEncode(lines[i]));
                }
                return new HtmlString(builder.ToString());
            }
        }
    }
}
