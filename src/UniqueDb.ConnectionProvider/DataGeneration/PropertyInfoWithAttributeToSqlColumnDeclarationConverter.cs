using UniqueDb.ConnectionProvider.Converters;
using UniqueDb.ConnectionProvider.DataGeneration.SqlManipulation;

namespace UniqueDb.ConnectionProvider.DataGeneration;

public static class PropertyInfoWithAttributeToSqlColumnDeclarationConverter
{
    public static SqlColumnDeclaration Convert(PropertyInfoWithAttributes propertyInfoWithAttributes)
    {
        var nullableSqlType = ClrTypeToSqlTypeConverter.Convert(propertyInfoWithAttributes.PropertyInfo.PropertyType);
        var name            = propertyInfoWithAttributes.PropertyInfo.Name;
        AddNecessaryAnnotations(nullableSqlType, propertyInfoWithAttributes);
        return new SqlColumnDeclaration(name, nullableSqlType);
    }

    private static void AddNecessaryAnnotations(NullableSqlType nullableSqlType, PropertyInfoWithAttributes propertyInfoWithAttributes)
    {
        foreach (var attribute in propertyInfoWithAttributes.Attributes)
        {
            DataAnnotationAttributeHandler.ProcessAttribute(nullableSqlType, attribute);
        }
    }
}