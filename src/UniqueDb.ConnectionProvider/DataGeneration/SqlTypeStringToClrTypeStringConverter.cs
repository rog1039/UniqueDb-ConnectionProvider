using System;

namespace UniqueDb.ConnectionProvider.DataGeneration
{
    public static class SqlTypeStringToClrTypeStringConverter
    {
        public static bool UseNativeSqlTypes = true;

        public static string GetClrDataType(string sqlDataType, bool isNullable)
        {
            string dataTypeName = ConvertSqlTypeNameToClrTypeName(sqlDataType);
            var clrDataTypeConversionResult = new SqlToClrDataTypeConversionResult(isNullable, dataTypeName);
            return clrDataTypeConversionResult.ToString();
        }

        public static string ConvertSqlTypeNameToClrTypeName(string sqlDataType)
        {
            if (sqlDataType == "int")
            {
                return "int";
            }
            if (sqlDataType == "tinyint")
            {
                return "int";
            }
            if (sqlDataType == "varchar")
            {
                return "string";
            }
            if (sqlDataType == "datetime")
            {
                return "DateTime";
            }
            if (sqlDataType == "smalldatetime")
            {
                return "DateTime";
            }
            if (sqlDataType == "date")
            {
                return "DateTime";
            }
            if (sqlDataType == "time")
            {
                return "TimeSpan";
            }
            if (sqlDataType == "decimal")
            {
                return "decimal";
            }
            if (sqlDataType == "bigint")
            {
                return "Int64";
            }
            if (sqlDataType == "timestamp")
            {
                return "byte[]";
            }
            if (sqlDataType == "sql_variant")
            {
                return "object";
            }
            if (sqlDataType == "text")
            {
                return "string";
            }
            if (sqlDataType == "bit")
            {
                return "bool";
            }
            if (sqlDataType == "nvarchar")
            {
                return "string";
            }
            if (sqlDataType == "ntext")
            {
                return "string";
            }
            if (sqlDataType == "float")
            {
                return "double";
            }
            if (sqlDataType == "real")
            {
                return "double";
            }
            if (sqlDataType == "nchar")
            {
                return "string";
            }
            if (sqlDataType == "xml")
            {
                return "string";
            }
            if (sqlDataType == "image")
            {
                return "byte[]";
            }
            if (sqlDataType == "uniqueidentifier")
            {
                return "Guid";
            }
            if (sqlDataType == "money")
            {
                return "decimal";
            }
            if (sqlDataType == "smallint")
            {
                return "Int16";
            }
            if (sqlDataType == "geography")
            {
                //Could perhaps use SqlGeography and/or DbGeography here rather than byte[].
                //See http://stackoverflow.com/questions/23186832/entity-framework-sqlgeography-vs-dbgeography
                //and http://stackoverflow.com/questions/15107977/sql-geography-to-dbgeography/29200641#29200641
                return UseNativeSqlTypes
                    ? "SqlGeography"
                    : "byte[]";
            }
            if (sqlDataType == "geometry")
            {
                return UseNativeSqlTypes
                    ? "SqlGeometry"
                    : "byte[]";
            }
            if (sqlDataType == "hierarchyid")
            {
                //Perhaps we could do something else here??
                //See http://blogs.msdn.com/b/jimoneil/archive/2009/02/23/h-is-for-hierarchyid.aspx
                return UseNativeSqlTypes
                    ? "SqlHierarchyId"
                    : "byte[]";
            }
            if (sqlDataType == "smallmoney")
            {
                return "decimal";
            }
            if (sqlDataType == "varbinary")
            {
                return "byte[]";
            }
            if (sqlDataType == "smallmoney")
            {
                return "decimal";
            }
            if (sqlDataType == "numeric")
            {
                return "decimal";
            }
            if (sqlDataType == "binary")
            {
                return "byte[]";
            }
            if (sqlDataType == "char")
            {
                return "string";
            }
            if (sqlDataType == "datetime2")
            {
                return "DateTime";
            }
            if (sqlDataType == "datetimeoffset")
            {
                return "DateTimeOffset";
            }
            if (sqlDataType == "sysname")
            {
                return "object";
            }
            
            throw new NotImplementedException(
                string.Format("SQL column type {0} cannot be translated to a C# property type", sqlDataType));
        }

        private class SqlToClrDataTypeConversionResult
        {
            public bool Nullable { get; set; }
            public string ClrDataType { get; set; }

            public SqlToClrDataTypeConversionResult(bool nullable, string clrDataType)
            {
                Nullable = nullable;
                ClrDataType = clrDataType;
            }

            public override string ToString()
            {
                return !Nullable || !CSharpPropertyTextGenerator.typesThatCanBeNullable.Contains(ClrDataType)
                    ? ClrDataType
                    : ClrDataType + "?";
            }
        }
    }
}