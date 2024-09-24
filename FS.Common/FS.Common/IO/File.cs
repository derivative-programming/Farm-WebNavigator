using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace FS.Common.IO
{
    /// <summary>
    /// This class is used to hold functions that help in using and manipulating windows files.
    /// </summary>
    public class File
    {
        public File()
        {
        }

        /// <summary>
        /// used to determine if a file exists
        /// </summary>
        /// <param name="filePath">the fullpath of the file</param>
        /// <returns>true, if a file exists</returns>
        static public Boolean FileExists(string filePath)
        {
            // Specify the directory you want to manipulate.
            Boolean result;

            result = false;
            // Determine whether the directory exists.
            if (System.IO.File.Exists(filePath))
            {
                result = true;

            }
            return result;
        }

        /// <summary>
        /// Used to append data to a file.
        /// </summary>
        /// <param name="filePath">The full path of a file</param>
        /// <param name="data">All data that needs to append to a file</param>
        static public void AppendToFile(string filePath, string data)
        {
            try
            {
                TextWriter writer = TextWriter.Synchronized(System.IO.File.AppendText(filePath));
                writer.Write(data);
                writer.Close();
            }
            catch (System.IO.IOException ex)
            {
                //the file is locked by another process, attempt to log a warning to its err file.
                TextWriter errWriter = TextWriter.Synchronized(System.IO.File.AppendText(filePath + ".err"));
                errWriter.Write("Error:" + ex.Message.ToString() + " when writing...");
                errWriter.Write(data);
                errWriter.Close();
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;

            }

        }

        static public string GetFolderPathFromFilePath(string filePath)
        {

            string[] filePathElements = null;
            string directoryToTest = string.Empty;

            filePath = filePath.Trim();
            filePathElements = filePath.Split("\\".ToCharArray());
            directoryToTest = filePathElements[0].Trim().TrimEnd("\\".ToCharArray());
            for (int a = 1; a <= filePathElements.GetUpperBound(0) - 1; a++)
            {

                directoryToTest = directoryToTest + "\\" + filePathElements[a];
            }
            directoryToTest = directoryToTest + "\\";
            return directoryToTest;
        }

        public static void ExportToCSVFile<T>(List<T> List, string FileName, List<string> Filters)
        {
            ExportToCSVFile(List, FileName, Filters, null);
        }

        public static void ExportToCSVFile<T>(List<T> List, string FileName, List<string> Filters, List<ExportColumnDisplay> ExportHeader)
        {
            StringBuilder sb = new StringBuilder();
            System.IO.StringWriter sw = new System.IO.StringWriter();

            if (List.Count == 0) { return; }

            //First line for column names
            Type t = List[0].GetType();
            foreach (System.Reflection.PropertyInfo pi in t.GetProperties())
            {
                if (Filters.Contains(pi.Name)) continue;
                if (ExportHeader != null)
                {
                    List<ExportColumnDisplay> column = ExportHeader.FindAll(delegate(ExportColumnDisplay c) { return c.PropertyName == pi.Name; });
                    if(column.Count > 0)
                        sb.Append(column[0].DisplayColumnName);
                    else
                        sb.Append(pi.Name);
                } else
                    sb.Append(pi.Name);
                sb.Append(",");
            }
            System.IO.File.AppendAllText(FileName, sb.ToString().Remove(sb.Length - 1, 1), Encoding.Default);
            System.IO.File.AppendAllText(FileName, Environment.NewLine, Encoding.Default);

            foreach (Object obj in List)
            {
                Type type = obj.GetType();
                System.Reflection.PropertyInfo[] props = type.GetProperties();
                sb.Length = 0;
                foreach (System.Reflection.PropertyInfo prop in props)
                {
                    string fieldValue = "";
                    try
                    {
                        if (Filters.Contains(prop.Name)) continue;
                        fieldValue += "\"";
                        fieldValue += FS.Common.Strings.Functions.HtmlDecode((obj == null ? "" : prop.GetValue(obj, null).ToString()));
                        fieldValue += "\",";
                    }
                    catch {
                        fieldValue += "\"\",";
                    }
                    sb.Append(fieldValue);
                }
                sw.WriteLine(sb.ToString().Remove(sb.Length - 1, 1));
                System.IO.File.AppendAllText(FileName, sb.ToString().Remove(sb.Length - 1, 1), Encoding.Default);
                System.IO.File.AppendAllText(FileName, Environment.NewLine, Encoding.Default);
            }
        }

        //		static public string GetFileNameFromFilePath(string filePath)
        //		{
        //			
        //		}
        //		static public string GetFileExtensionFromFilePath(string filePath)
        //		{
        //			
        //		}


        public static List<string[]> ParseCSV(string path)
        {
            List<string[]> parsedData = new List<string[]>();

            try
            {
                using (StreamReader readFile = new StreamReader(path))
                {
                    string line;
                    string[] row;

                    while ((line = readFile.ReadLine()) != null)
                    {
                        row = line.Split(',');
                        parsedData.Add(row);
                    }
                }
            }
            catch (Exception )
            {
                throw;
            }

            return parsedData;
        }

        public static byte[] ReadByteArrayFromFile(string filePath)
        {
            // this method is limited to 2^32 byte files (4.2 GB)

            FileStream fs = System.IO.File.OpenRead(filePath);
            try
            {
                byte[] bytes = new byte[fs.Length];
                fs.Read(bytes, 0, Convert.ToInt32(fs.Length));
                fs.Close();
                return bytes;
            }
            finally
            {
                fs.Close();
            }

        }
        public static bool WriteByteArrayToFile(string fileName, byte[] byteArray)
        {
            try
            {
                // Open file for reading
                System.IO.FileStream fileStream = new System.IO.FileStream(fileName, System.IO.FileMode.Create, System.IO.FileAccess.Write);

                // Writes a block of bytes to this stream using data from a byte array.
                fileStream.Write(byteArray, 0, byteArray.Length);

                // close file stream
                fileStream.Close();

                return true;
            }
            catch (Exception _Exception)
            {
                // Error
                Console.WriteLine("Exception caught in process: {0}", _Exception.ToString());
            }

            // error occured, return false
            return false;
        }

        public static Dictionary<string, string> GetKeyValuePairs(List<string> headers, string fileRecord)
        { 
            return GetKeyValuePairs(headers, fileRecord, true);
        }

        public static Dictionary<string, string> GetKeyValuePairs(List<string> headers, string fileRecord, bool csv)
        {
            return GetKeyValuePairs(headers, fileRecord, csv, true);
        }

        public static Dictionary<string, string> GetKeyValuePairs(List<string> headers, string fileRecord, bool csv, bool performRecordMatch)
        {
            string separator = csv ? "," : "\t";
            return GetKeyValuePairs(headers, fileRecord, separator, true);
        }

        public static Dictionary<string, string> GetKeyValuePairs(List<string> headers, string fileRecord, string separator, bool performRecordMatch)
        { 
            string tempRecord = string.Empty;
            bool inQuote = false;
            Dictionary<string, string> currentRow = new Dictionary<string, string>();

            for (int i = 0; i < fileRecord.ToCharArray().Length; i++)
            {
                if (fileRecord[i] == '"')
                {
                    inQuote = !inQuote;
                }
                else if (inQuote && fileRecord[i] == ',')
                {
                    // Comma inside quotes, replace with an improbable string
                    tempRecord += "|%|";
                }
                else
                {
                    tempRecord += fileRecord[i];
                }
            }

            string[] values = tempRecord.Split(separator.ToCharArray());
            try
            {
                for (int i = 0; i < headers.Count; i++)
                {
                    if (currentRow.ContainsKey(headers[i]))
                        continue;
                    if (performRecordMatch && headers.Count <= values.Length)
                        currentRow.Add(headers[i].Trim(), values[i].Replace("|%|", ",").Trim());
                    else if (performRecordMatch == false)
                    {
                        if (i <= (values.Length - 1))
                            currentRow.Add(headers[i].Trim(), values[i].Replace("|%|", ",").Trim());
                        else
                            currentRow.Add(headers[i].Trim(), "");
                    }
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            return currentRow;
        }

        public static string CombineKeyValuePairs(Dictionary<string, string> currentRow, string separator)
        {
            string tempRecord = string.Empty;
            bool inQuote = false;
            try
            {
                foreach (var rowValue in currentRow)
                {
                    if (rowValue.Value.Contains(","))
                        tempRecord += "\"" + rowValue.Value + "\"" + separator;
                    else
                        tempRecord += rowValue.Value + separator;
                }
                tempRecord = tempRecord.TrimEnd(separator.ToCharArray());
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            return tempRecord;
        }

        /// <summary>
        /// 160-bit hash, it's similar to MD5
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string GenerateChecksumForFile(string filePath)
        {
            string checksum = null;

            using (FileStream fs = new FileStream(filePath, FileMode.Open))
            using (BufferedStream bs = new BufferedStream(fs))
            {
                using (System.Security.Cryptography.SHA1Managed sha1 = new System.Security.Cryptography.SHA1Managed())
                {
                    byte[] hash = sha1.ComputeHash(bs);
                    StringBuilder formatted = new StringBuilder(2 * hash.Length);
                    foreach (byte b in hash)
                    {
                        formatted.AppendFormat("{0:X2}", b);
                    }

                    checksum = formatted.ToString();
                }
            }

            return checksum;
        }

    }

    public class ExportColumnDisplay
    {
        public string PropertyName { get; set; }
        public string DisplayColumnName { get; set; }
    }
}
