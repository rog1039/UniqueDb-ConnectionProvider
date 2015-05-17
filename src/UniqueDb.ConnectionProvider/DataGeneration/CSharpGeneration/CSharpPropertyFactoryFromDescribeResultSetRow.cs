using System;
using System.Collections;
using System.Collections.Generic;
using UniqueDb.ConnectionProvider.DataGeneration.SqlMetadata;

namespace UniqueDb.ConnectionProvider.DataGeneration
{
    public static class CSharpPropertyFactoryFromDescribeResultSetRow
    {
        public static CSharpProperty ToCSharpProperty(DescribeResultSetRow resultSetColumn)
        {
            var sqlColumn = new SqlColumn();
            sqlColumn.Name = resultSetColumn.name;
            sqlColumn.IsNullable = resultSetColumn.is_nullable;
            sqlColumn.SqlDataType = SqlTypeParser.Parse(GetTypeName(resultSetColumn));

            var cSharpProperty = CSharpPropertyFactoryFromSqlColumn.ToCSharpProperty(sqlColumn);
            return cSharpProperty;
        }

        private static string GetTypeName(DescribeResultSetRow resultSetColumn)
        {
            var hasSystemType =  resultSetColumn.system_type_name?.Length > 0;
            var hasUserType = resultSetColumn.user_type_name?.Length > 0;
            var noTypeSpecified = !hasSystemType && !hasUserType;
            var bothTypesSpecified = hasSystemType && hasUserType;
            var bothTypesEqual = string.Equals(resultSetColumn.system_type_name, resultSetColumn.user_type_name);

            if (noTypeSpecified || (bothTypesSpecified && !bothTypesEqual))
                throw new InvalidOperationException("Invalid SQL type specification.");

            var typeName = hasSystemType
                ? resultSetColumn.system_type_name
                : resultSetColumn.user_type_name;
            return typeName;
        }
    }
}