namespace UniqueDb.ConnectionProvider.CoreTypes;

public class SqlTable
{
    public string           Schema     { get; set; }
    public string           Name       { get; set; }
    public IList<SqlColumn> SqlColumns { get; set; }
}