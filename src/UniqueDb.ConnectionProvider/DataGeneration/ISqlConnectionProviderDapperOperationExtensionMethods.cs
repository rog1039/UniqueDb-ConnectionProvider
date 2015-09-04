using System.Collections.Generic;
using Dapper;

namespace UniqueDb.ConnectionProvider.DataGeneration
{
    public static class ISqlConnectionProviderDapperOperationExtensionMethods
    {
        public static void Execute(this ISqlConnectionProvider sqlConnectionProvider, string script)
        {
            sqlConnectionProvider.GetSqlConnection().Execute(script);
        }

        public static IEnumerable<T> Query<T>(this ISqlConnectionProvider sqlConnectionProvider, string script)
        {
            return sqlConnectionProvider.GetSqlConnection().Query<T>(script);
        }

        public static void BulkInsert<T>(this ISqlConnectionProvider sqlConnectionProvider, IList<T> list,
                                         string tableName, string schemaName = "dbo")
        {
            SqlConnectionProviderBulkCopyInsert.BulkInsert(sqlConnectionProvider, list, tableName, schemaName);
        }
    }
}