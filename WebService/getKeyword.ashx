<%@ WebHandler Language="C#" Class="addFiler" %>

using System;
using System.Web;
using System.Data;
using Newtonsoft.Json;
using JiebaNet.Segmenter;
using JiebaNet.Analyser;
/// <summary>
/// Service For Keyword Search
/// </summary>
public class addFiler : IHttpHandler
{
    HardDiskManage.KM_Filer.File_Helper FH = new HardDiskManage.KM_Filer.File_Helper();

    public void ProcessRequest(HttpContext context)
    {
        string filestring = "";

        string txt = context.Request["txt"];
        string mode = context.Request["mode"];

        if (!String.IsNullOrEmpty(txt) && !String.IsNullOrEmpty(mode))
        {

            switch (mode)
            {
                case "Tip":
                    filestring = Gettip(txt);
                    break;
                case "GetPath":
                    string[] reservedTagPool = {  "br" };
                    filestring = HardDiskManage.CommonFunc.CommonFunc.StripTags(GetPath(txt), reservedTagPool);
                    break;
                case "GetSuggestKeyword":
                    filestring = GetSugWord(txt);

                    break;

            }


        }
        context.Response.ContentType = "application/json";
        context.Response.Charset = "utf-8";
        context.Response.Write(filestring);

    }
    /// <summary>
    /// 取得搜尋提示列表
    /// </summary>
    /// <param name="txt"></param>
    /// <returns></returns>
    private String Gettip(string txt)
    {
        DataTable dt = FH.queryKeyWord(txt);

        return (dt.Rows.Count > 0 ? JsonConvert.SerializeObject(dt, Formatting.Indented) : "");

    }
    /// <summary>
    /// 取得搜尋結果
    /// </summary>
    /// <param name="txt"></param>
    /// <returns></returns>
    private String GetPath(string txt)
    {
        DataTable dt = FH.queryKeyWordPath(txt);

        return (dt.Rows.Count > 0 ? JsonConvert.SerializeObject(dt, Formatting.Indented) : "");

    }
    /// <summary>
    /// 透過分詞切割關鍵字後提出點擊率較高的關聯詞
    /// </summary>
    /// <param name="txt"></param>
    /// <returns></returns>
    private String GetSugWord(string txt)
    {
        var segmenter = new JiebaSegmenter();
        var segments = segmenter.Cut(txt);
        //Console.WriteLine("【全模式】：{0}", string.Join("/ ", segments));
        DataTable dt = new DataTable();
        foreach (string item in segments)
        {
            if (dt.Rows.Count == 0)
            {
                dt = FH.querySugKeyWord(item);
            }
            else
            {
                DataTable dts = FH.querySugKeyWord(item).Clone();
                foreach (DataRow dr in dts.Rows)
                {
                    dt.Rows.Add(dr);

                }
            }

        }
        dt.DefaultView.Sort = "Metering Desc";
        DataTable dtDistinct = dt.DefaultView.ToTable(true, new string[] { "keyword" });
        dtDistinct = dtDistinct.DefaultView.ToTable();
        return (dtDistinct.Rows.Count > 0 ? JsonConvert.SerializeObject(dtDistinct, Formatting.Indented) : "");

    }
    public bool IsReusable
    {
        get
        {
            return false;
        }
    }


}