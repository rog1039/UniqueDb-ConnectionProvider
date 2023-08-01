using UniqueDb.ConnectionProvider.CoreTypes;
using UniqueDb.ConnectionProvider.CSharpGeneration.DesignTimeDataGeneration;
using UniqueDb.ConnectionProvider.Infrastructure.Extensions;
using UniqueDb.ConnectionProvider.SqlMetadata.SysTables;

namespace UniqueDb.ConnectionProvider.SqlMetadata;

public static class SysSchemaExplorer
{
   public static MainToHistoryTableLink? GetTemporalTableInfo(SqlTableReference sqlTableReference)
   {
      var query = MainToHistoryTableLink.SqlQuery();
      IEnumerable<MainToHistoryTableLink?> result =
         sqlTableReference.SqlConnectionProvider.Query<MainToHistoryTableLink>(query);
      return result
         .SingleOrDefault(x => x.MainSchemaName == sqlTableReference.SchemaName
                            && x.MainTableName == sqlTableReference.TableName);
   }

   public static List<SysIndex> GetIndices(SqlTableReference sqlTableReference)
   {
      var sqlQuery = SysIndexColumn.GetSqlQuery(sqlTableReference);
      var result   = sqlTableReference.SqlConnectionProvider.Query<SysIndex>(sqlQuery).ToList();
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
            tableName  = sqlTableReference.TableName,
         });
      return computedColumns.ToList();
   }

   public static IList<SysTable> GetAllTables(ISqlConnectionProvider connectionProvider)
   {
      var query   = SysTable.Query;
      var results = connectionProvider.Query<SysTable>(query);
      return results.ToList();
   }

   public static IList<SysTable> GetTable(SqlTableReference sqlTableRef)
   {
      var results = sqlTableRef.SqlConnectionProvider.Query<SysTable>(
         SysTable.QueryForTable,
         sqlTableRef.ToAnonymousSqlParamObject());
      return results.ToList();
   }

   public static IList<SysColumn> GetAllColumns(ISqlConnectionProvider connectionProvider)
   {
      var results = connectionProvider.Query<SysColumn>(SysColumn.Query);
      return results.ToList();
   }

   public static IList<SysColumn> GetColumns(SqlTableReference sqlTableRef)
   {
      var connectionProvider = sqlTableRef.SqlConnectionProvider;
      var results = connectionProvider.Query<SysColumn>(
         SysColumn.QueryForTable(),
         sqlTableRef.ToAnonymousSqlParamObject());
      return results.ToList();
   }
}