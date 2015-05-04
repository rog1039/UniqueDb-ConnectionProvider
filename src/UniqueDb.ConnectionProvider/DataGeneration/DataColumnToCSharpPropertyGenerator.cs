using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;

namespace UniqueDb.ConnectionProvider.DataGeneration
{
    public static class DataColumnToCSharpPropertyGenerator
    {
        public static CSharpProperty ToCSharpProperty(DataColumn column)
        {
            var cSharpProperty = new CSharpProperty();
            cSharpProperty.Name = column.ColumnName;
            cSharpProperty.ClrAccessModifier = ClrAccessModifier.Public;
            cSharpProperty.DataType = ConvertDataColumnClrTypeNameToString(column);
            return cSharpProperty;
        }

        private static string ConvertDataColumnClrTypeNameToString(DataColumn column)
        {
            if (column.DataType.Name == "String")
            {
                return "string";
            }
            if (column.DataType.Name == "Boolean")
            {
                return "bool";
            }
            if (column.DataType.Name == "Int32")
            {
                return "int";
            }
            if (column.DataType.Name == "Int16")
            {
                return "short";
            }
            return column.DataType.Name;
        }
    }
    public class SysTypes
    {
        public string ColumnName { get; set; }
        public int ColumnOrdinal { get; set; }
        public int ColumnSize { get; set; }
        public short NumericPrecision { get; set; }
        public short NumericScale { get; set; }
        public bool IsUnique { get; set; }
        public bool IsKey { get; set; }
        public string BaseServerName { get; set; }
        public string BaseCatalogName { get; set; }
        public string BaseColumnName { get; set; }
        public string BaseSchemaName { get; set; }
        public string BaseTableName { get; set; }
        public Type DataType { get; set; }
        public bool AllowDBNull { get; set; }
        public int ProviderType { get; set; }
        public bool IsAliased { get; set; }
        public bool IsExpression { get; set; }
        public bool IsIdentity { get; set; }
        public bool IsAutoIncrement { get; set; }
        public bool IsRowVersion { get; set; }
        public bool IsHidden { get; set; }
        public bool IsLong { get; set; }
        public bool IsReadOnly { get; set; }
        public Type ProviderSpecificDataType { get; set; }
        public string DataTypeName { get; set; }
        public string XmlSchemaCollectionDatabase { get; set; }
        public string XmlSchemaCollectionOwningSchema { get; set; }
        public string XmlSchemaCollectionName { get; set; }
        public string UdtAssemblyQualifiedName { get; set; }
        public int NonVersionedProviderType { get; set; }
        public bool IsColumnSet { get; set; }
    }


    public static class SqlQueryToCSharpPropertyGenerator
    {
        public static IList<CSharpProperty> FromQuery(ISqlConnectionProvider sqlConnectionProvider, string sqlQuery)
        {
            var sqlDataReader = GetDataReaderFromQuery(sqlConnectionProvider, sqlQuery);
            var columns = ExtractColumnsFromDataReader(sqlDataReader);
            return columns;
            
        }

        private static SqlDataReader GetDataReaderFromQuery(ISqlConnectionProvider sqlConnectionProvider, string sqlQuery)
        {
            var connection = sqlConnectionProvider.GetSqlConnection();
            connection.Open();
            var query = new SqlCommand(sqlQuery, connection);
            return query.ExecuteReader();
        }

        private static List<CSharpProperty> ExtractColumnsFromDataReader(SqlDataReader sqlDataReader)
        {
            var columns = new List<CSharpProperty>();
            var schemaTable = sqlDataReader.GetSchemaTable();
            foreach (DataRow row in schemaTable.Rows)
            {
                var property = new CSharpProperty();
                property.ClrAccessModifier = ClrAccessModifier.Public;
                property.DataType = ConvertDataColumnClrTypeNameToString(((Type) row["DataType"]).Name);
                property.Name = row["ColumnName"].ToString();
                property.IsNullable = (bool) row["AllowDBNull"];
                columns.Add(property);
            }
            return columns;
        }

        private static string ConvertDataColumnClrTypeNameToString(string typeName)
        {
            if (typeName == "String")
            {
                return "string";
            }
            if (typeName == "Boolean")
            {
                return "bool";
            }
            if (typeName == "Int32")
            {
                return "int";
            }
            if (typeName == "SqlHierarchyId")
            {
                return "SqlHierarchyId";
            }
            return typeName;
        }
    }



}