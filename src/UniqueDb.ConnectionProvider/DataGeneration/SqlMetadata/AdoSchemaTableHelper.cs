using System.Data;
using Microsoft.Data.SqlClient;

namespace UniqueDb.ConnectionProvider.DataGeneration.SqlMetadata;

public static class AdoSchemaTableHelper
{
    public static IList<DataColumn> GetAdoSchemaDataColumns(ISqlConnectionProvider sqlConnectionProvider, string sqlQuery)
    {
        var sqlDataReader = GetDataReaderFromQuery(sqlConnectionProvider, sqlQuery);
        var columns       = ExtractColumnsFromDataReader(sqlDataReader);
        return columns;
    }

    private static SqlDataReader GetDataReaderFromQuery(ISqlConnectionProvider sqlConnectionProvider, string sqlQuery)
    {
        var connection = sqlConnectionProvider.ToSqlConnection();
        connection.Open();
        var query = new SqlCommand(sqlQuery, connection);
        return query.ExecuteReader();
    }

    public static List<DataColumn> ExtractColumnsFromDataReader(SqlDataReader sqlDataReader)
    {
        var columns = sqlDataReader
            .GetSchemaTable()
            .Columns
            .Cast<DataColumn>()
            .ToList();
        return columns;
    }
}