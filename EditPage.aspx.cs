using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class EditPage : System.Web.UI.Page
{
    HardDiskManage.KM_Filer.File_Helper FH = new HardDiskManage.KM_Filer.File_Helper();
    HardDiskManage.File_Annotation_Helper.File_Annotation_Helper FAH = new HardDiskManage.File_Annotation_Helper.File_Annotation_Helper();
    HardDiskManage.CommonFunc.CommonFunc Common = new HardDiskManage.CommonFunc.CommonFunc();

    protected string path = "";
    protected string filename = "";
    protected string ftype = "";
    protected bool isVirt;
    protected string FlowId   //KM_file.FlowId
    {
        get
        {
            return ViewState["FlowId"] as string ?? "";
        }
        set { ViewState["FlowId"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        filename = Path.GetFileName(Request["path"]);   //從PATH取得檔案名稱
        ftype = Request["ftype"];                       //檔案型態 D=資料夾 F=檔案 L=連結
        isVirt = Convert.ToBoolean(Request["isVirt"]);  //是否為虛擬路徑
        path = Request["path"].Replace(@"/", @"\");     //從Javascript轉入路徑

        //判斷是否為初始路徑，是的話則不去除檔名
        if (path != ConfigurationManager.AppSettings["rootPath"])
        {
            path = path.Replace(@"\" + filename, "");
        }
        


        CKEditorControl1.Visible = !String.IsNullOrEmpty(path);

        if (!IsPostBack)
        {
            FlowId = null;
            initcontrol();

        }
    }
    /// <summary>
    /// 初始控制項綁訂
    /// </summary>
    private void initcontrol()
    {
        FH.updateFileState(path, filename, "", ftype, isVirt);
        FlowId = FH.queryFileList(path, filename, ftype, isVirt).Rows[0]["FlowId"].ToString();
        DataTable dt = FAH.queryAnnotation(FlowId);
        CKEditorControl1.Text = (dt.Rows.Count > 0 ? dt.Rows[0]["Annotation"].ToString() : "");
        txtkeyword.Text = (dt.Rows.Count > 0 && dt.Rows[0]["keyword"].ToString() != "" ? dt.Rows[0]["keyword"].ToString() : "");

    }
    /// <summary>
    /// 返回主畫面並更新內容
    /// </summary>
    /// <param name="msg"></param>
    private void backToReflash(string msg)
    {
        string msgbox = "parent.bootbox.alert('" + msg + "');";

        string Refresh = "parent.fileAnnotationFunc('" + ftype + "','" + path.Replace(@"\", @"/")+"/"+ filename + "','"  + isVirt + "');";

        string jsClose = "parent.closeModal();parent.modelresize();";

        ScriptManager.RegisterStartupScript(this, this.GetType(), "prompt", "javascript:" + msgbox + Refresh + jsClose, true);
    }
    /// <summary>
    /// 儲存
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {

        try
        {
            string Annotation = CKEditorControl1.Text;
            FAH.updateAnnotation(FlowId, Annotation);
            string keywordarr = txtkeyword.Text.Trim();
            FAH.updateKeyWordDie(FlowId);
            foreach (string word in keywordarr.Split(','))
            {
                if (word.Trim() != "")
                {
                    FAH.updateKeyWords(FlowId, word);
                }
            }

            backToReflash("儲存成功");
        }
        catch (Exception ex)
        {

            throw ex;
        }

    }
}