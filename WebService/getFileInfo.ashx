<%@ WebHandler Language="C#" Class="getFileInfo" %>


using System;
using System.Web;
using System.Data;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Configuration;

public class getFileInfo : IHttpHandler
{
    HardDiskManage.KM_Filer.File_Helper FH = new HardDiskManage.KM_Filer.File_Helper();
    HardDiskManage.File_Annotation_Helper.File_Annotation_Helper FAH = new HardDiskManage.File_Annotation_Helper.File_Annotation_Helper();
    HardDiskManage.KM_Comments_Filer.File_Comments_Helper FCH = new HardDiskManage.KM_Comments_Filer.File_Comments_Helper();
    public void ProcessRequest(HttpContext context)
    {
        string filestring = "";
        string ftype = context.Request["ftype"];
        string kind = context.Request["kind"];
        string filename =   Path.GetFileName( context.Request["path"]);
        string path = context.Request["path"].Replace(@"/", @"\") ;
        if (path != ConfigurationManager.AppSettings["rootPath"])
        {
            path = path.Replace(@"\" + filename, "");
        }
        
        bool isvirt = Convert.ToBoolean(context.Request["isvirt"]);

        if (!String.IsNullOrEmpty(path) && !String.IsNullOrEmpty(ftype) && !String.IsNullOrEmpty(kind) && !String.IsNullOrEmpty(filename)&& !String.IsNullOrEmpty(filename))
        {
            DataTable dt = FH.queryFileList(path, filename, ftype, isvirt);

            switch (kind) {
                case "annotation":
                    filestring = GetAnnotation(dt,ftype, path, filename, isvirt);
                    break;
                case "comments":
                    filestring = GetComments(dt,ftype, path, filename, isvirt);
                    break;
                case "gird":
                    filestring = (dt.Rows.Count > 0 ? dt.Rows[0]["FlowId"].ToString() : "");
                    break;

                default:
                    break;
            }
        }

        context.Response.ContentType = "application/json";
        context.Response.Charset = "utf-8";
        context.Response.Write(filestring);
    }
    /// <summary>
    /// 取得檔案說明
    /// </summary>
    /// <param name="dt"></param>
    /// <param name="ftype"></param>
    /// <param name="path"></param>
    /// <param name="filename"></param>
    /// <param name="isvirt"></param>
    /// <returns></returns>
    private string GetAnnotation(DataTable dt, string ftype, string path, string filename, bool isvirt)
    {
        return (dt.Rows.Count > 0 && FAH.queryAnnotation(dt.Rows[0]["FlowId"].ToString()).Rows.Count > 0 ? JsonConvert.SerializeObject(FAH.queryAnnotation(dt.Rows[0]["FlowId"].ToString()), Formatting.Indented) : "");
    }
    /// <summary>
    /// 取得檔案評論
    /// </summary>
    /// <param name="dt"></param>
    /// <param name="ftype"></param>
    /// <param name="path"></param>
    /// <param name="filename"></param>
    /// <param name="isvirt"></param>
    /// <returns></returns>
    private string GetComments(DataTable dt, string ftype, string path, string filename, bool isvirt)
    {
        return (dt.Rows.Count > 0 && FCH.queryComments(dt.Rows[0]["FlowId"].ToString()).Rows.Count > 0 ? JsonConvert.SerializeObject(FCH.queryComments(dt.Rows[0]["FlowId"].ToString()), Formatting.Indented) : "");
    }
    
    public bool IsReusable {
        get {
            return false;
        }
    }

}