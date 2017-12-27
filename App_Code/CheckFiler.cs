using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace HardDiskManage.KM_Filer
{
    /// <summary>
    /// 排程掃描NAS
    /// </summary>
    public class CheckFiler
    {
        HardDiskManage.KM_Filer.File_Helper fh = new HardDiskManage.KM_Filer.File_Helper();
        string domain = ConfigurationManager.AppSettings["domain"];
        string uid = ConfigurationManager.AppSettings["uid"];
        string pwd = ConfigurationManager.AppSettings["pwd"];

        public CheckFiler()
        {
            using (new Impersonator(uid, domain, pwd))
            {
                fh.updateFileDie();
                string rootpath = ConfigurationManager.AppSettings["rootPath"].ToString();
                fh.updateFileState(rootpath, Path.GetFileName(rootpath), "", "D", false);

                ReadFileSys(rootpath);
            }
            ClearTemp();
        }

        private void ClearTemp()
        {
            string TempPath = ConfigurationManager.AppSettings["codepath"].ToString() + @"\Temp";
            System.IO.DirectoryInfo di = new DirectoryInfo(TempPath);
            try
            { 
                foreach (DirectoryInfo dir in di.GetDirectories())
                {
                    if (Directory.Exists(dir.FullName))
                    {
                        dir.Delete(true);
                    }
                }
                foreach (FileInfo file in di.GetFiles())
                {
                    if (File.Exists(file.FullName))
                    {
                        file.Delete();
                    }
                }

            }
            catch (Exception ex) {
                throw ex;
            }
        }
        private void ReadFileSys(string path)
        {

            try
            {


                if (Directory.Exists(path))
                {
                    string[] Dir =  Directory.GetDirectories(path);
                    Array.Sort(Dir);
                    foreach (string DItem in Dir)
                    {
                        string Item = DItem.Replace(path + @"\", "");

                        if (!FilterFunc.filterDirector(Path.GetFileName(Item)) && !FilterFunc.filterPartFiler(Path.GetFileName(Item)))
                        {
                            string DName = DItem;

                            fh.updateFileState(path, DName.Replace(path + @"\", ""), "", "D", false);//

                            ReadFileSys(DName);
                        }
                    }

                    string[] Filesr = Directory.GetFiles(path);
                    Array.Sort(Filesr);
                    foreach (string FItem in Filesr)
                    {

                        string Item = FItem.Replace(path + @"\", "");

                        if (Item.Substring(0, 1) != "~" && Item.Substring(0, 1) != "." && !FilterFunc.filterFiler(Path.GetFileName(Item)) && !FilterFunc.filterPartFiler(Path.GetFileName(Item)))
                        {

                            fh.updateFileState(path, Item, "", "F", false);//
                        }
                    }
                }
                return;
            }

            catch (Exception ex)
            {
                ex.ToString();

            }
        }


    }
}