using Microsoft.Win32;
using System;
using System.Security.AccessControl;
using System.Windows.Forms;

namespace WinReg
{
    public static class Registry
    {
        public static string ErrMsg {  get;  set; }

        
        public static WindowPos GetWindowPos(string key)
        {
            WindowPos windowPos = new WindowPos();
            RegistryKey userAppDataRegistry = Application.UserAppDataRegistry;
            RegistryKey registryKey = userAppDataRegistry.OpenSubKey(key);
            if (registryKey != null)
            {
                registryKey.OpenSubKey(key, RegistryKeyPermissionCheck.ReadWriteSubTree, RegistryRights.FullControl);
                try
                {
                    windowPos.Width = Convert.ToInt32(registryKey.GetValue("WinReg_Reg_1"));
                    windowPos.Height = Convert.ToInt32(registryKey.GetValue("WinReg_Reg_2"));
                    windowPos.PosX = Convert.ToInt32(registryKey.GetValue("WinReg_Reg_3"));
                    windowPos.PosY = Convert.ToInt32(registryKey.GetValue("WinReg_Reg_4"));
                    registryKey.Close();
                }
                catch
                {
                }
            }
            userAppDataRegistry.Close();
            return windowPos;
        }

        
        public static void SetWindowPos(WindowPos pos, string key)
        {
            try
            {
                RegistryKey userAppDataRegistry = Application.UserAppDataRegistry;
                RegistryKey subKey = userAppDataRegistry.CreateSubKey(key);
                subKey.SetValue("WinReg_Reg_5", pos.Width);
                subKey.SetValue("WinReg_Reg_6", pos.Height);
                subKey.SetValue("WinReg_Reg_7", pos.PosX);
                subKey.SetValue("WinReg_Reg_8", pos.PosY);
                subKey.Flush();
                subKey.Close();
                userAppDataRegistry.Close();
            }
            catch
            {
            }
        }
    }
}
