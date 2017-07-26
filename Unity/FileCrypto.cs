using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;

namespace Unity
{
    public static class FileCrypto
    {
        private static byte[] key;
        private static byte[] iv;

        static FileCrypto()
        {
            FileCrypto.key = new byte[32]
            {
        (byte) 15,
        (byte) 10,
        (byte) 15,
        (byte) 10,
        (byte) 12,
        (byte) 10,
        (byte) 15,
        (byte) 14,
        (byte) 11,
        (byte) 10,
        (byte) 11,
        (byte) 14,
        (byte) 1,
        (byte) 2,
        (byte) 3,
        (byte) 12,
        (byte) 10,
        (byte) 15,
        (byte) 14,
        (byte) 11,
        (byte) 10,
        (byte) 11,
        (byte) 14,
        (byte) 1,
        (byte) 2,
        (byte) 3,
        (byte) 15,
        (byte) 10,
        (byte) 15,
        (byte) 10,
        (byte) 12,
        (byte) 10
            };
            FileCrypto.iv = new byte[16]
            {
        (byte) 11,
        (byte) 10,
        (byte) 11,
        (byte) 14,
        (byte) 1,
        (byte) 2,
        (byte) 3,
        (byte) 15,
        (byte) 11,
        (byte) 10,
        (byte) 11,
        (byte) 14,
        (byte) 1,
        (byte) 2,
        (byte) 3,
        (byte) 15
            };
        }

        public static bool Save(object filedata, string fileName)
        {
            bool flag = false;
            try
            {
                AesCryptoServiceProvider cryptoServiceProvider = new AesCryptoServiceProvider();
                using (FileStream fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write))
                {
                    CryptoStream cryptoStream = new CryptoStream((Stream)fileStream, cryptoServiceProvider.CreateEncryptor(FileCrypto.key, FileCrypto.iv), CryptoStreamMode.Write);
                    new BinaryFormatter().Serialize((Stream)cryptoStream, filedata);
                    cryptoStream.FlushFinalBlock();
                }
                flag = true;
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }
            return flag;
        }

        public static object LoadConfig(string fileName)
        {
            object obj = (object)null;
            try
            {
                AesCryptoServiceProvider cryptoServiceProvider = new AesCryptoServiceProvider();
                using (FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                    obj = new BinaryFormatter().Deserialize((Stream)new CryptoStream((Stream)fileStream, cryptoServiceProvider.CreateDecryptor(FileCrypto.key, FileCrypto.iv), CryptoStreamMode.Read));
            }
            catch (Exception ex)
            {
                obj = (object)null;
            }
            return obj;
        }
    }
}
