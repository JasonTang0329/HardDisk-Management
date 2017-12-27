using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Word = Microsoft.Office.Interop.Word;
using Excel = Microsoft.Office.Interop.Excel;
using PowerPoint = Microsoft.Office.Interop.PowerPoint;
using Microsoft.Office.Core;

public partial class ViewFile : System.Web.UI.Page
{
    protected string path = "";
    protected string ext = "";
    protected string filename = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        path = Crypto.DecryptStringAES(Request["path"]);

        if (!IsPostBack)
        {

            if (!string.IsNullOrEmpty(path))
            {

                filename = Server.UrlDecode(Path.GetFileName(path));
                ext = Path.GetExtension(path).ToLower();
                labtitle.Text = filename;
                initpage();

            }

        }
    }
    /// <summary>
    /// 頁面綁訂
    /// </summary>
    private void initpage()
    {
        string src = Server.UrlDecode(path);
        movie.Visible = (ext == ".mp4");
        img.Visible = checkimg(ext);
        docview.Visible = false;
        //判斷檔案型態，選擇展示檔案的方式
        switch (ext)
        {
            case ".mp4":
                video.Attributes.Add("src", src);
                movie.Attributes.Add("autoplay", "autoplay");
                break;
            case ".jpg":
            case ".jpeg":
            case ".gif":
            case ".png":
                img.Src = src;
                break;
            case ".doc":
            case ".docx":
            case ".xls":
            case ".xlsx":
            case ".ppt":
            case ".pptx":
            case ".pdf":
            case ".html":
            case ".xml":
            case ".txt":

                if (ext != ".pdf" && ext != ".html" && ext != ".xml" && ext != ".txt")
                {
                    //http://smlboby.blogspot.tw/2013/07/aspnet-microsoftofficeinteropexcel-excel.html  
                    //以下三種格式皆透過Microsoft.Office.Interop轉換為PDF執行，因此請務必確認權限部份是否已經開啟才能確保轉換無虞
                    string folder = Guid.NewGuid().ToString();
                    string filePath = System.Web.HttpContext.Current.Server.MapPath("~/Temp/" + folder);
                    string pdffilename = Path.GetFileNameWithoutExtension(filename) + ".pdf";
                    Directory.CreateDirectory(filePath);
                    string sourcePath = System.Web.HttpContext.Current.Server.MapPath(@"~/") + Server.UrlDecode(src);

                    string targetFile = filePath + @"\" + pdffilename;
                    if (ext == ".doc" || ext == ".docx")
                    {
                        DOCConvertToPDF(sourcePath, targetFile);
                        GC.Collect();
                        GC.WaitForPendingFinalizers();

                    }
                    if (ext == ".xls" || ext == ".xlsx")
                    {
                        XLSConvertToPDF(sourcePath, targetFile);
                        GC.Collect();
                        GC.WaitForPendingFinalizers();

                    }

                    if (ext == ".ppt" || ext == ".pptx")
                    {
                        PPTConvertToPDF(sourcePath, targetFile);
                        GC.Collect();
                        GC.WaitForPendingFinalizers();

                    }
                    src = @"Temp\" + folder + @"\" + pdffilename;
                    System.IO.Directory.Delete(sourcePath.Replace(@"/" + filename, ""), true);
                }
                docview.Attributes.Add("src", src);
                docview.Visible = true;
                break;
            default:
                break;


        }

    }
    /// <summary>
    /// 確認圖片格式
    /// </summary>
    /// <param name="ext"></param>
    /// <returns></returns>
    private bool checkimg(string ext)
    {
        bool isimg = false;
        string[] imagetype = { ".jpg", ".jpeg", ".gif", ".png" };
        foreach (string t in imagetype)
        {
            if (ext == t)
            {
                isimg = true;
                break;
            }
        }
        return isimg;
    }

    /// <summary>
    /// Word轉換pdf
    /// </summary>
    /// <param name="sourcePath">原始文件路径</param>
    /// <param name="targetPath">目標文件路径</param>
    /// <returns>true=轉換成功</returns>
    public static bool DOCConvertToPDF(string sourcePath, string targetPath)
    {
        bool result = false;
        Word.WdExportFormat exportFormat = Word.WdExportFormat.wdExportFormatPDF;
        object paramMissing = Type.Missing;
        Word.ApplicationClass wordApplication = new Word.ApplicationClass();
        Word.Document wordDocument = null;
        try
        {
            object paramSourceDocPath = sourcePath;
            string paramExportFilePath = targetPath;
            Word.WdExportFormat paramExportFormat = exportFormat;
            bool paramOpenAfterExport = false;
            Word.WdExportOptimizeFor paramExportOptimizeFor = Word.WdExportOptimizeFor.wdExportOptimizeForPrint;
            Word.WdExportRange paramExportRange = Word.WdExportRange.wdExportAllDocument;
            int paramStartPage = 0;
            int paramEndPage = 0;
            Word.WdExportItem paramExportItem = Word.WdExportItem.wdExportDocumentContent;
            bool paramIncludeDocProps = true;
            bool paramKeepIRM = true;
            Word.WdExportCreateBookmarks paramCreateBookmarks = Word.WdExportCreateBookmarks.wdExportCreateWordBookmarks;
            bool paramDocStructureTags = true;
            bool paramBitmapMissingFonts = true;
            bool paramUseISO19005_1 = false;
            wordDocument = wordApplication.Documents.Open(
                ref paramSourceDocPath, ref paramMissing, ref paramMissing,
                ref paramMissing, ref paramMissing, ref paramMissing,
                ref paramMissing, ref paramMissing, ref paramMissing,
                ref paramMissing, ref paramMissing, ref paramMissing,
                ref paramMissing, ref paramMissing, ref paramMissing,
                ref paramMissing);
            if (wordDocument != null)
                wordDocument.ExportAsFixedFormat(paramExportFilePath,
                    paramExportFormat, paramOpenAfterExport,
                    paramExportOptimizeFor, paramExportRange, paramStartPage,
                    paramEndPage, paramExportItem, paramIncludeDocProps,
                    paramKeepIRM, paramCreateBookmarks, paramDocStructureTags,
                    paramBitmapMissingFonts, paramUseISO19005_1,
                    ref paramMissing);
            result = true;
        }
        catch
        {
            result = false;
        }
        finally
        {
            if (wordDocument != null)
            {
                wordDocument.Close(ref paramMissing, ref paramMissing, ref paramMissing);
                wordDocument = null;
            }
            if (wordApplication != null)
            {
                wordApplication.Quit(ref paramMissing, ref paramMissing, ref paramMissing);
                wordApplication = null;
            }
        }
        return result;
    }

    /// <summary>
    /// 把Excel文件轉換成PDF格式文件  
    /// </summary>
    /// <param name="sourcePath">原始文件文件路径</param>
    /// <param name="targetPath">目標文件路径</param>
    /// <returns>true=轉換成功</returns>
    public static bool XLSConvertToPDF(string sourcePath, string targetPath)
    {
        bool result = false;
        Excel.XlFixedFormatType targetType = Excel.XlFixedFormatType.xlTypePDF;
        object missing = Type.Missing;
        Excel.ApplicationClass application = null;
        Excel.Workbook workBook = null;
        try
        {
            application = new Excel.ApplicationClass();
            object target = targetPath;
            object type = targetType;
            workBook = application.Workbooks.Open(sourcePath, missing, missing, missing, missing, missing,
                missing, missing, missing, missing, missing, missing, missing, missing, missing);
            workBook.ExportAsFixedFormat(targetType, target, Excel.XlFixedFormatQuality.xlQualityStandard, true, false, missing, missing, missing, missing);
            result = true;
        }
        catch
        {
            result = false;
        }
        finally
        {
            if (workBook != null)
            {
                workBook.Close(true, missing, missing);
                workBook = null;
            }
            if (application != null)
            {
                application.Quit();
                application = null;
            }
        }
        return result;
    }
    ///<summary>        
    ///powerpoint轉換為pdf       
    ///</summary>        
    ///<param name="sourcePath">原始文件文件路径</param>     
    ///<param name="targetPath">目標文件路径</param> 
    ///<returns>true=轉換成功</returns> 
    public static bool PPTConvertToPDF(string sourcePath, string targetPath)
    {
        bool result;
        PowerPoint.PpSaveAsFileType targetFileType = PowerPoint.PpSaveAsFileType.ppSaveAsPDF;
        object missing = Type.Missing;
        PowerPoint.ApplicationClass application = null;
        PowerPoint.Presentation persentation = null;
        try
        {
            application = new PowerPoint.ApplicationClass();
            persentation = application.Presentations.Open(sourcePath, MsoTriState.msoTrue, MsoTriState.msoFalse, MsoTriState.msoFalse); persentation.SaveAs(targetPath, targetFileType, Microsoft.Office.Core.MsoTriState.msoTrue);
            result = true;
        }
        catch
        {
            result = false;
        }
        finally
        {
            if (persentation != null)
            {
                persentation.Close();
                persentation = null;
            }
            if (application != null)
            {
                application.Quit();
                application = null;
            }
        }
        return result;
    }

}