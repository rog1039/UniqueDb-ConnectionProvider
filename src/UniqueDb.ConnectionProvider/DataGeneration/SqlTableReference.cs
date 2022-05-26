namespace UniqueDb.ConnectionProvider.DataGeneration;

public class SqlTableReference
{
    public ISqlConnectionProvider SqlConnectionProvider { get; set; }
    public string                 TableName             { get; set; }
    public string                 SchemaName            { get; set; }
        
    private const string TableNameLengthMessage ="a schema and table name is expected -- use form similar to Schema.Table"; 

    public SqlTableReference(ISqlConnectionProvider sqlConnectionProvider, string qualifiedTableName)
    {
        SqlConnectionProvider = sqlConnectionProvider ?? throw new ArgumentNullException(nameof(sqlConnectionProvider));
            
        if (string.IsNullOrWhiteSpace(qualifiedTableName))
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(qualifiedTableName));

        var fullTableName = DbTableNameWithNullableSchemaParser.ParseFullTableName(qualifiedTableName);
        
        SchemaName = fullTableName.SchemaName ?? throw new Exception("Schema name must be provided.");
        TableName  = fullTableName.TableName;

    }

    public SqlTableReference(ISqlConnectionProvider sqlConnectionProvider, string schemaName, string tableName)
    {
        SqlConnectionProvider = sqlConnectionProvider;
        SchemaName            = schemaName.Debracketize();
        TableName             = tableName.Debracketize();
    }
}