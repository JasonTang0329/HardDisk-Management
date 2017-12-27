using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

/// <summary>
/// UserInfo 的摘要描述
/// </summary>
public class UserInfo
{
    private HttpSessionState _Session;
    private string _UserId;
    private string _Name;
    private string _Dept;
    private string _UserTitle;
    private string _Active;
    private string _Email;
    private string _Role;

    public UserInfo()
    {
        //
        // TODO: 在此加入建構函式的程式碼
        //        
    }

    public string UserId
    {
        get { return _UserId as string ?? ""; }
        set { this._UserId = value; }
    }

    public string Name
    {
        get { return _Name as string ?? ""; }
        set { this._Name = value; }
    }

    public string Dept
    {
        get { return _Dept as string ?? ""; }
        set { this._Dept = value; }
    }

    public string UserTitle
    {
        get { return _UserTitle as string ?? ""; }
        set { this._UserTitle = value; }
    }

    public string Active
    {
        get { return _Active as string ?? ""; }
        set { this._Active = value; }
    }

    public string Email
    {
        get { return _Email as string ?? ""; }
        set { this._Email = value; }
    }
    public string Role
    {
        get { return _Role as string ?? ""; }
        set { this._Role = value; }
    }
}