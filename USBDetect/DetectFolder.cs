using System;
using System.IO;
using System.Threading;

namespace USBDetect
{
    public class DetectFolder
    {
        private string Drive;

        private string[] Patterns;

        private bool IsAllFolders;

        public DetectFolder()
        {
            Drive = string.Empty;
        }

        private void Callback(string folder)
        {
            EVT_FolderScan?.Invoke(folder);
        }

        private void RunScan()
        {
            SearchOption searchOption = SearchOption.TopDirectoryOnly;
            if (IsAllFolders)
            {
                searchOption = SearchOption.AllDirectories;
            }
            string empty = string.Empty;
            try
            {
                string[] directories = Directory.GetDirectories(Drive, "*", searchOption);
                if ((int)directories.Length <= 0)
                {
                    empty = Drive;
                }
                else
                {
                    string[] strArrays = directories;
                    for (int i = 0; i < (int)strArrays.Length; i++)
                    {
                        string str = strArrays[i];
                        string[] patterns = this.Patterns;
                        for (int j = 0; j < (int)patterns.Length; j++)
                        {
                            string str1 = patterns[j];
                            if (str.ToUpper().Contains(str1.ToUpper()))
                            {
                                empty = str;
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                string message = exception.Message;
            }
            Callback(empty);
        }

        public void ScanFolders(string DriveID, string[] Pattern, bool AllFolders)
        {
            Drive = DriveID.Substring(0, 1);
            Patterns = Pattern;
            IsAllFolders = AllFolders;
            if (!string.IsNullOrEmpty(this.Drive))
            {
                DetectFolder detectFolder = this;
                detectFolder.Drive = string.Concat(detectFolder.Drive, ":");
                if (!this.Drive[this.Drive.Length - 1].Equals('\\'))
                {
                    DetectFolder detectFolder1 = this;
                    detectFolder1.Drive = string.Concat(detectFolder1.Drive, "\\");
                }
                (new Thread(new ThreadStart(this.RunScan))).Start();
            }
        }

        public event DEL_FolderScan EVT_FolderScan;

        public delegate void DEL_FolderScan(string folder);
    }
}