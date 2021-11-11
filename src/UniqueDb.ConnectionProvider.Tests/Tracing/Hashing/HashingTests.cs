using System.Security.Cryptography;
using System.Text;
using UniqueDb.ConnectionProvider.Tracing.Hashing;
using Xunit;
using Xunit.Abstractions;

namespace UniqueDb.ConnectionProvider.Tests.Tracing.Hashing;

public class HashingTests : UnitTestBaseWithConsoleRedirection
{
    [Fact()]
    [Trait("Category", "Instant")]
    public void TestHashesFromEncodingsAscii()
    {
        /*
         * In this case, default (which is utf8 in netcore), ascii, and utf8 give the same result.
         * This is because for ascii characters only, ascii == utf8
         */
        var text = "test";
        ExamineBytesAndHashes(text);
    }

    [Fact()]
    [Trait("Category", "Instant")]
    public void TestHashesFromEncodingsUnicode()
    {
        /*
         * In this case, default (which is utf8 in netcore), and utf8 of course, are different than ascii.
         * Makes sense because netcore will, for characters outside of 0-127, the normal ascii range, encode them
         * as ascii value 63 (0x3F) which is the '?' character.
         *
         * https://docs.microsoft.com/en-us/dotnet/api/system.text.encoding.ascii?view=net-5.0 explains.
         */
        var text         = "\u03a0"; // \u03a0 is the utf8 code for Pi
        ExamineBytesAndHashes(text);
    }

    private void ExamineBytesAndHashes(string text)
    {
        var defaultBytes = Encoding.Default.GetBytes(text); //Default is utf8 in netcore.
        var asciiBytes   = Encoding.ASCII.GetBytes(text);
        var utf8Bytes    = Encoding.UTF8.GetBytes(text);
        var utf16Bytes   = Encoding.Unicode.GetBytes(text);
        var utf32Bytes   = Encoding.UTF32.GetBytes(text);

        PrintBytes(defaultBytes, "default");
        PrintBytes(asciiBytes,   "ascii");
        PrintBytes(utf8Bytes,    "utf8");
        PrintBytes(utf16Bytes,   "utf16");
        PrintBytes(utf32Bytes,   "utf32");

        Console.WriteLine("\r\n");

        PrintMd5Bytes(defaultBytes, "default");
        PrintMd5Bytes(asciiBytes,   "ascii");
        PrintMd5Bytes(utf8Bytes,    "utf8");
        PrintMd5Bytes(utf16Bytes,   "utf16");
        PrintMd5Bytes(utf32Bytes,   "utf32");
    }

    private void PrintMd5Bytes(byte[] bytes, string encoding)
    {
        var md5Bytes = MD5.Create().ComputeHash(bytes);
        var md5Hex   = ByteArrayToHexConverter.ByteArrayToHexViaLookupPerByte(md5Bytes);
        Console.WriteLine($"{encoding,-7}: {md5Hex}");
    }

    private void PrintBytes(byte[] bytes, string encoding)
    {
        Console.WriteLine($"{encoding,-7}: {ByteArrayToHexConverter.ByteArrayToHexViaLookupPerByte(bytes)}");
    }

    public HashingTests(ITestOutputHelper outputHelperHelper) : base(outputHelperHelper) { }
}