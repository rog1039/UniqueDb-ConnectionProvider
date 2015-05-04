using System.Collections.Generic;

namespace UniqueDb.ConnectionProvider.DataGeneration
{
    public class InformationSchemaTableDefinition
    {
        public InformationSchemaTable InformationSchemaTable { get; set; }
        public IList<InformationSchemaColumn> InformationSchemaColumns { get; set; }
    }

    public static class InformationSchemaTableExtensions
    {
        public static SqlTableReference ToSqlTableReference(this InformationSchemaTable informationSchemaTable,
            ISqlConnectionProvider sqlConnectionProvider)
        {
            var sqlTableReference = new SqlTableReference(sqlConnectionProvider,
                informationSchemaTable.TABLE_SCHEMA, 
                informationSchemaTable.TABLE_NAME);
            return sqlTableReference;
        }
    }
}