using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Web;

namespace GISFCU.Extention
{

    /// <summary></summary>
    [XmlRootAttribute(ElementName = "Root", IsNullable = false)]
    public class FilesSyncConfigDtype
    {
        /// <summary></summary>
        [XmlElementAttribute("RemoteSync")]
        public RemoteSyncDtype RemoteSync = new RemoteSyncDtype();

        /// <summary></summary>
        [XmlElementAttribute("LocalSync")]
        public LocalSyncDtype LocalSync = new LocalSyncDtype();

        /// <summary></summary>
        public class RemoteSyncDtype
        {
            /// <summary></summary>
            [XmlElementAttribute("Server")]
            public List<ServerDtype> Server = new List<ServerDtype>();

            /// <summary></summary>
            public class ServerDtype
            {
                /// <summary></summary>
                [XmlElementAttribute("TargetUrl")]
                public string TargetUrl = "";

                /// <summary></summary>
                [XmlElementAttribute("RelayUrl")]
                public string RelayUrl = "";

                /// <summary></summary>
                [XmlElementAttribute("SyncFilesPut")]
                public SyncFilesPutDtype SyncFilesPut = new SyncFilesPutDtype();

                /// <summary></summary>
                [XmlElementAttribute("SyncFilesGet")]
                public SyncFilesGetDtype SyncFilesGet = new SyncFilesGetDtype();

                /// <summary></summary>
                public class SyncFilesPutDtype
                {
                    /// <summary></summary>
                    [XmlElementAttribute("Catalog")]
                    public List<CatalogDtype> Catalog = new List<CatalogDtype>();
                }

                /// <summary></summary>
                public class SyncFilesGetDtype
                {
                    /// <summary></summary>
                    [XmlElementAttribute("Catalog")]
                    public List<CatalogDtype> Catalog = new List<CatalogDtype>();
                }

                /// <summary></summary>
                public class CatalogDtype
                {
                    /// <summary></summary>
                    [XmlAttributeAttribute("Name")]
                    public string Name = "";

                    /// <summary></summary>
                    [XmlAttributeAttribute("MapPath")]
                    public string MapPath = "";

                    /// <summary></summary>
                    [XmlAttributeAttribute("AfterAction")]
                    public string AfterAction = "";

                    /// <summary></summary>
                    [XmlAttributeAttribute("MovePath")]
                    public string MovePath = "";
                }
            }
        }

        /// <summary></summary>
        public class LocalSyncDtype
        {
            /// <summary></summary>
            [XmlElementAttribute("SyncFiles")]
            public SyncFilesDtype SyncFiles = new SyncFilesDtype();

            /// <summary></summary>
            public class SyncFilesDtype
            {
                /// <summary></summary>
                [XmlElementAttribute("Folder")]
                public List<FolderDtype> Folder = new List<FolderDtype>();

                /// <summary></summary>
                public class FolderDtype
                {
                    /// <summary></summary>
                    [XmlAttributeAttribute("SourcePath")]
                    public string SourcePath = "";

                    /// <summary></summary>
                    [XmlAttributeAttribute("TargetPath")]
                    public string TargetPath = "";

                    /// <summary></summary>
                    [XmlAttributeAttribute("AfterAction")]
                    public string AfterAction = "";

                    /// <summary></summary>
                    [XmlAttributeAttribute("MovePath")]
                    public string MovePath = "";

                    /// <summary></summary>
                    [XmlElementAttribute("Impersonator")]
                    public ImpersonatorDtype Impersonator = new ImpersonatorDtype();

                    public class ImpersonatorDtype
                    {
                        /// <summary></summary>
                        [XmlAttributeAttribute("UserName")]
                        public string UserName = "";

                        /// <summary></summary>
                        [XmlAttributeAttribute("DomainName")]
                        public string DomainName = "";

                        /// <summary></summary>
                        [XmlAttributeAttribute("Password")]
                        public string Password = "";
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public FilesSyncConfigDtype()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="XmlData"></param>
        /// <returns></returns>
        public static FilesSyncConfigDtype WebGetObject()
        {
            FilesSyncConfigDtype RtObj = null;
            string xmlFilePath = HttpContext.Current.Server.MapPath("FilesSyncConfig.xml");
            using (XmlReader reader = XmlReader.Create(xmlFilePath))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(FilesSyncConfigDtype));
                RtObj = (FilesSyncConfigDtype)serializer.Deserialize(reader);
            }
            return RtObj;
        }

    }

    /// <summary></summary>
    [XmlRootAttribute(ElementName = "Response", IsNullable = false)]
    public class Response
    {
        [XmlElementAttribute("State")]
        public string State = "S";

        [XmlElementAttribute("Content")]
        public string Content = "";

        [XmlElementAttribute("ErrorMsg")]
        public string ErrorMsg = "";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="XmlData"></param>
        /// <returns></returns>
        public static Response GetObject(string KmlXmlData)
        {
            Response RtObj = null;
            using (XmlReader reader = XmlReader.Create(new System.IO.StringReader(KmlXmlData)))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Response));
                RtObj = (Response)serializer.Deserialize(reader);
            }
            return RtObj;
        }
    }

}
