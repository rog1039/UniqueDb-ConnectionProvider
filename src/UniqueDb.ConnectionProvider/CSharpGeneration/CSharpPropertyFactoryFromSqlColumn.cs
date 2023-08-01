using UniqueDb.ConnectionProvider.Converters;
using UniqueDb.ConnectionProvider.CoreTypes;
using UniqueDb.ConnectionProvider.Infrastructure.Extensions;

namespace UniqueDb.ConnectionProvider.CSharpGeneration;

public static class CSharpPropertyFactoryFromSqlColumn
{
    public static CSharpProperty ToCSharpProperty(SqlColumn sqlColumn)
    {
        var cSharpProperty = new CSharpProperty();

        var propertyName = AutomaticPropertyNameRewrites.GetNameWithRewriting(sqlColumn.Name);
        cSharpProperty.Name              = propertyName;
        cSharpProperty.ClrAccessModifier = ClrAccessModifier.Public;
        cSharpProperty.IsNullable        = sqlColumn.IsNullable;

        cSharpProperty.DataType = SqlTypeLists.IsSystemType(sqlColumn.SqlDataType.TypeName)
            ? SqlToClrTypeConverter.GetClrTypeName(sqlColumn.SqlDataType.TypeName)
            : sqlColumn.SqlDataType.TypeName;
                
            
        cSharpProperty.DataAnnotationDefinitionBases.AddRange(DataAnnotationFactory.CreateDataAnnotations(sqlColumn));

        return cSharpProperty;
    }
}