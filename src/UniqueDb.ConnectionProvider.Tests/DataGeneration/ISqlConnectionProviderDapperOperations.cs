using System.Collections.Generic;
using Dapper;

namespace UniqueDb.ConnectionProvider.Tests.DataGeneration
{
    public static class ISqlConnectionProviderDapperOperations
    {
        public static void ExecuteScript(this ISqlConnectionProvider sqlConnectionProvider, string script)
        {
            sqlConnectionProvider.GetSqlConnection().Execute(script);
        }

        public static IEnumerable<T> Query<T>(this ISqlConnectionProvider sqlConnectionProvider, string script)
        {
            return sqlConnectionProvider.GetSqlConnection().Query<T>(script);
        }
    }
}