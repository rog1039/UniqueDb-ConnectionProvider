namespace UniqueDb.ConnectionProvider.DataGeneration
{
    public static class SqlDmlDropTableGeneratorFromInformationSchema
    {
        public static string GenerateDropTableScript(SqlTableReference tableReference)
        {
            var sqlCommand = string.Format("DROP TABLE {0}.{1}", tableReference.SchemaName, tableReference.TableName);
            return sqlCommand;
        }
    }
}