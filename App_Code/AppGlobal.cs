using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

/// <summary>
/// AppGlobal 的摘要描述
/// </summary>
public class AppGlobal
{
	public AppGlobal()
	{
		//
		// TODO: 在此加入建構函式的程式碼
		//


	}

    public static string ConnectionString
    {
        get { return ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString; }
    }

}