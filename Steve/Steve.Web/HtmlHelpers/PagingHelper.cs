using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Steve.BLL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Html;
using System.Text;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.Encodings.Web;

namespace Steve.Web.HtmlHelpers
{
    public static class PagingHelper
    {
        public static HtmlString PageLinks(PagingInfo pagingInfo, Func<int, string> pageUrl)
        {
            StringBuilder result = new StringBuilder();

            for (int i = 1; i <= pagingInfo.TotalPages; i++)
            {
                TagBuilder tag = new TagBuilder("a");

                tag.MergeAttribute("href", pageUrl(i));
                tag.InnerHtml.AppendHtml(i.ToString());
                if (i == pagingInfo.CurrentPage)
                {
                    tag.AddCssClass("selected");
                    tag.AddCssClass("btn-primary");
                }
                tag.AddCssClass("btn btn-default");

                var writer = new System.IO.StringWriter();
                tag.WriteTo(writer, HtmlEncoder.Default);

                result.Append(writer.ToString());
            }
            return new HtmlString(result.ToString());
        }
    }
}
