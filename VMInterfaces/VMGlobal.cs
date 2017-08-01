using System.IO;
using Unity;
using VMModels.Model;

namespace VMInterfaces
{
    public static class VMGlobal
    {
        public static string ProfilePath;
        public static string ProfileFile;
        public static string DBConnectionProfile;
        public static string CameraActions;
        public static string LogActions;
        public static string[] SystemRoles;
        public static string MachineID {  get;  set; }
        public static string MachineAccount {  get;  set; }
        public static string MachineName {  get;  set; }
        public static string DomainName {  get;  set; }
        public static string IPAddress {  get;  set; }
        public static string LoginIDName {  get;  set; }
        public static Account AccountRecord {  get;  set; }

        static VMGlobal()
        {
            VMGlobal.ProfilePath = "C:\\ProgramData\\HD Protech\\C3Sentinel";
            VMGlobal.ProfileFile = "C3Sentinel.dat";
            VMGlobal.DBConnectionProfile = "";
            VMGlobal.CameraActions = "Check-In,Check-Out,Download,Clear,Battery,Disk Space,Is Cite Camera,Update Camera Profile,Clear Camera Profile,Camera Docked";
            VMGlobal.LogActions = "Logon,Logout,Logon Failed,Logon Count,List,View,Save,Update,Delete,Video,Image,DVD,Export,Upload,Error Code,System Error,Password";
            VMGlobal.SystemRoles = new string[6]
            {
                "Administor",
                "Supevisor",
                "Standad Account",
                "View Only",
                "Guest",
                "None"
            };
        }

        public static void GetDBConnection()
        {
            if (!Directory.Exists(ProfilePath))
            {
                Directory.CreateDirectory(ProfilePath);
                Network.SetAcl(ProfilePath);
            }
            string str = Path.Combine(ProfilePath, ProfileFile);
            if (!File.Exists(str))
            {
                DBConnectionProfile = "C3Sentinel";
                return;
            }
            DBProfileData dBProfileDatum = (DBProfileData)FileCrypto.LoadConfig(str);
            if (dBProfileDatum.IsLocalDB)
            {
                DBConnectionProfile = "C3Sentinel";
                return;
            }
            object[] dataSource = new object[] { dBProfileDatum.DataSource, dBProfileDatum.Catalog, dBProfileDatum.PersistSecurityInfo, dBProfileDatum.UserId, dBProfileDatum.Password };
            DBConnectionProfile = string.Format("Data Source={0};Initial Catalog={1};Persist Security Info={2};User ID={3};Password={4}", dataSource);
        }

        public static void SetTestConnection(string conn)
        {
            DBConnectionProfile = conn;
        }

        /// <summary>
        /// Действия лога
        /// </summary>
        public enum LOG_ACTION
        {
            LOGON,
            LOGOUT,
            LOGON_FAILED,
            LOGON_COUNT,
            LIST,
            VIEW,
            SAVE,
            UPDATE,
            DELETE,
            VIDEO,
            IMAGE,
            DVD,
            EXPORT,
            UPLOAD,
            CODE_ERROR,
            SYSTEM_ERROR,
            PASSWORD,
        }
    }
}
