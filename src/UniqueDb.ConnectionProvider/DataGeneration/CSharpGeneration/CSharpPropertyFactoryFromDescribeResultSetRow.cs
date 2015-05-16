using System.Collections;
using System.Collections.Generic;
using UniqueDb.ConnectionProvider.DataGeneration.SqlMetadata;

namespace UniqueDb.ConnectionProvider.DataGeneration
{
    public static class CSharpPropertyFactoryFromDescribeResultSetRow
    {
        public static CSharpProperty ToCSharpProperty(DescribeResultSetRow resultSetColumn)
        {
            var property = new CSharpProperty();
            property.Name = resultSetColumn.name;
            property.ClrAccessModifier = ClrAccessModifier.Public;
            property.IsNullable = resultSetColumn.is_nullable;

            var sqlTypeParseResult = SqlResultSetColumnTypeParser.ParseSqlSystemType(resultSetColumn);
            SetCSharpPropertyType(property, sqlTypeParseResult);

            property.DataAnnotationDefinitionBases.AddRange(GetDataAnnotations(resultSetColumn, sqlTypeParseResult));

            return property;
        }
        
        private static void SetCSharpPropertyType(CSharpProperty property, SqlResultSetColumnTypeParser.SqlResultSetColumnTypeParseResult sqlResultSetColumnTypeParseResult)
        {
            if (sqlResultSetColumnTypeParseResult.IsSystemDefined)
            {
                property.DataType = SqlTypeStringToClrTypeStringConverter.GetClrDataType(sqlResultSetColumnTypeParseResult.SqlType);
            }
            else
            {
                property.DataType = sqlResultSetColumnTypeParseResult.SqlType;
            }
        }

        private static IEnumerable<DataAnnotationDefinitionBase> GetDataAnnotations(DescribeResultSetRow resultSetColumn, SqlResultSetColumnTypeParser.SqlResultSetColumnTypeParseResult sqlResultSetColumnTypeParseResult)
        {
            if (sqlResultSetColumnTypeParseResult.Precision1.HasValue
                && sqlResultSetColumnTypeParseResult.Precision1.Value > 0 
                && SqlTypes.IsCharType(sqlResultSetColumnTypeParseResult.SqlType))
            {
                yield return new DataAnnotationDefinitionMaxCharacterLength(sqlResultSetColumnTypeParseResult.Precision1.Value);
            }
        }
    }
}