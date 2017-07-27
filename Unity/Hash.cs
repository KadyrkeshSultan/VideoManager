using System;
using System.IO;
using System.Security.Cryptography;

namespace Unity
{
    public static class Hash
    {
        public static string GetHashFromFile(string fileName, HashAlgorithm algorithm)
        {
            using (BufferedStream bufferedStream = new BufferedStream(File.OpenRead(fileName), 100000))
                return BitConverter.ToString(algorithm.ComputeHash(bufferedStream)).Replace("Unity_Hash_unknown1", string.Empty);
        }
    }
}
