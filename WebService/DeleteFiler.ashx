<%@ WebHandler Language="C#" Class="DeleteFiler" %>

using System;
using System.Web;
using System.Data;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Configuration;

public class DeleteFiler : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        string filestring = "";
        string ftype = context.Request["ftype"];
        string filename = Path.GetFileName(context.Request["path"]);
        string path = context.Request["path"].Replace(@"/", @"\");
        bool isVirt = Convert.ToBoolean(context.Request["isVirt"]);
        if (path != ConfigurationManager.AppSettings["rootPath"])
        {
            path = path.Replace(@"\" + filename, "");
        }

        try
        {

            HardDiskManage.KM_Filer.File_Helper FH = new HardDiskManage.KM_Filer.File_Helper();
            if (!String.IsNullOrEmpty(path) && !String.IsNullOrEmpty(ftype))
            {
                FH.updateFileDelete(path, filename, ftype, isVirt);

            }
        }
        catch (Exception ex) {
            filestring = ex.ToString();
        }
        context.Response.ContentType = "application/json";
        context.Response.Charset = "utf-8";
        context.Response.Write(filestring);

    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}