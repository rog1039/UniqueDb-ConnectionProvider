using System.Collections.Generic;

namespace UniqueDb.ConnectionProvider.DataGeneration
{
    public static class SqlTypes
    {
        public static IList<string> SqlCharTypes = new List<string> {"char", "nchar", "varchar", "nvarchar"};
        public static IList<string> SqlSystemTypes = new List<string>
        {
            "bigint", "bit", "decimal", "int", "money", "numeric", "smallint", "smallmoney", "tinyint",
            "float", "real",
            "date", "datetime2", "datetime", "datetimeoffset", "smalldatetime", "time",
            "char", "nchar", "varchar", "nvarchar", "text",
            "binary", "image", "varbinary",
            "cursor", "hierarchyid", "sql_variant", "table", "timestamp", "uniqueidentifier", "xml"
            //Not sure if we should include spatial types here or not...
        };

        public static bool IsCharType(string sqlTypeName)
        {
            return SqlCharTypes.Contains(sqlTypeName.ToLower());
        }

        public static bool IsSystemType(string sqlTypeName)
        {
            return SqlSystemTypes.Contains(sqlTypeName.ToLower());
        }
    }
}