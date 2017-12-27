using GISFCU.Data;
using GISFCU.Data.Sql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace HardDiskManage.File_Annotation_Helper
{
    /// <summary>
    /// File_Helper 的摘要描述
    /// </summary>
    public class File_Annotation_Helper
    {
        public File_Annotation_Helper()
        {
            //
            // TODO: 在這裡新增建構函式邏輯
            //
        }
        DbHelper db = new DbHelper();



        private DataTable queryOnlyAnnotation(string FlowId)
        {
            db.CommandText = @" 
        SELECT *
        FROM KM_file_Annotation
        WHERE FlowId = @FlowId  
";

            db.Parameters.Clear();
            db.Parameters.Add("@FlowId", SqlDbType.NVarChar, FlowId);
 
            return db.GetTable();
        }
        /// <summary>
        /// 查詢檔案說明
        /// </summary>
        /// <param name="FlowId"></param>
        /// <returns></returns>
        public DataTable queryAnnotation(string FlowId)
        {
            db.CommandText = @" 
SELECT flowid,
       annotation,
       LEFT(keyword, LEN(keyword) - 1) AS keyword
FROM( 
      SELECT FlowId,
             Annotation,
             (
               SELECT CAST(Keywords AS NVARCHAR) + ','
               FROM KM_file_Keywords
               WHERE FlowId = @FlowId
                 AND isDel = 0
               FOR XML PATH( '' )) AS keyword
      FROM KM_file_Annotation
     WHERE FlowId = @FlowId ) m;";

            db.Parameters.Clear();
            db.Parameters.Add("@FlowId", SqlDbType.NVarChar, FlowId);

            return db.GetTable();
        }
        /// <summary>
        /// 查詢關鍵字
        /// </summary>
        /// <param name="FlowId"></param>
        /// <param name="word"></param>
        /// <returns></returns>
        private DataTable queryKeyWords(string FlowId, string word)
        {
            db.CommandText = @" 
        SELECT *
        FROM KM_file_Keywords
        WHERE FlowId = @FlowId 
          AND Keywords = @Keywords
";

            db.Parameters.Clear();
            db.Parameters.Add("@FlowId", SqlDbType.NVarChar, FlowId);
            db.Parameters.Add("@Keywords", SqlDbType.NVarChar, word);

            return db.GetTable();
        }
        private void insertAnnotation(string FlowId, string Annotation)
        {
            db.CommandText = @" 
INSERT INTO KM_file_Annotation
       ( FlowId,
         Annotation,
         updatetime
       )
VALUES( @FlowId, @Annotation, GETDATE());
                                ";


            db.Parameters.Clear();
            db.Parameters.Add("@FlowId", SqlDbType.NVarChar, FlowId);
            db.Parameters.Add("@Annotation", SqlDbType.NVarChar, Annotation);

            db.ExecuteNonQuery();
        }
        private void insertKeyWord(string FlowId, string word)
        {
            db.CommandText = @" 
INSERT INTO KM_file_Keywords
       ( FlowId,
         Keywords,
         updatetime
       )
VALUES( @FlowId, @Keywords, GETDATE());
                                ";


            db.Parameters.Clear();
            db.Parameters.Add("@FlowId", SqlDbType.NVarChar, FlowId);
            db.Parameters.Add("@Keywords", SqlDbType.NVarChar, word);

            db.ExecuteNonQuery();
        }

        public void updateAnnotation(string FlowId, string Annotation)
        {
            DataTable dt = queryOnlyAnnotation(FlowId);
            if (dt.Rows.Count > 0)
            {
                updateAnnotations(dt.Rows[0]["FlowId"].ToString(), Annotation);
            }
            else
            {
                insertAnnotation(FlowId, Annotation);
            }
        }

        private void updateAnnotations(string FlowId, string Annotation)
        {
            db.CommandText = @" 
UPDATE KM_file_Annotation
   SET updatetime = getdate(),
       Annotation = @Annotation
 WHERE FlowId= @FlowId
";
            db.Parameters.Clear();
            db.Parameters.Add("@FlowId", SqlDbType.NVarChar, FlowId);
            db.Parameters.Add("@Annotation", SqlDbType.NVarChar, Annotation);

            db.ExecuteNonQuery();
        }





        public void updateKeyWords(string FlowId, string word)
        {
            DataTable dt = queryKeyWords(FlowId, word);
            if (dt.Rows.Count > 0)
            {
                updateKeyWordLive(dt.Rows[0]["FlowId"].ToString(),word);
            }
            else
            {
                insertKeyWord(FlowId, word);
            }
        }




        public void updateKeyWordDie(string FlowId)
        {
            db.CommandText = @" 
UPDATE KM_file_Keywords
   SET updatetime = getdate(),
       isDel = 1
 WHERE FlowId= @FlowId
";
            db.Parameters.Clear();
            db.Parameters.Add("@FlowId", SqlDbType.NVarChar, FlowId);
            db.ExecuteNonQuery();
        }

        public void updateKeyWordLive(string FlowId, string Keywords)
        {
            db.CommandText = @" 
UPDATE KM_file_Keywords
   SET updatetime = getdate(),
       isDel = 0
 WHERE FlowId=@id and Keywords = @Keywords
";
            db.Parameters.Clear();
            db.Parameters.Add("@id", SqlDbType.NVarChar, FlowId);
            db.Parameters.Add("@Keywords", SqlDbType.NVarChar, Keywords);

            db.ExecuteNonQuery();

        }


    }
}