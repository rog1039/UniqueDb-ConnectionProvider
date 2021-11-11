using System;
using System.Linq;

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
        var names = qualifiedTableName.Split(new[] {'.'}, StringSplitOptions.RemoveEmptyEntries);
        if (names.Length < 2) throw new ArgumentException(TableNameLengthMessage, nameof(qualifiedTableName));
            
        SchemaName = names[0].Debracketize();
        TableName  = string.Join(".", names.Skip(1)).Debracketize();
    }

    public SqlTableReference(ISqlConnectionProvider sqlConnectionProvider, string schemaName, string tableName)
    {
        SqlConnectionProvider = sqlConnectionProvider;
        SchemaName            = schemaName.Debracketize();
        TableName             = tableName.Debracketize();
    }
}