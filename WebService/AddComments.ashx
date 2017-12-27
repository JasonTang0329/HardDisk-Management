<%@ WebHandler Language="C#" Class="AddComments" %>

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public class AddComments : IHttpHandler, System.Web.SessionState.IRequiresSessionState
{

    public void ProcessRequest(HttpContext context)
    {
        HardDiskManage.KM_Filer.File_Helper FH = new HardDiskManage.KM_Filer.File_Helper();
        HardDiskManage.KM_Comments_Filer.File_Comments_Helper FCH = new HardDiskManage.KM_Comments_Filer.File_Comments_Helper();
        string filestring = "";
        string filename = Path.GetFileName(context.Request["path"]);
        string ftype = context.Request["ftype"];
        bool isVirt = Convert.ToBoolean(context.Request["isVirt"]);
        string path = context.Request["path"].Replace(@"/", @"\");
        string comments = context.Request["comments"];
        string FlowId = "";
        if (path != ConfigurationManager.AppSettings["rootPath"])
        {
            path = path.Replace(@"\" + filename, "");
        }


        if (!String.IsNullOrEmpty(path) && !String.IsNullOrEmpty(ftype) && !String.IsNullOrEmpty(comments))
        {
            FH.updateFileState(path, filename, "", ftype, isVirt);
            FlowId = FH.queryFileList(path, filename, ftype, isVirt).Rows[0]["FlowId"].ToString();
            try
            {
                FCH.updateComments(FlowId, comments, context.Session["Username"].ToString());
            }
            catch (Exception ex)
            {

                filestring = "儲存失敗:" + ex;
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