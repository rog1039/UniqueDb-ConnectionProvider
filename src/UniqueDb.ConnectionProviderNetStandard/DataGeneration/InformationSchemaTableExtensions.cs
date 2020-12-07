using UniqueDb.ConnectionProvider.DataGeneration.SqlMetadata;

namespace UniqueDb.ConnectionProvider.DataGeneration
{
    public static class InformationSchemaTableExtensions
    {
        public static SqlTableReference ToSqlTableReference(this InformationSchemaTable informationSchemaTable,
            ISqlConnectionProvider sqlConnectionProvider)
        {
            var sqlTableReference = new SqlTableReference(sqlConnectionProvider,
                informationSchemaTable.TABLE_SCHEMA, 
                informationSchemaTable.TABLE_NAME);
            return sqlTableReference;
        }
    }
}