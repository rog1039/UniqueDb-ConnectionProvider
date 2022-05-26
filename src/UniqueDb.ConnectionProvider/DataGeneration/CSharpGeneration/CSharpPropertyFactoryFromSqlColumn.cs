using UniqueDb.ConnectionProvider.Converters;

namespace UniqueDb.ConnectionProvider.DataGeneration.CSharpGeneration;

public static class CSharpPropertyFactoryFromSqlColumn
{
    public static CSharpProperty ToCSharpProperty(SqlColumn sqlColumn)
    {
        var cSharpProperty = new CSharpProperty();

        var propertyName = AutomaticPropertyNameRewrites.GetNameWithRewriting(sqlColumn.Name);
        cSharpProperty.Name              = propertyName;
        cSharpProperty.ClrAccessModifier = ClrAccessModifier.Public;
        cSharpProperty.IsNullable        = sqlColumn.IsNullable;

        cSharpProperty.DataType = SqlTypes.IsSystemType(sqlColumn.SqlDataType.TypeName)
            ? SqlToClrTypeConverter.GetClrTypeName(sqlColumn.SqlDataType.TypeName)
            : sqlColumn.SqlDataType.TypeName;
                
            
        cSharpProperty.DataAnnotationDefinitionBases.AddRange(DataAnnotationFactory.CreateDataAnnotations(sqlColumn));

        return cSharpProperty;
    }
}