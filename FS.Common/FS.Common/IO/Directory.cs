using System;
using System.IO;

namespace FS.Common.IO
{

    /// <summary>
    /// This class is used to hold functions that help in using and manipulating windows folders.
    /// </summary>
    public class Directory
    {
        /// <summary>
        /// constructor
        /// </summary>
        public Directory()
        {
        }

        static public void Delete(string directoryPath)
        {
            if (!System.IO.Directory.Exists(directoryPath))
                return;
            System.IO.DirectoryInfo directory = new System.IO.DirectoryInfo(directoryPath);

            foreach (System.IO.FileInfo file in directory.GetFiles()) file.Delete();
            foreach (System.IO.DirectoryInfo subDirectory in directory.GetDirectories()) Delete(subDirectory.FullName);
            foreach (System.IO.DirectoryInfo subDirectory in directory.GetDirectories()) subDirectory.Delete(true);
            directory.Delete();

        }

        static public void DeleteFiles(string directoryPath)
        {
            if (!System.IO.Directory.Exists(directoryPath))
                return;
            System.IO.DirectoryInfo directory = new System.IO.DirectoryInfo(directoryPath);

            foreach (System.IO.FileInfo file in directory.GetFiles()) file.Delete();
            foreach (System.IO.DirectoryInfo subDirectory in directory.GetDirectories()) Delete(subDirectory.FullName);
            foreach (System.IO.DirectoryInfo subDirectory in directory.GetDirectories()) subDirectory.Delete(true);
            //directory.Delete();

        }
        static public bool DriveExists(string directoryPath)
        {
            bool result = false;

            result = System.IO.Directory.Exists(directoryPath[0].ToString() + @":\");
            return result;
        }

        /// <summary>
        /// This is used to create a folder if it does not exist.  It also creates all other folders in its path that do not exist.
        /// </summary>
        /// <param name="directoryPath">Path of desired folder.  It may or may not coantain a '\' at the end of the string.</param>
        static public void CreateDirectory(string directoryPath)
        {
            string[] directoryElements = null;
            string directoryToTest = string.Empty;

            directoryPath = directoryPath.Trim().TrimEnd("\\".ToCharArray());
            directoryElements = directoryPath.Split("\\".ToCharArray());
            directoryToTest = directoryElements[0].Trim().TrimEnd("\\".ToCharArray()); ;
            for (int a = 1; a <= directoryElements.GetUpperBound(0) - 1; a++)
            {

                directoryToTest = directoryToTest + "\\" + directoryElements[a];
            }
            directoryToTest = directoryToTest + "\\";
            if (DirectoryExists(directoryToTest) != true)
            {
                CreateDirectory(directoryToTest);
            }
            DirectoryInfo di = System.IO.Directory.CreateDirectory(directoryPath.TrimEnd("\\".ToCharArray()));

        }

        public static void CreateDirectory(System.IO.DirectoryInfo directory) 
        {
            if (!directory.Parent.Exists)
            {   
                CreateDirectory(directory.Parent);
            }
            if (!directory.Exists)
                directory.Create();
        }

        /// <summary>
        /// Used to determine if a directory exists.
        /// </summary>
        /// <param name="directoryPath">Path of desired folder.  It may or may not coantain a '\' at the end of the string.</param>
        /// <returns>True, if the fodler already exists.</returns>
        static public Boolean DirectoryExists(string directoryPath)
        {
            // Specify the directory you want to manipulate.
            Boolean result;

            result = false;
            try
            {
                // Determine whether the directory exists.
                if (System.IO.Directory.Exists(directoryPath))
                {
                    result = true;
                }
            }
            catch (System.Exception)
            {
            }

            return result;

        }

        static public System.Collections.ArrayList GetFolderFiles(string folderPath, string searchPattern)
        {
            System.Collections.ArrayList fileArrayList = null;
            string[] files = null;

            fileArrayList = new System.Collections.ArrayList();
            if (FS.Common.IO.Directory.DirectoryExists(folderPath))
            {
                files = System.IO.Directory.GetFiles(folderPath, searchPattern);
                for (int a = 0; a < files.Length; a++)
                {
                    fileArrayList.Add(files.GetValue(a));
                }
            }
            return fileArrayList;
        }

        public static string GetBinDirectory()
        {
            string folderPath = string.Empty;
            try
            {
                //if (System.IO.File.Exists(AppDomain.CurrentDomain.BaseDirectory.TrimEnd(@"\".ToCharArray()) + @"\FS.Common.dll"))
                //{
                //    folderPath = AppDomain.CurrentDomain.BaseDirectory.TrimEnd(@"\".ToCharArray());
                //}
                //else if (System.IO.File.Exists(AppDomain.CurrentDomain.BaseDirectory.TrimEnd(@"\".ToCharArray()) + @"\Bin\FS.Common.dll"))
                //{
                //    folderPath = AppDomain.CurrentDomain.BaseDirectory.TrimEnd(@"\".ToCharArray()) + @"\Bin";
                //}
            }

            catch (System.Exception)
            {
            }
            return folderPath;
                 
        }
         
    }
}
