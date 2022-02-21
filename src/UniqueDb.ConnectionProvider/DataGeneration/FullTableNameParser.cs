using StackExchange.Profiling.Internal;

namespace UniqueDb.ConnectionProvider.DataGeneration;

public record QualifiedTableName
{
    public string? SchemaName { get; init; }
    public string  TableName  { get; init; }

    public QualifiedTableName(string? schemaName, string tableName)
    {
        SchemaName = schemaName.IsNullOrWhiteSpace() ? null : schemaName;
        TableName  = tableName;
    }

    public void Deconstruct(out string? SchemaName, out string TableName)
    {
        SchemaName = this.SchemaName;
        TableName  = this.TableName;
    }

    public static QualifiedTableName Parse(string input) => FullTableNameParser.ParseFullTableName(input);

    public string ToStringBrackets()   => $"[{SchemaName}].[{TableName}]";
    public string ToStringNoBrackets() => $"{SchemaName}.{TableName}";
};

public class FullTableNameParser
{
    public static QualifiedTableName ParseFullTableName(string input)
    {
        var chars = input.AsSpan();

        bool insideBrackets    = false,
             seenSeparatingDot = false;

        string name1       = String.Empty,
               name2       = String.Empty,
               currentName = String.Empty;

        for (int i = 0; i < chars.Length; i++)
        {
            char current = chars[i];
            switch (current)
            {
                case '.' when !insideBrackets:
                {
                    if (seenSeparatingDot) throw new Exception("Seen more than 1 separating dot in name: {input}");

                    //We are not in a bracket, so let's skip.
                    name1             = currentName;
                    currentName       = String.Empty;
                    seenSeparatingDot = true;
                    break;
                }
                case '[':
                    insideBrackets = true;
                    break;
                case ']':
                    insideBrackets = false;
                    break;
                default:
                    currentName += current;
                    break;
            }
        }

        if (name1.IsNullOrWhiteSpace()) return new QualifiedTableName(null, currentName);

        name2 = currentName;
        return new QualifiedTableName(name1, name2);
    }
}