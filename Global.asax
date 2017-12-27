<%@ Application Language="C#" %>
<%@ Import Namespace="GISFCU.Data.Sql" %>
<%@ Import Namespace="System.Web.Routing" %>
<%@ Import Namespace="System.Web.Compilation" %>
<script runat="server">

    void Application_Start(object sender, EventArgs e) 
    {
        // 應用程式啟動時執行的程式碼
        // *****************************************************************
        // { EllyChen } : { 2012/02/22 }
        //    GISFCU Initial ConnectionString Setting 
        // *****************************************************************
                //設定資料庫連結 , 兩個方法擇一使用 
        //方式一 , 可以設定連線 Pool 
        Generic.DBConnectionString = AppGlobal.ConnectionString;
        //方式二 , 設定各別參數 , 連線 pool 為預設值 0 - 100 
        //Generic.DatabaseName = "資料庫名稱";
        //Generic.DataSource = "DataBase Server";
        //Generic.UserID = "登入帳號";
        //Generic.UserPWD = "登入密碼";        
        //Mapping 強型別的方法 , 預設(false) 
        // false : 使用 DataTable , 優點 : 離線處理, 不佔用連線數 ; 缺點 : 佔用主機記憶體 , 速度比較慢
        // true  : 使用 DataReader , 優點 : 速度快 , 不佔用主機記憶體 ; 缺點 : 在處理中會佔用連線數 , 當連線數有限制時 , 不建議使用
        Generic.UseReaderMode = false;
        // *****************************************************************      
        RoutineJobs.Start();
        HardDiskManage.KM_Filer.RedirectRoutes.RegisterRoutes(RouteTable.Routes);
    }
    
    void Application_End(object sender, EventArgs e) 
    {
        //  應用程式關閉時執行的程式碼

    }
        
    void Application_Error(object sender, EventArgs e) 
    { 
        // 發生未處理錯誤時執行的程式碼

    }

    void Session_Start(object sender, EventArgs e) 
    {
        // 啟動新工作階段時執行的程式碼

    }

    void Session_End(object sender, EventArgs e) 
    {
        // 工作階段結束時執行的程式碼。 
        // 注意: 只有在 Web.config 檔將 sessionstate 模式設定為 InProc 時，
        // 才會引發 Session_End 事件。如果將工作階段模式設定為 StateServer 
        // 或 SQLServer，就不會引發這個事件。

    }
       
</script>
