using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using System.Runtime.Serialization.Formatters.Binary;
using System.Data;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using System.IO.Compression;

namespace FS.Common.Compression
{
    public class Compression
    {
        public static List<string> DecompressZipFileToDirectory(string inFileName, string outDirectory)
        {
            List<string> unzippedFileList = new List<string>();
            try
            {
                using (ZipInputStream s = new ZipInputStream(File.OpenRead(inFileName)))
                {
                    ZipEntry theEntry;
                    while ((theEntry = s.GetNextEntry()) != null)
                    {
                        Console.WriteLine(theEntry.Name);

                        string directoryName = Path.GetDirectoryName(theEntry.Name);
                        string fileName = Path.GetFileName(theEntry.Name);

                        // create directory
                        if (directoryName.Length > 0)
                        {
                            Directory.CreateDirectory(outDirectory + "\\" + directoryName);
                        }

                        if (fileName != String.Empty)
                        {
                            using (FileStream streamWriter = File.Create(outDirectory + "\\" + theEntry.Name))
                            {
                                int size = 2048;
                                byte[] data = new byte[2048];
                                while (true)
                                {
                                    size = s.Read(data, 0, data.Length);
                                    if (size > 0)
                                    {
                                        streamWriter.Write(data, 0, size);
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                            }

                            unzippedFileList.Add(outDirectory + "\\" + theEntry.Name);
                        }
                    }
                }
            }
            catch (Exception)
            {
                unzippedFileList.Clear();
                FastZip fastZip = new FastZip();
                string fileFilter = null;
                fastZip.ExtractZip(inFileName, outDirectory, fileFilter);

                DirectoryInfo d = new DirectoryInfo(outDirectory);
                FileInfo[] Files = d.GetFiles("*.*", SearchOption.AllDirectories);
                string str = "";
                foreach (FileInfo file in Files)
                {
                    unzippedFileList.Add(file.FullName);
                }
            }
            return unzippedFileList;
        }

        public static List<string> DecompressZipFileReturnList(string inFileName)
        {
            List<string> unzippedFileList = new List<string>();
            string outDirectory = Path.GetDirectoryName(inFileName);

            try
            {
                using (ZipInputStream s = new ZipInputStream(File.OpenRead(inFileName)))
                {
                    ZipEntry theEntry;
                    while ((theEntry = s.GetNextEntry()) != null)
                    {
                        Console.WriteLine(theEntry.Name);

                        string directoryName = Path.GetDirectoryName(theEntry.Name);
                        string fileName = Path.GetFileName(theEntry.Name);

                        // create directory
                        if (directoryName.Length > 0)
                        {
                            Directory.CreateDirectory(outDirectory + "\\" + directoryName);
                        }

                        if (fileName != String.Empty)
                        {
                            using (FileStream streamWriter = File.Create(outDirectory + "\\" + theEntry.Name))
                            {
                                int size = 2048;
                                byte[] data = new byte[2048];
                                while (true)
                                {
                                    size = s.Read(data, 0, data.Length);
                                    if (size > 0)
                                    {
                                        streamWriter.Write(data, 0, size);
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                            }

                            unzippedFileList.Add(outDirectory + "\\" + theEntry.Name);
                        }
                    }
                }
            }
            catch (Exception)
            {
                outDirectory = outDirectory + "/DecompressZipFile" + Guid.NewGuid().ToString();

                Directory.CreateDirectory(outDirectory);

                unzippedFileList.Clear();
                FastZip fastZip = new FastZip();
                string fileFilter = null;
                fastZip.ExtractZip(inFileName, outDirectory, fileFilter);

                DirectoryInfo d = new DirectoryInfo(outDirectory);                
                FileInfo[] Files = d.GetFiles("*.*", SearchOption.AllDirectories);
                string str = "";
                foreach (FileInfo file in Files)
                {
                    unzippedFileList.Add(file.FullName);
                }
            }

            return unzippedFileList;
        }

        public static void DecompressZipFile(String inFileName)
        {
            try
            {
                using (ZipInputStream s = new ZipInputStream(File.OpenRead(inFileName)))
                {

                    ZipEntry theEntry;
                    while ((theEntry = s.GetNextEntry()) != null)
                    {
                        Console.WriteLine(theEntry.Name);

                        string directoryName = Path.GetDirectoryName(theEntry.Name);
                        string fileName = Path.GetFileName(theEntry.Name);

                        // create directory
                        if (directoryName.Length > 0)
                        {
                            Directory.CreateDirectory(directoryName);
                        }

                        if (fileName != String.Empty)
                        {
                            using (FileStream streamWriter = File.Create(theEntry.Name))
                            {

                                int size = 2048;
                                byte[] data = new byte[2048];
                                while (true)
                                {
                                    size = s.Read(data, 0, data.Length);
                                    if (size > 0)
                                    {
                                        streamWriter.Write(data, 0, size);
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                var outDirectory = Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
                FastZip fastZip = new FastZip();
                string fileFilter = null;
                fastZip.ExtractZip(inFileName, outDirectory, fileFilter);
            }
        }

        public static void DecompressZipFile(String inFileName, String outputFileName)
        {
            try
            {
                using (ZipInputStream s = new ZipInputStream(File.OpenRead(inFileName)))
                {
                    ZipEntry theEntry;
                    while ((theEntry = s.GetNextEntry()) != null)
                    {

                        Console.WriteLine(theEntry.Name);

                        string directoryName = Path.GetDirectoryName(outputFileName);
                        string fileName = Path.GetFileName(outputFileName);

                        // create directory
                        if (directoryName.Length > 0)
                        {
                            Directory.CreateDirectory(directoryName);
                        }

                        if (fileName != String.Empty)
                        {
                            using (FileStream streamWriter = File.Create(outputFileName))
                            {

                                int size = 2048;
                                byte[] data = new byte[2048];
                                while (true)
                                {
                                    size = s.Read(data, 0, data.Length);
                                    if (size > 0)
                                    {
                                        streamWriter.Write(data, 0, size);
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                var outDirectory = Path.GetDirectoryName(outputFileName);
                FastZip fastZip = new FastZip();
                string fileFilter = null;
                fastZip.ExtractZip(inFileName, outDirectory, fileFilter);

                DirectoryInfo d = new DirectoryInfo(outDirectory);
                FileInfo[] Files = d.GetFiles("*.*", SearchOption.AllDirectories);
                string str = "";
                var lstfile = Files[Files.Length - 1].FullName;
                
                for (var i = 0;i< Files.Length - 1; i++)
                {
                    File.Delete(Files[i].FullName);
                }
                File.Move(lstfile, outputFileName);
            }
        }

        public static void CompressToZipFile(String inFileName, String outputFileName)
        {

            try
            {
                // Depending on the directory this could be very large and would require more attention
                // in a commercial package.
                string[] filenames = Directory.GetFiles(inFileName);

                // 'using' statements gaurantee the stream is closed properly which is a big source
                // of problems otherwise.  Its exception safe as well which is great.
                using (ZipOutputStream s = new ZipOutputStream(File.Create(outputFileName)))
                {

                    s.SetLevel(9); // 0 - store only to 9 - means best compression

                    byte[] buffer = new byte[4096];

                    foreach (string file in filenames)
                    {

                        // Using GetFileName makes the result compatible with XP
                        // as the resulting path is not absolute.
                        ZipEntry entry = new ZipEntry(Path.GetFileName(file));

                        // Setup the entry data as required.

                        // Crc and size are handled by the library for seakable streams
                        // so no need to do them here.

                        // Could also use the last write time or similar for the file.
                        entry.DateTime = DateTime.Now;
                        FileInfo inf = new FileInfo(file);
                        entry.Size = inf.Length;
                        entry.CompressionMethod = CompressionMethod.Stored;
                        s.PutNextEntry(entry);

                        using (FileStream fs = File.OpenRead(file))
                        {

                            // Using a fixed size buffer here makes no noticeable difference for output
                            // but keeps a lid on memory usage.
                            int sourceBytes;
                            do
                            {
                                sourceBytes = fs.Read(buffer, 0, buffer.Length);
                                s.Write(buffer, 0, sourceBytes);
                            } while (sourceBytes > 0);
                        }
                    }

                    // Finish/Close arent needed strictly as the using statement does this automatically

                    // Finish is important to ensure trailing information for a Zip file is appended.  Without this
                    // the created file would be invalid.
                    s.Finish();

                    // Close is important to wrap things up and unlock the file.
                    s.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception during processing {0}", ex);

                // No need to rethrow the exception as for our purposes its handled.
            }
        }

        public static void CompressToZipFile(string[] filenames, String outputFileName)
        {
            try
            {
                FileStream fsOut = File.Create(outputFileName);
                ZipOutputStream zipStream = new ZipOutputStream(fsOut);

                zipStream.SetLevel(9); //0-9, 9 being the highest level of compression

                //zipStream.Password = password;	// optional. Null is the same as not setting. Required if using AES.

                // This setting will strip the leading part of the folder path in the entries, to
                // make the entries relative to the starting folder.
                // To include the full path for each entry up to the drive root, assign folderOffset = 0.
                //int folderOffset = folderName.Length + (folderName.EndsWith("\\") ? 0 : 1);

                CompressFiles(filenames, zipStream, 0);

                zipStream.IsStreamOwner = true;	// Makes the Close also Close the underlying stream
                zipStream.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception during processing {0}", ex);

                // No need to rethrow the exception as for our purposes its handled.
            }
        }

        private static void CompressFiles(string[] files, ZipOutputStream zipStream, int folderOffset)
        {
            foreach (string filename in files)
            {
                if (!System.IO.File.Exists(filename))
                    continue;

                FileInfo fi = new FileInfo(filename);

                //string entryName = filename.Substring(folderOffset); // Makes the name in zip based on the folder
                //entryName = ZipEntry.CleanName(entryName); // Removes drive from name and fixes slash direction
                ZipEntry newEntry = new ZipEntry(fi.Name);
                newEntry.DateTime = fi.LastWriteTime; // Note the zip format stores 2 second granularity

                // Specifying the AESKeySize triggers AES encryption. Allowable values are 0 (off), 128 or 256.
                // A password on the ZipOutputStream is required if using AES.
                //   newEntry.AESKeySize = 256;

                // To permit the zip to be unpacked by built-in extractor in WinXP and Server2003, WinZip 8, Java, and other older code,
                // you need to do one of the following: Specify UseZip64.Off, or set the Size.
                // If the file may be bigger than 4GB, or you do not need WinXP built-in compatibility, you do not need either,
                // but the zip will be in Zip64 format which not all utilities can understand.
                //   zipStream.UseZip64 = UseZip64.Off;
                newEntry.Size = fi.Length;

                zipStream.PutNextEntry(newEntry);

                // Zip the file in buffered chunks
                // the "using" will close the stream even if an exception occurs
                byte[] buffer = new byte[4096];
                using (FileStream streamReader = File.OpenRead(filename))
                {
                    ICSharpCode.SharpZipLib.Core.StreamUtils.Copy(streamReader, zipStream, buffer);
                }
                zipStream.CloseEntry();
            }
        }

        public static string DecompressGZFile(string fileName)
        {
            string newFileName = string.Empty;
            FileInfo fileToDecompress = new FileInfo(fileName);
            using (FileStream originalFileStream = fileToDecompress.OpenRead())
            {
                string currentFileName = fileToDecompress.FullName;
                newFileName = currentFileName.Remove(currentFileName.Length - fileToDecompress.Extension.Length);

                using (FileStream decompressedFileStream = File.Create(newFileName))
                {
                    using (GZipStream decompressionStream = new GZipStream(originalFileStream, CompressionMode.Decompress))
                    {
                        decompressionStream.CopyTo(decompressedFileStream);
                    }
                }
            }
            return newFileName;
        }

        public static void DecompressGZFile(string[] fileNames)
        {
            foreach (string fileName in fileNames)
            {
                FileInfo fileToDecompress = new FileInfo(fileName);
                using (FileStream originalFileStream = fileToDecompress.OpenRead())
                {
                    string currentFileName = fileToDecompress.FullName;
                    string newFileName = currentFileName.Remove(currentFileName.Length - fileToDecompress.Extension.Length);

                    using (FileStream decompressedFileStream = File.Create(newFileName))
                    {
                        using (GZipStream decompressionStream = new GZipStream(originalFileStream, CompressionMode.Decompress))
                        {
                            decompressionStream.CopyTo(decompressedFileStream);
                        }
                    }
                }
            }
        }
    }
}
