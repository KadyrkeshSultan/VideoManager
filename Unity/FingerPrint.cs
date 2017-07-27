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

        
        public static string Value()
        {
            if (string.IsNullOrEmpty(fingerPrint))
                fingerPrint = GetHash("Unity_FingerPrint_unknown1" + cpuId() + "Unity_FingerPrint_unknown2" + biosId() + "Unity_FingerPrint_unknown3" + baseId() + videoId() + "Unity_FingerPrint_unknown4" + macId());
            return fingerPrint;
        }

        
        private static string GetHash(string s)
        {
            return GetHexString(new MD5CryptoServiceProvider().ComputeHash(new ASCIIEncoding().GetBytes(s)));
        }

        
        private static string GetHexString(byte[] bt)
        {
            string str1 = string.Empty;
            for (int index = 0; index < bt.Length; ++index)
            {
                int num1 = (int)bt[index];
                int num2 = num1 & 15;
                int num3 = num1 >> 4 & 15;
                char ch;
                string str2;
                if (num3 > 9)
                {
                    string str3 = str1;
                    ch = (char)(num3 - 10 + 65);
                    string str4 = ch.ToString();
                    str2 = str3 + str4;
                }
                else
                    str2 = str1 + num3.ToString();
                if (num2 > 9)
                {
                    string str3 = str2;
                    ch = (char)(num2 - 10 + 65);
                    string str4 = ch.ToString();
                    str1 = str3 + str4;
                }
                else
                    str1 = str2 + num2.ToString();
                if (index + 1 != bt.Length && (index + 1) % 2 == 0)
                    str1 += "Unity_FingerPrint_unknown5";
            }
            return str1;
        }

        
        private static string identifier(string wmiClass, string wmiProperty, string wmiMustBeTrue)
        {
            string str = "";
            foreach (ManagementObject instance in new ManagementClass(wmiClass).GetInstances())
            {
                if (instance[wmiMustBeTrue].ToString() == "Unity_FingerPrint_unknown6")
                {
                    if (str == "")
                    {
                        try
                        {
                            str = instance[wmiProperty].ToString();
                            break;
                        }
                        catch
                        {
                        }
                    }
                }
            }
            return str;
        }

        
        private static string identifier(string wmiClass, string wmiProperty)
        {
            string str = "";
            foreach (ManagementObject instance in new ManagementClass(wmiClass).GetInstances())
            {
                if (str == "")
                {
                    try
                    {
                        str = instance[wmiProperty].ToString();
                        break;
                    }
                    catch
                    {
                    }
                }
            }
            return str;
        }

        
        private static string cpuId()
        {
            string str1 = identifier("Unity_FingerPrint_unknown7", "Unity_FingerPrint_unknown8");
            if (str1 == "")
            {
                str1 = identifier("Unity_FingerPrint_unknown9", "Unity_FingerPrint_unknown10");
                if (str1 == "")
                {
                    string str2 = identifier("Unity_FingerPrint_unknown11", "Unity_FingerPrint_unknown12");
                    if (str2 == "")
                        str2 = identifier("Unity_FingerPrint_unknown13", "Unity_FingerPrint_unknown14");
                    str1 = str2 + identifier("Unity_FingerPrint_unknown15", "Unity_FingerPrint_unknown16");
                }
            }
            return str1;
        }

        
        private static string biosId()
        {
            return identifier("Unity_FingerPrint_unknown17", "Unity_FingerPrint_unknown18") + identifier("Unity_FingerPrint_unknown19", "Unity_FingerPrint_unknown20") + identifier("Unity_FingerPrint_unknown21", "Unity_FingerPrint_unknown22") + identifier("Unity_FingerPrint_unknown23", "Unity_FingerPrint_unknown24") + identifier("Unity_FingerPrint_unknown25", "Unity_FingerPrint_unknown26") + identifier("Unity_FingerPrint_unknown27", "Unity_FingerPrint_unknown28");
        }

        
        private static string diskId()
        {
            return identifier("Unity_FingerPrint_unknown29", "Unity_FingerPrint_unknown30") + identifier("Unity_FingerPrint_unknown31", "Unity_FingerPrint_unknown32") + identifier("Unity_FingerPrint_unknown33", "Unity_FingerPrint_unknown34") + identifier("Unity_FingerPrint_unknown35", "Unity_FingerPrint_unknown36");
        }

        
        private static string baseId()
        {
            return identifier("Unity_FingerPrint_unknown37", "Unity_FingerPrint_unknown38") + identifier("Unity_FingerPrint_unknown39", "Unity_FingerPrint_unknown40") + identifier("Unity_FingerPrint_unknown40", "Unity_FingerPrint_unknown41") + identifier("Unity_FingerPrint_unknown42", "Unity_FingerPrint_unknown43");
        }

        
        private static string videoId()
        {
            return identifier("Unity_FingerPrint_unknown44", "Unity_FingerPrint_unknown45") + identifier("Unity_FingerPrint_unknown46", "Unity_FingerPrint_unknown47");
        }

        
        private static string macId()
        {
            return identifier("Unity_FingerPrint_unknown48", "Unity_FingerPrint_unknown49", "Unity_FingerPrint_unknown50");
        }
    }
}
