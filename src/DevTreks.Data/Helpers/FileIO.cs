using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DevTreks.Data.Helpers
{
    /// <summary>
    ///Purpose:		file managment utilities
    ///Author:		www.devtreks.org
    ///Date:		2016, May
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    /// </summary>
    public class FileIO
    {
        public FileIO()
        {
            //each instance holds its own state
        }
        
        public bool WriteTextFile(ContentURI uri, StringWriter writer,
            string filePath)
        {
            bool bHasFile = false;
            using (writer)
            {
                string sHtml = writer.GetStringBuilder().ToString();
                //don't save </root> files
                if (sHtml.Length > 10)
                {
                    bHasFile = WriteTextFile(filePath,
                        sHtml);
                }
                else
                {
                    uri.ErrorMessage = DevTreks.Exceptions.Errors.GetMessage("DISPLAYHELPER_NOHTMLFILE");
                }
            }
            return bHasFile;
        }
        
        public bool WriteHtmlTextFile(string fileName, StringWriter writer)
        {
            bool bHasSaved = false;
            using (writer)
            {
                bHasSaved = WriteTextFile(fileName, 
                    System.Net.WebUtility.HtmlDecode(writer.GetStringBuilder().ToString()));
            }
            return bHasSaved;
        }
        public async Task<bool> WriteHtmlTextFileAsync(string fileName, 
            StringWriter writer)
        {
            bool bHasSaved = false;
            using (writer)
            {
                bHasSaved = await WriteTextFileAsync(fileName, 
                    System.Net.WebUtility.HtmlDecode(writer.GetStringBuilder().ToString()));
            }
            return bHasSaved;
        }
        public bool WriteTextFile(string fileName, StringWriter writer)
        {
            bool bHasSaved = false;
            using (writer)
            {
                bHasSaved = WriteTextFile(fileName, writer.ToString());
            }
            return bHasSaved;
        }
        public async Task<bool> WriteTextFileAsync(string fileName, StringWriter writer)
        {
            bool bHasSaved = false;
            using (writer)
            {
                bHasSaved = await WriteTextFileAsync(fileName, writer.ToString());
            }
            return bHasSaved;
        }
        public bool WriteTextFile(string fileName, string text)
        {
            bool bHasSaved = false;
            byte[] encodedText = Encoding.UTF8.GetBytes(text);
            if (!Directory.Exists(fileName))
            {
                string sDir = Path.GetDirectoryName(fileName);
                Directory.CreateDirectory(sDir);
            }
            using (FileStream sourceStream = new FileStream(fileName,
                FileMode.Create, FileAccess.Write, FileShare.None,
                bufferSize: 4096, useAsync: true))
            {
                sourceStream.WriteAsync(encodedText, 0, encodedText.Length);
                bHasSaved = true;
            };
            return bHasSaved;
        }
        public async Task<bool> WriteTextFileAsync(string fileName, string text)
        {
            bool bHasSaved = false;
            byte[] encodedText = Encoding.UTF8.GetBytes(text);
            using (FileStream sourceStream = new FileStream(fileName,
                FileMode.Create, FileAccess.Write, FileShare.None,
                bufferSize: 4096, useAsync: true))
            {
                await sourceStream.WriteAsync(encodedText, 0, encodedText.Length);
                bHasSaved = true;
            };
            return bHasSaved;
        }
        public async Task WriteTextFileToWriterAsync(ContentURI uri,
            StringWriter writer, string filePath)
        {
            string sHtml = await ReadTextAsync(uri, filePath);
            if (sHtml.Length > 10)
            {
                writer.Write(sHtml);
            }
            else
            {
                uri.ErrorMessage = DevTreks.Exceptions.Errors.GetMessage("DISPLAYHELPER_NOHTMLFILE");
            }
        }
        public async void WriteTextFilesAsync(List<string> dataWriteURLs)
        {
            string folder = @"tempfolder\";
            List<Task> tasks = new List<Task>();
            List<FileStream> sourceStreams = new List<FileStream>();
            try
            {
                foreach (var dataURL in dataWriteURLs)
                {
                    byte[] encodedText = Encoding.Unicode.GetBytes(dataURL);

                    FileStream sourceStream = new FileStream(dataURL,
                        FileMode.Append, FileAccess.Write, FileShare.None,
                        bufferSize: 4096, useAsync: true);

                    Task theTask = sourceStream.WriteAsync(encodedText, 0, encodedText.Length);
                    sourceStreams.Add(sourceStream);

                    tasks.Add(theTask);
                }
                for (int index = 1; index <= 10; index++)
                {
                    //this has to be replaced with the bytes to write
                    string text = "In file " + index.ToString() + "\r\n";

                    string fileName = "thefile" + index.ToString("00") + ".txt";
                    string filePath = folder + fileName;

                    byte[] encodedText = Encoding.Unicode.GetBytes(text);

                    FileStream sourceStream = new FileStream(filePath,
                        FileMode.Append, FileAccess.Write, FileShare.None,
                        bufferSize: 4096, useAsync: true);

                    Task theTask = sourceStream.WriteAsync(encodedText, 0, encodedText.Length);
                    sourceStreams.Add(sourceStream);

                    tasks.Add(theTask);
                }

                await Task.WhenAll(tasks);
            }

            finally
            {
                foreach (FileStream sourceStream in sourceStreams)
                {
                    sourceStream.Dispose();
                }
            }
        }
        /// <summary>
        /// This includes a try catch block
        /// </summary>
        /// <param name="filePath">path to file being read</param>
        /// <returns>string holding file contents</returns>
        public async Task<string> ReadTextAsync(ContentURI uri, 
            string filePath)
        {
            StringBuilder sb = new StringBuilder();
            if (Helpers.FileStorageIO.URIAbsoluteExists(uri, filePath) == false)
            {
                return sb.ToString();
            }
            try
            {
                using (FileStream sourceStream = new FileStream(filePath,
                    FileMode.Open, FileAccess.Read, FileShare.Read,
                    bufferSize: 4096, useAsync: true))
                {
                    byte[] buffer = new byte[0x1000];
                    int numRead;
                    while ((numRead = await sourceStream.ReadAsync(buffer, 0, buffer.Length)) != 0)
                    {
                        string text = Encoding.UTF8.GetString(buffer, 0, numRead);
                        sb.Append(text);
                    }
                    //this also will work
                    //byte[] result = new byte[sourceStream.Length];
                    //await sourceStream.ReadAsync(result, 0, (int)sourceStream.Length);
                    //UnicodeEncoding uniencoding = new UnicodeEncoding();
                    //string file = uniencoding.GetString(result);
                }
            }
            catch (Exception x)
            {
                sb.Append(DevTreks.Exceptions.Errors.MakeStandardErrorMsg(
                        string.Empty, "RESOURCES_NOFILEREAD"));
            }
            return sb.ToString();
        }
        /// <summary>
        /// This does not includes a try catch block
        /// </summary>
        /// <param name="filePath">path to file being read</param>
        /// <returns>string holding file contents</returns>
        public async Task<string> ReadTextAsync2(string filePath)
        {
            string sFile = string.Empty;
            using (FileStream sourceStream = new FileStream(filePath,
              FileMode.Open, FileAccess.Read, FileShare.Read,
              bufferSize: 4096, useAsync: true))
            {
                byte[] buffer = new byte[sourceStream.Length];
                int bytes = await sourceStream.ReadAsync(buffer, 0, (int)sourceStream.Length);
                //this returns tab return characters \r and newline \chars that is inconsistent with new line \n chars in non async
                //bytes = buffer.length
                sFile = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
            }
            return sFile;
        }
        public async Task<List<string>> ReadLinesAsync(string filePath)
        {
            List<string> lines = new List<string>();
            string sFile = await ReadTextAsync2(filePath);
            lines = GeneralHelpers.GetLinesFromUTF8Encoding(sFile);
            return lines;
        }
        public void WriteTextFileToWriter(ContentURI uri,
            StringWriter writer, string filePath)
        {
            string sHtml = ReadText(uri, filePath);
            if (sHtml.Length > 10)
            {
                writer.Write(sHtml);
            }
            else
            {
                uri.ErrorMessage = DevTreks.Exceptions.Errors.GetMessage("DISPLAYHELPER_NOHTMLFILE");
            }
        }
        
        public string ReadText(ContentURI uri, string fileName)
        {
            string sTextString = string.Empty;
            if (Helpers.FileStorageIO.URIAbsoluteExists(uri, fileName) == false)
            {
                return sTextString;
            }
            using (StreamReader oReader = File.OpenText(fileName))
            {
                sTextString = oReader.ReadToEnd();
            }
            return sTextString;
        }
        public byte[] DownloadFile(ContentURI uri, string fullURIPath)
        {
            byte[] data = { };
            if (Helpers.FileStorageIO.URIAbsoluteExists(
                uri, fullURIPath) == false)
            {
                return data;
            }
            data = Encoding.UTF8.GetBytes(ReadText(uri, fullURIPath));
            return data;
        }
        
        public List<string> ReadLines(string fullURIPath)
        {
            List<string> lines = new List<string>();
            foreach (var line in System.IO.File.ReadLines(fullURIPath))
            {
                lines.Add(line);
            }
            //or
            //using (StreamReader sr = new StreamReader(fullURIPath))
            //{
            //    string str;
            //    int i = 0;
            //    while ((str = sr.ReadLine()) != null)
            //    {
            //        if (i != 0)
            //        {
            //            lines.Add(str);
            //            //double[] arr = str.Split(',').Select(s => double.Parse(s)).ToArray();
            //            //dataList.Add(Vector.FromArray(arr));
            //        }
            //        i++;
            //    }
            //}
            return lines;
        }
        
        //184 version of next async script
        public static void CopyBinaryValueToFile(string fullPathToFile, SqlDataReader sqlReader, int columnIndex)
        {
            using (FileStream file = new FileStream(fullPathToFile, FileMode.Create, FileAccess.Write))
            {
                using (Stream data = sqlReader.GetStream(columnIndex))
                {

                    // synchronously copy the stream from the server to the file we just created
                    data.CopyTo(file);
                }
            }
        }
        //this will not work if called consecutively
        public static async Task CopyBinaryValueToFileAsync(string fullPathToFile, SqlDataReader sqlReader, int columnIndex)
        {
            using (FileStream file = new FileStream(fullPathToFile, FileMode.Create, FileAccess.Write))
            {
                using (Stream data = sqlReader.GetStream(columnIndex))
                {

                    // Asynchronously copy the stream from the server to the file we just created
                    await data.CopyToAsync(file);
                }
            }
        }
        //this comes directly from SqlDataClient documentation
        // Application retrieving a large BLOB from SQL Server in .NET 4.5 using the new asynchronous capability
        //private static async Task CopyBinaryValueToFile()
        //{
        //    string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "binarydata.bin");

        //    using (SqlConnection connection = new SqlConnection(connectionString))
        //    {
        //        await connection.OpenAsync();
        //        using (SqlCommand command = new SqlCommand("SELECT [bindata] FROM [Streams] WHERE [id]=@id", connection))
        //        {
        //            command.Parameters.AddWithValue("id", 1);

        //            // The reader needs to be executed with the SequentialAccess behavior to enable network streaming
        //            // Otherwise ReadAsync will buffer the entire BLOB into memory which can cause scalability issues or even OutOfMemoryExceptions
        //            using (SqlDataReader reader = await command.ExecuteReaderAsync(CommandBehavior.SequentialAccess))
        //            {
        //                if (await reader.ReadAsync())
        //                {
        //                    if (!(await reader.IsDBNullAsync(0)))
        //                    {
        //                        using (FileStream file = new FileStream(filePath, FileMode.Create, FileAccess.Write))
        //                        {
        //                            using (Stream data = reader.GetStream(0))
        //                            {

        //                                // Asynchronously copy the stream from the server to the file we just created
        //                                await data.CopyToAsync(file);
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}
        //184 deprecated: GetBytes left out final byte which caused script file errors
        //public bool WriteBinaryBlobFile(string fullPathToFile, SqlDataReader sqlReader, int columnIndex)
        //{
        //    bool bHasWrote = false;
        //    //always create a new file (existing file should have been archived)
        //    string sToDirectory = Path.GetDirectoryName(fullPathToFile);
        //    if (!Directory.Exists(sToDirectory))
        //    {
        //        Directory.CreateDirectory(sToDirectory);
        //    }
        //    //note the stream is closed by the calling procedure
        //    FileStream oFileStream = new FileStream(fullPathToFile, FileMode.Create, FileAccess.Write, FileShare.None,
        //        8192, true);
        //    if (oFileStream != null)
        //    {
        //        using (oFileStream)
        //        {
        //            //create the writer for data.
        //            using (BinaryWriter oBinaryWriter = new BinaryWriter(oFileStream))
        //            {
        //                //size of the buffer.
        //                int iBufferSize = 100;
        //                //blob byte[] buffer to be filled by GetBytes.
        //                byte[] outBytes = new byte[iBufferSize];
        //                //the bytes returned from GetBytes.
        //                long lRetVal;
        //                //the starting position in the BLOB output.
        //                long lStartIndex = 0;
        //                //read bytes into outByte[] and retain the number of bytes returned.
        //                lRetVal = sqlReader.GetBytes(columnIndex, lStartIndex, outBytes, 0, iBufferSize);
        //                //continue while there are bytes beyond the size of the buffer.
        //                while (lRetVal == iBufferSize)
        //                {
        //                    oBinaryWriter.Write(outBytes);
        //                    //reposition start index to end of last buffer and fill buffer.
        //                    lStartIndex += iBufferSize;
        //                    lRetVal = sqlReader.GetBytes(columnIndex, lStartIndex, outBytes, 0, iBufferSize);
        //                }
        //                if (lRetVal > 0)
        //                {
        //                    //write the remaining buffer.
        //                    oBinaryWriter.Write(outBytes, 0, (int)lRetVal);
        //                    //184: deprecated because it left out last byte
        //                    //not so: problem is with Sequential access in SqlDataReader
        //                    //oBinaryWriter.Write(outBytes, 0, (int)lRetVal - 1);
        //                }
        //                bHasWrote = true;
        //            }
        //        }
        //    }
        //    return bHasWrote;
        //}

        
        public static string GetDirectoryName(string existingDirectoryName)
        {
            string sToDirectory = existingDirectoryName;
            if (!Path.HasExtension(existingDirectoryName))
            {
                if (!existingDirectoryName.EndsWith(GeneralHelpers.FILE_PATH_DELIMITER))
                {
                    sToDirectory = string.Concat(existingDirectoryName, GeneralHelpers.FILE_PATH_DELIMITER);
                }
            }
            //this directory doesn't have a final delimiter
            sToDirectory = Path.GetDirectoryName(sToDirectory);
            if (!sToDirectory.EndsWith(GeneralHelpers.FILE_PATH_DELIMITER))
            {
                sToDirectory = string.Concat(sToDirectory, GeneralHelpers.FILE_PATH_DELIMITER);
            }
            return sToDirectory;
        }
        
        public static bool CopyFiles(ContentURI uri,
            string fromFile, string toFile)
        {
            bool bHasCopied = false;
            if (Helpers.FileStorageIO.URIAbsoluteExists(uri, fromFile) == true
                && fromFile.Equals(toFile) == false
                && (!string.IsNullOrEmpty(toFile)))
            {
                string sDirectory = Path.GetDirectoryName(toFile);
                if (!Directory.Exists(sDirectory))
                {
                    Directory.CreateDirectory(sDirectory);
                }
                //and overwrite existing file
                File.Copy(fromFile, toFile, true);
                bHasCopied = true;
            }
            return bHasCopied;
        }
        public static async Task<bool> CopyFilesAsync(ContentURI uri, 
            string fromFile, string toFile)
        {
            bool bHasCopied = false;
            if (Helpers.FileStorageIO.URIAbsoluteExists(uri, fromFile) == true
                && fromFile.Equals(toFile) == false
                && (!string.IsNullOrEmpty(toFile)))
            {
                string sDirectory = Path.GetDirectoryName(toFile);
                if (!Directory.Exists(sDirectory))
                {
                    Directory.CreateDirectory(sDirectory);
                }
                bool bIsAsync = true;
                FileStream fsSoure = new FileStream(fromFile, FileMode.Open,
                    FileAccess.Read, FileShare.None, 8192, bIsAsync);
                if (fsSoure != null)
                {
                    using (fsSoure)
                    {
                        //and overwrite existing file
                        bHasCopied = await WriteBinaryBlobFileAsync(toFile, fsSoure);
                    }
                }
            }
            return bHasCopied;
        }
        public static async Task<bool> CopyDirectoryFilesAsync(
            ContentURI uri, string fromFile, string toFile)
        {
            bool bHasCopied = false;
            if (Helpers.FileStorageIO.URIAbsoluteExists(uri, fromFile) == true
                && fromFile.Equals(toFile) == false
                && (!string.IsNullOrEmpty(toFile)))
            {
                string sFromDirectory = Path.GetDirectoryName(
                    fromFile);
                string sToDirectory = Path.GetDirectoryName(
                    toFile);
                if (!Directory.Exists(sToDirectory))
                {
                    Directory.CreateDirectory(sToDirectory);
                }
                foreach (string filename in Directory.EnumerateFiles(sFromDirectory))
                {
                    using (FileStream SourceStream = File.Open(filename, FileMode.Open))
                    {
                        using (FileStream DestinationStream = File.Create(
                            sToDirectory + filename.Substring(filename.LastIndexOf('\\'))))
                        {
                            await SourceStream.CopyToAsync(DestinationStream);
                        }
                    }
                }
            }
            return bHasCopied;
        }
        

        public static void MoveFiles(ContentURI uri, 
            string fromFile, string toFile)
        {
            if (Helpers.FileStorageIO.URIAbsoluteExists(uri, fromFile) == true
                && fromFile.Equals(toFile) == false
                && (!string.IsNullOrEmpty(fromFile))
                && (!string.IsNullOrEmpty(toFile)))
            {
                string sDirectory = Path.GetDirectoryName(toFile);
                if (!Directory.Exists(sDirectory))
                {
                    Directory.CreateDirectory(sDirectory);
                }
                if (Helpers.FileStorageIO.URIAbsoluteExists(uri, toFile))
                {
                    File.Delete(toFile);
                }
                //move
                File.Move(fromFile, toFile);
            }
        }
        public static void CopyDirectories(string fromDirectory, 
            string toDirectory, bool copySubDirs, bool needsNewSubDirectories)
        {
            DirectoryInfo dir = new DirectoryInfo(fromDirectory);
            DirectoryInfo[] dirs = dir.GetDirectories();
            // If the destination directory does not exist, create it.
            if (!Directory.Exists(toDirectory))
            {
                Directory.CreateDirectory(toDirectory);
            }
            // Get the file contents of the directory to copy.
            FileInfo[] files = dir.GetFiles();
            string sFullFilePath = string.Empty;
            if (files != null)
            {
                foreach (FileInfo file in files.AsParallel())
                {
                    //create the path to the new copy of the file.
                    sFullFilePath = Path.Combine(toDirectory, file.Name);
                    //copy the file.
                    file.CopyTo(sFullFilePath, true);
                }
            }
            if (copySubDirs)
            {
                string sFullDirectoryPath = toDirectory; 
                //parallel doesn't work well with recursive method Parallel.ForEach(dirs, subdir =>
                foreach (DirectoryInfo subdir in dirs)
                {
                    //packages generally don't want subdirectories because of 
                    //need for consistent relpath to associated resources
                    if (needsNewSubDirectories)
                    {
                        //create the subdirectory.
                        sFullDirectoryPath = Path.Combine(toDirectory, subdir.Name);
                    }
                    //copy the subdirectories.
                    CopyDirectories(subdir.FullName, sFullDirectoryPath,
                        copySubDirs, needsNewSubDirectories);
                }
            }
        }
        public static async Task<bool> CopyDirectoriesAsync(
            ContentURI uri, string fromDirectory,
            string toDirectory, bool copySubDirs, bool needsNewSubDirectories)
        {
            bool bHasCopied = false; 
            DirectoryInfo dir = new DirectoryInfo(fromDirectory);
            if (dir != null)
            {
                DirectoryInfo[] dirs = dir.GetDirectories();
                // If the destination directory does not exist, create it.
                if (!Directory.Exists(toDirectory))
                {
                    Directory.CreateDirectory(toDirectory);
                }
                // Get the file contents of the directory to copy.
                FileInfo[] files = dir.GetFiles();
                string sFullFilePath = string.Empty;
                if (files != null)
                {
                    foreach (FileInfo file in files.AsParallel())
                    {
                        //create the path to the new copy of the file.
                        sFullFilePath = Path.Combine(toDirectory, file.Name);
                        //copy the file.
                        bHasCopied = await CopyFilesAsync(
                            uri, file.FullName, sFullFilePath);
                    }
                }
                if (copySubDirs)
                {
                    string sFullDirectoryPath = toDirectory;
                    if (dirs != null)
                    {
                        //parallel doesn't work well with recursive method Parallel.ForEach(dirs, subdir =>
                        foreach (DirectoryInfo subdir in dirs)
                        {
                            //packages generally don't want subdirectories because of 
                            //need for consistent relpath to associated resources
                            if (needsNewSubDirectories)
                            {
                                //create the subdirectory.
                                sFullDirectoryPath = Path.Combine(toDirectory, subdir.Name);
                            }
                            //copy the subdirectories.
                            await CopyDirectoriesAsync(uri, 
                                subdir.FullName, sFullDirectoryPath,
                                copySubDirs, needsNewSubDirectories);
                        }
                    }
                }
            }
            return bHasCopied;
        }
        public static void DeleteFilesWithChangedNames(string changedFilePath,
            string oldURIName)
        {
            if (Directory.Exists(Path.GetDirectoryName(changedFilePath)))
            {
                DirectoryInfo dir = new DirectoryInfo(Path.GetDirectoryName(changedFilePath));
                FileInfo[] files = dir.GetFiles();
                foreach (FileInfo file in files)
                {
                    if (file.Name.ToLower().StartsWith(oldURIName.ToLower()))
                    {
                        file.Delete();
                    }
                }
            }
        }
        public static void DeleteFile(ContentURI uri, 
            string filetoDelete)
        {
            if (Helpers.FileStorageIO.URIAbsoluteExists(uri, 
                filetoDelete))
            {
                File.Delete(filetoDelete);
            }
        }
        
        public static bool DeleteDirectoryFilesContainingSubstring(
            ContentURI uri, string changedFilePath, string subString)
        {
            bool bIsDeleted = false;
            //160 changed from if fileexists
            if (Helpers.FileStorageIO.DirectoryExists(uri, 
                changedFilePath))
            {
                DirectoryInfo dir = new DirectoryInfo(
                    Path.GetDirectoryName(changedFilePath));
                if (dir != null)
                {
                    FileInfo[] files = dir.GetFiles();
                    if (files != null)
                    {
                        if (files.Count() > 0)
                        {
                            foreach (FileInfo file in files)
                            {
                                if (string.IsNullOrEmpty(subString))
                                {
                                    file.Delete();
                                    bIsDeleted = true;
                                }
                                else
                                {
                                    if (file.Name.ToLower().Contains(subString.ToLower()))
                                    {
                                        file.Delete();
                                        bIsDeleted = true;
                                    }
                                }
                            }
                        }
                        else
                        {
                            bIsDeleted = true;
                        }
                    }
                    else
                    {
                        bIsDeleted = true;
                    }
                }
            }
            return bIsDeleted;
        }

        public static string GetDescendentDirectory(string directoryPath, string directoryPattern)
        {
            string sDescDir = string.Empty;
            string[] arrDirectories
                    = Directory.GetDirectories(Path.GetDirectoryName(directoryPath),
                    directoryPattern, SearchOption.AllDirectories);
            //if Directory.GetDirectories returns nothing then nothing needs 
            //to be deleted or updated
            if (arrDirectories.Count() != 0)
            {
                //won't be more than one (nodename and nodeid combo is unique)
                sDescDir = arrDirectories[0];
            }
            return sDescDir;
        }
        public void GetFileNameAndSize(string filePath, out string fileName, 
            out string fileSize)
        {
            fileName = string.Empty;
            fileSize = string.Empty;
            FileInfo oFileInfo = new FileInfo(filePath);
            if (oFileInfo.Exists)
            {
                fileName = oFileInfo.Name;
                fileSize = oFileInfo.Length.ToString();
            }
        }

        public static bool DeleteDirectory(ContentURI uri, string fullFilePath)
        {
            bool bIsDeleted = false;
            //delete the tempdoc folder
            if (!string.IsNullOrEmpty(fullFilePath))
            {
                //directories have to be empty before being deleted
                bool bDirectoryIsEmpty
                    = DeleteDirectoryFilesContainingSubstring(
                        uri, fullFilePath, string.Empty);
                if (bDirectoryIsEmpty)
                {
                    Directory.Delete(Path.GetDirectoryName(fullFilePath));
                    bIsDeleted = true;
                }
            }
            return bIsDeleted;
        }
        public static bool DeleteDirectory(ContentURI uri, string fullFilePath, bool includeSubDirs)
        {
            bool bIsDeleted = false;
            //delete the tempdoc folder
            if (!string.IsNullOrEmpty(fullFilePath))
            {
                //directories have to be empty before being deleted
                bool bDirectoryIsEmpty
                    = DeleteDirectoryFilesContainingSubstring(
                        uri, fullFilePath, string.Empty);
                if (bDirectoryIsEmpty)
                {
                    Directory.Delete(Path.GetDirectoryName(fullFilePath), includeSubDirs);
                    bIsDeleted = true;
                }
            }
            return bIsDeleted;
        }
        //vs2012 techniques
        public bool WriteBinaryBlobFile(string fullPathToFile, Stream source)
        {
            bool bHasWrote = false;
            //always create a new file (existing file should have been archived)
            string sToDirectory = Path.GetDirectoryName(fullPathToFile);
            if (!Directory.Exists(sToDirectory))
            {
                Directory.CreateDirectory(sToDirectory);
            }
            //always create a new file (existing file should have been archived)
            //note the stream is closed by the calling procedure
            FileStream fsDestination = new FileStream(fullPathToFile, FileMode.Create,
                FileAccess.Write, FileShare.None, 8192, true);
            if (fsDestination != null)
            {
                using (fsDestination)
                {
                    bHasWrote = CopyStream(source, fsDestination);
                }
            }
            return bHasWrote;
        }
        //deprecated
        public bool WriteBinaryBlobFileOld(string fullPathToFile, Stream stream)
        {
            bool bHasWrote = false;
            //always create a new file (existing file should have been archived)
            string sToDirectory = Path.GetDirectoryName(fullPathToFile);
            if (!Directory.Exists(sToDirectory))
            {
                Directory.CreateDirectory(sToDirectory);
            }
            //always create a new file (existing file should have been archived)
            //note the stream is closed by the calling procedure
            FileStream oFileStream = new FileStream(fullPathToFile, FileMode.Create,
                FileAccess.Write, FileShare.None, 8192, true);
            if (oFileStream != null)
            {
                using (oFileStream)
                {
                    //read the stream into a byte array
                    int iStreamLength = (int)stream.Length;
                    byte[] arrFileBytes = new byte[iStreamLength];
                    stream.Read(arrFileBytes, 0, iStreamLength);
                    //write the bytes to the filestream
                    oFileStream.Write(arrFileBytes, 0, arrFileBytes.Length);
                    bHasWrote = true;

                    //alternative way uses less memory, but should be slower
                    ////create the writer for data.
                    //BinaryWriter oBinaryWriter = new BinaryWriter(oFileStream);
                    ////size of the buffer.
                    //int iBufferSize = 100;
                    ////blob byte[] buffer to be filled by GetBytes.
                    //byte[] outBytes = new byte[iBufferSize];
                    ////the bytes returned from GetBytes.
                    //int iRetVal = 0;
                    ////the starting position in the BLOB output.
                    //int iStartIndex = 0;
                    ////read bytes into outByte[] and retain the number of bytes returned.
                    //iRetVal = stream.Read(outBytes, iStartIndex, outBytes.Length);
                    ////continue while there are bytes beyond the size of the buffer.
                    //while (iRetVal == iBufferSize)
                    //{
                    //    oBinaryWriter.Write(outBytes);
                    //    oBinaryWriter.Flush();
                    //    //reposition start index to end of last buffer and fill buffer.
                    //    iStartIndex += iBufferSize;
                    //    iRetVal = stream.Read(outBytes, iStartIndex, outBytes.Length);
                    //}
                    ////write the remaining buffer.
                    //oBinaryWriter.Write(outBytes, 0, iRetVal - 1);
                    //oBinaryWriter.Flush();
                    ////flush and save to the file stream
                    //oBinaryWriter.Close();
                    //bHasWrote = true;
                }
            }
            return bHasWrote;
        }
        public static async Task<bool> WriteBinaryBlobFileAsync(string fullPathToFile, Stream source)
        {
            //stay consistent with azure syntax
            bool bHasWrote = false;
            //always create a new file (existing file should have been archived)
            string sToDirectory = Path.GetDirectoryName(fullPathToFile);
            if (!Directory.Exists(sToDirectory))
            {
                Directory.CreateDirectory(sToDirectory);
            }
            try
            {
                bool bIsAsync = true;
                FileStream fsDestination = new FileStream(fullPathToFile, FileMode.Create,
                    FileAccess.Write, FileShare.None, 8192, bIsAsync);
                if (fsDestination != null)
                {
                    using (fsDestination)
                    {
                        //the source stream has an earlier using statement
                        await CopyStreamAsync(source, fsDestination);
                    }
                }
                bHasWrote = true;
            }
            catch (Exception x)
            {
                bHasWrote = false;
            }
            return bHasWrote;
        }
        //this works but the former method has better async methods
        //public static async Task<bool> WriteBinaryBlobFileAsync(string fullPathToFile, Stream source)
        //{
        //    //stay consistent with azure syntax
        //    bool bHasWrote = false;
        //    //always create a new file (existing file should have been archived)
        //    string sToDirectory = Path.GetDirectoryName(fullPathToFile);
        //    if (!Directory.Exists(sToDirectory))
        //    {
        //        Directory.CreateDirectory(sToDirectory);
        //    }
        //    //always create a new file (existing file should have been archived)
        //    //note the stream is closed by the calling procedure
        //    try
        //    {
        //        //read the stream into a byte array
        //        int iStreamLength = (int)source.Length;
        //        byte[] arrFileBytes = new byte[iStreamLength];
        //        //async did not work
        //        source.Read(arrFileBytes, 0, iStreamLength);
        //        //UnicodeEncoding uniencoding = new UnicodeEncoding();
        //        //string filename = @"c:\Users\exampleuser\Documents\userinputlog.txt";
        //        //byte[] result = uniencoding.GetBytes(source);
        //        //the filemode.create will replace any existing file
        //        using (FileStream SourceStream = File.Open(fullPathToFile, FileMode.Create))
        //        {
        //            SourceStream.Seek(0, SeekOrigin.End);
        //            await SourceStream.WriteAsync(arrFileBytes, 0, arrFileBytes.Length);
        //        }
        //        bHasWrote = true;
        //    }
        //    catch (Exception x)
        //    {
        //        return false;
        //    }

        //    return bHasWrote;
        //}
        //both streams should be wrapped in a using clause that disposes of them
        public static async Task CopyStreamAsync(Stream source, Stream destination)
        {
            //works with small files but not large files why?
            byte[] buffer = new byte[0x1000];
            int numRead;
            while ((numRead = await source.ReadAsync(buffer, 0, buffer.Length)) > 0)
            {
                await destination.WriteAsync(buffer, 0, numRead);
            }
        }
        public static bool CopyStream(Stream source, Stream target)
        {
            bool bHasWrote = false;
            const int bufSize = 0x1000;
            byte[] buf = new byte[bufSize];
            int bytesRead = 0;
            while ((bytesRead = source.Read(buf, 0, bufSize)) > 0)
            {
                target.Write(buf, 0, bytesRead);
            }
            bHasWrote = true;
            return bHasWrote;
        }
    }
}
