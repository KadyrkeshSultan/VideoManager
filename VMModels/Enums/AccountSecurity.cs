using System;
using System.Collections.Generic;
using Unity;

namespace VMModels.Enums
{
    public static class AccountSecurity
    {
        
        public static string DescString()
        {
            return LangCtrl.GetString("VMModels_AcctSecur_1", "VMModels_AcctSecur_2");
        }

        
        public static List<string> SecurityList()
        {
            List<string> stringList = new List<string>();
            string str1 = DescString();
            char[] chArray = new char[1] { ',' };
            foreach (string str2 in str1.Split(chArray))
                stringList.Add(str2.Trim());
            return stringList;
        }

        
        public static int SecurityValue(SECURITY security)
        {
            return (int)security;
        }

        
        public static string GetSecurityDesc(SECURITY item)
        {
            Array values = Enum.GetValues(typeof(SECURITY));
            List<string> stringList = SecurityList();
            string str = stringList[stringList.Count - 1];
            int index = 0;
            foreach (object obj in values)
            {
                if (item.Equals(obj))
                {
                    str = stringList[index];
                    break;
                }
                ++index;
            }
            return str;
        }

        
        public static SECURITY GetByString(string item)
        {
            Array values = Enum.GetValues(typeof(SECURITY));
            SECURITY security1 = SECURITY.UNCLASSIFIED;
            int index = 0;
            foreach (string security2 in SecurityList())
            {
                if (security2.Equals(item))
                {
                    security1 = (SECURITY)values.GetValue(index);
                    break;
                }
                ++index;
            }
            return security1;
        }

        
        public static List<string> MaxSecurityLevel(SECURITY max)
        {
            List<string> stringList1 = SecurityList();
            List<string> stringList2 = new List<string>();
            Array values = Enum.GetValues(typeof(SECURITY));
            int index = 0;
            bool flag = false;
            foreach (object obj in values)
            {
                if (max.Equals(obj) || flag)
                {
                    stringList2.Add(stringList1[index]);
                    flag = true;
                }
                ++index;
            }
            return stringList2;
        }

        
        public static List<string> MaxSecurityLevel(int x)
        {
            return MaxSecurityLevel((SECURITY)x);
        }
    }
}
