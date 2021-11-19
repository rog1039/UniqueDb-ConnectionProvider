using StackExchange.Profiling.Internal;

namespace UniqueDb.ConnectionProvider.DataGeneration;

public record FullTableName(string? SchemaName, string TableName);

public class FullTableNameParser
{
    public static FullTableName ParseFullTableName(string input)
    {
        var    chars       = input.AsSpan();
        var    inBracket   = false;
        string name1       = String.Empty,
               name2       = String.Empty,
               currentName = String.Empty;
        ;
        for (int i = 0; i < chars.Length; i++)
        {
            char current = chars[i];
            if (current == '.')
            {
                if (!inBracket)
                {
                    //We are not in a bracket, so let's skip.
                    name1 = currentName;
                    currentName = String.Empty;
                    continue;
                }
            }

            if (current == '[')
            {
                inBracket = true;
                continue;
            }

            if (current == ']')
            {
                inBracket = false;
                continue;
            }

            currentName += current;
        }

        if (!name1.IsNullOrWhiteSpace())
        {
            name2 = currentName;
            return new FullTableName(name1, name2);
        }

        return new FullTableName(null, currentName);
    }

    public enum ParserStatus
    {
        ReadingName,
    }
}