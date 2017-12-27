using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Default : BasePage
{
    DataTable DT = new DataTable();
    public String rootpath = (string.IsNullOrEmpty(ConfigurationManager.AppSettings["rootPath"]) ? "" :  HttpUtility.UrlEncode(ConfigurationManager.AppSettings["rootPath"]));
    public string ShortURI
    {
        get
        {
            return ViewState["URI"] as string ?? "";
        }
        set { ViewState["URI"] = value; }
    }
    protected void Page_Load(object sender, EventArgs e)
    {

        Session["Username"] = Path.GetFileName(System.Web.HttpContext.Current.Request.ServerVariables["AUTH_USER"]);//讀取Windows驗證身分

        if (!IsPostBack)
        {
            //是否為短網址轉入
            if (!string.IsNullOrEmpty(Request["URI"]))
            {
                ShortURI = ConvertURIToPath(Request["URI"]);
            }
        }

    }
    /// <summary>
    /// Grid轉換為Path
    /// </summary>
    /// <param name="grid"></param>
    /// <returns>由grid轉換為path</returns> 
    private string ConvertURIToPath(string grid)
    {
        Guid g = Guid.Empty;
        string path = "";
        if (Guid.TryParse(grid, out g))
        {
            HardDiskManage.KM_Filer.File_Helper fh = new HardDiskManage.KM_Filer.File_Helper();
            DataTable DT = fh.GridGetingPath(grid);

            path = DT.Rows[0]["file_path"].ToString().Replace(rootpath + @"\", "");
            string fname = (path == rootpath && DT.Rows[0]["file_name"].ToString() == Path.GetFileName(path) ? "" : DT.Rows[0]["file_name"].ToString());//去除ROOT
            path = ((path + @"\" + fname));
        }
        return path;
    }



}