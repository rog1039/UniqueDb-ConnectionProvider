using System;
using System.Collections.Generic;
using System.Linq;
using UniqueDb.ConnectionProvider.DataGeneration.SqlMetadata;

namespace UniqueDb.ConnectionProvider.DataGeneration
{
    public static class InformationSchemaMetadataExplorer
    {
        public static (List<InformationSchemaTable> schemaTables, List<InformationSchemaColumn> schemaColumns)
            GetAllTableAndColumnInfoForDatabase(ISqlConnectionProvider sqlConnectionProvider)
        {
            var informationSchemaTables = sqlConnectionProvider
                .Query<InformationSchemaTable>("SELECT * FROM INFORMATION_SCHEMA.TABLES ORDER BY TABLE_SCHEMA, TABLE_NAME")
                .ToList();
            var informationSchemaColumns = sqlConnectionProvider
                .Query<InformationSchemaColumn>("SELECT * FROM INFORMATION_SCHEMA.COLUMNS ORDER BY TABLE_NAME, ORDINAL_POSITION")
                .ToList();
            return (informationSchemaTables, informationSchemaColumns);
        } 

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
            definition.InformationSchemaTable   = GetInformationSchemaTable(sqlTableReference);
            definition.InformationSchemaColumns = GetInformationSchemaColumns(sqlTableReference);
            definition.TableConstraints         = GetTableConstraints(sqlTableReference);
            return definition;
        }

        public static InformationSchemaTable GetInformationSchemaTable(SqlTableReference sqlTableReference)
        {
            var sqlQuery = InformationSchemaMetadataSqlQueryGenerator.GetInformationSchemaTableSqlQuery(sqlTableReference);
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
            var sqlQuery = InformationSchemaMetadataSqlQueryGenerator.GetInformationSchemaColumnsSqlQuery(sqlTableReference);
            var tableColumns = sqlTableReference.SqlConnectionProvider.Query<InformationSchemaColumn>(sqlQuery).ToList();
            return tableColumns;
        }
        
        private static List<TableConstraintInfoDto> GetTableConstraints(SqlTableReference sqlTableReference)
        {
            var whereclause =
                $"WHERE tableConstraint.TABLE_SCHEMA = @schemaName AND tableConstraint.TABLE_NAME = @tableName ";
            var query       = TableConstraintInfoDto.SqlQuery.Replace("--WHERE", whereclause);
            var queryParams = new {schemaName = sqlTableReference.SchemaName, tableName = sqlTableReference.TableName};
            var result      = sqlTableReference.SqlConnectionProvider.Query<TableConstraintInfoDto>(query, queryParams).ToList();
            return result;    
        }

    }
}