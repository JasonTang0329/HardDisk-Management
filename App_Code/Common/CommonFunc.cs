using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
namespace HardDiskManage.CommonFunc
{
    /// <summary>
    /// CommonFunc 的摘要描述
    /// </summary>
    public class CommonFunc
    {
        public CommonFunc()
        {
            //
            // TODO: 在這裡新增建構函式邏輯
            //
        }
        public string StrLeft(string s, int length)
        {
            return s.Substring(0, length);
        }

        public string StrRight(string s, int length)
        {
            return s.Substring(s.Length - length);
        }

        public string StrMid(string s, int start, int length)
        {
            return s.Substring(start, length);
        }
        /// 去除 HTML 標籤，可自訂合法標籤加以保留
        /// </summary>
        /// <param name="src">來源字串</param>
        /// <param name="reservedTagPool">合法標籤集</param>
        /// <returns></returns>
        public static string StripTags(string src, string[] reservedTagPool)
        {
            return Regex.Replace(
                src,
                String.Format("<(?!{0}).*?>", string.Join("|", reservedTagPool)),
                String.Empty);
        }
    }
}