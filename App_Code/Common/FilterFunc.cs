using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

/// <summary>
/// FilterFunc 的摘要描述
/// </summary>
public class FilterFunc
{
	public FilterFunc()
	{
		//
		// TODO: 在這裡新增建構函式邏輯
		//
	}

    public static bool filterDirector(string DItem)
    {
        bool filter = false;
        string filterpath = ConfigurationManager.AppSettings["filterPath"];
        foreach (string path in filterpath.Split(','))
        {
            if (DItem == path) {
                filter = true;
                break;
            };
        }
        return filter;
    }
    public static bool filterFiler(string File)
    {
        bool filter = false;
        string filterpath = ConfigurationManager.AppSettings["filterFile"];
        foreach (string path in filterpath.Split(','))
        {
            if (File == path) {
                filter = true;
                break;
            };
        }
        return filter;
    }
    public static bool filterPartFiler(string File)
    {
        bool filter = false;
        string filterpath = ConfigurationManager.AppSettings["filterPartFile"];
        foreach (string path in filterpath.Split(','))
        {
            if (File.IndexOf(path) > 0)
            {
                filter = true;
                break;
            };
        }
        return filter;
    }


}