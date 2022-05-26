using UniqueDb.ConnectionProvider.DataGeneration.SqlMetadata;

namespace UniqueDb.ConnectionProvider.DataGeneration;

public static class InformationSchemaTableExtensions
{
    public static SqlTableReference ToSqlTableReference(this SISTable sisTable,
                                                        ISqlConnectionProvider      sqlConnectionProvider)
    {
        var sqlTableReference = new SqlTableReference(sqlConnectionProvider,
                                                      sisTable.TABLE_SCHEMA, 
                                                      sisTable.TABLE_NAME);
        return sqlTableReference;
    }
}