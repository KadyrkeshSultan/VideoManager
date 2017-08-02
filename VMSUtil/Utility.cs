using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using VMInterfaces;

namespace VMSUtil
{
    public static class Utility
    {
        public static string PwdMsg
        {
            get;
            set;
        }

        public static bool CheckPassword(string pwd)
        {
            bool flag = false;
            Utility.PwdMsg = string.Empty;
            int num = 3;
            bool flag1 = false;
            bool flag2 = false;
            bool flag3 = false;
            using (RPM_GlobalConfig rPMGlobalConfig = new RPM_GlobalConfig())
            {
                bool flag4 = Convert.ToBoolean(rPMGlobalConfig.GetConfigValue("PWD_POLICY_ENABLED"));
                flag3 = flag4;
                if (flag4)
                {
                    flag1 = Convert.ToBoolean(rPMGlobalConfig.GetConfigValue("PWD_UPPERCASE", "False"));
                    flag2 = Convert.ToBoolean(rPMGlobalConfig.GetConfigValue("PWD_NUMBER", "False"));
                    num = Convert.ToInt32(rPMGlobalConfig.GetConfigValue("PWD_MIN_LEN", "3"));
                }
            }
            bool flag5 = false;
            bool flag6 = true;
            bool flag7 = true;
            if (!flag3)
            {
                flag = true;
            }
            else
            {
                if (pwd.Length >= num)
                {
                    flag5 = true;
                }
                if (flag2)
                {
                    if (!pwd.Any<char>((char c) => char.IsDigit(c)))
                    {
                        flag6 = false;
                    }
                }
                if (flag1)
                {
                    if (!pwd.Any<char>((char c) => char.IsUpper(c)))
                    {
                        flag7 = false;
                    }
                }
                bool flag8 = flag7 & flag6 & flag5;
                flag = flag8;
                if (!flag8)
                {
                    using (RPM_GlobalConfig rPMGlobalConfig1 = new RPM_GlobalConfig())
                    {
                        string str = string.Format("Password Requirements\nLength: {0}", num);
                        if (flag2)
                        {
                            str = string.Concat(str, "\nAt least one number");
                        }
                        if (flag1)
                        {
                            str = string.Concat(str, "\nAt least one upper case letter");
                        }
                        Utility.PwdMsg = str;
                    }
                }
            }
            return flag;
        }

        public static PasswordScore CheckStrength(string password)
        {
            int num = 1;
            if (password.Length < 1)
            {
                return PasswordScore.Blank;
            }
            if (password.Length < 4)
            {
                return PasswordScore.VeryWeak;
            }
            if (password.Length >= 8)
            {
                num++;
            }
            if (password.Length >= 12)
            {
                num++;
            }
            if (Regex.Match(password, "/\\d+/", RegexOptions.ECMAScript).Success)
            {
                num++;
            }
            if (Regex.Match(password, "/[a-z]/", RegexOptions.ECMAScript).Success && Regex.Match(password, "/[A-Z]/", RegexOptions.ECMAScript).Success)
            {
                num++;
            }
            if (Regex.Match(password, "/.[!,@,#,$,%,^,&,*,?,_,~,-,£,(,)]/", RegexOptions.ECMAScript).Success)
            {
                num++;
            }
            return (PasswordScore)num;
        }

        public static void ClearCameraINI(string USBDrive, string serialNum)
        {
            if (!string.IsNullOrEmpty(USBDrive))
            {
                try
                {
                    string str = Path.Combine(string.Concat(USBDrive, "\\"), "CITE.INI");
                    if (File.Exists(str))
                    {
                        using (StreamWriter streamWriter = new StreamWriter(str))
                        {
                            streamWriter.WriteLine(string.Format(string.Concat("SerialNo={0}", Environment.NewLine), serialNum));
                            streamWriter.WriteLine("BadgeNo=");
                            streamWriter.WriteLine("UserName=");
                            streamWriter.WriteLine("DeptID=");
                            streamWriter.WriteLine("DeptName=");
                        }
                    }
                }
                catch
                {
                }
            }
        }

        public static List<string> GetCameraINI(string USBDrive)
        {
            List<string> strs = new List<string>();
            if (!string.IsNullOrEmpty(USBDrive))
            {
                string str = Path.Combine(string.Concat(USBDrive, "\\"), "CITE.INI");
                if (File.Exists(str))
                {
                    try
                    {
                        using (StreamReader streamReader = new StreamReader(str))
                        {
                            while (!streamReader.EndOfStream)
                            {
                                strs.Add(streamReader.ReadLine());
                            }
                        }
                    }
                    catch
                    {
                    }
                }
            }
            return strs;
        }

        public static void SaveCameraINI(string USBDrive, string serialNum, string badge, string name, string deptID, string dept)
        {
            if (!string.IsNullOrEmpty(USBDrive))
            {
                string str = Path.Combine(string.Concat(USBDrive, "\\"), "CITE.INI");
                if (File.Exists(str))
                {
                    try
                    {
                        using (StreamWriter streamWriter = new StreamWriter(str))
                        {
                            streamWriter.WriteLine(string.Format("SerialNo={0}", serialNum));
                            streamWriter.WriteLine(string.Format("BadgeNo={0}", badge));
                            streamWriter.WriteLine(string.Format("UserName={0}", name));
                            streamWriter.WriteLine(string.Format("DeptID={0}", deptID));
                            streamWriter.WriteLine(string.Format("DeptName={0}", dept));
                        }
                    }
                    catch (Exception exception)
                    {
                        string message = exception.Message;
                    }
                }
            }
        }
    }
}