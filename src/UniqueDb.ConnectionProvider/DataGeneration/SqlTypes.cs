using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.SqlServer.Types;

namespace UniqueDb.ConnectionProvider.DataGeneration
{
    public static class SqlTypes
    {
        public static IList<string> SqlCharTypes = new List<string> {"char", "nchar", "varchar", "nvarchar"};
        
        public static IList<string> SqlNumericTypes = new List<string>
        {
            "bigint", "bit", "decimal", "int", "money", "numeric", "smallint", "smallmoney", "tinyint",
            "float", "real",
        };

        public static IList<string> SqlExactNumericTypes = new List<string>
        {
            "bigint", "bit", "decimal", "int", "money", "numeric", "smallint", "smallmoney", "tinyint"
        };

        public static IList<string> SqlApproximateNumericTypes = new List<string>
        {
            "float", "real",
        };

        public static IList<string> SqlSystemTypes = new List<string>
        {
            "bigint", "bit", "decimal", "int", "money", "numeric", "smallint", "smallmoney", "tinyint",
            "float", "real",
            "date", "datetime2", "datetime", "datetimeoffset", "smalldatetime", "time",
            "char", "nchar", "varchar", "nvarchar", "text",
            "binary", "image", "varbinary",
            "cursor", "hierarchyid", "sql_variant", "table", "timestamp", "uniqueidentifier", "xml",
            "geometry", "geography", "sysname"
        };

        public static IList<string> SqlSpecialTypes = new List<string>
        {
            "hierarchyid", "geometry", "geography", "sysname"
        };

        public static IList<string> SqlDateTimeTypes = new List<string>
        {
            "date", "datetime", "datetime2", "datetimeoffset", "smalldatetime", "time"
        };

        public static IList<string> SqlDateTimeTypesWithPrecision = new List<string>
        {
            "datetime2", "datetimeoffset", "time"
        };


        public static IList<Type> ClrTypesThatAreHaveSqlSystemTypes = new List<Type>
        {
            typeof(bool),
            typeof(Int16), typeof(Int32), typeof(Int64), typeof(int), typeof(long), typeof(uint), typeof(ushort), typeof(ulong),
            typeof(decimal), typeof(double),
            typeof(byte), typeof(byte[]),
            typeof(DateTime), typeof(TimeSpan),
            typeof(string), typeof(Guid),
            typeof(SqlHierarchyId),
            typeof(XElement),

        };

        public static bool IsCharType(string sqlTypeName)
        {
            return SqlCharTypes.Contains(sqlTypeName.ToLower());
        }

        public static bool IsSystemType(string sqlTypeName)
        {
            return SqlSystemTypes.Contains(sqlTypeName.ToLower());
        }

        public static bool IsNumeric(string sqlTypeName)
        {
            return SqlNumericTypes.Contains(sqlTypeName.ToLower());
        }

        public static bool IsSpecialSystemType(string sqlTypeName)
        {
            return SqlSpecialTypes.Contains(sqlTypeName.ToLower());
        }

        public static bool IsExactNumeric(string sqlTypeName)
        {
            return SqlExactNumericTypes.Contains(sqlTypeName.ToLower());
        }

        public static bool IsApproximateNumeric(string sqlTypeName)
        {
            return SqlApproximateNumericTypes.Contains(sqlTypeName.ToLower());
        }

        public static bool IsClrTypeASqlSystemType(Type propertyType)
        {
            return ClrTypesThatAreHaveSqlSystemTypes.Contains(propertyType);
        }

        public static bool IsDateTimeWithPrecision(string sqlTypeName)
        {
            return SqlDateTimeTypesWithPrecision.Contains(sqlTypeName.ToLower());
        }

        public static bool IsDateTime(string sqlTypeName)
        {
            return SqlDateTimeTypes.Contains(sqlTypeName.ToLower());
        }
    }
}