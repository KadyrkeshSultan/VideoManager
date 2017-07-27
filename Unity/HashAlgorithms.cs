using System.Security.Cryptography;

namespace Unity
{
    public static class HashAlgorithms
    {
        public static readonly HashAlgorithm MD5;
        public static readonly HashAlgorithm SHA1;
        public static readonly HashAlgorithm SHA256;
        public static readonly HashAlgorithm SHA384;
        public static readonly HashAlgorithm SHA512;
        public static readonly HashAlgorithm RIPEMD160;

        static HashAlgorithms()
        {
            MD5 = new MD5CryptoServiceProvider();
            SHA1 = new SHA1Managed();
            SHA256 = new SHA256Managed();
            SHA384 = new SHA384Managed();
            SHA512 = new SHA512Managed();
            RIPEMD160 = new RIPEMD160Managed();
        }
    }
}
