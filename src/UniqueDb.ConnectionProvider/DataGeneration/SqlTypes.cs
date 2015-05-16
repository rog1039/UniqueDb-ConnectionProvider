using System.Collections.Generic;

namespace UniqueDb.ConnectionProvider.DataGeneration
{
    public static class SqlTypes
    {
        public static IList<string> SqlCharTypes = new List<string> {"char", "nchar", "varchar", "nvarchar"};

        public static bool IsCharType(string sqlTypeName)
        {
            return SqlCharTypes.Contains(sqlTypeName);
        }
    }
}