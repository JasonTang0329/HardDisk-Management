using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Routing;
namespace HardDiskManage.KM_Filer
{
    /// <summary>
    /// RedirectRoutes 的摘要描述
    /// </summary>
    public class RedirectRoutes
    {
        public RedirectRoutes()
        {
            //
            // TODO: 在這裡新增建構函式邏輯
            //
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.Add(new Route("gd/{grid}", new OrderRouteHandler("~/Default.aspx")));

        }
    }
}