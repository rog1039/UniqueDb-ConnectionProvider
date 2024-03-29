using UniqueDb.ConnectionProvider.CoreTypes;
using UniqueDb.ConnectionProvider.Infrastructure.Extensions;
using UniqueDb.ConnectionProvider.SqlMetadata;

namespace UniqueDb.ConnectionProvider.SqlScripting;

public static class TableManipulation
{
    public static void CopyTableStructure(SqlTableReference sourceTable, SqlTableReference targetTable)
    {
        var sisTable       = InformationSchemaExplorer.GetSisTableDefinition(sourceTable);
        var createTableScript = SISToSqlDmlCreateStatementGenerator.GenerateCreateTableScript(sisTable);
        targetTable.SqlConnectionProvider.Execute(createTableScript);
    }

    public static void DropTable(SqlTableReference table)
    {
        var dropTableScript = DropSqlTableReference.GenerateDropTableScript(table);
        table.SqlConnectionProvider.Execute(dropTableScript);
    }
}