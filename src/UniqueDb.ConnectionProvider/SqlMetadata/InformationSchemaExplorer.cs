using UniqueDb.ConnectionProvider.CoreTypes;
using UniqueDb.ConnectionProvider.Infrastructure.Extensions;
using UniqueDb.ConnectionProvider.SqlMetadata.InformationSchema;
using UniqueDb.ConnectionProvider.SqlMetadata.SysTables;

namespace UniqueDb.ConnectionProvider.SqlMetadata;

public static class InformationSchemaExplorer
{
   public static (List<SISTable> schemaTables, List<SISColumn> schemaColumns)
      GetAllTableAndColumnInfoForDatabase(ISqlConnectionProvider sqlConnectionProvider)
   {
      var informationSchemaTables  = GetAllSisTables(sqlConnectionProvider);
      var informationSchemaColumns = GetAllSisColumns(sqlConnectionProvider);
      return (informationSchemaTables, informationSchemaColumns);
   }

   public static List<SISTable> GetAllSisTables(ISqlConnectionProvider sqlConnectionProvider)
   {
      var informationSchemaTables = sqlConnectionProvider
         .Query<SISTable>(
            "SELECT * FROM INFORMATION_SCHEMA.TABLES ORDER BY TABLE_SCHEMA, TABLE_NAME")
         .ToList();
      return informationSchemaTables;
   }

   private static List<SISColumn> GetAllSisColumns(ISqlConnectionProvider sqlConnectionProvider)
   {
      var informationSchemaColumns = sqlConnectionProvider
         .Query<SISColumn>(
            "SELECT * FROM INFORMATION_SCHEMA.COLUMNS ORDER BY TABLE_NAME, ORDINAL_POSITION")
         .ToList();
      return informationSchemaColumns;
   }

   public static IList<SISTable> GetSisTablesOnly(
      ISqlConnectionProvider sqlConnectionProvider)
   {
      var informationSchemaTables = sqlConnectionProvider
         .Query<SISTable>("SELECT * FROM INFORMATION_SCHEMA.TABLES")
         .ToList();
      return informationSchemaTables;
   }

   public static SISTableDefinition GetSisTableDefinition(
      SqlTableReference sqlTableReference)
   {
      var definition = new SISTableDefinition();
      definition.InformationSchemaTable   = GetSisTable(sqlTableReference);
      definition.InformationSchemaColumns = GetSisColumns(sqlTableReference);
      definition.TableConstraints         = GetSisTableConstraints(sqlTableReference);
      return definition;
   }

   public static SISTable GetSisTable(SqlTableReference sqlTableReference)
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

   public static IList<SISColumn> GetSisColumns(SqlTableReference sqlTableReference)
   {
      var sqlQuery =
         InformationSchemaMetadataSqlQueryGenerator.GetInformationSchemaColumnsSqlQuery(sqlTableReference);
      var tableColumns = sqlTableReference.SqlConnectionProvider.Query<SISColumn>(sqlQuery)
         .ToList();
      return tableColumns;
   }

   public static List<SISTableConstraint> GetSisTableConstraints(SqlTableReference sqlTableReference)
   {
      var whereclause =
         $"WHERE tableConstraint.TABLE_SCHEMA = @schemaName AND tableConstraint.TABLE_NAME = @tableName ";
      var query       = SISTableConstraint.SqlQuery.Replace("--WHERE", whereclause);
      var queryParams = new { schemaName = sqlTableReference.SchemaName, tableName = sqlTableReference.TableName };
      var result = sqlTableReference.SqlConnectionProvider.Query<SISTableConstraint>(query, queryParams)
         .ToList();
      return result;
   }

   #region Get All TableDefinitions

   public static async Task<List<SISTableDefinition>> GetSisTableDefinitions(
      ISqlConnectionProvider sqlConnectionProvider)
   {
      var tables         = GetAllSisTables(sqlConnectionProvider);
      var allColumns     = GetAllSisColumns(sqlConnectionProvider);
      var allConstraints = await GetAllSisTableConstraints(sqlConnectionProvider);

      var columnsByTable     = allColumns.ToDictionaryMany(z => new { z.TABLE_SCHEMA, z.TABLE_NAME });
      var constraintsByTable = allConstraints.ToDictionaryMany(z => new { z.TABLE_SCHEMA, z.TABLE_NAME });

      var results = new List<SISTableDefinition>();
      foreach (var table in tables)
      {
         var key         = new { table.TABLE_SCHEMA, table.TABLE_NAME };
         var columns     = columnsByTable.GetValueOrDefault(key);
         var constraints = constraintsByTable.GetValueOrDefault(key);

         var tableDef = new SISTableDefinition()
         {
            InformationSchemaTable   = table,
            InformationSchemaColumns = columns ?? new List<SISColumn>(),
            TableConstraints         = constraints ?? new List<SISTableConstraint>()
         };
         results.Add(tableDef);
      }

      return results;
   }


   public static async Task<IList<SISTableConstraint>> GetAllSisTableConstraints(ISqlConnectionProvider scp)
   {
      var query  = SISTableConstraint.SqlQuery;
      var result = scp.Query<SISTableConstraint>(query).ToList();
      return result;
   }

   #endregion
}