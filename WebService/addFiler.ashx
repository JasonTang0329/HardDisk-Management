<%@ WebHandler Language="C#" Class="addFiler" %>

using System;
using System.Web;

public class addFiler : IHttpHandler {

    public void ProcessRequest(HttpContext context)
    {
        string filestring = "";

        string path = context.Request["path"];
        string ftype = context.Request["ftype"];
        string file = context.Request["file"];
        string link = context.Request["link"];
        HardDiskManage.KM_Filer.File_Helper FH = new HardDiskManage.KM_Filer.File_Helper();
        if (!String.IsNullOrEmpty(path) && !String.IsNullOrEmpty(ftype) && !String.IsNullOrEmpty(file))
        {
            System.Data.DataTable dt = FH.queryFileList(path, file, ftype, true);
            if (dt.Rows.Count > 0 && Convert.ToBoolean(dt.Rows[0]["isDel"].ToString()) == false)
            {
                filestring = "重複新增相同名稱" + (ftype == "D" ? "目錄" : (ftype == "L" ? "連結" : ""));

            }
            else
            {

                FH.updateFileState(path, file, link, ftype, true);
                filestring = "";
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