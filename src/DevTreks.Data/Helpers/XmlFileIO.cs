using DevTreks.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.XPath;

namespace DevTreks.Data.Helpers
{
    /// <summary>
    ///Purpose:		General utilities supporting xml file io
    ///Author:		www.devtreks.org
    ///Date:		2013, February
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    /// </summary>
    public class XmlFileIO
    {
        public XmlFileIO()
        {
            //each instance holds its own state
        }
        public static bool AuthorizedEditNeedsHtmlFragment(ContentURI uri,
            GeneralHelpers.DOC_STATE_NUMBER displayDocType, 
            string xmlDocPath)
        {
            bool bNeedsNewXhtmlFrag = false;
            //tempdocs can be edited by anyone
            if (uri.URIMember.ClubInUse.PrivateAuthorizationLevel
                == AccountHelper.AUTHORIZATION_LEVELS.fulledits
                || uri.URIFileExtensionType == GeneralHelpers.FILENAME_EXTENSIONS.temp.ToString())
            {
                if (uri.URIDataManager.ServerSubActionType
                    == GeneralHelpers.SERVER_SUBACTION_TYPES.runaddin)
                {
                    //if temp doc exists they are in the middle of running calcs
                    //if save = calcs they have just finished running calcs
                    if (FileStorageIO.URIAbsoluteExists(
                        uri, uri.URIDataManager.TempDocPath)
                        && (!string.IsNullOrEmpty(uri.URIDataManager.TempDocSaveMethod)
                        && uri.URIDataManager.TempDocSaveMethod != Helpers.GeneralHelpers.NONE))
                    {
                        if (displayDocType
                            == GeneralHelpers.DOC_STATE_NUMBER.seconddoc)
                        {
                            //v1.2.0: need to pass params to seconddoc telling which save message to display
                            bNeedsNewXhtmlFrag = true;
                        }
                        else
                        {
                            bNeedsNewXhtmlFrag = false;
                        }
                    }
                    else
                    {
                        if (displayDocType
                            == GeneralHelpers.DOC_STATE_NUMBER.seconddoc)
                        {
                            //always init calcors/analyzers with new doc
                            bNeedsNewXhtmlFrag = true;
                        }
                    }
                }
                else
                {
                    bNeedsNewXhtmlFrag = true;
                }
            }
            return bNeedsNewXhtmlFrag;
        }
        
        public static bool NeedsNewXhtmlDoc(ContentURI uri, 
            GeneralHelpers.DOC_STATE_NUMBER displayDocType,
            string xmlDocPath, string xhtmlDocPath)
        {
            bool bNeedsNewXhtmlDoc = false;
            //rule 1: authorized edits always get new html
            bNeedsNewXhtmlDoc = AuthorizedEditNeedsHtmlFragment(uri,
                displayDocType, xmlDocPath);
            if (bNeedsNewXhtmlDoc)
            {
                bNeedsNewXhtmlDoc = true;
                return bNeedsNewXhtmlDoc;
            }
            if (!FileStorageIO.URIAbsoluteExists(uri, xhtmlDocPath))
            {
                //rule 2: if the html doesn't exist, make it
                bNeedsNewXhtmlDoc = true;
                return bNeedsNewXhtmlDoc;
            }
            bNeedsNewXhtmlDoc = FileStorageIO.File1IsNewer(
                uri, xmlDocPath, xhtmlDocPath);
            return bNeedsNewXhtmlDoc;
        }
        
        public static void AddNewestXmlFileWithExtensionToList(
            ContentURI uri, string docPath,
            string fileExtension, IDictionary<string, string> lstFilePaths,
            ref string errorMsg)
        {
            DirectoryInfo dir = null;
            if (!FileStorageIO.DirectoryExists(uri, docPath))
            {
                errorMsg = Exceptions.Errors.MakeStandardErrorMsg(
                    string.Empty, "DIRECTORY_NOEXIST");
                return;
            }
            if (Path.HasExtension(docPath))
            {
                //if it has an extension it needs the parent directory
                dir = new DirectoryInfo(
                    Path.GetDirectoryName(docPath));
            }
            else
            {
                dir = new DirectoryInfo(docPath);
            }
            AddNewestFileWithFileExtension(uri, dir, fileExtension,
                lstFilePaths);
        }
        public static void AddNewestFileWithFileExtension(
            ContentURI uri, DirectoryInfo folder,
            string fileExtension, IDictionary<string, string> lstFilePaths)
        {
            FileInfo[] files = folder.GetFiles();
            FileInfo newestFile = null;
            bool bIsNewerFile = false;
            foreach (FileInfo file in files)
            {
                if (file.FullName.EndsWith(fileExtension)
                    && file.Extension == Helpers.GeneralHelpers.EXTENSION_XML)
                {
                    //use only the latest file calculated with that fileextension
                    //if this folder had more than one file with this extension, 
                    //it could mean that an old calculation, 
                    //from a previous calculator, was not deleted properly
                    if (newestFile != null)
                    {
                        bIsNewerFile
                            = FileStorageIO.File1IsNewer(
                                uri, file.FullName, newestFile.FullName);
                        if (bIsNewerFile)
                        {
                            newestFile = file;
                        }
                    }
                    else
                    {
                        newestFile = file;
                    }
                }
            }
            if (newestFile != null)
            {
                int i = 0;
                AddFileToList(newestFile, i, ref lstFilePaths);
            }
        }
        public static void AddNewestFileWithFileExtension(
            ContentURI uri, DirectoryInfo folder,
            string fileExtension, ref int i,
            ref IDictionary<string, string> lstFilePaths)
        {
            FileInfo[] files = folder.GetFiles();
            FileInfo newestFile = null;
            bool bIsNewerFile = false;
            foreach (FileInfo file in files)
            {
                if (Path.GetFileNameWithoutExtension(file.FullName).EndsWith(fileExtension)
                    && file.Extension == Helpers.GeneralHelpers.EXTENSION_XML)
                {
                    //rule 1: analyzers can only use calculator data
                    if (file.Name.StartsWith(GeneralHelpers.ADDIN))
                    {
                        //rule2: use only the latest file calculated with that fileextension
                        //if this folder had more than one file with this extension, 
                        //it could mean that an old calculation, 
                        //from a previous calculator, was not deleted properly
                        i++;
                        if (newestFile != null)
                        {
                            bIsNewerFile
                                = FileStorageIO.File1IsNewer(
                                    uri, file.FullName, newestFile.FullName);
                            if (bIsNewerFile)
                            {
                                newestFile = file;
                            }
                        }
                        else
                        {
                            newestFile = file;
                        }
                    }
                }
            }
            if (newestFile != null)
            {
                AddFileToList(newestFile, i, ref lstFilePaths);
            }
        }
        public static void AddNewestIOFile(
            ContentURI uri, DirectoryInfo folder,
            ref int i, ref IDictionary<string, string> lstFilePaths)
        {
            //inputs and outputs don't have base NPV calcs and can be used directly without running new calculations
            FileInfo[] files = folder.GetFiles();
            FileInfo newestFile = null;
            bool bIsNewerFile = false;
            foreach (FileInfo file in files)
            {
                if (file.Extension == Helpers.GeneralHelpers.EXTENSION_XML)
                {
                    //rule2: use only the latest file calculated with that fileextension
                    //if this folder had more than one file with this extension, 
                    //it could mean that an old calculation, 
                    //from a previous calculator, was not deleted properly
                    i++;
                    if (newestFile != null)
                    {
                        bIsNewerFile
                            = FileStorageIO.File1IsNewer(
                                uri, file.FullName, newestFile.FullName);
                        if (bIsNewerFile)
                        {
                            newestFile = file;
                        }
                    }
                    else
                    {
                        newestFile = file;
                    }
                }
            }
            if (newestFile != null)
            {
                AddFileToList(newestFile, i, ref lstFilePaths);
            }
        }
        private static void AddFileToList(FileInfo file,
            int i, ref IDictionary<string, string> lstFilePaths)
        {
            if (lstFilePaths.ContainsKey(i.ToString()) == false
                && file != null)
            {
                lstFilePaths.Add(i.ToString(), file.FullName);
            }
        }
        public static string GetXmlToConvertToXhtmlPath(ContentURI uri)
        {
            string sOldFullXHtmlFilePath
                = uri.URIMember.MemberDocFullPath;
            if (uri.URIDataManager.ServerActionType
                == Helpers.GeneralHelpers.SERVER_ACTION_TYPES.edit)
            {
                //refactor: why is this needed? both paths mirror one another
                sOldFullXHtmlFilePath = uri.URIClub.ClubDocFullPath;
            }
            return sOldFullXHtmlFilePath;
        }
        
        
        public void WriteFileXml(ContentURI uri,
            XmlReader reader, string filePath)
        {
            string sErrorMsg = string.Empty;
            WriteFileXml(uri, reader, filePath, out sErrorMsg);
            uri.ErrorMessage = sErrorMsg;
        }
        public async Task SaveFilesAsync(XmlDocument doc,
           string fullURIPath, List<Stream> sourceStreams)
        {
            //byte[] encodedText = Encoding.Unicode.GetBytes(doc.OuterXml);
            byte[] encodedText = Encoding.UTF8.GetBytes(doc.OuterXml);
            if (encodedText.Length > 0)
            {
                //do not close this stream; the closing procedure closes parallel streams
                FileStream sourceStream = new FileStream(fullURIPath,
                    FileMode.Create, FileAccess.Write, FileShare.None,
                    bufferSize: 4096, useAsync: true);
                await sourceStream.WriteAsync(encodedText, 0, encodedText.Length);
                sourceStreams.Add(sourceStream);
            }
        }
        public async Task SaveFileAsync(XmlDocument doc, string fullURIPath)
        {
            //byte[] encodedText = Encoding.Unicode.GetBytes(doc.OuterXml);
            byte[] encodedText = Encoding.UTF8.GetBytes(doc.OuterXml);
            if (encodedText.Length > 0)
            {
                //do not close this stream; the closing procedure closes parallel streams
                FileStream sourceStream = new FileStream(fullURIPath,
                    FileMode.Create, FileAccess.Write, FileShare.None,
                    bufferSize: 4096, useAsync: true);
                using (sourceStream)
                {
                    await sourceStream.WriteAsync(encodedText, 0, encodedText.Length);
                }
            }
        }
        public void WriteFileXml(ContentURI uri, 
            XmlDocument doc, string filePath, out string errorMsg)
        {
            errorMsg = string.Empty;
            bool bCanWrite = true;
            XmlTextWriter oTextWriter;
            try
            {
                //uses FileStream to open filepath; could open filestream here asynchronously and pass filestream instead of filePath
                FileStorageIO.DirectoryCreate(uri, filePath);
                oTextWriter = new XmlTextWriter(filePath, Encoding.UTF8);
                using (oTextWriter)
                {
                    bCanWrite = oTextWriter.BaseStream.CanWrite;
                    if (bCanWrite == true)
                    {
                        doc.WriteTo(oTextWriter);
                        oTextWriter.Flush();
                    }
                    else
                    {
                        bCanWrite = false;
                    }
                }
            }
            catch (Exception x)
            {
                errorMsg = Exceptions.Errors.MakeStandardErrorMsg(
                    x.ToString(), "FILEIO_CANTSAVEFILE");
                File.Delete(filePath);
            }

        }
        public async Task<bool> WriteFileXmlAsync(
            ContentURI uri, XmlReader reader, string filePath)
        {
            bool bHasSaved = false;
            FileStorageIO.DirectoryCreate(uri, filePath);
            using (reader)
            {
                reader.MoveToContent();
                FileIO fileIO = new FileIO();
                bHasSaved = await fileIO.WriteTextFileAsync(filePath, reader.ReadOuterXml());
            }
            return bHasSaved;
        }
        public void WriteFileXml(ContentURI uri, 
            XmlReader reader, string filePath, 
            out string errorMsg)
        {
            errorMsg = string.Empty;
            bool bCanWrite = true;
            XmlTextWriter oTextWriter;
            try
            {
                //uses FileStream to open filepath; could open filestream here asynchronously and pass filestream instead of filePath
                FileStorageIO.DirectoryCreate(uri, filePath);
                oTextWriter = new XmlTextWriter(filePath, Encoding.UTF8);
                using (oTextWriter)
                {
                    bCanWrite = oTextWriter.BaseStream.CanWrite;
                    if (bCanWrite == true)
                    {
                        using (reader)
                        {
                            reader.MoveToContent();
                            oTextWriter.WriteRaw(reader.ReadOuterXml());
                            oTextWriter.Flush();
                        }
                    }
                    else
                    {
                        bCanWrite = false;
                    }
                }
            }
            catch (Exception x)
            {
                errorMsg = Exceptions.Errors.MakeStandardErrorMsg(
                    x.ToString(), "FILEIO_CANTSAVEFILE");
                File.Delete(filePath);
            }
            if (!bCanWrite)
            {
                errorMsg = Exceptions.Errors.MakeStandardErrorMsg(
                    string.Empty, "FILEIO_STREAMCLOSED");
            }
            oTextWriter = null;
        }
        public void GetXmlFromFile(ContentURI uri, 
            string filePath, out XPathDocument xPathDoc)
        {
            xPathDoc = null;
            if (FileStorageIO.URIAbsoluteExists(uri, filePath))
            {
                XmlReader reader = FileStorageIO.GetXmlReader(uri, filePath);
                if (reader != null)
                {
                    using (reader)
                    {
                        xPathDoc = new XPathDocument(reader);
                    }
                }
            }
        }
        public void GetXmlFromFile(ContentURI uri, 
            string filePath, out XPathNavigator xPathNav)
        {
            xPathNav = null;
            if (FileStorageIO.URIAbsoluteExists(uri, filePath))
            {
                XmlDocument oSelectedDoc = new XmlDocument();
                XmlReader reader 
                    = Helpers.FileStorageIO.GetXmlReader(uri, filePath);
                if (reader != null)
                {
                    using (reader)
                    {
                        oSelectedDoc.Load(reader);
                    }
                }
                if (oSelectedDoc != null)
                {
                    xPathNav = oSelectedDoc.CreateNavigator();
                }
            }
        }
        //these were deprecated in 160 in favor of FileStorage.GetXml
        public void GetXmlFromFile(string filePath, out XmlTextReader reader)
        {
            reader = null;
            ReadFileXml(filePath, out reader);
        }
        
        private void ReadFileXml(string filePath, out XmlTextReader reader)
        {
            reader = null;
            //will generate an exception that will be passed to calling procedure
            FileStream oFileStream = File.OpenRead(filePath);
            if (oFileStream != null)
            {
                reader = new XmlTextReader(oFileStream);
                oFileStream.Flush();
                //closing the reader will close the stream
            }
        }
        public void GetXmlFromFile(Stream uploadFileStream, out XmlReader reader)
        {
            reader = null;
            if (uploadFileStream.CanRead)
            {
                //keep settings consistent with linq to xml default
                XmlReaderSettings oSettings = new XmlReaderSettings();
                oSettings.ConformanceLevel = ConformanceLevel.Document;
                oSettings.IgnoreWhitespace = true;
                oSettings.IgnoreComments = true;
                reader = XmlReader.Create(uploadFileStream, oSettings);
            }
        }
        public static void GetXmlFromFile(string filePath, 
            out XmlReader reader)
        {
            reader = null;
            //filepath can be uri with http or filesystem
            if (FileStorageIO.FileExists(filePath))
            {
                //keep settings consistent with linq to xml default
                XmlReaderSettings oSettings = new XmlReaderSettings();
                oSettings.ConformanceLevel = ConformanceLevel.Document;
                oSettings.IgnoreWhitespace = true;
                oSettings.IgnoreComments = true;
                reader = XmlReader.Create(filePath, oSettings);
            }
        }
        public static XmlReader GetXmlFromFile( 
            string filePath)
        {
            //filepath can be uri with http or filesystem
            XmlReader reader = null;
            GetXmlFromFile(filePath, out reader);
            return reader;
        }
        public static XmlReader GetXmlFromFileAsync( 
            string filePath)
        {
            //filepath can be uri with http or filesystem
            //keep settings consistent with linq to xml default
            XmlReaderSettings oSettings = new XmlReaderSettings();
            oSettings.ConformanceLevel = ConformanceLevel.Document;
            oSettings.IgnoreWhitespace = true;
            oSettings.IgnoreComments = true;
            //the reader can then be read with a while(reader.ReadAsync())
            oSettings.Async = true;
            XmlReader reader = XmlReader.Create(filePath, oSettings);
            return reader;
        }
        public static XmlDocument MakeRootDoc(string rootElementString)
        {
            XmlDocument oRootDoc = new XmlDocument();
            if (rootElementString != string.Empty)
            {
                oRootDoc.LoadXml(rootElementString);
            }
            return oRootDoc;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
            }
        }
        ~XmlFileIO()
        {
            Dispose(false);
        }
    }
}
