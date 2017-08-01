using System;
using System.IO;
using VIBlend.Utilities;

namespace Unity
{
    public static class GlobalAppCfg
    {
        public static string MachineName;
        public static string MachineDomain;
        public static string MachineAccount;

        public static string AppDataPath
        {
            get
            {
                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "HD Protech\\Sentinel");
            }
        }

        public static string DatabaseConnStr {  get;  set; }

        public static DateTime StartTime {  get;  set; }

        public static string LoginName {  get;  set; }

        public static DateTime LoginTime {  get;  set; }

        public static bool IsDebug {  get;  set; }

        public static VIBLEND_THEME AppTheme {  get;  set; }

        public static event DEL_StatusMsg EVT_StatusMsg;

        public static event DEL_MsgBox EVT_MsgBox;

        
        static GlobalAppCfg()
        {
            MachineName = Environment.MachineName;
            MachineDomain = Environment.UserDomainName;
            MachineAccount = Environment.UserName;
        }

        
        public static void StatusMsgCallback(string txt)
        {
            if (EVT_StatusMsg == null)
                return;
            EVT_StatusMsg(txt);
        }

        
        public static void MsgBoxCallback(string txt)
        {
            if (EVT_MsgBox == null)
                return;
            EVT_MsgBox(txt);
        }

        public delegate void DEL_StatusMsg(string txt);

        public delegate void DEL_MsgBox(string txt);
    }
}
