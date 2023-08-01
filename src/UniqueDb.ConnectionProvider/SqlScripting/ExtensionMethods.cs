using UniqueDb.ConnectionProvider.CoreTypes;
using UniqueDb.ConnectionProvider.Infrastructure.Extensions;

namespace UniqueDb.ConnectionProvider.SqlScripting;

public static class ExtensionMethods
{
   public static void EnsureTableExists<T>(this ISqlConnectionProvider sqlConnectionProvider, string schema)
   {
      EnsureTableExists<T>(sqlConnectionProvider, schema, typeof(T).Name);
   }

   public static void EnsureTableExists<T>(this SqlTableReference tableReference)
   {
      var tableExists = CheckTableExistence(tableReference.SqlConnectionProvider, tableReference.SchemaName,
                                            tableReference.TableName);
      if (!tableExists)
         CreateTable<T>(tableReference.SqlConnectionProvider, tableReference.SchemaName, tableReference.TableName);
   }

   public static void EnsureTableExists<T>(this ISqlConnectionProvider sqlConnectionProvider, string schema,
                                           string                      table)
   {
      var tableExists = CheckTableExistence(sqlConnectionProvider, schema, table);
      if (!tableExists)
         CreateTable<T>(sqlConnectionProvider, schema, table);
   }

   public static bool CheckTableExistence(this ISqlConnectionProvider sqlConnectionProvider, string schema,
                                          string                      table)
   {
      var objectId = sqlConnectionProvider.ExecuteScalar<int?>($"SELECT OBJECT_ID('{schema}.{table}', 'U')");
      return objectId.HasValue;
   }

   public static void CreateTable<T>(this ISqlConnectionProvider sqlConnectionProvider, string schema, string table)
   {
      var createTableScript = ClrTypeToSqlDmlCreateStatementGenerator.GetCreateTableScript<T>(schema, table);
      Console.WriteLine(createTableScript);
      sqlConnectionProvider.Execute(createTableScript);
   }

   public static void DropTable(this SqlTableReference tableReference)
   {
      DropTable(tableReference.SqlConnectionProvider, tableReference.SchemaName, tableReference.TableName);
   }

   public static void DropTable(this ISqlConnectionProvider sqlConnectionProvider, string schema, string table)
   {
      var dropScript = DropSqlTableReference.GenerateDropTableScript(schema, table);
      sqlConnectionProvider.Execute(dropScript);
   }

   public static void TruncateTable<T>(this ISqlConnectionProvider sqlConnectionProvider, string schema)
   {
      EnsureTableExists<T>(sqlConnectionProvider, schema, typeof(T).Name);
   }

   public static void TruncateTable(this SqlTableReference tableReference)
   {
      TruncateTable(tableReference.SqlConnectionProvider, tableReference.SchemaName, tableReference.TableName);
   }

   public static void TruncateTable(this ISqlConnectionProvider   sqlConnectionProvider,
                                    DbTableNameWithNullableSchema dbTableNameWithNullableSchema)
   {
      var truncateScript = $"TRUNCATE TABLE {dbTableNameWithNullableSchema.ToStringBrackets()}";
      sqlConnectionProvider.Execute(truncateScript);
   }

   public static void TruncateTable(this ISqlConnectionProvider sqlConnectionProvider, string schema, string table)
   {
      var truncateScript = $"TRUNCATE TABLE {schema}.{table}";
      sqlConnectionProvider.Execute(truncateScript);
   }
}