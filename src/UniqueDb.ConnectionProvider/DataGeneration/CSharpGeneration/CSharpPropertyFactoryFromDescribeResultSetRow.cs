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
            var sqlColumn = SqlColumnFactory.FromDescribeResultSetRow(resultSetColumn);
            var cSharpProperty = CSharpPropertyFactoryFromSqlColumn.ToCSharpProperty(sqlColumn);
            return cSharpProperty;
        }
    }
}