namespace UniqueDb.ConnectionProvider.CoreTypes;

public class SqlColumn
{
    public string  Name            { get; set; }
    public int     OrdinalPosition { get; set; }
    public SqlType SqlDataType     { get; set; }
    public bool    IsNullable      { get; set; }
    public string  Default         { get; set; }
    public bool?   IsIdentity      { get; set; }
    public bool?   IsComputed      { get; set; }
    public bool?   IsPrimaryKey    { get; set; }
}