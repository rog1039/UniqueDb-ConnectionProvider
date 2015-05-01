using System;
using System.Collections.Generic;
using System.Linq;

namespace UniqueDb.ConnectionProvider.Tests.DataGeneration
{
    public static class InformationSchemaMetadataExplorer
    {
        public static IList<InformationSchemaTable> GetInformationSchemaTables(ISqlConnectionProvider sqlConnectionProvider)
        {
            var informationSchemaTables = sqlConnectionProvider
                .Query<InformationSchemaTable>("SELECT * FROM INFORMATION_SCHEMA.TABLES")
                .ToList();
            return informationSchemaTables;
        } 

        public static InformationSchemaTableDefinition GetInformationSchemaTableDefinition(
            SqlTableReference sqlTableReference)
        {
            var definition = new InformationSchemaTableDefinition();
            definition.InformationSchemaTable = GetInformationSchemaTable(sqlTableReference);
            definition.InformationSchemaColumns = GetInformationSchemaColumns(sqlTableReference);
            return definition;
        }

        public static InformationSchemaTable GetInformationSchemaTable(SqlTableReference sqlTableReference)
        {
            var sqlQuery = InformationSchemaMetadataQueryGenerator.GetInformationSchemaTableSqlQuery(sqlTableReference);
            var tables = sqlTableReference.SqlConnectionProvider.Query<InformationSchemaTable>(sqlQuery).ToList();
            
            CheckOnlyOneTableWasReturned(tables);
            return tables.SingleOrDefault();
        }

        private static void CheckOnlyOneTableWasReturned(List<InformationSchemaTable> infSchTable)
        {
            if (infSchTable.Count > 1)
            {
                throw new InvalidOperationException(
                    "More than one matching table found.  Please specify a schema name for the table to prevent this.");
            }
        }

        public static IList<InformationSchemaColumn> GetInformationSchemaColumns(SqlTableReference sqlTableReference)
        {
            var sqlQuery = InformationSchemaMetadataQueryGenerator.GetInformationSchemaColumnsSqlQuery(sqlTableReference);
            var tableColumns = sqlTableReference.SqlConnectionProvider.Query<InformationSchemaColumn>(sqlQuery).ToList();
            return tableColumns;
        }
    }
}