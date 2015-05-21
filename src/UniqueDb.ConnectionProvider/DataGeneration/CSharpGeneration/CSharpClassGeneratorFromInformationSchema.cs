using System;
using System.Linq;
using UniqueDb.ConnectionProvider.DataGeneration.SqlMetadata;

namespace UniqueDb.ConnectionProvider.DataGeneration
{
    public static class CSharpClassGeneratorFromInformationSchema
    {
        public static string CreateCSharpClass(SqlTableReference sqlTableReference, string className = default(string))
        {
            var schemaColumns = InformationSchemaMetadataExplorer.GetInformationSchemaColumns(sqlTableReference);
            var cSharpProperties = schemaColumns
                .Select(InformationSchemaColumnToSqlColumnConverter.FromInformationSchemaColumn)
                .Select(CSharpPropertyFactoryFromSqlColumn.ToCSharpProperty)
                .ToList();
            var tableName = className ?? sqlTableReference.TableName;
            var classText = CSharpClassTextGenerator.GenerateClassText(tableName, cSharpProperties);
            return classText.Trim();
        }
    }

    public static class InformationSchemaColumnToSqlTypeConverter
    {
        public static SqlType Convert(InformationSchemaColumn column)
        {
            var typeName = GetSqlTypeName(column);
            if (typeName == "int")
            {
                return SqlTypeFactory.Int();
            }
            if (typeName == "smallint")
            {
                return SqlTypeFactory.SmallInt();
            }
            if (typeName == "decimal")
            {
                return SqlTypeFactory.Decimal(column.NUMERIC_PRECISION.Value, column.NUMERIC_SCALE);
            }
            if (typeName == "numeric")
            {
                return SqlTypeFactory.Numeric(column.NUMERIC_PRECISION.Value, column.NUMERIC_SCALE);
            }
            if (typeName == "float")
            {
                return SqlTypeFactory.Float(column.NUMERIC_PRECISION.Value);
            }
            if (typeName == "real")
            {
                return SqlTypeFactory.Real();
            }
            if (typeName == "date")
            {
                return SqlTypeFactory.Date();
            }
            if (typeName == "datetime")
            {
                return SqlTypeFactory.DateTime();
            }
            if (typeName == "datetime2")
            {
                return SqlTypeFactory.DateTime2(column.DATETIME_PRECISION);
            }
            if (typeName == "char")
            {
                return SqlTypeFactory.Char(SqlTextualDataTypeOptionsFactory.FromInformationSchemaColumn(column));
            }
            if (typeName == "varchar")
            {
                return SqlTypeFactory.VarChar(SqlTextualDataTypeOptionsFactory.FromInformationSchemaColumn(column));
            }
            if (typeName == "nchar")
            {
                return SqlTypeFactory.NChar(SqlTextualDataTypeOptionsFactory.FromInformationSchemaColumn(column));
            }
            if (typeName == "nvarchar")
            {
                return SqlTypeFactory.NVarChar(SqlTextualDataTypeOptionsFactory.FromInformationSchemaColumn(column));
            }
            Console.WriteLine($"This is bad mkay: {typeName}");
            return new SqlType(typeName);
        }

        private static string GetSqlTypeName(InformationSchemaColumn column)
        {
            //Explanation:  So it turns out that for the AdventureWorks DB, there exist user-defined data types.
            //These are alias' for built-in sql types it seems -- see definition below:
            //      CREATE TYPE [dbo].[Phone]
            //          FROM NVARCHAR (25) NULL;
            //So, the question is how do we represent these user-defined data types?  We could simply use the backing sql
            //data type or we could use the user-defined data types themselves.  The downside to using the user-defined
            //data type is that we must then create a c# class to represent this datatype or else it won't compile.
            //In the interest of simplicity, let's simply use the backing sql type unless there
            //develops a reason to the contrary.

            //Easy Option:
            return column.DATA_TYPE;

            //Hard Option:
            //return column.DOMAIN_NAME ?? column.DATA_TYPE;
        }
    }
}