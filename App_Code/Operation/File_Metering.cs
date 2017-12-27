using GISFCU.Data;
using GISFCU.Data.Sql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;


namespace HardDiskManage.KM_Filer
{
    /// <summary>
    /// File_Metering 的摘要描述
    /// </summary>
    public class File_Metering
    {
        public File_Metering()
        {
            //
            // TODO: 在這裡新增建構函式邏輯
            //
        }
        DbHelper db = new DbHelper();

        public void UpdateMetering(string FlowId)
        {
            db.CommandText = @" 
UPDATE KM_file
  SET Metering+=1
WHERE FlowId = @FlowId

";
            db.Parameters.Clear();
            db.Parameters.Add("@FlowId", SqlDbType.NVarChar, FlowId);
            db.ExecuteNonQuery();
        }

        public void UpdateMeteringWithKeyword(string FlowId, string keyword)
        {
            db.CommandText = @" 
UPDATE KM_file_Keywords
  SET Metering+=1
WHERE FlowId = @FlowId
  AND Keywords = @Keyword
";
            db.Parameters.Clear();
            db.Parameters.Add("@FlowId", SqlDbType.NVarChar, FlowId);
            db.Parameters.Add("@Keyword", SqlDbType.NVarChar, keyword);

            db.ExecuteNonQuery();
        }
    }
}