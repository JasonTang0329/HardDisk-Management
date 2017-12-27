using GISFCU.Data;
using GISFCU.Data.Sql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace HardDiskManage.KM_Comments_Filer
{
    /// <summary>
    /// File_Helper 的摘要描述
    /// </summary>
    public class File_Comments_Helper
    {
        public File_Comments_Helper()
        {
            //
            // TODO: 在這裡新增建構函式邏輯
            //
        }
        DbHelper db = new DbHelper();



        /// <summary>
        /// 查詢評論
        /// </summary>
        /// <param name="FlowId"></param>
        /// <returns></returns>
        public DataTable queryComments(string FlowId)
        {
            db.CommandText = @" 
        SELECT *,CONVERT(varchar, updatetime, 120 ) date
        FROM KM_file_Comments
        WHERE FlowId = @FlowId 
        Order by updatetime desc
";

            db.Parameters.Clear();
            db.Parameters.Add("@FlowId", SqlDbType.NVarChar, FlowId);

            return db.GetTable();
        }
        /// <summary>
        /// 插入評論
        /// </summary>
        /// <param name="FlowId"></param>
        /// <param name="Comments"></param>
        /// <param name="CommentsAuthor"></param>
        public void updateComments(string FlowId, string Comments, string CommentsAuthor)
        {
            //DataTable dt = queryComments(FlowId);
            //if (dt.Rows.Count > 0)
            //{
            //    updateComment(FlowId, Comments);
            //}
            //else
            //{
                insertComments(FlowId, Comments, CommentsAuthor);
            //}

        }
        /// <summary>
        /// 插入評論
        /// </summary>
        /// <param name="FlowId"></param>
        /// <param name="Comments"></param>
        /// <param name="CommentsAuthor"></param>
        private void insertComments(string FlowId, string Comments, string CommentsAuthor)
        {
            db.CommandText = @" 
INSERT INTO KM_file_Comments
       ( FlowId,
         Comments,
         updatetime,
         CommentsAuthor
       )
VALUES( @FlowId, @Comments, GETDATE(),@CommentsAuthor);
                                ";


            db.Parameters.Clear();
            db.Parameters.Add("@FlowId", SqlDbType.NVarChar, FlowId);
            db.Parameters.Add("@Comments", SqlDbType.NVarChar, Comments);
            db.Parameters.Add("@CommentsAuthor", SqlDbType.NVarChar, CommentsAuthor);

            db.ExecuteNonQuery();
        }
        private void updateComment(string FlowId, string Comments)
        {
            db.CommandText = @" 
UPDATE KM_file_Comments
   SET updatetime = getdate(),
       Comments = @Comments
 WHERE FlowId=@FlowId
";
            db.Parameters.Clear();
            db.Parameters.Add("@FlowId", SqlDbType.NVarChar, FlowId);
            db.Parameters.Add("@Comments", SqlDbType.NVarChar, Comments);
            db.ExecuteNonQuery();
        }


    }
}