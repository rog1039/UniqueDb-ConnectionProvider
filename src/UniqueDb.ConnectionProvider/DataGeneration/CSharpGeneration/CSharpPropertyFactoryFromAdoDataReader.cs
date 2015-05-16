using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using UniqueDb.ConnectionProvider.DataGeneration.SqlMetadata;

namespace UniqueDb.ConnectionProvider.DataGeneration
{
    public static class CSharpPropertyFactoryFromAdoDataReader
    {
        public static IList<CSharpProperty> FromQuery(ISqlConnectionProvider sqlConnectionProvider, string sqlQuery)
        {
            var reader = GetDataReaderFromQuery(sqlConnectionProvider, sqlQuery);
            var properties = ExtractColumnsFromDataReader(reader);
            return properties;
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
            foreach (DataRow resultSetColumn in schemaTable.Rows)
            {
                var property = new CSharpProperty();
                property.ClrAccessModifier = ClrAccessModifier.Public;
                property.DataType = ConvertDataColumnClrTypeNameToString(((Type) resultSetColumn["DataType"]).Name);
                property.Name = resultSetColumn["ColumnName"].ToString();
                property.IsNullable = (bool) resultSetColumn["AllowDBNull"];
                property.DataAnnotationDefinitionBases.AddRange(CreateDataAnnotations(resultSetColumn, property.DataType));
                columns.Add(property);
            }
            return columns;
        }

        private static IEnumerable<DataAnnotationDefinitionBase> CreateDataAnnotations(DataRow row, string clrDataType)
        {
            if (clrDataType == "string" && row["ColumnSize"] is int)
            {
                var stringLengthDataAnnotation = new DataAnnotationDefinitionMaxCharacterLength(
                    (int) row["ColumnSize"]);
                yield return stringLengthDataAnnotation;
            }
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