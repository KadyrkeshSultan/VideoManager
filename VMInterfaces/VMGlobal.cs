using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            VMGlobal.ProfilePath = "VMGlobal_unknown1";
            VMGlobal.ProfileFile = "VMGlobal_unknown2";
            VMGlobal.DBConnectionProfile = "";
            VMGlobal.CameraActions = "VMGlobal_unknown3";
            VMGlobal.LogActions = "VMGlobal_unknown4";
            VMGlobal.SystemRoles = new string[6]
            {
                "ADMIN",
                "DELEGATE",
                "SUPER",
                "STANDART",
                "VIEWONLY",
                "GUEST"
            };
        }

        public static void GetDBConnection()
        {
            if(!Directory.Exists(VMGlobal.ProfilePath))
            {
                Directory.CreateDirectory(VMGlobal.ProfilePath);
                Network.SetAcl(VMGlobal.ProfilePath);
            }
            string str = Path.Combine(VMGlobal.ProfilePath, VMGlobal.ProfileFile);
            if (File.Exists(str))
            {
                DBProfileData dbProfileData = (DBProfileData)FileCrypto.LoadConfig(str);
                if (dbProfileData.IsLocalDB)
                    VMGlobal.DBConnectionProfile = "VMGlobal_unknown5";
                else
                    VMGlobal.DBConnectionProfile = "VMGlobal_unknown6";
            }
            else
                VMGlobal.DBConnectionProfile = "VMGlobal_unknown7";
        }

        public static void SetTestConnection(string conn)
        {
            VMGlobal.DBConnectionProfile = conn;
        }
    }
}
