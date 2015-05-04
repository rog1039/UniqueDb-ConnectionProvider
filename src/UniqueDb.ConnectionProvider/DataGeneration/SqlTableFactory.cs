using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Configuration;

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

    public static class DataColumnGenerator
    {
        public static IList<DataColumn> RetrieveDataColumnsFromQuery(ISqlConnectionProvider sqlConnectionProvider, string sqlQuery)
        {
            var sqlDataReader = GetDataReaderFromQuery(sqlConnectionProvider, sqlQuery);
            var columns = ExtractColumnsFromDataReader(sqlDataReader);
            return columns;
        }

        private static SqlDataReader GetDataReaderFromQuery(ISqlConnectionProvider sqlConnectionProvider, string sqlQuery)
        {
            var connection = sqlConnectionProvider.GetSqlConnection();
            connection.Open();
            var query = new SqlCommand(sqlQuery, connection);
            return query.ExecuteReader();
        }

        private static List<DataColumn> ExtractColumnsFromDataReader(SqlDataReader sqlDataReader)
        {
            var columns = new List<DataColumn>();
            var schemaTable = sqlDataReader.GetSchemaTable();
            foreach (var row in schemaTable.Rows)
            {
                var column = new ColumnInfo();
            }
            

            
            foreach (var column in schemaTable.Columns)
            {
                columns.Add((DataColumn) column);
            }
            return columns;
        }
    }

    internal class ColumnInfo
    {
        public string ColumnName { get; set; }
        public string ColumnOrdinal { get; set; }

    }
}