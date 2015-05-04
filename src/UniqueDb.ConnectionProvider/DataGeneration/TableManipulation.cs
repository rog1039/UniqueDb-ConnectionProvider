namespace UniqueDb.ConnectionProvider.DataGeneration
{
    public static class TableManipulation
    {
        public static void CopyTable(SqlTableReference sourceTable, SqlTableReference targetTable)
        {
            var infschTable = InformationSchemaMetadataExplorer.GetInformationSchemaTableDefinition(sourceTable);
            var createTableScript = SqlDmlCreateTableGeneratorFromInformationSchema.GenerateCreateTableScript(infschTable);
            targetTable.SqlConnectionProvider.Execute(createTableScript);
        }

        public static void DropTable(SqlTableReference table)
        {
            var dropTableScript = SqlDmlDropTableGeneratorFromInformationSchema.GenerateDropTableScript(table);
            table.SqlConnectionProvider.Execute(dropTableScript);
        }
    }
}