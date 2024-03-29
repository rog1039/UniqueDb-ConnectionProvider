using UniqueDb.ConnectionProvider.CoreTypes;
using UniqueDb.ConnectionProvider.SqlMetadata.InformationSchema;

namespace UniqueDb.ConnectionProvider.Converters;

public static class InformationSchemaTableConverter
{
   public static SqlTableReference ToSqlTableReference(this SISTable          sisTable,
                                                       ISqlConnectionProvider sqlConnectionProvider)
   {
      var sqlTableReference = new SqlTableReference(sqlConnectionProvider,
                                                    sisTable.TABLE_SCHEMA,
                                                    sisTable.TABLE_NAME);
      return sqlTableReference;
   }
}