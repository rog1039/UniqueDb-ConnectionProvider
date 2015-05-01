namespace UniqueDb.ConnectionProvider.Tests.DataGeneration
{
    public static class StringExtensions
    {
        public static string Bracketize(this string input)
        {
            return "[" + input + "]";
        }

        public static string Bracify(this string input)
        {
            return "{" + input + "}";
        }
    }
}