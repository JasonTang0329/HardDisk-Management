<%@ WebHandler Language="C#" Class="Filer" %>

using System;
using System.Web;
using System.Data;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Configuration;
public class Filer : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        string filestring = "";
        string path = context.Request["path"];
        string kind = context.Request["kind"];

        string domain = ConfigurationManager.AppSettings["domain"];
        string uid = ConfigurationManager.AppSettings["uid"];
        string pwd = ConfigurationManager.AppSettings["pwd"];
        HardDiskManage.KM_Filer.File_Helper FH = new HardDiskManage.KM_Filer.File_Helper();


        if (!String.IsNullOrEmpty(path) && !String.IsNullOrEmpty(kind))
        {
            switch (kind)
            {
                 case "Nas":
                    try
                    {
                        using (new Impersonator(uid, domain, pwd))
                        {

                            DataTable DT = new DataTable();
                            DT.Columns.Add("path", typeof(string));
                            DT.Columns.Add("Type", typeof(string));
                            DT.Columns.Add("data", typeof(string));
                            DT.Columns.Add("isVirt", typeof(string));
                            DT.Columns.Add("link", typeof(string));

                            //DT.Columns.Add("id", typeof(string));
                            if (Directory.Exists(path))
                            {
                                string[] Dir = Directory.GetDirectories(path);
                                Array.Sort(Dir);

                                foreach (string DItem in Dir)
                                {
                                    string Item = DItem.Replace(path + @"\", "");

                                    if (!FilterFunc.filterDirector(Item) && !FilterFunc.filterPartFiler(Path.GetFileName(Item)))
                                    {
                                        DataRow row;
                                        row = DT.NewRow();
                                        row["path"] = DItem;
                                        row["Type"] = "D";
                                        row["data"] = Item;
                                        row["isVirt"] = "false";
                                        row["link"] = "#";
                                        //row["id"] = path;

                                        DT.Rows.Add(row);
                                    }
                                }
                                
                                string[] Filesr = Directory.GetFiles(path);
                                Array.Sort(Filesr);
                                foreach (string FItem in Filesr)
                                {
                                    
                                   string Item = FItem.Replace(path + @"\", "");
                                   if (Item.Substring(0, 1) != "~" && Item.Substring(0, 1) != "." && !FilterFunc.filterFiler(Path.GetFileName(Item)) && !FilterFunc.filterPartFiler(Path.GetFileName(Item)))
                                    {
                                        DataRow row;
                                        row = DT.NewRow();
                                        row["path"] = FItem;
                                        row["Type"] = "F";
                                        row["data"] = Item;
                                        row["isVirt"] = "false";
                                        row["link"] = "#";

                                        DT.Rows.Add(row);
                                    }
                                }
                            }
                            DataTable dts = FH.queryVirtFileList(path);
                            foreach (DataRow dr in dts.Rows) {
                                DataRow row;
                                row = DT.NewRow();
                                row["path"] = path + @"\" + dr["file_name"].ToString();
                                row["Type"] = dr["file_type"].ToString();
                                row["data"] = dr["file_name"].ToString();
                                row["link"] = dr["Link"].ToString(); 
                                row["isVirt"] = "true";
                                DT.Rows.Add(row);
                                
                            }
                             DT.DefaultView.Sort = "Type,data";
                            DT = DT.DefaultView.ToTable();
                            filestring = (DT.Rows.Count > 0 ? JsonConvert.SerializeObject(DT, Formatting.Indented) : "");

                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    break;

            }
        }
        context.Response.ContentType = "application/json";
        context.Response.Charset = "utf-8";
        context.Response.Write(filestring);

    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}