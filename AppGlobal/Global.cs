using System;
using System.Collections.Generic;
using VMInterfaces;
using VMModels.Enums;
using VMModels.Model;

namespace AppGlobal
{
    public static class Global
    {
        public static string VisionSN;
        public static bool IsVisionCamera;
        public static bool IS_WOLFCOM;
        public static Account GlobalAccount;
        public static HASH_ALGORITHM DefaultHashAlgorithm;
        public static Dictionary<string, int> ClassificationDays;
        public static string UNCServer;
        public static string RelativePath;
        public static string PrimaryUNCServer;
        public static string PrimaryRelativePath;
        public static string INIFILE;
        public static int RightsProfile;
        public static Guid Camera_LogRecordID;
        public static int Camera_Battery;
        public static int Camera_Disk;
        public static int Camera_FileCount;
        public static string Camera_SerialNum;

        public static string MachineID {  get;  set; }

        public static string IPAddress {  get;  set; }

        public static bool IsRedact {  get;  set; }

        public static string DefaultLanguage {  get;  set; }

        public static bool IsEmail {  get;  set; }

        public static int CameraLicense {  get;  set; }

        public static bool LockLogin {  get;  set; }

        public static string LoginIDName {  get;  set; }

        public static event Global.DEL_DeviceDetection EVT_DeviceDetection;

        
        static Global()
        {
            VisionSN = string.Empty;
            IsVisionCamera = false;
            IS_WOLFCOM = true;
            GlobalAccount = new Account();
            DefaultHashAlgorithm = HASH_ALGORITHM.SHA1;
            ClassificationDays = new Dictionary<string, int>();
            UNCServer = "AppGlobal_Global_unknown1";
            RelativePath = "AppGlobal_Global_unknown2";
            PrimaryUNCServer = "AppGlobal_Global_unknown3";
            PrimaryRelativePath = "AppGlobal_Global_unknown4";
            INIFILE = "AppGlobal_Global_unknown5";
            RightsProfile = 0;
            Camera_LogRecordID = Guid.Empty;
            Camera_Battery = 0;
            Camera_Disk = 0;
            Camera_FileCount = 0;
            Camera_SerialNum = string.Empty;
        }

        
        public static bool IsRights(int profile, UserRights rights)
        {
            bool flag = false;
            UserRights userRights = (UserRights)profile;
            if ((rights & userRights) == rights)
                flag = true;
            return flag;
        }

        
        public static void Log(SystemLog log)
        {
            try
            {
                log.DomainName = Environment.UserDomainName;
                log.LogTimestamp = new DateTime?(DateTime.Now);
                log.MachineAccount = Environment.UserName;
                log.MachineName = Environment.MachineName;
                log.MachineID = Global.MachineID;
                using (RPM_Logs rpmLogs = new RPM_Logs())
                    rpmLogs.LogSystem(log);
            }
            catch
            {
            }
        }

        
        public static void Log(AccountLog log)
        {
            try
            {
                log.DomainName = Environment.UserDomainName;
                log.LogTimestamp = new DateTime?(DateTime.Now);
                log.MachineAccount = Environment.UserName;
                log.MachineName = Environment.MachineName;
                log.AccountId = new Guid?(GlobalAccount.Id);
                log.AccountName = GlobalAccount.ToString();
                log.BadgeNumber = GlobalAccount.BadgeNumber;
                log.IPAddress = IPAddress;
                log.MachineID = MachineID;
                using (RPM_Logs rpmLogs = new RPM_Logs())
                    rpmLogs.LogAccount(log);
            }
            catch
            {
            }
        }

        
        public static void Log(string Action, string memo)
        {
            try
            {
                Global.Log(new AccountLog()
                {
                    Action = Action.ToUpper(),
                    Memo = memo
                });
            }
            catch
            {
            }
        }

        
        public static void Log(CameraLog log)
        {
            try
            {
                log.DomainName = Environment.UserDomainName;
                if (log.AccountID == Guid.Empty)
                {
                    log.AccountID = GlobalAccount.Id;
                    log.AccountName = GlobalAccount.ToString();
                    log.BadgeNumber = GlobalAccount.BadgeNumber;
                }
                log.MachineAccount = Environment.UserName;
                log.MachineName = Environment.MachineName;
                log.IPAddress = IPAddress;
                log.MachineID = MachineID;
                using (RPM_CameraLog rpmCameraLog = new RPM_CameraLog())
                {
                    rpmCameraLog.SaveUpdate(log);
                    rpmCameraLog.Save();
                    Global.Camera_LogRecordID = log.Id;
                }
            }
            catch
            {
            }
        }

        
        public static void DeviceDetection_Callback(bool on_off)
        {
            if (EVT_DeviceDetection == null)
                return;
            EVT_DeviceDetection(on_off);
        }

        public delegate void DEL_DeviceDetection(bool on_off);
    }
}
