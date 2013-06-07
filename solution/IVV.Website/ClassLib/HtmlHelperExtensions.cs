using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace System.Web.Mvc.Html
{
    public static class HtmlHelperExtensions
    {
        /// <summary>
        /// mvc分页辅助
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="partialViewName">默认为“_PageBar”</param>
        /// <param name="totalCount">总记录数</param>
        /// <param name="pageSize">每页的最大记录数</param>
        /// <param name="pageIndex">当前页索引</param>
        /// <param name="url"></param>
        /// <param name="pars"></param>
        public static void RenderPager(this HtmlHelper htmlHelper, string partialViewName,
            int totalCount,int pageSize,int pageIndex,string url,Dictionary<string, object> pars)
        {
            if(string.IsNullOrEmpty(partialViewName))
                partialViewName = "_PageBar";

			var bar = new IVV.Website.ClassLib.PageBar();
            bar.TotalCount = totalCount;
            bar.PageSize = pageSize;
            bar.PageIndex = pageIndex;
            bar.Url = url;
            bar.Params = pars;

            htmlHelper.RenderPartial(partialViewName, bar);
        }

        public static MvcHtmlString TipPartial(this HtmlHelper htmlHelper,bool alwaysDisplay = false)
        {
            htmlHelper.ViewData["alwaysDisplay"] = alwaysDisplay;            
            return htmlHelper.Partial("_Info");
        }

       
    }
}
