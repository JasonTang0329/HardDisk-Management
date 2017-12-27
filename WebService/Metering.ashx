<%@ WebHandler Language="C#" Class="Metering" %>

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public class Metering : IHttpHandler {

    public void ProcessRequest(HttpContext context)
    {
        HardDiskManage.KM_Filer.File_Helper FH = new HardDiskManage.KM_Filer.File_Helper();
        HardDiskManage.KM_Filer.File_Metering FM = new HardDiskManage.KM_Filer.File_Metering();
        string filestring = "";
        string kind = context.Request["kind"];
        string keyword = context.Request["keyword"];
        string filename = Path.GetFileName(context.Request["path"]);
        string ftype = context.Request["ftype"];
        bool isVirt = Convert.ToBoolean(context.Request["isVirt"]);
        string path = context.Request["path"].Replace(@"/", @"\");
         string FlowId = "";
        if (path != ConfigurationManager.AppSettings["rootPath"])
        {
            path = path.Replace(@"\" + filename, "");
        }
        if (!String.IsNullOrEmpty(path) && !String.IsNullOrEmpty(ftype) && !String.IsNullOrEmpty(kind) )
        {
            FH.updateFileState(path, filename, "", ftype, isVirt);
            FlowId = FH.queryFileList(path, filename, ftype, isVirt).Rows[0]["FlowId"].ToString();
            if (kind == "path")
            {
                FM.UpdateMetering(FlowId);
            }
            else if (kind == "keyword")
            {
                FM.UpdateMeteringWithKeyword(FlowId,keyword);

            }
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