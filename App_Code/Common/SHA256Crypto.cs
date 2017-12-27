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
    public class SHA256Crypto
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="FileName"></param>
        public static string EncryptFile(string FileName)
        {
            string RtVal = "";

            try
            {

                using (FileStream sourceStream = new FileStream(FileName, FileMode.Open, FileAccess.Read))
                {
                    byte[] dataByteArray = new byte[sourceStream.Length];
                    sourceStream.Read(dataByteArray, 0, dataByteArray.Length);

                    using (SHA256CryptoServiceProvider sha256 = new SHA256CryptoServiceProvider())
                    {
                        byte[] crypto = sha256.ComputeHash(dataByteArray);
                        RtVal = Convert.ToBase64String(crypto);   //把加密後的Byte[]轉為字串 
                    }

                    //Close the streams.
                    sourceStream.Close();
                }

            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
           

            return RtVal;
        }
    }
}
