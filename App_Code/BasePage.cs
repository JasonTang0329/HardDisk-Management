// *******************************************************************************
// { EllyChen } IPage 說明 , 
// *******************************************************************************
//  1. 如果要做權限管控 , 請在 OnPreInit 進行處理 , 
//  2. 將原本 [ASPX] 內的 System.Web.UI.Page 修改為 BasePage 
// *******************************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// BasePage 的摘要描述
/// </summary>
public class BasePage : GISFCU.Web.IPage 
{
    protected override void OnPreInit(EventArgs e)
    {
        base.OnPreInit(e);
        //主要用來處理權限
    }
    protected bool CheckWindowsLogin()
    {
        bool isvalid = true;
        string DomainUser = null;
        if (System.Web.HttpContext.Current != null)
        {
            // Get Win Auth Account
            DomainUser = System.Web.HttpContext.Current.Request.ServerVariables["AUTH_USER"];
            Session["Username"] = DomainUser;
        }
        else
        {
            isvalid = false;
        }
        return isvalid;
    }
}