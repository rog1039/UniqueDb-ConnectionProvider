using System.Collections.Generic;
using System.Linq;
using Dapper;

namespace UniqueDb.ConnectionProvider.DataGeneration;

public class RandomTableSelector
{
    public static IList<SqlTableReference> GetRandomSqlTableReferences(
        ISqlConnectionProvider sqlConnectionProvider,
        int                    count)
    {
        var sql = string.Format("SELECT top {0} * FROM INFORMATION_SCHEMA.TABLES order by newid()", count);
        var randomTableNames = sqlConnectionProvider
            .GetSqlConnection()
            .Query(sql);
        var sqlTableReferences = randomTableNames
            .Select(x => new SqlTableReference(sqlConnectionProvider, x.TABLE_SCHEMA + "." + x.TABLE_NAME))
            .ToList();
        return sqlTableReferences;
    } 
}