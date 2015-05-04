using System.Linq;

namespace UniqueDb.ConnectionProvider.DataGeneration
{
    public static class SqlTableFactory
    {
        public static SqlTable Create(SqlTableReference sqlTableReference)
        {
            var columns = InformationSchemaMetadataExplorer.GetInformationSchemaColumns(sqlTableReference);
            var sqlColumns = columns.Select(SqlColumnFactory.Create).ToList();
            var sqlTable = new SqlTable()
            {
                Name = sqlTableReference.TableName,
                Schema = sqlTableReference.SchemaName,
                SqlColumns = sqlColumns
            };
            return sqlTable;
        }
    }
}