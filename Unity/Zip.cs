using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.IO;

namespace Unity
{
    public static class Zip
    {
        public static bool ZipToFile(string sourceFile, string targetPath, string fileName)
        {
            bool flag = false;
            string fileName1 = Path.Combine(targetPath, fileName);
            try
            {
                if (File.Exists(sourceFile))
                {
                    if (Directory.Exists(targetPath))
                    {
                        using (ZipFile zipFile = new ZipFile())
                        {
                            zipFile.AddFile(sourceFile);
                            zipFile.Save(fileName1);
                            flag = true;
                        }
                    }
                }
            }
            catch
            {
            }
            return flag;
        }

        public static bool ZipFolders(List<string> Folders, string ZipFile, string Pwd)
        {
            bool flag;
            try
            {
                using (ZipFile zipFile = new ZipFile(ZipFile))
                {
                    if (!string.IsNullOrEmpty(Pwd))
                        zipFile.Password = Pwd;
                    foreach (string folder in Folders)
                    {
                        if (Directory.Exists(folder))
                            zipFile.AddDirectory(folder, Path.GetFileName(folder));
                    }
                    zipFile.Save();
                }
                flag = true;
            }
            catch (Exception ex)
            {
                flag = false;
            }
            return flag;
        }

        public static bool UnZipFile(string rootFolder, string targetFile)
        {
            bool flag = false;
            try
            {
                if (File.Exists(rootFolder))
                {
                    using (ZipFile zipFile = ZipFile.Read(targetFile))
                    {
                        zipFile.ExtractAll(rootFolder, ExtractExistingFileAction.DoNotOverwrite);
                        flag = true;
                    }
                }
            }
            catch
            {
            }
            return flag;
        }
    }
}
