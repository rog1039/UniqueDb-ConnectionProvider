using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace UniqueDb.ConnectionProvider.DataGeneration
{
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
}