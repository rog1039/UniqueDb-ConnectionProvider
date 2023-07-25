using UniqueDb.ConnectionProvider.DataGeneration;
using UniqueDb.ConnectionProvider.DataGeneration.DesignTimeDataGeneration;
using UniqueDb.ConnectionProvider.Infrastructure.Extensions;
using UniqueDb.ConnectionProvider.SqlMetadata.InformationSchema;
using UniqueDb.ConnectionProvider.SqlMetadata.SysTables;

namespace UniqueDb.ConnectionProvider.SqlMetadata;

public static class InformationSchemaMetadataExplorer
{
   public static (List<SISTable> schemaTables, List<SISColumn> schemaColumns)
      GetAllTableAndColumnInfoForDatabase(ISqlConnectionProvider sqlConnectionProvider)
   {
      var informationSchemaTables  = GetAllTables(sqlConnectionProvider);
      var informationSchemaColumns = GetAllColumns(sqlConnectionProvider);
      return (informationSchemaTables, informationSchemaColumns);
   }

   private static List<SISTable> GetAllTables(ISqlConnectionProvider sqlConnectionProvider)
   {
      var informationSchemaTables = sqlConnectionProvider
         .Query<SISTable>(
            "SELECT * FROM INFORMATION_SCHEMA.TABLES ORDER BY TABLE_SCHEMA, TABLE_NAME")
         .ToList();
      return informationSchemaTables;
   }

   private static List<SISColumn> GetAllColumns(ISqlConnectionProvider sqlConnectionProvider)
   {
      var informationSchemaColumns = sqlConnectionProvider
         .Query<SISColumn>(
            "SELECT * FROM INFORMATION_SCHEMA.COLUMNS ORDER BY TABLE_NAME, ORDINAL_POSITION")
         .ToList();
      return informationSchemaColumns;
   }

   public static IList<SISTable> GetInformationSchemaTablesOnly(
      ISqlConnectionProvider sqlConnectionProvider)
   {
      var informationSchemaTables = sqlConnectionProvider
         .Query<SISTable>("SELECT * FROM INFORMATION_SCHEMA.TABLES")
         .ToList();
      return informationSchemaTables;
   }

   public static SISTableDefinition GetInformationSchemaTableDefinition(
      SqlTableReference sqlTableReference)
   {
      var definition = new SISTableDefinition();
      definition.InformationSchemaTable   = GetInformationSchemaTable(sqlTableReference);
      definition.InformationSchemaColumns = GetInformationSchemaColumns(sqlTableReference);
      definition.TableConstraints         = GetTableConstraints(sqlTableReference);
      return definition;
   }

   public static SISTable GetInformationSchemaTable(SqlTableReference sqlTableReference)
   {
      var sqlQuery =
         InformationSchemaMetadataSqlQueryGenerator.GetInformationSchemaTableSqlQuery(sqlTableReference);
      var tables = sqlTableReference.SqlConnectionProvider.Query<SISTable>(sqlQuery).ToList();

      CheckOnlyOneTableWasReturned(tables);
      return tables.SingleOrDefault();
   }

   private static void CheckOnlyOneTableWasReturned(List<SISTable> infSchTable)
   {
      if (infSchTable.Count > 1)
      {
         throw new InvalidOperationException(
            "More than one matching table found.  Please specify a schema name for the table to prevent this.");
      }
   }

   public static IList<SISColumn> GetInformationSchemaColumns(SqlTableReference sqlTableReference)
   {
      var sqlQuery =
         InformationSchemaMetadataSqlQueryGenerator.GetInformationSchemaColumnsSqlQuery(sqlTableReference);
      var tableColumns = sqlTableReference.SqlConnectionProvider.Query<SISColumn>(sqlQuery)
         .ToList();
      return tableColumns;
   }

   public static List<SISTableConstraint> GetTableConstraints(SqlTableReference sqlTableReference)
   {
      var whereclause =
         $"WHERE tableConstraint.TABLE_SCHEMA = @schemaName AND tableConstraint.TABLE_NAME = @tableName ";
      var query       = SISTableConstraint.SqlQuery.Replace("--WHERE", whereclause);
      var queryParams = new { schemaName = sqlTableReference.SchemaName, tableName = sqlTableReference.TableName };
      var result = sqlTableReference.SqlConnectionProvider.Query<SISTableConstraint>(query, queryParams)
         .ToList();
      return result;
   }

   public static List<SysIndexColumn> GetIndexColumnDtos(SqlTableReference sqlTableReference)
   {
      var sqlQuery = SysIndexColumn.GetSqlQuery(sqlTableReference);
      var result   = sqlTableReference.SqlConnectionProvider.Query<SysIndexColumn>(sqlQuery).ToList();
      return result;
   }

   public static List<SysForeignKey> GetForeignKeyColumnDtos(SqlTableReference sqlTableReference)
   {
      var sqlQuery = SysForeignKey.GetSqlQuery(sqlTableReference);
      sqlQuery.ToConsole();
      var result = sqlTableReference.SqlConnectionProvider.Query<SysForeignKey>(sqlQuery).ToList();
      return result;
   }

   public static MainToHistoryTableLink? GetTemporalTableInfo(SqlTableReference sqlTableReference)
   {
      var query = MainToHistoryTableLink.SqlQuery();
      IEnumerable<MainToHistoryTableLink?> result =
         sqlTableReference.SqlConnectionProvider.Query<MainToHistoryTableLink>(query);
      return result
         .SingleOrDefault(x => x.MainSchemaName == sqlTableReference.SchemaName
                            && x.MainTableName == sqlTableReference.TableName);
   }

   public static List<SysComputedColumn> GetComputedColumns(SqlTableReference sqlTableReference)
   {
      var whereClause = "WHERE SCHEMA_NAME(o.schema_id) = @schemaName AND OBJECT_NAME(o.object_id) = @tableName";
      var sqlQuery = ComputedColumnQuery.Query
         .Replace(
            "--WHERE",
            whereClause
         );
      var computedColumns = sqlTableReference.SqlConnectionProvider.Query<SysComputedColumn>(
         sqlQuery,
         new
         {
            schemaName = sqlTableReference.SchemaName,
            tableName = sqlTableReference.TableName,
         });
      return computedColumns.ToList();
   }

   #region Get All TableDefinitions

   public static async Task<List<SISTableDefinition>> GetInformationSchemaTableDefinitions(
      ISqlConnectionProvider sqlConnectionProvider)
   {
      var tables         = GetAllTables(sqlConnectionProvider);
      var allColumns     = GetAllColumns(sqlConnectionProvider);
      var allConstraints = await GetAllTableConstraints(sqlConnectionProvider);

      var columnsByTable     = allColumns.ToDictionaryMany(z => new { z.TABLE_SCHEMA, z.TABLE_NAME });
      var constraintsByTable = allConstraints.ToDictionaryMany(z => new { z.TABLE_SCHEMA, z.TABLE_NAME });

      var results = new List<SISTableDefinition>();
      foreach (var table in tables)
      {
         var key         = new { table.TABLE_SCHEMA, table.TABLE_NAME };
         var columns     = columnsByTable.TryGet(key);
         var constraints = constraintsByTable.TryGet(key);

         var tableDef = new SISTableDefinition()
         {
            InformationSchemaTable   = table,
            InformationSchemaColumns = columns.Value ?? new List<SISColumn>(),
            TableConstraints         = constraints.Value ?? new List<SISTableConstraint>()
         };
         results.Add(tableDef);
      }

      return results;
   }


   public static async Task<IList<SISTableConstraint>> GetAllTableConstraints(ISqlConnectionProvider scp)
   {
      var query  = SISTableConstraint.SqlQuery;
      var result = scp.Query<SISTableConstraint>(query).ToList();
      return result;
   }

   #endregion
}

public class DictionaryResult<T>
{
   private DictionaryResult()
   {
      WasFound = false;
   }

   private DictionaryResult(T value)
   {
      WasFound = true;
      Value    = value;
   }

   public bool WasFound { get; }
   public T    Value    { get; }

   public static DictionaryResult<T> Found(T value)
   {
      var result = new DictionaryResult<T>(value);
      return result;
   }

   public static DictionaryResult<T> NotFound()
   {
      return new DictionaryResult<T>();
   }
}

public class MainToHistoryTableLink
{
   public string? MainSchemaName    { get; set; }
   public string  MainTableName     { get; set; }
   public string? HistorySchemaName { get; set; }
   public string? HistoryTableName  { get; set; }

   public static string SqlQuery()
   {
      var sql =
         """
         SELECT SCHEMA_NAME(t.schema_id) AS MainSchemaName,
         t.name                   AS MainTableName,
         SCHEMA_NAME(h.schema_id) AS HistorySchemaName,
         h.name                   AS HistoryTableName,
         t.*
         FROM sys.tables t
          LEFT JOIN sys.tables h ON t.history_table_id = h.object_id
         ORDER BY SCHEMA_NAME(t.schema_id), t.name
         """;
      return sql;
   }
}