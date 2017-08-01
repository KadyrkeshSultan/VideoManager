using System;
using System.Management;
using System.Security.Cryptography;
using System.Text;

namespace Unity
{
    public static class FingerPrint
    {
        private static string fingerPrint;

        static FingerPrint()
        {
            fingerPrint = string.Empty;
        }

        private static string baseId()
        {
            string str = string.Concat(identifier("Win32_BaseBoard", "Model"), identifier("Win32_BaseBoard", "Manufacturer"), identifier("Win32_BaseBoard", "Name"), identifier("Win32_BaseBoard", "SerialNumber"));
            return str;
        }

        private static string biosId()
        {
            string[] strArrays = new string[] { identifier("Win32_BIOS", "Manufacturer"), identifier("Win32_BIOS", "SMBIOSBIOSVersion"), identifier("Win32_BIOS", "IdentificationCode"), identifier("Win32_BIOS", "SerialNumber"), identifier("Win32_BIOS", "ReleaseDate"), identifier("Win32_BIOS", "Version") };
            return string.Concat(strArrays);
        }

        private static string cpuId()
        {
            string str = identifier("Win32_Processor", "UniqueId");
            if (str == "")
            {
                str = identifier("Win32_Processor", "ProcessorId");
                if (str == "")
                {
                    str = identifier("Win32_Processor", "Name");
                    if (str == "")
                    {
                        str = identifier("Win32_Processor", "Manufacturer");
                    }
                    str = string.Concat(str, identifier("Win32_Processor", "MaxClockSpeed"));
                }
            }
            return str;
        }

        private static string diskId()
        {
            string str = string.Concat(identifier("Win32_DiskDrive", "Model"), identifier("Win32_DiskDrive", "Manufacturer"), identifier("Win32_DiskDrive", "Signature"), identifier("Win32_DiskDrive", "TotalHeads"));
            return str;
        }

        private static string GetHash(string s)
        {
            MD5 mD5CryptoServiceProvider = new MD5CryptoServiceProvider();
            byte[] bytes = (new ASCIIEncoding()).GetBytes(s);
            return GetHexString(mD5CryptoServiceProvider.ComputeHash(bytes));
        }

        private static string GetHexString(byte[] bt)
        {
            char chr;
            string empty = string.Empty;
            for (int i = 0; i < (int)bt.Length; i++)
            {
                int num = bt[i];
                int num1 = num & 15;
                int num2 = num >> 4 & 15;
                if (num2 <= 9)
                {
                    empty = string.Concat(empty, num2.ToString());
                }
                else
                {
                    chr = (char)(num2 - 10 + 65);
                    empty = string.Concat(empty, chr.ToString());
                }
                if (num1 <= 9)
                {
                    empty = string.Concat(empty, num1.ToString());
                }
                else
                {
                    chr = (char)(num1 - 10 + 65);
                    empty = string.Concat(empty, chr.ToString());
                }
                if ((i + 1 == (int)bt.Length ? false : (i + 1) % 2 == 0))
                {
                    empty = string.Concat(empty, "-");
                }
            }
            return empty;
        }

        private static string identifier(string wmiClass, string wmiProperty, string wmiMustBeTrue)
        {
            string str = "";
            foreach (ManagementObject instance in (new ManagementClass(wmiClass)).GetInstances())
            {
                if (!(instance[wmiMustBeTrue].ToString() == "True") || !(str == ""))
                {
                    continue;
                }
                try
                {
                    str = instance[wmiProperty].ToString();
                    break;
                }
                catch
                {
                }
            }
            return str;
        }

        private static string identifier(string wmiClass, string wmiProperty)
        {
            string str = "";
            foreach (ManagementObject instance in (new ManagementClass(wmiClass)).GetInstances())
            {
                if (str != "")
                {
                    continue;
                }
                try
                {
                    str = instance[wmiProperty].ToString();
                    break;
                }
                catch
                {
                }
            }
            return str;
        }

        private static string macId()
        {
            return identifier("Win32_NetworkAdapterConfiguration", "MACAddress", "IPEnabled");
        }

        public static string Value()
        {
            if (string.IsNullOrEmpty(fingerPrint))
            {
                string[] strArrays = new string[] { "CPU >> ", cpuId(), "\nBIOS >> ", biosId(), "\nBASE >> ", baseId(), videoId(), "\nMAC >> ", macId() };
                fingerPrint = GetHash(string.Concat(strArrays));
            }
            return fingerPrint;
        }

        private static string videoId()
        {
            return string.Concat(identifier("Win32_VideoController", "DriverVersion"), identifier("Win32_VideoController", "Name"));
        }
    }
}