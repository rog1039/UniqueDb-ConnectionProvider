namespace UniqueDb.ConnectionProvider.DataGeneration
{
    public static class InformationSchemaMetadataSqlQueryGenerator
    {
        private static string _sqlInformationSchemaColumn = "SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = N'{0}' AND TABLE_CATALOG = '{1}'";
        private static string _sqlInformationSchemaTable = "SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'{0}' AND TABLE_CATALOG = '{1}'";

        public static string GetInformationSchemaColumnsSqlQuery(SqlTableReference sqlTableReference)
        {
            var sql = string.Format(_sqlInformationSchemaColumn, sqlTableReference.TableName, sqlTableReference.SqlConnectionProvider.DatabaseName);
            sql = AddSchemaWhereClauseIfNecessary(sql, sqlTableReference);
            return sql;
        }

        private static string AddSchemaWhereClauseIfNecessary(string sql, SqlTableReference sqlTableReference)
        {
            if (!string.IsNullOrWhiteSpace(sqlTableReference.SchemaName))
            {
                sql += string.Format(" AND TABLE_SCHEMA = '{0}'", sqlTableReference.SchemaName);
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