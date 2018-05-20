using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

/// <summary>
/// Crypto 的摘要描述
/// </summary>
public class Crypto
{
	public Crypto()
	{
		//
		// TODO: 在這裡新增建構函式邏輯
		//
	}
    //解密資料
    public static string DecryptStringAES(string cipherText)
    {
        var keybytes = Encoding.UTF8.GetBytes("1122112211221122");   //自行設定，但要與JavaScript端 一致
        var iv = Encoding.UTF8.GetBytes("1122112211221122"); // 自行設定，但要與JavaScript端 一致
        var encrypted = Convert.FromBase64String(cipherText);
        var decriptedFromJavascript = DecryptStringFromBytes(encrypted, keybytes, iv);
        return string.Format(decriptedFromJavascript);
    }

    private static string DecryptStringFromBytes(byte[] cipherText, byte[] key, byte[] iv)
    {
        if (cipherText == null || cipherText.Length <= 0)
        {
            throw new ArgumentNullException("cipherText");
        }
        if (key == null || key.Length <= 0)
        {
            throw new ArgumentNullException("key");
        }
        if (iv == null || iv.Length <= 0)
        {
            throw new ArgumentNullException("key");
        }
        string plaintext = null;
        using (var rijAlg = new RijndaelManaged())
        {
            rijAlg.Mode = CipherMode.CBC;
            rijAlg.Padding = PaddingMode.PKCS7;
            rijAlg.FeedbackSize = 128;
            rijAlg.Key = key;
            rijAlg.IV = iv;
            var decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);
            try
            {
                using (var msDecrypt = new MemoryStream(cipherText))
                {
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (var srDecrypt = new StreamReader(csDecrypt))
                        {
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
            catch
            {
                plaintext = "keyError";
            }
        }
        return plaintext;
    }
    //加密資料
    public static string EncryptStringAES(string cipherText)
    {
        var keybytes = Encoding.UTF8.GetBytes("1122112211221122");   //自行設定
        var iv = Encoding.UTF8.GetBytes("1122112211221122");         //自行設定
        var EncryptString = EncryptStringToBytes(cipherText, keybytes, iv);
        return Convert.ToBase64String(EncryptString);
    }
    private static byte[] EncryptStringToBytes(string plainText, byte[] key, byte[] iv)
    {
        if (plainText == null || plainText.Length <= 0)
        {
            throw new ArgumentNullException("plainText");
        }
        if (key == null || key.Length <= 0)
        {
            throw new ArgumentNullException("key");
        }
        if (iv == null || iv.Length <= 0)
        {
            throw new ArgumentNullException("key");
        }
        byte[] encrypted;
        using (var rijAlg = new RijndaelManaged())
        {
            rijAlg.Mode = CipherMode.CBC;
            rijAlg.Padding = PaddingMode.PKCS7;
            rijAlg.FeedbackSize = 128;
            rijAlg.Key = key;
            rijAlg.IV = iv;
            var encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);
            using (var msEncrypt = new MemoryStream())
            {
                using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (var swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(plainText);
                    }
                    encrypted = msEncrypt.ToArray();
                }
            }
        }
        return encrypted;
    }

}