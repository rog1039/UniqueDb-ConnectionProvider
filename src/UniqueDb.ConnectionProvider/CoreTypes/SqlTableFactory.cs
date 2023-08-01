using UniqueDb.ConnectionProvider.CSharpGeneration;
using UniqueDb.ConnectionProvider.SqlMetadata;

namespace UniqueDb.ConnectionProvider.CoreTypes;

public static class SqlTableFactory
{
    public static SqlTable Create(SqlTableReference sqlTableReference)
    {
        var columns    = InformationSchemaExplorer.GetSisColumns(sqlTableReference);
        var sqlColumns = columns.Select(CSharpClassGeneratorFromInformationSchema.InformationSchemaColumnToSqlColumn).ToList();
        var sqlTable = new SqlTable()
        {
            Name       = sqlTableReference.TableName,
            Schema     = sqlTableReference.SchemaName,
            SqlColumns = sqlColumns
        };
        return sqlTable;
    }
}