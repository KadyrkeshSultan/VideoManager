using System;
using System.IO;

namespace Unity
{
    public static class Logging
    {
        public static void WriteLog(string msg)
        {
            if (!GlobalAppCfg.IsDebug)
                return;
            if (!Directory.Exists(GlobalAppCfg.AppDataPath))
                Directory.CreateDirectory(GlobalAppCfg.AppDataPath);
            if (Directory.Exists(GlobalAppCfg.AppDataPath))
            {
                string path = Path.Combine(GlobalAppCfg.AppDataPath, "Unity_Logging_unknown1");
                try
                {
                    using (StreamWriter streamWriter = new StreamWriter(path, true))
                        streamWriter.WriteLine(string.Format("Unity_Logging_unknown2", DateTime.Now, msg));
                }
                catch
                {
                }
            }
        }
    }
}
