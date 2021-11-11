using System.Security.Cryptography;
using System.Text;

namespace UniqueDb.ConnectionProvider.Tracing.Hashing;

internal static class HashHelpers
{
    public static string GetSha512hash(this string input)
    {
        using var hashAlgorithm = SHA512.Create();
        return GetHashAsHexString(input, hashAlgorithm);
    }

    public static string GetSha256hash(this string input)
    {
        using var hashAlgorithm = SHA256.Create();
        return GetHashAsHexString(input, hashAlgorithm);
    }

    public static string GetMd5hash(this string input)
    {
        using var hashAlgorithm = MD5.Create();
        return GetHashAsHexString(input, hashAlgorithm);
    }

    public static string GetHashAsHexString(string input, HashAlgorithm hashAlgorithm)
    {
        var bytes = GetBytes(input);
        return GetHexStringFromBytes(bytes, hashAlgorithm);
    }

    public static byte[] GetBytes(string input)
    {
        return Encoding.UTF8.GetBytes(input);
    }

    public static string GetHexStringFromBytes(byte[] bytes, HashAlgorithm hashAlgorithm)
    {
        var hashBytes  = hashAlgorithm.ComputeHash(bytes);
        var hashString = ByteArrayToHexConverter.ByteArrayToHexViaLookupPerByte(hashBytes);
        return hashString;
    }
}