using System.Security.Cryptography;
using System.Text;

namespace Woeber.Logistics.Infrastructure.Hashing
{
    internal static class HashHelpers
    {
        public static string GetSha512hash(this string input)
        {
            using var hashAlgorithm = SHA512.Create();
            return GetHashHex(input, hashAlgorithm);
        }

        public static string GetSha256hash(this string input)
        {
            using var hashAlgorithm = SHA256.Create();
            return GetHashHex(input, hashAlgorithm);
        }

        public static string GetMd5hash(this string input)
        {
            using var hashAlgorithm = MD5.Create();
            return GetHashHex(input, hashAlgorithm);
        }

        public static string GetHashHex(string input, HashAlgorithm hashAlgorithm)
        {
            var bytes = GetBytes(input);
            return GetHashHex(bytes, hashAlgorithm);
        }

        public static byte[] GetBytes(string input)
        {
            return Encoding.UTF8.GetBytes(input);
        }

        public static string GetHashHex(byte[] bytes, HashAlgorithm hashAlgorithm)
        {
            var hashBytes  = hashAlgorithm.ComputeHash(bytes);
            var hashString = ByteArrayToHexConverter.ByteArrayToHexViaLookupPerByte(hashBytes);
            return hashString;
        }
    }
}