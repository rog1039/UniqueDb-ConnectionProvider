using UniqueDb.ConnectionProvider.SqlMetadata.DescribeResultSet;

namespace UniqueDb.ConnectionProvider.CSharpGeneration;

public static class CSharpPropertyFactoryFromDescribeResultSetRow
{
    public static CSharpProperty ToCSharpProperty(DescribeResultSetContainer resultSetContainer)
    {
        var sqlColumn      = DescribeResultSetRowToSqlColumnConverter.Convert(resultSetContainer);
        var cSharpProperty = CSharpPropertyFactoryFromSqlColumn.ToCSharpProperty(sqlColumn);
        return cSharpProperty;
    }
}