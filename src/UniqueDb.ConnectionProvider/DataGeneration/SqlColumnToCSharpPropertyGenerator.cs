using System;
using System.Collections.Generic;
using System.Linq;

namespace UniqueDb.ConnectionProvider.DataGeneration
{
    public static class SqlColumnToCSharpPropertyGenerator
    {
        public static CSharpProperty ToCSharpProperty(SqlColumn tableColumnDto)
        {
            var cSharpProperty = new CSharpProperty();

            var propertyName = GetNameWithRewriting(tableColumnDto.Name);
            cSharpProperty.Name = propertyName;
            cSharpProperty.ClrAccessModifier = ClrAccessModifier.Public;
            cSharpProperty.DataType = GetClrDataType(tableColumnDto);
            return cSharpProperty;
        }

        private static string GetNameWithRewriting(string name)
        {
            var rewriters = AutomaticPropertyNameRewrites.Rewriters.Where(x => x.ShouldRewrite(name)).FirstOrDefault();
            if (rewriters != null)
            {
                return rewriters.Rewrite(name);
            }
            return name;
        }

        private static string GetClrDataType(SqlColumn column)
        {
            string dataTypeName = ConvertSqlTypeNameToClrTypeName(column);
            var clrDataTypeConversionResult = new SqlToClrDataTypeConversionResult(column.IsNullable, dataTypeName);
            return clrDataTypeConversionResult.ToString();
        }

        public static bool UseNativeSqlTypes = true;

        public static string ConvertSqlTypeNameToClrTypeName(SqlColumn column)
        {
            if (column.SqlDataType == "int")
            {
                return "int";
            }
            if (column.SqlDataType == "tinyint")
            {
                return "int";
            }
            if (column.SqlDataType == "varchar")
            {
                return "string";
            }
            if (column.SqlDataType == "datetime")
            {
                return "DateTime";
            }
            if (column.SqlDataType == "smalldatetime")
            {
                return "DateTime";
            }
            if (column.SqlDataType == "date")
            {
                return "DateTime";
            }
            if (column.SqlDataType == "time")
            {
                return "TimeSpan";
            }
            if (column.SqlDataType == "decimal")
            {
                return "decimal";
            }
            if (column.SqlDataType == "bigint")
            {
                return "Int64";
            }
            if (column.SqlDataType == "timestamp")
            {
                return "byte[]";
            }
            if (column.SqlDataType == "sql_variant")
            {
                return "object";
            }
            if (column.SqlDataType == "text")
            {
                return "string";
            }
            if (column.SqlDataType == "bit")
            {
                return "bool";
            }
            if (column.SqlDataType == "nvarchar")
            {
                return "string";
            }
            if (column.SqlDataType == "ntext")
            {
                return "string";
            }
            if (column.SqlDataType == "float")
            {
                return "double";
            }
            if (column.SqlDataType == "real")
            {
                return "double";
            }
            if (column.SqlDataType == "nchar")
            {
                return "string";
            }
            if (column.SqlDataType == "xml")
            {
                return "string";
            }
            if (column.SqlDataType == "image")
            {
                return "byte[]";
            }
            if (column.SqlDataType == "uniqueidentifier")
            {
                return "Guid";
            }
            if (column.SqlDataType == "money")
            {
                return "decimal";
            }
            if (column.SqlDataType == "smallint")
            {
                return "Int16";
            }
            if (column.SqlDataType == "geography")
            {
                //Could perhaps use SqlGeography and/or DbGeography here rather than byte[].
                //See http://stackoverflow.com/questions/23186832/entity-framework-sqlgeography-vs-dbgeography
                //and http://stackoverflow.com/questions/15107977/sql-geography-to-dbgeography/29200641#29200641
                return UseNativeSqlTypes
                    ? "SqlGeography"
                    : "byte[]";
            }
            if (column.SqlDataType == "geometry")
            {
                return UseNativeSqlTypes
                    ? "SqlGeometry"
                    : "byte[]";
            }
            if (column.SqlDataType == "hierarchyid")
            {
                //Perhaps we could do something else here??
                //See http://blogs.msdn.com/b/jimoneil/archive/2009/02/23/h-is-for-hierarchyid.aspx
                return UseNativeSqlTypes
                    ? "SqlHierarchyId"
                    : "byte[]";
            }
            if (column.SqlDataType == "smallmoney")
            {
                return "decimal";
            }
            if (column.SqlDataType == "varbinary")
            {
                return "byte[]";
            }
            if (column.SqlDataType == "smallmoney")
            {
                return "decimal";
            }
            if (column.SqlDataType == "numeric")
            {
                return "decimal";
            }
            if (column.SqlDataType == "binary")
            {
                return "byte[]";
            }
            if (column.SqlDataType == "char")
            {
                return "string";
            }
            if (column.SqlDataType == "datetime2")
            {
                return "DateTime";
            }
            if (column.SqlDataType == "datetimeoffset")
            {
                return "DateTimeOffset";
            }
            if (column.SqlDataType == "sysname")
            {
                return "object";
            }
            
            throw new NotImplementedException(
                string.Format("SQL column type {0} cannot be translated to a C# property type", column.SqlDataType));
        }

        private class SqlToClrDataTypeConversionResult
        {
            public static List<string> typesThatCanBeNullable = new List<string>() { "int", "Int", "Int16", "Int32", "Int64", "DateTime", "bool", "double", "decimal" }; 

            public bool Nullable { get; set; }
            public string ClrDataType { get; set; }

            public SqlToClrDataTypeConversionResult(bool nullable, string clrDataType)
            {
                Nullable = nullable;
                ClrDataType = clrDataType;
            }

            public override string ToString()
            {
                return !Nullable || !typesThatCanBeNullable.Contains(ClrDataType)
                    ? ClrDataType
                    : ClrDataType + "?";
            }
        }
    }
}