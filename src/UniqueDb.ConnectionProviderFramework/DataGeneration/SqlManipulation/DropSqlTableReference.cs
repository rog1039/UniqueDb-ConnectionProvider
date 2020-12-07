namespace UniqueDb.ConnectionProvider.DataGeneration.SqlManipulation
{
    public static class DropSqlTableReference
    {
        public static string GenerateDropTableScript(SqlTableReference tableReference)
        {
            return GenerateDropTableScript(tableReference.SchemaName, tableReference.TableName);
        }

        public static string GenerateDropTableScript(string schema, string table)
        {
            var sqlCommand = $"DROP TABLE {schema}.{table}";
            return sqlCommand;
        }
    }
}