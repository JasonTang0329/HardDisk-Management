<%@ WebHandler Language="C#" Class="getFiler" %>

using System;
using System.Data;
using System.Web;
using System.IO;
using GISFCU.Extention;
using System.Configuration;
using System.Collections.Generic;
public class getFiler : IHttpHandler
{
    HardDiskManage.KM_Filer.File_Helper FH = new HardDiskManage.KM_Filer.File_Helper();


    public void ProcessRequest(HttpContext context)
    {
        string path = context.Request["path"];
        string ftype = context.Request["ftype"];
        string show = context.Request["show"];

        string domain = ConfigurationManager.AppSettings["domain"];
        string uid = ConfigurationManager.AppSettings["uid"];
        string pwd = ConfigurationManager.AppSettings["pwd"];

        if (!string.IsNullOrEmpty(path) && !string.IsNullOrEmpty(ftype))
        {
            //try
            //{
                using (new Impersonator(uid, domain, pwd))
                {
                    if ((ftype == "D" && Directory.Exists(path)) || (ftype == "F" && File.Exists(path)))
                    {
                        string filename = Path.GetFileName(path);
                        string folder = Guid.NewGuid().ToString();
                        string filePath = System.Web.HttpContext.Current.Server.MapPath("~/Temp/" + folder);

                        if (ftype == "D" || (ftype == "F" && show == "view"))
                        {
                            Directory.CreateDirectory(filePath);
                            filePath += "/" + filename;

                            if (ftype == "D")
                            {
                                DirectoryCopy(path, filePath, true);
                                var ezZip = new EzZipData();
                                ezZip.ZipFolder(filePath, filePath + ".zip", EzZipData.CompressionLevel.MiddleCompression);
                                System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(filePath);
                                try
                                {
                                    di.Delete(true);
                                }
                                catch (System.IO.IOException ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                                path = filePath + ".zip";
                            }
                            else if (ftype == "F" && show == "view")
                            {
                                File.Copy(path, filePath, true);
                            }
                        }
                        Dictionary<string, string> CtType = new Dictionary<string, string>();
                        CreateDictionary(CtType);
                        byte[] buffer = new byte[1024 * 1024];
                        int read;
                        context.Response.Clear();
                        context.Response.ContentType = (CtType.ContainsKey(Path.GetExtension(path)) ? CtType[Path.GetExtension(path)] : CtType[".*"]);
                        context.Response.Headers["content-disposition"] = "attachment;filename=" + HttpUtility.UrlEncode(filename + (ftype == "D" ? ".zip" : ""));
                        if (!string.IsNullOrEmpty(show) && show == "view")
                        {
                            string encodePath = "";
                            filePath = filePath.Replace(ConfigurationManager.AppSettings["codepath"].ToString(), "").Replace(@"\", @"/");
                            foreach (string str in filePath.Split('/'))
                            {
                                encodePath += (encodePath == "" ? "" : "/") + (str == "http:" ? str : context.Server.UrlEncode(str));
                            }
                            context.Response.Redirect("../ViewFile.aspx?path=" + HttpUtility.UrlEncode(Crypto.EncryptStringAES(encodePath)));
                        }
                        else
                        {
                            Stream fileStream = File.OpenRead(path);

                            while ((read = fileStream.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                context.Response.OutputStream.Write(buffer, 0, read);
                            }
                            fileStream.Close();
                            fileStream.Dispose();

                            context.Response.Flush();

                        }

                    }
                    else
                    {
                        context.Response.ContentType = "text/plain";
                        context.Response.Write("File not be found!");
                    }
                }
            //}
            //catch (HttpException ex)
            //{
            //    throw ex;
            //}
        }
        else
        {
            context.Response.ContentType = "text/plain";
            context.Response.Write("File not be found!");
        }
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
    /// <summary>
    /// 資料夾複製
    /// </summary>
    /// <param name="sourceDirName"></param>
    /// <param name="destDirName"></param>
    /// <param name="copySubDirs"></param>
    private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
    {
        // Get the subdirectories for the specified directory.
        DirectoryInfo dir = new DirectoryInfo(sourceDirName);

        if (!dir.Exists)
        {
            throw new DirectoryNotFoundException(
                "Source directory does not exist or could not be found: "
                + sourceDirName);
        }

        DirectoryInfo[] dirs = dir.GetDirectories();
        // If the destination directory doesn't exist, create it.
        if (!Directory.Exists(destDirName))
        {
            Directory.CreateDirectory(destDirName);
        }

        // Get the files in the directory and copy them to the new location.
        FileInfo[] files = dir.GetFiles();
        foreach (FileInfo file in files)
        {
            if (file.Extension.ToLower() != ".mp4")
            {
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, true);
            }
        }

        // If copying subdirectories, copy them and their contents to new location.
        if (copySubDirs)
        {
            foreach (DirectoryInfo subdir in dirs)
            {
                string temppath = Path.Combine(destDirName, subdir.Name);
                DirectoryCopy(subdir.FullName, temppath, copySubDirs);
            }
        }
    }
    /// <summary>
    /// 取得TYPE轉換的Key value
    /// </summary>
    /// <param name="CtType"></param>
    private void CreateDictionary(Dictionary<string, string> CtType)
    {
        CtType.Add(".*", "application/octet-stream");
        CtType.Add(".001", "application/x-001");
        CtType.Add(".301", "application/x-301");
        CtType.Add(".323", "text/h323");
        CtType.Add(".906", "application/x-906");
        CtType.Add(".907", "drawing/907");
        CtType.Add(".a11", "application/x-a11");
        CtType.Add(".acp", "audio/x-mei-aac");
        CtType.Add(".ai", "application/postscript");
        CtType.Add(".aif", "audio/aiff");
        CtType.Add(".aifc", "audio/aiff");
        CtType.Add(".aiff", "audio/aiff");
        CtType.Add(".anv", "application/x-anv");
        CtType.Add(".asa", "text/asa");
        CtType.Add(".asf", "video/x-ms-asf");
        CtType.Add(".asp", "text/asp");
        CtType.Add(".asx", "video/x-ms-asf");
        CtType.Add(".au", "audio/basic");
        CtType.Add(".avi", "video/avi");
        CtType.Add(".awf", "application/vnd.adobe.workflow");
        CtType.Add(".biz", "text/xml");
        CtType.Add(".bmp", "application/x-bmp");
        CtType.Add(".bot", "application/x-bot");
        CtType.Add(".c4t", "application/x-c4t");
        CtType.Add(".c90", "application/x-c90");
        CtType.Add(".cal", "application/x-cals");
        CtType.Add(".cat", "application/vnd.ms-pki.seccat");
        CtType.Add(".cdf", "application/x-netcdf");
        CtType.Add(".cdr", "application/x-cdr");
        CtType.Add(".cel", "application/x-cel");
        CtType.Add(".cer", "application/x-x509-ca-cert");
        CtType.Add(".cg4", "application/x-g4");
        CtType.Add(".cgm", "application/x-cgm");
        CtType.Add(".cit", "application/x-cit");
        CtType.Add(".class", "java/*");
        CtType.Add(".cml", "text/xml");
        CtType.Add(".cmp", "application/x-cmp");
        CtType.Add(".cmx", "application/x-cmx");
        CtType.Add(".cot", "application/x-cot");
        CtType.Add(".crl", "application/pkix-crl");
        CtType.Add(".crt", "application/x-x509-ca-cert");
        CtType.Add(".csi", "application/x-csi");
        CtType.Add(".css", "text/css");
        CtType.Add(".cut", "application/x-cut");
        CtType.Add(".dbf", "application/x-dbf");
        CtType.Add(".dbm", "application/x-dbm");
        CtType.Add(".dbx", "application/x-dbx");
        CtType.Add(".dcd", "text/xml");
        CtType.Add(".dcx", "application/x-dcx");
        CtType.Add(".der", "application/x-x509-ca-cert");
        CtType.Add(".dgn", "application/x-dgn");
        CtType.Add(".dib", "application/x-dib");
        CtType.Add(".dll", "application/x-msdownload");
        CtType.Add(".doc", "application/msword");
        CtType.Add(".dot", "application/msword");
        CtType.Add(".drw", "application/x-drw");
        CtType.Add(".dtd", "text/xml");
        //CtType.Add(".dwf", "Model/vnd.dwf");
        CtType.Add(".dwf", "application/x-dwf");
        CtType.Add(".dwg", "application/x-dwg");
        CtType.Add(".dxb", "application/x-dxb");
        CtType.Add(".dxf", "application/x-dxf");
        CtType.Add(".edn", "application/vnd.adobe.edn");
        CtType.Add(".emf", "application/x-emf");
        CtType.Add(".eml", "message/rfc822");
        CtType.Add(".ent", "text/xml");
        CtType.Add(".epi", "application/x-epi");
        CtType.Add(".eps", "application/x-ps");
        //CtType.Add(".eps", "application/postscript");
        CtType.Add(".etd", "application/x-ebx");
        CtType.Add(".exe", "application/x-msdownload");
        CtType.Add(".fax", "image/fax");
        CtType.Add(".fdf", "application/vnd.fdf");
        CtType.Add(".fif", "application/fractals");
        CtType.Add(".fo", "text/xml");
        CtType.Add(".frm", "application/x-frm");
        CtType.Add(".g4", "application/x-g4");
        CtType.Add(".gbr", "application/x-gbr");
        CtType.Add(".gcd", "application/x-gcd");
        CtType.Add(".gif", "image/gif");
        CtType.Add(".gl2", "application/x-gl2");
        CtType.Add(".gp4", "application/x-gp4");
        CtType.Add(".hgl", "application/x-hgl");
        CtType.Add(".hmr", "application/x-hmr");
        CtType.Add(".hpg", "application/x-hpgl");
        CtType.Add(".hpl", "application/x-hpl");
        CtType.Add(".hqx", "application/mac-binhex40");
        CtType.Add(".hrf", "application/x-hrf");
        CtType.Add(".hta", "application/hta");
        CtType.Add(".htc", "text/x-component");
        CtType.Add(".htm", "text/html");
        CtType.Add(".html", "text/html");
        CtType.Add(".htt", "text/webviewhtml");
        CtType.Add(".htx", "text/html");
        CtType.Add(".icb", "application/x-icb");
        CtType.Add(".ico", "image/x-icon");
        //CtType.Add(".ico", "application/x-ico");
        CtType.Add(".iff", "application/x-iff");
        CtType.Add(".ig4", "application/x-g4");
        CtType.Add(".igs", "application/x-igs");
        CtType.Add(".iii", "application/x-iphone");
        CtType.Add(".img", "application/x-img");
        CtType.Add(".ins", "application/x-internet-signup");
        CtType.Add(".isp", "application/x-internet-signup");
        CtType.Add(".IVF", "video/x-ivf");
        CtType.Add(".java", "java/*");
        CtType.Add(".jfif", "image/jpeg");
        CtType.Add(".jpe", "image/jpeg");
        //CtType.Add(".jpe", "application/x-jpe");
        CtType.Add(".jpeg", "image/jpeg");
        CtType.Add(".jpg", "image/jpeg");
        //CtType.Add(".jpg", "application/x-jpg");
        CtType.Add(".js", "application/x-javascript");
        CtType.Add(".jsp", "text/html");
        CtType.Add(".la1", "audio/x-liquid-file");
        CtType.Add(".lar", "application/x-laplayer-reg");
        CtType.Add(".latex", "application/x-latex");
        CtType.Add(".lavs", "audio/x-liquid-secure");
        CtType.Add(".lbm", "application/x-lbm");
        CtType.Add(".lmsff", "audio/x-la-lms");
        CtType.Add(".ls", "application/x-javascript");
        CtType.Add(".ltr", "application/x-ltr");
        CtType.Add(".m1v", "video/x-mpeg");
        CtType.Add(".m2v", "video/x-mpeg");
        CtType.Add(".m3u", "audio/mpegurl");
        CtType.Add(".m4e", "video/mpeg4");
        CtType.Add(".mac", "application/x-mac");
        CtType.Add(".man", "application/x-troff-man");
        CtType.Add(".math", "text/xml");
        //CtType.Add(".mdb", "application/msaccess");
        CtType.Add(".mdb", "application/x-mdb");
        CtType.Add(".mfp", "application/x-shockwave-flash");
        CtType.Add(".mht", "message/rfc822");
        CtType.Add(".mhtml", "message/rfc822");
        CtType.Add(".mi", "application/x-mi");
        CtType.Add(".mid", "audio/mid");
        CtType.Add(".midi", "audio/mid");
        CtType.Add(".mil", "application/x-mil");
        CtType.Add(".mml", "text/xml");
        CtType.Add(".mnd", "audio/x-musicnet-download");
        CtType.Add(".mns", "audio/x-musicnet-stream");
        CtType.Add(".mocha", "application/x-javascript");
        CtType.Add(".movie", "video/x-sgi-movie");
        CtType.Add(".mp1", "audio/mp1");
        CtType.Add(".mp2", "audio/mp2");
        CtType.Add(".mp2v", "video/mpeg");
        CtType.Add(".mp3", "audio/mp3");
        CtType.Add(".mp4", "video/mpeg4");
        CtType.Add(".mpa", "video/x-mpg");
        CtType.Add(".mpd", "application/vnd.ms-project");
        CtType.Add(".mpe", "video/x-mpeg");
        CtType.Add(".mpeg", "video/mpg");
        CtType.Add(".mpg", "video/mpg");
        CtType.Add(".mpga", "audio/rn-mpeg");
        CtType.Add(".mpp", "application/vnd.ms-project");
        CtType.Add(".mps", "video/x-mpeg");
        CtType.Add(".mpt", "application/vnd.ms-project");
        CtType.Add(".mpv", "video/mpg");
        CtType.Add(".mpv2", "video/mpeg");
        CtType.Add(".mpw", "application/vnd.ms-project");
        CtType.Add(".mpx", "application/vnd.ms-project");
        CtType.Add(".mtx", "text/xml");
        CtType.Add(".mxp", "application/x-mmxp");
        CtType.Add(".net", "image/pnetvue");
        CtType.Add(".nrf", "application/x-nrf");
        CtType.Add(".nws", "message/rfc822");
        CtType.Add(".odc", "text/x-ms-odc");
        CtType.Add(".out", "application/x-out");
        CtType.Add(".p10", "application/pkcs10");
        CtType.Add(".p12", "application/x-pkcs12");
        CtType.Add(".p7b", "application/x-pkcs7-certificates");
        CtType.Add(".p7c", "application/pkcs7-mime");
        CtType.Add(".p7m", "application/pkcs7-mime");
        CtType.Add(".p7r", "application/x-pkcs7-certreqresp");
        CtType.Add(".p7s", "application/pkcs7-signature");
        CtType.Add(".pc5", "application/x-pc5");
        CtType.Add(".pci", "application/x-pci");
        CtType.Add(".pcl", "application/x-pcl");
        CtType.Add(".pcx", "application/x-pcx");
        CtType.Add(".pdf", "application/pdf");
        CtType.Add(".pdx", "application/vnd.adobe.pdx");
        CtType.Add(".pfx", "application/x-pkcs12");
        CtType.Add(".pgl", "application/x-pgl");
        CtType.Add(".pic", "application/x-pic");
        CtType.Add(".pko", "application/vnd.ms-pki.pko");
        CtType.Add(".pl", "application/x-perl");
        CtType.Add(".plg", "text/html");
        CtType.Add(".pls", "audio/scpls");
        CtType.Add(".plt", "application/x-plt");
        CtType.Add(".png", "image/png");
        //CtType.Add(".png", "application/x-png");
        CtType.Add(".pot", "application/vnd.ms-powerpoint");
        CtType.Add(".ppa", "application/vnd.ms-powerpoint");
        CtType.Add(".ppm", "application/x-ppm");
        CtType.Add(".pps", "application/vnd.ms-powerpoint");
        CtType.Add(".ppt", "application/vnd.ms-powerpoint");
        //CtType.Add(".ppt", "application/x-ppt");
        CtType.Add(".pr", "application/x-pr");
        CtType.Add(".prf", "application/pics-rules");
        CtType.Add(".prn", "application/x-prn");
        CtType.Add(".prt", "application/x-prt");
        CtType.Add(".ps", "application/x-ps");
        //CtType.Add(".ps", "application/postscript");
        CtType.Add(".ptn", "application/x-ptn");
        CtType.Add(".pwz", "application/vnd.ms-powerpoint");
        CtType.Add(".r3t", "text/vnd.rn-realtext3d");
        CtType.Add(".ra", "audio/vnd.rn-realaudio");
        CtType.Add(".ram", "audio/x-pn-realaudio");
        CtType.Add(".ras", "application/x-ras");
        CtType.Add(".rat", "application/rat-file");
        CtType.Add(".rdf", "text/xml");
        CtType.Add(".rec", "application/vnd.rn-recording");
        CtType.Add(".red", "application/x-red");
        CtType.Add(".rgb", "application/x-rgb");
        CtType.Add(".rjs", "application/vnd.rn-realsystem-rjs");
        CtType.Add(".rjt", "application/vnd.rn-realsystem-rjt");
        CtType.Add(".rlc", "application/x-rlc");
        CtType.Add(".rle", "application/x-rle");
        CtType.Add(".rm", "application/vnd.rn-realmedia");
        CtType.Add(".rmf", "application/vnd.adobe.rmf");
        CtType.Add(".rmi", "audio/mid");
        CtType.Add(".rmj", "application/vnd.rn-realsystem-rmj");
        CtType.Add(".rmm", "audio/x-pn-realaudio");
        CtType.Add(".rmp", "application/vnd.rn-rn_music_package");
        CtType.Add(".rms", "application/vnd.rn-realmedia-secure");
        CtType.Add(".rmvb", "application/vnd.rn-realmedia-vbr");
        CtType.Add(".rmx", "application/vnd.rn-realsystem-rmx");
        CtType.Add(".rnx", "application/vnd.rn-realplayer");
        CtType.Add(".rp", "image/vnd.rn-realpix");
        CtType.Add(".rpm", "audio/x-pn-realaudio-plugin");
        CtType.Add(".rsml", "application/vnd.rn-rsml");
        CtType.Add(".rt", "text/vnd.rn-realtext");
        CtType.Add(".rtf", "application/msword");
        //CtType.Add(".rtf", "application/x-rtf");
        CtType.Add(".rv", "video/vnd.rn-realvideo");
        CtType.Add(".sam", "application/x-sam");
        CtType.Add(".sat", "application/x-sat");
        CtType.Add(".sdp", "application/sdp");
        CtType.Add(".sdw", "application/x-sdw");
        CtType.Add(".sit", "application/x-stuffit");
        CtType.Add(".slb", "application/x-slb");
        CtType.Add(".sld", "application/x-sld");
        CtType.Add(".slk", "drawing/x-slk");
        CtType.Add(".smi", "application/smil");
        CtType.Add(".smil", "application/smil");
        CtType.Add(".smk", "application/x-smk");
        CtType.Add(".snd", "audio/basic");
        CtType.Add(".sol", "text/plain");
        CtType.Add(".sor", "text/plain");
        CtType.Add(".spc", "application/x-pkcs7-certificates");
        CtType.Add(".spl", "application/futuresplash");
        CtType.Add(".spp", "text/xml");
        CtType.Add(".ssm", "application/streamingmedia");
        CtType.Add(".sst", "application/vnd.ms-pki.certstore");
        CtType.Add(".stl", "application/vnd.ms-pki.stl");
        CtType.Add(".stm", "text/html");
        CtType.Add(".sty", "application/x-sty");
        CtType.Add(".svg", "text/xml");
        CtType.Add(".swf", "application/x-shockwave-flash");
        CtType.Add(".tdf", "application/x-tdf");
        CtType.Add(".tg4", "application/x-tg4");
        CtType.Add(".tga", "application/x-tga");
        CtType.Add(".tif", "image/tiff");
        //CtType.Add(".tif", "application/x-tif");
        CtType.Add(".tiff", "image/tiff");
        CtType.Add(".tld", "text/xml");
        CtType.Add(".top", "drawing/x-top");
        CtType.Add(".torrent", "application/x-bittorrent");
        CtType.Add(".tsd", "text/xml");
        CtType.Add(".txt", "text/plain");
        CtType.Add(".uin", "application/x-icq");
        CtType.Add(".uls", "text/iuls");
        CtType.Add(".vcf", "text/x-vcard");
        CtType.Add(".vda", "application/x-vda");
        CtType.Add(".vdx", "application/vnd.visio");
        CtType.Add(".vml", "text/xml");
        CtType.Add(".vpg", "application/x-vpeg005");
        //CtType.Add(".vsd", "application/vnd.visio");
        CtType.Add(".vsd", "application/x-vsd");
        CtType.Add(".vss", "application/vnd.visio");
        //CtType.Add(".vst", "application/vnd.visio");
        CtType.Add(".vst", "application/x-vst");
        CtType.Add(".vsw", "application/vnd.visio");
        CtType.Add(".vsx", "application/vnd.visio");
        CtType.Add(".vtx", "application/vnd.visio");
        CtType.Add(".vxml", "text/xml");
        CtType.Add(".wav", "audio/wav");
        CtType.Add(".wax", "audio/x-ms-wax");
        CtType.Add(".wb1", "application/x-wb1");
        CtType.Add(".wb2", "application/x-wb2");
        CtType.Add(".wb3", "application/x-wb3");
        CtType.Add(".wbmp", "image/vnd.wap.wbmp");
        CtType.Add(".wiz", "application/msword");
        CtType.Add(".wk3", "application/x-wk3");
        CtType.Add(".wk4", "application/x-wk4");
        CtType.Add(".wkq", "application/x-wkq");
        CtType.Add(".wks", "application/x-wks");
        CtType.Add(".wm", "video/x-ms-wm");
        CtType.Add(".wma", "audio/x-ms-wma");
        CtType.Add(".wmd", "application/x-ms-wmd");
        CtType.Add(".wmf", "application/x-wmf");
        CtType.Add(".wml", "text/vnd.wap.wml");
        CtType.Add(".wmv", "video/x-ms-wmv");
        CtType.Add(".wmx", "video/x-ms-wmx");
        CtType.Add(".wmz", "application/x-ms-wmz");
        CtType.Add(".wp6", "application/x-wp6");
        CtType.Add(".wpd", "application/x-wpd");
        CtType.Add(".wpg", "application/x-wpg");
        CtType.Add(".wpl", "application/vnd.ms-wpl");
        CtType.Add(".wq1", "application/x-wq1");
        CtType.Add(".wr1", "application/x-wr1");
        CtType.Add(".wri", "application/x-wri");
        CtType.Add(".wrk", "application/x-wrk");
        CtType.Add(".ws", "application/x-ws");
        CtType.Add(".ws2", "application/x-ws");
        CtType.Add(".wsc", "text/scriptlet");
        CtType.Add(".wsdl", "text/xml");
        CtType.Add(".wvx", "video/x-ms-wvx");
        CtType.Add(".xdp", "application/vnd.adobe.xdp");
        CtType.Add(".xdr", "text/xml");
        CtType.Add(".xfd", "application/vnd.adobe.xfd");
        CtType.Add(".xfdf", "application/vnd.adobe.xfdf");
        CtType.Add(".xhtml", "text/html");
        CtType.Add(".xls", "application/vnd.ms-excel");
        //CtType.Add(".xls", "application/x-xls");
        CtType.Add(".xlw", "application/x-xlw");
        CtType.Add(".xml", "text/xml");
        CtType.Add(".xpl", "audio/scpls");
        CtType.Add(".xq", "text/xml");
        CtType.Add(".xql", "text/xml");
        CtType.Add(".xquery", "text/xml");
        CtType.Add(".xsd", "text/xml");
        CtType.Add(".xsl", "text/xml");
        CtType.Add(".xslt", "text/xml");
        CtType.Add(".xwd", "application/x-xwd");
        CtType.Add(".x_b", "application/x-x_b");
        CtType.Add(".x_t", "application/x-x_t");
    }
    
}