using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace UniqueDb.ConnectionProvider.DataGeneration
{
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
            return typeName;
        }
    }
}