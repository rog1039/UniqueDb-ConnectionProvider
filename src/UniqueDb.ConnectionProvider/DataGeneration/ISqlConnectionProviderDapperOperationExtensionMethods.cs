using Dapper;

namespace UniqueDb.ConnectionProvider.DataGeneration;

public static class ISqlConnectionProviderDapperOperationExtensionMethods
{
    public static void Execute(this ISqlConnectionProvider sqlConnectionProvider, string script)
    {
        sqlConnectionProvider.GetSqlConnection().Execute(script);
    }

    public static Task<T> QuerySingleAsync<T>(this ISqlConnectionProvider sqlConnectionProvider, string script, object param = null)
    {
        return sqlConnectionProvider.GetSqlConnection().QuerySingleAsync<T>(script, param);
    }

    public static Task<dynamic> QuerySingleAsync(this ISqlConnectionProvider sqlConnectionProvider, string script, object param = null)
    {
        return sqlConnectionProvider.GetSqlConnection().QuerySingleAsync(script, param);
    }

    public static Task<IEnumerable<dynamic>> QueryAsync(this ISqlConnectionProvider sqlConnectionProvider, string script, object param = null)
    {
        return sqlConnectionProvider.GetSqlConnection().QueryAsync(script, param);
    }

    public static IEnumerable<T> Query<T>(this ISqlConnectionProvider sqlConnectionProvider, string script, object param = null)
    {
        return sqlConnectionProvider.GetSqlConnection().Query<T>(script, param);
    }

    public static async Task<IEnumerable<T>> QueryAsync<T>(this ISqlConnectionProvider sqlConnectionProvider, string script, object param = null)
    {
        var queryIenumerable =await  sqlConnectionProvider.GetSqlConnection().QueryAsync<T>(script, param);
        return queryIenumerable.ToList();

    }

    public static void BulkInsert<T>(this ISqlConnectionProvider sqlConnectionProvider, IList<T> list,
                                     string                      tableName,             string   schemaName = "dbo")
    {
        SqlConnectionProviderBulkCopyInsert.BulkInsert(sqlConnectionProvider, list, tableName, schemaName);
    }
}