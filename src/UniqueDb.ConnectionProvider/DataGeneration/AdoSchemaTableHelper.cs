using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace UniqueDb.ConnectionProvider.DataGeneration
{
    public static class AdoSchemaTableHelper
    {
        public static IList<DataColumn> GetAdoSchemaTableColumns(ISqlConnectionProvider sqlConnectionProvider, string sqlQuery)
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
            var columns = sqlDataReader
                .GetSchemaTable()
                .Columns
                .Cast<DataColumn>()
                .ToList();
            return columns;
        }
    }
}