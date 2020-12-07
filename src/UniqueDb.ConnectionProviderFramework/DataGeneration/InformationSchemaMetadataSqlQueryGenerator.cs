namespace UniqueDb.ConnectionProvider.DataGeneration
{
    public static class InformationSchemaMetadataSqlQueryGenerator
    {
        private static string _sqlInformationSchemaColumn = "SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = N'{0}' AND TABLE_CATALOG = '{1}'";
        private static string _sqlInformationSchemaTable = "SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'{0}' AND TABLE_CATALOG = '{1}'";

        public static string GetInformationSchemaColumnsSqlQuery(SqlTableReference sqlTableReference)
        {
            var sql = string.Format(_sqlInformationSchemaColumn, sqlTableReference.TableName.Debracketize(), sqlTableReference.SqlConnectionProvider.DatabaseName);
            sql = AddSchemaWhereClauseIfNecessary(sql, sqlTableReference);
            return sql;
        }

        private static string AddSchemaWhereClauseIfNecessary(string sql, SqlTableReference sqlTableReference)
        {
            var hasSchemaName = !string.IsNullOrWhiteSpace(sqlTableReference.SchemaName);
            if (hasSchemaName)
            {
                sql += $" AND TABLE_SCHEMA = '{sqlTableReference.SchemaName}'";
            }
            return sql;
        }

        public static string GetInformationSchemaTableSqlQuery(SqlTableReference sqlTableReference)
        {
            var sql = string.Format(_sqlInformationSchemaTable, sqlTableReference.TableName,
                sqlTableReference.SqlConnectionProvider.DatabaseName);
            sql = AddSchemaWhereClauseIfNecessary(sql, sqlTableReference);
            return sql;
        }

    }
}