using System.Linq;

namespace UniqueDb.ConnectionProvider.Tracing.Hashing;

internal static class ByteArrayToHexConverter
{
    static readonly uint[] Lookup32 = Enumerable.Range(0, 256).Select(i =>
    {
        string s = i.ToString("X2");
        return ((uint) s[0]) + ((uint) s[1] << 16);
    }).ToArray();

    public static string ByteArrayToHexViaLookupPerByte(byte[] bytes)
    {
        var result = new char[bytes.Length * 2];
        for (var i = 0; i < bytes.Length; i++)
        {
            var val = Lookup32[bytes[i]];
            result[2 * i]     = (char) val;
            result[2 * i + 1] = (char) (val >> 16);
        }
        return new string(result);
    }
}