using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.XPath;

namespace DevTreks.Data.Helpers
{
    public class EPub
    {
        /// <summary>
        ///Purpose:		utilites for making idpf-compliant ebook packages
        ///Author:		www.devtreks.org
        ///Date:		2012, January
        ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148     
        /// </summary>

        //idpf file-name conventions (refer to http://www.idpf.org/2007/opf)
        private const string META_INF_SUBFOLDER = "META-INF";
        private const string CONTAINER_FILE = "container.xml";
        private const string CONTENT_FILE = "content.opf";
        private const string NCX_FILE = "toc.ncx";
        private const string MIMETYPE_FILE = "mimetype.txt";
        //placeholder in files used above
        private const string TITLEPAGE_FILE = "title.html";
        //package folder holding initial idpf files
        public const string PACKAGE = "package";
        //current directory
        private string CurrentDirectory { get; set; }

        public bool PackageIDPFFiles(ContentURI uri, string packageFilePathName, 
            IDictionary<string, string> args)
        {
            bool bIsPackaged = false;
            string errorMsg = string.Empty;
            PackageIO packIO = new PackageIO();
            packIO.CheckForPackageFileErrors(packageFilePathName, args, 
                ref errorMsg);
            if (!string.IsNullOrEmpty(errorMsg))
            {
                return bIsPackaged;
            }
            //files are found in this directory and its subfolders
            CurrentDirectory = FileStorageIO.GetDirectoryName(packageFilePathName);
            //Directory.SetCurrentDirectory(Path.GetDirectoryName(packageFilePathName));
            //1. copy container.xml file into a meta-inf subfolder 
            string sContainerNewPath = CopyContainerFile(uri, ref errorMsg);
            //2. copy mimetype.txt into root folder
            string sMimeTypeNewPath = CopyEPubFile(uri, MIMETYPE_FILE, MIMETYPE_FILE, ref errorMsg);
            //3. create content.opf file and place in root folder
            //.opf extension is not liked when saving file so use temp.xml
            string sTempFileName = string.Concat(Helpers.GeneralHelpers.GetRandomInteger(0).ToString(),
                Helpers.GeneralHelpers.EXTENSION_XML);
            string sContentNewPath = CopyEPubFile(uri, CONTENT_FILE, sTempFileName, ref errorMsg);
            //copy the files in args into content.opf
            if (errorMsg == string.Empty)
            {
                MakeContentFile(uri, Path.GetFileNameWithoutExtension(packageFilePathName),
                    args, ref sContentNewPath, ref errorMsg);
                //4. create toc.ncx file and place in root
                //.ncx extension is not liked when saving file so use temp.xml
                sTempFileName = string.Concat(Helpers.GeneralHelpers.GetRandomInteger(12223).ToString(),
                    Helpers.GeneralHelpers.EXTENSION_XML);
                string sNavigationNewPath = CopyEPubFile(uri, NCX_FILE, sTempFileName, ref errorMsg);
                //copy whatever files from args that are needed into the ncx file
                MakeNavigationFile(uri, packageFilePathName,
                    args, ref sNavigationNewPath, ref errorMsg);
                //copy misc. files
                string sTitlePath = CopyEPubFile(uri, TITLEPAGE_FILE, TITLEPAGE_FILE, ref errorMsg);
                //add the epub files to args for zipping
                args.Add(sContainerNewPath, "epub");
                args.Add(sMimeTypeNewPath, "epub");
                args.Add(sContentNewPath, "epub");
                args.Add(sNavigationNewPath, "epub");
                args.Add(sTitlePath, "epub");
                if (errorMsg == string.Empty)
                {
                    bIsPackaged = true;
                }
            }
            return bIsPackaged;
        }
        private static string MakeEPubRootFilePath(ContentURI uri, string epubFileName)
        {
            string sEPubFilePath = Helpers.AppSettings.GetPackageFullPath(uri, epubFileName);
            return sEPubFilePath;
        }
        private string CopyContainerFile(ContentURI uri, ref string errorMsg)
        {
            string sContainerNewPath = string.Empty;
            string sContainerFilePath = MakeEPubRootFilePath(uri, CONTAINER_FILE);
            if (Helpers.FileStorageIO.URIAbsoluteExists(
                uri, sContainerFilePath))
            {
                //1a. Make META-INF subfolder
                string sMetaDirectory = Path.Combine(CurrentDirectory, META_INF_SUBFOLDER);  
                Helpers.FileStorageIO.DirectoryCreate(uri, 
                    sMetaDirectory);
                sContainerNewPath = Path.Combine(sMetaDirectory, CONTAINER_FILE);
                Helpers.FileStorageIO.CopyURIs(uri, 
                    sContainerFilePath, sContainerNewPath);
            }
            else
            {
                errorMsg = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                    string.Empty, "EPUB_NOCONTAINER");
            }
            return sContainerNewPath;
        }
        
        private string CopyEPubFile(ContentURI uri, string fileName, 
            string toFileName, ref string errorMsg)
        {
            string sEPubToPath = string.Empty;
            string sEPubFromPath = MakeEPubRootFilePath(uri, fileName); 
            if (Helpers.FileStorageIO.URIAbsoluteExists(
                uri, sEPubFromPath))
            {
                sEPubToPath = Path.Combine(CurrentDirectory, toFileName);
                Helpers.FileStorageIO.CopyURIs(
                    uri, sEPubFromPath, sEPubToPath);
            }
            else
            {
                errorMsg = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg("", "EPUB_NOFILE");
            }
            return sEPubToPath;
        }
        private void MakeContentFile(ContentURI uri, string packageId,
            IDictionary<string, string> args, ref string contentNewPath, 
            ref string errorMsg)
        {
            string[] arrValues = { };
            int i = 0;
            string sArg = string.Empty;
            string sRelPartPath = string.Empty;
            string sMimeType = string.Empty;
            //need an editable navigator
            XmlDocument xmlContentDoc = new XmlDocument();
            XmlReader reader = Helpers.FileStorageIO.GetXmlReader(uri, contentNewPath);
            if (reader != null)
            {
                using (reader)
                {
                    xmlContentDoc.Load(reader);
                }
                XPathNavigator navContentDoc = xmlContentDoc.CreateNavigator();
                //move to package
                navContentDoc.MoveToFirstChild();
                navContentDoc.CreateAttribute(string.Empty, "unique-identifier", string.Empty, packageId);
                //move to meta
                navContentDoc.MoveToFirstChild();
                //move to manifest
                navContentDoc.MoveToNext(XPathNodeType.Element);
                //two default items
                navContentDoc.MoveToChild(XPathNodeType.Element);
                bool bHasMoved = navContentDoc.MoveToNext(XPathNodeType.Element);
                if (bHasMoved)
                {
                    string sKey = string.Empty;
                    string sValue = string.Empty;
                    foreach (KeyValuePair<string, string> kvp in args)
                    {
                        sKey = kvp.Key;
                        sValue = kvp.Value;
                        if (sValue != "-p")
                        {
                            //clients use same relative paths as server
                            //convert the absolute path of partFullPath into a path relative to the package root (getcurrentdirectory)
                            sRelPartPath = AppSettings.ConvertAbsPathToRelPath(CurrentDirectory, sKey);
                            sMimeType = AppHelpers.Resources.GetMimeTypeFromFileExt(Path.GetExtension(sRelPartPath), ref errorMsg);
                            //add this href attribute to a new item element child of manifest element
                            navContentDoc.InsertElementAfter(string.Empty, "item", string.Empty, string.Empty);
                            navContentDoc.MoveToNext();
                            navContentDoc.CreateAttribute(string.Empty, "id", string.Empty, i.ToString());
                            navContentDoc.CreateAttribute(string.Empty, "href", string.Empty, sRelPartPath);
                            navContentDoc.CreateAttribute(string.Empty, "media-type", string.Empty, sMimeType);
                            i++;
                        }
                    }
                }
            }
            string sContainerNewPath = Path.Combine(Path.GetDirectoryName(contentNewPath), CONTAINER_FILE);
            Helpers.FileStorageIO.MoveURIs(uri, contentNewPath, sContainerNewPath);
            contentNewPath = sContainerNewPath;
        }
        private void MakeNavigationFile(
            ContentURI uri, string packageFilePathName,
            IDictionary<string, string> args, ref string contentNewPath,
            ref string errorMsg)
        {
            string sPackagePartValue = Path.GetFileName(packageFilePathName);
            string sPartFullPath = string.Empty;
            string sRelPartPath = string.Empty;
            string sMimeType = string.Empty;
            //need an editable navigator
            XmlDocument xmlNavigationDoc = new XmlDocument();
            XmlReader reader 
                = Helpers.FileStorageIO.GetXmlReader(uri, contentNewPath);
            if (reader != null)
            {
                using (reader)
                {
                    xmlNavigationDoc.Load(reader);
                }
            XPathNavigator navNavigationDoc = xmlNavigationDoc.CreateNavigator();
            //move to ncx
            navNavigationDoc.MoveToFirstChild();
            //move to navMap
            navNavigationDoc.MoveToFirstChild();
            //move to navPoint
            bool bHasMoved = navNavigationDoc.MoveToFirstChild();
                if (bHasMoved)
                {
                    string sKey = string.Empty;
                    string sValue = string.Empty;
                    int j = 2;
                    foreach (KeyValuePair<string, string> kvp in args)
                    {
                        sKey = kvp.Key;
                        sValue = kvp.Value;
                        if (sValue == sPackagePartValue)
                        {
                            if (string.IsNullOrEmpty(sKey) == false)
                            {
                                //clients use same relative paths as server
                                //convert the absolute path of partFullPath into a path relative to the package root (getcurrentdirectory)
                                sRelPartPath = AppSettings.ConvertAbsPathToRelPath(CurrentDirectory, sKey);
                                navNavigationDoc.InsertElementAfter(string.Empty, "navPoint", string.Empty, string.Empty);
                                navNavigationDoc.MoveToNext();
                                navNavigationDoc.CreateAttribute(string.Empty, "id", string.Empty, j.ToString());
                                navNavigationDoc.CreateAttribute(string.Empty, "playOrder", string.Empty, j.ToString());
                                navNavigationDoc.AppendChildElement(string.Empty, "navLabel", string.Empty, string.Empty);
                                navNavigationDoc.MoveToChild(XPathNodeType.Element);
                                navNavigationDoc.AppendChildElement(string.Empty, "text", string.Empty, Path.GetFileNameWithoutExtension(sRelPartPath));
                                navNavigationDoc.InsertElementAfter(string.Empty, "content", string.Empty, string.Empty);
                                navNavigationDoc.MoveToNext();
                                navNavigationDoc.CreateAttribute(string.Empty, "src", string.Empty, sRelPartPath);
                                //move to navPoint
                                navNavigationDoc.MoveToParent();
                                j++;
                            }
                        }
                    }
                }
                string sTocNewPath = Path.Combine(Path.GetDirectoryName(contentNewPath), NCX_FILE);
                Helpers.FileStorageIO.MoveURIs(uri, contentNewPath, sTocNewPath);
                contentNewPath = sTocNewPath;
            }
        }
    }
}
