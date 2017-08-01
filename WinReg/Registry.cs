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
                    windowPos.Width = Convert.ToInt32(registryKey.GetValue("Width"));
                    windowPos.Height = Convert.ToInt32(registryKey.GetValue("Height"));
                    windowPos.PosX = Convert.ToInt32(registryKey.GetValue("PosX"));
                    windowPos.PosY = Convert.ToInt32(registryKey.GetValue("PosY"));
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
                subKey.SetValue("Width", pos.Width);
                subKey.SetValue("Height", pos.Height);
                subKey.SetValue("PosX", pos.PosX);
                subKey.SetValue("PosY", pos.PosY);
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
