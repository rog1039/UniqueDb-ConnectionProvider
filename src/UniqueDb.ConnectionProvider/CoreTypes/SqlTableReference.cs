using UniqueDb.ConnectionProvider.Infrastructure.Extensions;

namespace UniqueDb.ConnectionProvider.CoreTypes;

public class SqlTableReference
{
   public ISqlConnectionProvider SqlConnectionProvider { get; set; }
   public string                 TableName             { get; set; }
   public string                 SchemaName            { get; set; }

   private const string TableNameLengthMessage =
      "a schema and table name is expected -- use form similar to Schema.Table";

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

   public override string ToString()
   {
      var text      = $"{SqlConnectionProvider.ServerName}.{SqlConnectionProvider.DatabaseName}.{FullTableName()}";
      return text;
   }

   public string FullTableName()
   {
      var tableName = new TableName(SchemaName, TableName);
      return tableName.ToString();
   }

   public object ToAnonymousSqlParamObject()
   {
      return new { SchemaName, TableName };
   }

   public TableName ToTableName() => new TableName(this);
}

public class TableName
{
   public TableName(SqlTableReference sqlTableRef) :this(sqlTableRef.SchemaName, sqlTableRef.TableName)
   {
      
   }
   public TableName(string Schema, string Table)
   {
      this.Schema = Schema;
      this.Table  = Table;
   }

   public override string ToString()
   {
      return $"{Schema}.{Table}";
   }

   public string Schema { get; init; }
   public string Table  { get; init; }
   
   public void Deconstruct(out string Schema, out string Table)
   {
      Schema = this.Schema;
      Table  = this.Table;
   }
}