using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace VMSUtil
{
    public static class Cite
    {
        private const string _dll = "UMDSC.DLL";

        public static string DeviceName
        {
            get;
            set;
        }

        public static List<string> GetUserInfo(string Serial)
        {
            Cite.DeviceName = Serial;
            List<string> strs = new List<string>();
            StringBuilder stringBuilder = new StringBuilder(16);
            StringBuilder stringBuilder1 = new StringBuilder(16);
            StringBuilder stringBuilder2 = new StringBuilder(16);
            StringBuilder stringBuilder3 = new StringBuilder(16);
            StringBuilder stringBuilder4 = new StringBuilder(16);
            if (Cite.XX_GetUserInfo(Serial, stringBuilder, stringBuilder1, stringBuilder2, stringBuilder3, stringBuilder4))
            {
                strs.Add(string.Format("Serial No={0}", stringBuilder4.ToString()));
                strs.Add(string.Format("Dept Name={0}", stringBuilder1.ToString()));
                strs.Add(string.Format("Dept ID={0}", stringBuilder2.ToString()));
                strs.Add(string.Format("User Name={0}", stringBuilder3.ToString()));
                strs.Add(string.Format("Badge No={0}", stringBuilder.ToString()));
            }
            return strs;
        }

        [DllImport("UMDSC.DLL", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.None, ExactSpelling = false)]
        public static extern int XX_CheckSystemPwd(string pSerial, string pPwd);

        [DllImport("UMDSC.DLL", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.None, ExactSpelling = false)]
        public static extern bool XX_GetDeviceBattery(string pSerial, out int pBatteryPercent, out int pNeedTimeForBatteryFull);

        [DllImport("UMDSC.DLL", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.None, ExactSpelling = false)]
        public static extern bool XX_GetDevSerial([Out] char[,] Items);

        [DllImport("UMDSC.DLL", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ExactSpelling = false)]
        public static extern bool XX_GetUserInfo(string pSerial, StringBuilder DeptName, StringBuilder DeptID, StringBuilder UserName, StringBuilder Badge, StringBuilder CamSerial);

        [DllImport("UMDSC.DLL", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.None, ExactSpelling = false)]
        public static extern bool XX_SetTime(string pSerial, int nDateFormat);
    }
}