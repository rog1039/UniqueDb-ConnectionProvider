using System;
using System.Collections.Generic;
using System.Linq;

namespace UniqueDb.ConnectionProvider.Tests.DataGeneration
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
        
        private static string ConvertSqlTypeNameToClrTypeName(SqlColumn column)
        {
            if (column.DataType == "int")
            {
                return "int";
            }
            if (column.DataType == "tinyint")
            {
                return "int";
            }
            if (column.DataType == "varchar")
            {
                return "string";
            }
            if (column.DataType == "datetime")
            {
                return "DateTime";
            }
            if (column.DataType == "date")
            {
                return "DateTime";
            }
            if (column.DataType == "time")
            {
                return "DateTime";
            }
            if (column.DataType == "decimal")
            {
                return "decimal";
            }
            if (column.DataType == "bigint")
            {
                return "Int64";
            }
            if (column.DataType == "timestamp")
            {
                return "byte[]";
            }
            if (column.DataType == "text")
            {
                return "string";
            }
            if (column.DataType == "bit")
            {
                return "bool";
            }
            if (column.DataType == "nvarchar")
            {
                return "string";
            }
            if (column.DataType == "float")
            {
                return "double";
            }
            if (column.DataType == "nchar")
            {
                return "string";
            }
            if (column.DataType == "xml")
            {
                return "string";
            }
            if (column.DataType == "uniqueidentifier")
            {
                return "string";
            }
            if (column.DataType == "money")
            {
                return "decimal";
            }
            if (column.DataType == "smallint")
            {
                return "Int16";
            }
            if (column.DataType == "geography")
            {
                //Could perhaps use SqlGeography and/or DbGeography here rather than byte[].
                //See http://stackoverflow.com/questions/23186832/entity-framework-sqlgeography-vs-dbgeography
                //and http://stackoverflow.com/questions/15107977/sql-geography-to-dbgeography/29200641#29200641
                return "byte[]";
            }
            if (column.DataType == "hierarchyid")
            {
                //Perhaps we could do something else here??
                //See http://blogs.msdn.com/b/jimoneil/archive/2009/02/23/h-is-for-hierarchyid.aspx
                return "byte[]";
            }
            if (column.DataType == "smallmoney")
            {
                return "decimal";
            }
            if (column.DataType == "varbinary")
            {
                return "byte[]";
            }
            if (column.DataType == "smallmoney")
            {
                return "decimal";
            }
            if (column.DataType == "numeric")
            {
                return "decimal";
            }
            
            throw new NotImplementedException(
                string.Format("SQL column type {0} cannot be translated to a C# property type", column.DataType));
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
                if (!Nullable || !typesThatCanBeNullable.Contains(ClrDataType))
                    return ClrDataType;
                return ClrDataType + "?";
            }
        }
    }
}