namespace UniqueDb.ConnectionProvider;

public class UniqueNameManager
{
    public static string GenerateUniqueName(string prefix, bool includeDateTime = true)
    {
        var dateTimeNamePart = includeDateTime
            ? $"-{DateTime.Now:yyyy.MM.ddThh.mm.ss.fff}"
            : String.Empty;
        var uniquePart = Guid.NewGuid().ToString("D");
        var newName    = $"{prefix}{dateTimeNamePart}-{uniquePart}";
        return newName;
    }
}