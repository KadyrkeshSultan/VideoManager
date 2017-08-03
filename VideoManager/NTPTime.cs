using System;
using System.Runtime.InteropServices;
using System.Security.Principal;
using Unity;

namespace VideoManager
{
    public class NTPTime
    {
        public NTPTime()
        {
        }

        public void method_0(string url)
        {
            if (!string.IsNullOrEmpty(url))
            {
                try
                {
                    DateTime networkTime = NTP.GetNetworkTime(url);
                    NTPTime.SYSTEMTIME sYSTEMTIME = new NTPTime.SYSTEMTIME()
                    {
                        wHour = (short)networkTime.Hour,
                        wMinute = (short)networkTime.Minute,
                        wSecond = (short)networkTime.Second,
                        wMilliseconds = (short)networkTime.Millisecond,
                        wDay = (short)networkTime.Day,
                        wMonth = (short)networkTime.Month,
                        wYear = (short)networkTime.Year,
                        wDayOfWeek = (short)networkTime.DayOfWeek
                    };
                    IntPtr token = WindowsIdentity.GetCurrent().Token;
                    (new WindowsIdentity(token)).Impersonate();
                    NTPTime.SetSystemTime(ref sYSTEMTIME);
                    NTPTime.SetLocalTime(ref sYSTEMTIME);
                    DateTime now = DateTime.Now;
                }
                catch (Exception exception)
                {
                    string message = exception.Message;
                }
            }
        }

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, ExactSpelling = false, SetLastError = true)]
        internal static extern bool SetLocalTime(ref NTPTime.SYSTEMTIME lpSystemTime);

        [DllImport("kernel32.dll", CharSet = CharSet.None, ExactSpelling = false, SetLastError = true)]
        public static extern bool SetSystemTime([In] ref NTPTime.SYSTEMTIME st);

        public struct SYSTEMTIME
        {
            public short wYear;

            public short wMonth;

            public short wDayOfWeek;

            public short wDay;

            public short wHour;

            public short wMinute;

            public short wSecond;

            public short wMilliseconds;
        }
    }
}