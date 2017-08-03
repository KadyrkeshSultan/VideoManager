using System;
using System.IO;

namespace VideoManager
{
    public static class CiteCamera
    {
        public static string CameraProfile(string RootDrive)
        {
            string empty = string.Empty;
            string str = Path.Combine(RootDrive, "CONFIG\\HDPCITE.INI");
            if (File.Exists(str))
            {
                try
                {
                    using (StreamReader streamReader = new StreamReader(str))
                    {
                        string empty1 = string.Empty;
                        while (true)
                        {
                            string str1 = streamReader.ReadLine();
                            empty1 = str1;
                            if (str1 == null)
                            {
                                break;
                            }
                            empty = string.Concat(empty, empty1, Environment.NewLine);
                        }
                    }
                }
                catch
                {
                }
            }
            return empty;
        }

        public static string DailyLog(string RootDrive)
        {
            string empty = string.Empty;
            string str = Path.Combine(RootDrive, "DailyLog");
            try
            {
                if (Directory.Exists(str))
                {
                    string[] files = Directory.GetFiles(str, "*.txt");
                    string[] strArrays = files;
                    for (int i = 0; i < (int)strArrays.Length; i++)
                    {
                        string str1 = strArrays[i];
                        string empty1 = string.Empty;
                        using (StreamReader streamReader = new StreamReader(str1))
                        {
                            while (true)
                            {
                                string str2 = streamReader.ReadLine();
                                empty1 = str2;
                                if (str2 == null)
                                {
                                    break;
                                }
                                empty = string.Concat(empty, empty1, Environment.NewLine);
                            }
                        }
                    }
                    string[] strArrays1 = files;
                    for (int j = 0; j < (int)strArrays1.Length; j++)
                    {
                        File.Delete(strArrays1[j]);
                    }
                }
            }
            catch
            {
            }
            return empty;
        }
    }
}