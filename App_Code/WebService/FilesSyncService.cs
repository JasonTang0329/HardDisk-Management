using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Collections;
using System.Web.Script.Serialization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using GISFCU.Extention;
using System.IO;

/// <summary>
/// FilesSyncService 的摘要描述
/// </summary>
[WebService(Namespace = "http://tm.gis.tw/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[System.Web.Script.Services.ScriptService]
public class FilesSyncService : System.Web.Services.WebService {


    public FilesSyncService() {

        //如果使用設計的元件，請取消註解下行程式碼 
        //string SubPath = "StringDecrypt(zSubPath)";


    }

    [WebMethod]
    public Response CheckFile(string zCatalogName, string zSubPath, string zFileName)
    {
        string CatalogName = StringDecrypt(zCatalogName);
        string SubPath = StringDecrypt(zSubPath);
        string FileName = StringDecrypt(zFileName);

        Response Rs = new Response();
        if ((CatalogName != "") && (FileName != ""))
        {
            try
            {
                FilesSyncConfigDtype.RemoteSyncDtype.ServerDtype.CatalogDtype Catalog = SyncFilesPutCatalog(CatalogName);
                //Rs.Content = StringEncrypt(Catalog.MapPath);
                string FileFullPath = Path.GetFullPath(Catalog.MapPath + "\\" + SubPath + FileName);
                Rs.Content = StringEncrypt(SHA256Crypto.EncryptFile(FileFullPath));
                Rs.State = "S";
            }
            catch (Exception ex)
            {
                Rs.State = "F";
                Rs.ErrorMsg = ex.Message;
            }
        }
        else
        {
            Rs.State = "F";
            Rs.ErrorMsg = StringEncrypt("傳入的參數為空值或不正確");
        }

        return Rs;
    }


    [WebMethod]
    public Response PutFile(string zCatalogName, string zSubPath, string zFileName)
    {
        string CatalogName = StringDecrypt(zCatalogName);
        string SubPath = StringDecrypt(zSubPath);
        string FileName = StringDecrypt(zFileName);

        Response Rs = new Response();
        if ((CatalogName != "") && (FileName != ""))
        {
            try
            {
                FilesSyncConfigDtype.RemoteSyncDtype.ServerDtype.CatalogDtype Catalog = SyncFilesPutCatalog(CatalogName);
                //Rs.Content = StringEncrypt(Catalog.MapPath);
                string FileFullPath = Path.GetFullPath(Catalog.MapPath + "\\" + SubPath + FileName);
                Rs.Content = StringEncrypt(SHA256Crypto.EncryptFile(FileFullPath));
                Rs.State = "S";
            }
            catch (Exception ex)
            {
                Rs.State = "F";
                Rs.ErrorMsg = ex.Message;
            }
        }
        else
        {
            Rs.State = "F";
            Rs.ErrorMsg = StringEncrypt("傳入的參數為空值或不正確");
        }

        return Rs;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    private FilesSyncConfigDtype.RemoteSyncDtype.ServerDtype.CatalogDtype SyncFilesPutCatalog(string name)
    {
        FilesSyncConfigDtype.RemoteSyncDtype.ServerDtype.CatalogDtype Catalog = null;
        FilesSyncConfigDtype Fsc = FilesSyncConfigDtype.WebGetObject();

        foreach (FilesSyncConfigDtype.RemoteSyncDtype.ServerDtype Server in Fsc.RemoteSync.Server)
        {
            foreach (FilesSyncConfigDtype.RemoteSyncDtype.ServerDtype.CatalogDtype Cg in Server.SyncFilesPut.Catalog)
            {
                if (Cg.Name == name)
                {
                    Catalog = Cg;
                }
            }
        }
        return Catalog;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="Data"></param>
    /// <returns></returns>
    private string StringEncrypt(string Data)
    {
        string ReturnValue = "";
        using (EzAESCrypto EasyAES1 = new EzAESCrypto())
        {
            try
            {
                //設定加密的Key及IV, 加密
                EasyAES1.Key = "lG8LhTJJVbnWtbEz1WJQf+/QSn3E8YzSkbGdqyD8jNo=";
                EasyAES1.IV = "IDpnIgpMy6pDptFCmZZ09g==";
                ReturnValue = EasyAES1.EncryptTextToBase64String(ref Data);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }
        return ReturnValue;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="EncryptData"></param>
    /// <returns></returns>
    private string StringDecrypt(string EncryptData)
    {
        string ReturnValue = "";
        using (EzAESCrypto EasyAES1 = new EzAESCrypto())
        {
            try
            {
                //設定解密的Key及IV, 解密
                EasyAES1.Key = "lG8LhTJJVbnWtbEz1WJQf+/QSn3E8YzSkbGdqyD8jNo=";
                EasyAES1.IV = "IDpnIgpMy6pDptFCmZZ09g==";
                ReturnValue = EasyAES1.DecryptText(ref EncryptData);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }
        return ReturnValue;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="Data"></param>
    /// <param name="EncryptData"></param>
    /// <returns></returns>
    private bool BytesZipEncrypt(ref byte[] Data, out string EncryptData)
    {
        bool ReturnValue = false;
        EncryptData = "";
        try
        {
            using (EzAESCrypto EasyAES1 = new EzAESCrypto())
            {
                EzZipData objEZF = new EzZipData();
                MemoryStream ZipDataMS = objEZF.ZipData(ref Data, EzZipData.CompressionLevel.FastCompression);

                //設定加密的Key及IV, 加密
                EasyAES1.Key = "lG8LhTJJVbnWtbEz1WJQf+/QSn3E8YzSkbGdqyD8jNo=";
                EasyAES1.IV = "IDpnIgpMy6pDptFCmZZ09g==";
                EncryptData = EasyAES1.EncryptDataToBase64String(ZipDataMS.ToArray());

                ReturnValue = true;
            }
        }
        catch (Exception ex)
        {
            Console.Write(ex.Message);
        }
        return ReturnValue;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="EncryptData"></param>
    /// <param name="Data"></param>
    /// <returns></returns>
    private bool BytesDecryptUnZip(ref string EncryptData, out byte[] Data)
    {
        bool ReturnValue = false;
        Data = null;
        try
        {
            using (EzAESCrypto EasyAES1 = new EzAESCrypto())
            {
                //設定解密的Key及IV, 解密
                EasyAES1.Key = "lG8LhTJJVbnWtbEz1WJQf+/QSn3E8YzSkbGdqyD8jNo=";
                EasyAES1.IV = "IDpnIgpMy6pDptFCmZZ09g==";
                MemoryStream ZipDataMS = EasyAES1.DecryptDataToMemoryStream(ref EncryptData);

                EzZipData objEZF = new EzZipData();
                Data = objEZF.UnZipData(ref ZipDataMS);
                ReturnValue = true;
            }
        }
        catch (Exception ex)
        {
            Console.Write(ex.Message);
        }
        return ReturnValue;
    }
}
