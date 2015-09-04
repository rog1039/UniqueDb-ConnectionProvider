using UniqueDb.ConnectionProvider.DataGeneration.SqlManipulation;

namespace UniqueDb.ConnectionProvider.DataGeneration
{
    public static class TableManipulation
    {
        public static void CopyTable(SqlTableReference sourceTable, SqlTableReference targetTable)
        {
            var infschTable = InformationSchemaMetadataExplorer.GetInformationSchemaTableDefinition(sourceTable);
            var createTableScript = SqlDmlCreateTableFromInformationSchemaGenerator.GenerateCreateTableScript(infschTable);
            targetTable.SqlConnectionProvider.Execute(createTableScript);
        }

        public static void DropTable(SqlTableReference table)
        {
            var dropTableScript = DropSqlTableReference.GenerateDropTableScript(table);
            table.SqlConnectionProvider.Execute(dropTableScript);
        }
    }
}