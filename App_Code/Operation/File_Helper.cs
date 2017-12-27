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
    /// File_Helper 的摘要描述
    /// </summary>
    public class File_Helper
    {
        public File_Helper()
        {
            //
            // TODO: 在這裡新增建構函式邏輯
            //
        }
        DbHelper db = new DbHelper();






        public GisTable queryFileList(string path, string name, string filetype, bool isVirt)
        {
            db.CommandText = @" 
        SELECT *
        FROM KM_file
        WHERE file_path = @path
          AND file_name = @name
          AND file_type = @filetype
";
            if (filetype != "D") { 
            db.CommandText +="          AND isVirt = @isVirt "; 
            }
            db.Parameters.Clear();
            db.Parameters.Add("@path", SqlDbType.NVarChar, path);
            db.Parameters.Add("@name", SqlDbType.NVarChar, name);
            db.Parameters.Add("@filetype", SqlDbType.NVarChar, filetype);
            db.Parameters.Add("@isVirt", SqlDbType.Bit, isVirt);

            return db.GetTable();
        }
        public GisTable queryVirtFileList(string path )
        {
            db.CommandText = @" 
        SELECT *
        FROM KM_file
        WHERE file_path = @path
          AND isVirt = 1 
          AND isDel = 0
 ";
            db.Parameters.Clear();
            db.Parameters.Add("@path", SqlDbType.NVarChar, path);


            return db.GetTable();
        }
        public DataTable queryFileTempList(string path)
        {
            db.CommandText = @" 
        SELECT *
        FROM KM_File_Temp_Path
        WHERE file_path = @path 
";

            db.Parameters.Clear();
            db.Parameters.Add("@path", SqlDbType.NVarChar, path);

            return db.GetTable();
        }
        public DataTable queryKeyWord(string txt)
        {
            db.CommandText = @" 
SELECT DISTINCT TOP 10 keyword 
FROM(
    SELECT file_name keyword
    FROM [KM_file] where file_name like  @keyword AND isDel = 0
    UNION
    SELECT Keywords
    FROM KM_file_Keywords  where Keywords like @keyword) M
     
";

            db.Parameters.Clear();
            db.Parameters.Add("@keyword", SqlDbType.NVarChar,  txt + "%");

            return db.GetTable();
        }

        public DataTable queryKeyWordPath(string txt)
        {
            db.CommandText = @" 
 SELECT file_path,
        file_name,
        file_type,
        isVirt,
        link,
        case when Annotation is null 
		   then '尚無檔案說明' 
	        else Annotation end Annotation
 FROM [KM_file]
 LEFT JOIN KM_file_Annotation 
   ON KM_file.FlowId = KM_file_Annotation.FlowId
WHERE( file_name LIKE @keyword
   OR KM_file.FlowId in ( 
                          SELECT DISTINCT FlowId
                          FROM KM_file_Keywords
                          WHERE Keywords LIKE @keyword )
     )
  AND isDel = 0
ORDER BY file_type, file_name;
";

            db.Parameters.Clear();
            db.Parameters.Add("@keyword", SqlDbType.NVarChar,   txt + "%");

            return db.GetTable();
        }



        public DataTable querySugKeyWord(string txt)
        {
            db.CommandText = @" 
SELECT DISTINCT TOP 10 keyword ,Metering
FROM(
    SELECT file_name keyword,Metering
    FROM [KM_file] where Metering > 0 and file_name like  @keyword AND isDel = 0
    UNION
    SELECT Keywords,Metering
    FROM KM_file_Keywords  where Keywords like @keyword ) M
Order by Metering 
";

            db.Parameters.Clear();
            db.Parameters.Add("@keyword", SqlDbType.NVarChar, "%" + txt + "%");

            return db.GetTable();
        }



        public DataTable GridGetingPath(string grid)
        {
            db.CommandText = @" 
SELECT *
FROM KM_file
WHERE FlowId = @FlowId 
";

            db.Parameters.Clear();
            db.Parameters.Add("@FlowId", SqlDbType.NVarChar, grid);
            return db.GetTable();
        }
        public void insertFile(string path, string name,string link, string filetype, bool isVal)
        {

            db.CommandText = @" 
INSERT INTO dbo.KM_file
       ( file_path,file_name,link,file_type,updatetime ,isVirt)
VALUES( @file_path, @file_name,@link, @file_type, getdate() , @isVirt )

                                ";
            db.Parameters.Clear();
            db.Parameters.Add("@isVirt", SqlDbType.NVarChar, isVal);
            db.Parameters.Add("@file_path", SqlDbType.NVarChar, path);
            db.Parameters.Add("@file_name", SqlDbType.NVarChar, name);
            db.Parameters.Add("@file_type", SqlDbType.NVarChar, filetype);
            db.Parameters.Add("@link", SqlDbType.NVarChar, link);

            db.ExecuteNonQuery();

        }
        public void insertTempFile(string path, string TempPath)
        {


            db.CommandText = @" 
INSERT INTO dbo.KM_File_Temp_Path
       ( file_path,
         file_guid_path,
         file_createtime
       )
VALUES( @file_path, @file_guid_path, GETDATE());
                                ";


            db.Parameters.Clear();
            db.Parameters.Add("@file_path", SqlDbType.NVarChar, path);
            db.Parameters.Add("@file_guid_path", SqlDbType.NVarChar, TempPath);
 
            db.ExecuteNonQuery();
        }
        public void updateFileLive(string FlowId)
        {
            db.CommandText = @" 
UPDATE KM_file
   SET updatetime = getdate(),
       isDel = 0
 WHERE FlowId=@id
";
            db.Parameters.Clear();
            db.Parameters.Add("@id", SqlDbType.NVarChar, FlowId);
            db.ExecuteNonQuery();

        }
        public void updateForderForTrue(string FlowId)
        {
            db.CommandText = @" 
UPDATE KM_file
   SET updatetime = getdate(),
       isVirt = 0
 WHERE FlowId=@id
";
            db.Parameters.Clear();
            db.Parameters.Add("@id", SqlDbType.NVarChar, FlowId);
            db.ExecuteNonQuery();

        }
        public void updateFileDie()
        {
            db.CommandText = @" 
UPDATE KM_file
   SET updatetime = getdate(),
       isDel = 1
 WHERE isVirt= 0
";
            db.Parameters.Clear();
            db.ExecuteNonQuery();
        }
        public void updateFileDelete(string path, string FName , string DataTypes, bool isVirt)
        {
            db.CommandText = @" 
UPDATE KM_file
  SET updatetime = GETDATE(),
      isDel = 1
WHERE file_path = @path
  AND file_name = @name
  AND file_type = @filetype
  AND isVirt = @isVirt;
";
            db.Parameters.Clear();
            db.Parameters.Add("@path", SqlDbType.NVarChar, path);
            db.Parameters.Add("@name", SqlDbType.NVarChar, FName);
            db.Parameters.Add("@filetype", SqlDbType.NVarChar, DataTypes);
            db.Parameters.Add("@isVirt", SqlDbType.Bit, isVirt);

            db.ExecuteNonQuery();
        }
        public void updateFileState(string path, string FName, string link, string DataTypes, bool isVirt)
        {

            DataTable dt = queryFileList(path, FName, DataTypes, isVirt);
            if (dt.Rows.Count > 0)
            {

                updateFileLive(dt.Rows[0]["FlowId"].ToString());
                if (DataTypes == "D" && isVirt == false)
                {
                    updateForderForTrue(dt.Rows[0]["FlowId"].ToString());
                }
            }
            else
            {
                insertFile(path, FName, link, DataTypes, isVirt);
            }

        }



    }
}