using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Odbc;
using System.Data.SqlClient;

namespace UniqueDb.ConnectionProvider.DataGeneration.CSharpGeneration
{
    public static class CSharpPropertyFactoryFromAdoDataReader
    {
        public static IList<CSharpProperty> FromQuery(DbConnection dbConnection, string sqlQuery)
        {
            var reader = GetReader(dbConnection, sqlQuery);
            var properties = ExtractColumnsFromDataReader(reader);
            return properties;
        }

        private static IDataReader GetReader(IDbConnection connection, string query)
        {
            var command = GetCommand(connection, query);
            var reader = GetReader(command);
            return reader;
        }

        private static IDbCommand GetCommand(IDbConnection connection, string query)
        {
            if (connection is OdbcConnection)
                return new OdbcCommand(query, (OdbcConnection)connection);
            if (connection is SqlConnection)
                return new SqlCommand(query, (SqlConnection) connection);

            throw new NotImplementedException();
        }

        private static IDataReader GetReader(IDbCommand command)
        {
            var connection = command.Connection;
            if (connection.State != ConnectionState.Open) connection.Open();

            return command.ExecuteReader();
        }

        public static IList<CSharpProperty> FromQuery(ISqlConnectionProvider sqlConnectionProvider, string sqlQuery)
        {
            var connection = sqlConnectionProvider.GetSqlConnection();
            var reader = GetReader(connection, sqlQuery);
            var properties = ExtractColumnsFromDataReader(reader);
            return properties;
        }

        private static List<CSharpProperty> ExtractColumnsFromDataReader(IDataReader sqlDataReader)
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
            if (typeName == "Bit")
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