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
            var sqlColumn = DescribeResultSetRowToSqlColumnConverter.Convert(resultSetColumn);
            var cSharpProperty = CSharpPropertyFactoryFromSqlColumn.ToCSharpProperty(sqlColumn);
            return cSharpProperty;
        }
        
        public static CSharpProperty ToCSharpProperty(DescribeResultSetContainer resultSetContainer)
        {
            var sqlColumn = DescribeResultSetRowToSqlColumnConverter.Convert(resultSetContainer);
            var cSharpProperty = CSharpPropertyFactoryFromSqlColumn.ToCSharpProperty(sqlColumn);
            return cSharpProperty;
        }
    }
}