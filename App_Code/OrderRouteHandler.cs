using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Compilation;
using System.Web.Routing;
using System.Web.UI;

public class OrderRouteHandler : IRouteHandler
{
    private string virtualPath;

    public OrderRouteHandler(string virtualPath)
    {
        this.virtualPath = virtualPath;
    }

    public IHttpHandler GetHttpHandler(RequestContext requestContext)
    {
        //origquerystr 透過httpcontext 取出 gpo_no 值應用
        string origQueryStr = HttpContext.Current.Request.QueryString.ToString();
        string queryStr = "?URI=" + requestContext.RouteData.Values["grid"];

        if (!string.IsNullOrEmpty(origQueryStr))
            queryStr += "&" + origQueryStr;
        HttpContext.Current.RewritePath(string.Concat(virtualPath, queryStr));

        return BuildManager.CreateInstanceFromVirtualPath(virtualPath, typeof(Page)) as Page;
    }
}