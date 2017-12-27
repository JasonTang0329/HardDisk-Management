using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace GISFCU.Extention
{
    /// <summary>
    /// 
    /// </summary>
    public class EzAESCrypto : IDisposable
    {
        /// <summary></summary>
        public string Key;
        /// <summary></summary>
        public string IV;
        /// <summary></summary>
        private AesCryptoServiceProvider AEScsp;
        /// <summary>Flag: Has Dispose already been called?</summary>
        private bool disposed = false;


        /// <summary></summary>
        public EzAESCrypto()
        {
            AEScsp = new AesCryptoServiceProvider();
            AEScsp.KeySize = 256;
            Init();
        }


        /// <summary></summary>
        public void Dispose()
        {
            AEScsp.Dispose();
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        /// <summary>Protected implementation of Dispose pattern.</summary>
        protected virtual void Dispose(bool disposing)
        {
            if (disposed) { return; }
            if (disposing)
            {
                // Free any other managed objects here.
                AEScsp.Dispose();
            }
            // Free any unmanaged objects here.

            disposed = true;
        }
        

        /// <summary></summary>
        public void GenerateNewKey()
        {
            AEScsp.GenerateKey();
            Key = Convert.ToBase64String(AEScsp.Key);
        }


        /// <summary></summary>
        public void GenerateNewIV()
        {
            AEScsp.GenerateIV();
            IV = Convert.ToBase64String(AEScsp.IV);
        }


        /// <summary></summary>
        /// <param name="PlainFileName"></param>
        /// <param name="CipherFileName"></param>
        public void EncryptFile(string PlainFileName, string CipherFileName)
        {
            using (FileStream sourceStream = new FileStream(PlainFileName, FileMode.Open, FileAccess.Read))
            {
                using (FileStream encryptStream = new FileStream(CipherFileName, FileMode.Create, FileAccess.Write))
                {
                    //檔案加密
                    byte[] dataByteArray = new byte[sourceStream.Length];
                    sourceStream.Read(dataByteArray, 0, dataByteArray.Length);

                    using (CryptoStream cs = new CryptoStream(encryptStream, new AesCryptoServiceProvider().CreateEncryptor(Convert.FromBase64String(Key), Convert.FromBase64String(IV)), CryptoStreamMode.Write))
                    {
                        cs.Write(dataByteArray, 0, dataByteArray.Length);
                        cs.FlushFinalBlock();
                        //Close the streams.
                        cs.Close();
                    }
                    //Close the streams.
                    encryptStream.Close();
                }
                //Close the streams.
                sourceStream.Close();
            }
        }


        /// <summary>檔案解密</summary>
        /// <param name="CipherFileName">來源加密檔名稱(含路徑)</param>
        /// <param name="PlainFileName">目的檔案名稱(含路徑)</param>
        public void DecryptFile(string CipherFileName, string PlainFileName)
        {
            using (FileStream encryptStream = new FileStream(CipherFileName, FileMode.Open, FileAccess.Read))
            {
                using (FileStream decryptStream = new FileStream(PlainFileName, FileMode.Create, FileAccess.Write))
                {
                    byte[] dataByteArray = new byte[encryptStream.Length];
                    encryptStream.Read(dataByteArray, 0, dataByteArray.Length);
                    using (CryptoStream cs = new CryptoStream(decryptStream, new AesCryptoServiceProvider().CreateDecryptor(Convert.FromBase64String(Key), Convert.FromBase64String(IV)), CryptoStreamMode.Write))
                    {
                        cs.Write(dataByteArray, 0, dataByteArray.Length);
                        cs.FlushFinalBlock();
                        //Close the streams.
                        cs.Close();
                    }
                    //Close the streams.
                    decryptStream.Close();
                }
                //Close the streams.
                encryptStream.Close();
            }
        }


        /// <summary></summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        public byte[] EncryptDataToByteArray(byte[] Data)
        {
            byte[] ReturnValue;

            //Create a MemoryStream.
            using (MemoryStream mStream = new MemoryStream())
            {
                //Create a CryptoStream using the MemoryStream and the passed key and initialization vector (IV).
                using (CryptoStream cs = new CryptoStream(mStream, new AesCryptoServiceProvider().CreateEncryptor(Convert.FromBase64String(Key), Convert.FromBase64String(IV)), CryptoStreamMode.Write))
                {
                    //Write the byte array to the crypto stream and flush it.
                    cs.Write(Data, 0, Data.Length);
                    cs.FlushFinalBlock();
                    //Close the streams.
                    cs.Close();
                }
                //Get an array of bytes from the MemoryStream that holds the encrypted data.
                ReturnValue = mStream.ToArray();
                //Close the streams.
                mStream.Close();
            }

            //Return the encrypted buffer.
            return ReturnValue;
        }


        /// <summary></summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        public string EncryptDataToBase64String(byte[] Data)
        {
            return Convert.ToBase64String(EncryptDataToByteArray(Data));
        }


        /// <summary></summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        public byte[] EncryptTextToByteArray(ref string Data)
        {
            byte[] ReturnValue;

            //Create a MemoryStream.
            using (MemoryStream mStream = new MemoryStream())
            {
                //Create a CryptoStream using the MemoryStream and the passed key and initialization vector (IV).
                using (CryptoStream cs = new CryptoStream(mStream, new AesCryptoServiceProvider().CreateEncryptor(Convert.FromBase64String(Key), Convert.FromBase64String(IV)), CryptoStreamMode.Write))
                {
                    //Convert the passed string to a byte array.
                    byte[] toEncrypt = new UnicodeEncoding().GetBytes(Data);

                    //Write the byte array to the crypto stream and flush it.
                    cs.Write(toEncrypt, 0, toEncrypt.Length);
                    cs.FlushFinalBlock();
                    //Close the streams.
                    cs.Close();
                }
                //Get an array of bytes from the MemoryStream that holds the encrypted data.
                ReturnValue = mStream.ToArray();
                //Close the streams.
                mStream.Close();
            }

            //Return the encrypted buffer.
            return ReturnValue;
        }


        /// <summary></summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        public string EncryptTextToBase64String(ref string Data)
        {
            return Convert.ToBase64String(EncryptTextToByteArray(ref Data));
        }


        /// <summary></summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        public MemoryStream DecryptDataToMemoryStream(byte[] Data)
        {
            //Create a new MemoryStream using the passed array of decrypted data.
            MemoryStream msReturn = new MemoryStream();

            //Create a CryptoStream using the MemoryStream and the passed key and initialization vector (IV).
            using (CryptoStream cs = new CryptoStream(msReturn, new AesCryptoServiceProvider().CreateDecryptor(Convert.FromBase64String(Key), Convert.FromBase64String(IV)), CryptoStreamMode.Write))
            {
                cs.Write(Data, 0, Data.Length);
                cs.FlushFinalBlock();
                //Close the streams.
                cs.Close();
            }

            //set Position = 0 and return it.
            msReturn.Position = 0;
            return msReturn;
        }


        /// <summary></summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        public MemoryStream DecryptDataToMemoryStream(ref string Data)
        {
            byte[] tmpValue = Convert.FromBase64String(Data);
            return DecryptDataToMemoryStream(tmpValue);
        }


        /// <summary></summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        public string DecryptText(byte[] Data)
        {
           string ReturnValue;

            //Create a new MemoryStream using the passed array of decrypted data.
            using (MemoryStream msDecrypt = new MemoryStream())
            {

                //Create a CryptoStream using the MemoryStream and the passed key and initialization vector (IV).
                using (CryptoStream cs = new CryptoStream(msDecrypt, new AesCryptoServiceProvider().CreateDecryptor(Convert.FromBase64String(Key), Convert.FromBase64String(IV)), CryptoStreamMode.Write))
                {
                    cs.Write(Data, 0, Data.Length);
                    cs.FlushFinalBlock();
                    //Close the streams.
                    cs.Close();
                }
                //Get an array of bytes from the MemoryStream that holds the decrypted data.
                ReturnValue = new UnicodeEncoding().GetString(msDecrypt.ToArray());
                //Close the streams.
                msDecrypt.Close();
            }

            //Convert the buffer into a string and return it.
            return ReturnValue;
        }


        /// <summary></summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        public string DecryptText(ref string Data)
        {
            byte[] tmpValue = Convert.FromBase64String(Data);
            return DecryptText(tmpValue);
        }


        /// <summary></summary>
        private void Init()
        {
            Key = @"VCzBD/AKrRQCp7gYV2getSVUWqdUfpLLt2sx6DPVtgY=";
            IV = @"gYi2qLzWNDK95lqik3XKog==";
        }
    
    
    }
}
