using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web.Security;

namespace Unity
{
    public static class CryptoIO
    {
        private static int _iterations;
        private static int _keySize;
        private static string _hash;
        private static string _salt;
        private static string _vector;
        private static string _pwd;

        
        static CryptoIO()
        {
            CryptoIO._iterations = 2;
            CryptoIO._keySize = 256;
            CryptoIO._hash = "Unity_CryptoIO_unknown1";
            CryptoIO._salt = "Unity_CryptoIO_unknown2";
            CryptoIO._vector = "Unity_CryptoIO_unknown3";
            CryptoIO._pwd = "Unity_CryptoIO_unknown4";
        }

        
        public static string MD5Encrypt(string txt)
        {
            return FormsAuthentication.HashPasswordForStoringInConfigFile(txt, "Unity_CryptoIO_unknown5");
        }

        
        public static string Encrypt(string value)
        {
            return Encrypt<AesManaged>(value);
        }

        
        public static string Encrypt<T>(string value) where T : SymmetricAlgorithm, new()
        {
            byte[] bytes1 = Encoding.ASCII.GetBytes(CryptoIO._vector);
            byte[] bytes2 = Encoding.ASCII.GetBytes(CryptoIO._salt);
            byte[] bytes3 = Encoding.UTF8.GetBytes(value);
            T instance = Activator.CreateInstance<T>();
            byte[] array;
            try
            {
                byte[] bytes4 = new PasswordDeriveBytes(_pwd, bytes2, _hash, _iterations).GetBytes(_keySize / 8);
                instance.Mode = CipherMode.CBC;
                using (ICryptoTransform encryptor = instance.CreateEncryptor(bytes4, bytes1))
                {
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                        {
                            cryptoStream.Write(bytes3, 0, bytes3.Length);
                            cryptoStream.FlushFinalBlock();
                            array = memoryStream.ToArray();
                        }
                    }
                }
                instance.Clear();
            }
            finally
            {
                if (instance != null)
                    instance.Dispose();
            }
            return Convert.ToBase64String(array);
        }

        
        public static string Decrypt(string value)
        {
            return CryptoIO.Decrypt<AesManaged>(value);
        }

        
        public static string Decrypt<T>(string value) where T : SymmetricAlgorithm, new()
        {
            byte[] bytes1 = Encoding.ASCII.GetBytes(CryptoIO._vector);
            byte[] bytes2 = Encoding.ASCII.GetBytes(CryptoIO._salt);
            byte[] buffer = Convert.FromBase64String(value);
            int count = 0;
            T instance = Activator.CreateInstance<T>();
            byte[] numArray;
            try
            {
                byte[] bytes3 = new PasswordDeriveBytes(_pwd, bytes2, _hash, _iterations).GetBytes(_keySize / 8);
                instance.Mode = CipherMode.CBC;
                try
                {
                    using (ICryptoTransform decryptor = instance.CreateDecryptor(bytes3, bytes1))
                    {
                        using (MemoryStream memoryStream = new MemoryStream(buffer))
                        {
                            using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                            {
                                numArray = new byte[buffer.Length];
                                count = cryptoStream.Read(numArray, 0, numArray.Length);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    return string.Empty;
                }
                instance.Clear();
            }
            finally
            {
                if ((object)instance != null)
                    instance.Dispose();
            }
            return Encoding.UTF8.GetString(numArray, 0, count);
        }
    }
}
