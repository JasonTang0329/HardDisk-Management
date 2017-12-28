using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Net;
using System.Web;
using System.Threading;
using System.Configuration;

/// <summary>
/// RoutineJobs 的摘要描述
/// </summary>
public class RoutineJobs
{
     private static bool DailyRunning = false;

    private static Timer dailyTimer = null;
    private static System.Threading.Thread thread = null;

    public static void Start()
    {
        Close();
        if (thread != null) thread.Abort();
 
            thread = new System.Threading.Thread(new System.Threading.ThreadStart(DailyThreadRun));
            thread.Start();

            dailyTimer = new Timer(new TimerCallback(DailyTimerRun), null, 0, 3600000);
    }

    protected static void DailyTimerRun(object t)
    {
        if (!DailyRunning)
            DailyThreadRun();
    }

    protected static void DailyThreadRun()
    {
        DailyRunning = true;
        
        string hour = DateTime.Now.ToString("HH");

        if (hour == "23")//true
        {
             #region CheckUser
            try
            {
                // 取得最新資料夾結構
                new HardDiskManage.KM_Filer.CheckFiler();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            #endregion
        }
        DailyRunning = false;
    }

    protected static void Close()
    {
        if (dailyTimer != null)
        {
            dailyTimer.Dispose();
            dailyTimer = null;
        }
    }
}