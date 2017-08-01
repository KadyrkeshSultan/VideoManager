using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

namespace Unity
{
    public static class LangCtrl
    {
        public static ResourceManager rm;

        public static string Language {  get;  set; }

        public static event DEL_ChangeLanguage EVT_ChangeLanguage;

        
        public static List<CultureInfo> GetAppLanguages()
        {
            List<CultureInfo> cultureInfos = new List<CultureInfo>();
            string directoryName = Path.GetDirectoryName(Application.ExecutablePath);
            string[] directories = Directory.GetDirectories(Path.Combine(directoryName, "Lang"));
            for (int i = 0; i < (int)directories.Length; i++)
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(directories[i]);
                try
                {
                    cultureInfos.Add(CultureInfo.GetCultureInfo(directoryInfo.Name));
                }
                catch
                {
                }
            }
            return cultureInfos;
        }

        
        public static string GetString(string str)
        {
            string empty1 = string.Empty;
            string empty2;
            try
            {
                empty2 = rm.GetString(str);
            }
            catch (Exception ex)
            {
                empty2 = string.Empty;
            }
            if (empty2 == null)
                empty2 = string.Empty;
            return empty2;
        }

        
        public static string GetString(string str, string str2)
        {
            string empty = string.Empty;
            string str1;
            try
            {
                str1 = rm.GetString(str);
            }
            catch (Exception ex)
            {
                str1 = str2;
            }
            if (str1 == null)
                str1 = str2;
            return str1;
        }

        
        public static bool SetLanguage(string lng)
        {
            bool flag = false;
            try
            {
                string directoryName = Path.GetDirectoryName(Application.ExecutablePath);
                if (File.Exists(Path.Combine(directoryName, string.Concat("Lang/", lng, "/locale.dll"))))
                {
                    Assembly assembly = Assembly.LoadFrom(Path.Combine(directoryName, string.Concat("Lang/", lng, "/locale.dll")));
                    rm = new ResourceManager(string.Concat("resources.", lng), assembly);
                    Language = lng;
                    flag = true;
                }
            }
            catch (Exception exception)
            {
                throw;
            }
            return flag;
        }

        
        public static void reText(Control pControl)
        {
            foreach (Control control1 in (ArrangedElementCollection)pControl.Controls)
            {
                try
                {
                    string str = rm.GetString(control1.Name);
                    if (str != null)
                        control1.Text = str;
                    if (pControl.Controls.Count > 0)
                    {
                        foreach (Control control2 in (ArrangedElementCollection)pControl.Controls)
                            reText(control2);
                    }
                }
                catch (Exception ex)
                {
                    string message = ex.Message;
                }
            }
        }

        
        public static void Update()
        {
            LangCallback();
        }

        
        private static void LangCallback()
        {
            if (EVT_ChangeLanguage == null)
                return;
            EVT_ChangeLanguage();
        }

        public delegate void DEL_ChangeLanguage();
    }
}
