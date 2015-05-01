namespace UniqueDb.ConnectionProvider.Tests.DataGeneration
{
    public static class TableManipulation
    {
        public static void CreateTable(SqlTableReference sourceTable, SqlTableReference targetTable)
        {
            var infschTable = InformationSchemaMetadataExplorer.GetInformationSchemaTableDefinition(sourceTable);
            var createTableScript = SqlDmlGeneratorFromInformationSchema.GenerateCreateTableScript(infschTable);
            targetTable.SqlConnectionProvider.ExecuteScript(createTableScript);
        }

        public static void DropTable(SqlTableReference sourceTable)
        {
            var informationSchemaTableDefinition = InformationSchemaMetadataExplorer.GetInformationSchemaTableDefinition(sourceTable);
            var dropTableScript = SqlDmlGeneratorFromInformationSchema.GenerateCreateTableScript(informationSchemaTableDefinition);
            sourceTable.SqlConnectionProvider.ExecuteScript(dropTableScript);
        }
    }
}