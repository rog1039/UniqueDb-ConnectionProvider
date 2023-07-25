using System.Linq.Expressions;
using System.Reflection;
using UniqueDb.ConnectionProvider.Converters;

namespace UniqueDb.ConnectionProvider.DataGeneration.Crud;

public static class SqlClrHelpers
{
    public static List<PropertyInfo> GetRelevantPropertyInfos(object obj, IEnumerable<string> columnsToIgnore)
    {
        columnsToIgnore = columnsToIgnore ?? new List<string>();
        var propertyInfos = obj.GetType()
            .GetProperties()
            .Where(x => ShouldTranslateClrPropertyToSqlColumn(x, columnsToIgnore))
            .ToList();
        return propertyInfos;
    }

    public static bool ShouldTranslateClrPropertyToSqlColumn(PropertyInfo        arg,
                                                             IEnumerable<string> columnsToIgnore = null)
    {
        if (IsColumnIgnored(arg, columnsToIgnore)) return false;
        if (IsPropertyDatabaseGenerated(arg)) return false;
            
        return ClrTypeToSqlTypeConverter.CanTranslateToSqlType(arg.PropertyType);

        // var isString = argType == typeof(string);
        // if (isString)
        //     return true;
        //
        //
        // var implementsEnumerable = argType
        //     .GetInterfaces()
        //     .Any(interfaceType => interfaceType.Name.ToLower().Equals("ienumerable"));
        //
        // var isDatabaseGenerated = arg
        //     .CustomAttributes
        //     .Any(a => a.AttributeType == typeof(DatabaseGeneratedAttribute));
        //
        // var shouldSkip = implementsEnumerable || isDatabaseGenerated;
        // return !shouldSkip;
    }

    private static bool IsNotMapped(PropertyInfo arg)
    {
        var isNotMapped = arg
            .CustomAttributes
            .Any(a => a.AttributeType.IsNotMappedAttribute());
        return isNotMapped;
    }

    private static bool IsPropertyDatabaseGenerated(PropertyInfo arg)
    {
        var isDatabaseGenerated = arg
            .CustomAttributes
            .Any(a => a.AttributeType.IsDatabaseGeneratedAttribute());
        return isDatabaseGenerated;
    }

    private static bool IsColumnIgnored(PropertyInfo arg, IEnumerable<string> columnsToIgnore)
    {
        var onIgnoreList = columnsToIgnore?.Contains(arg.Name) == true;
        if (onIgnoreList) return true;

        var isNotMapped = IsNotMapped(arg);
        if (isNotMapped) return true;

        return false;
    }

    public static IList<PropertyInfo> GetPropertiesFromObject<T>(T obj, Expression<Func<T, object>> keyProperties)
    {
        bool useAllObjectProperties = false;
        var  propertiesOfKey        = new List<string>();

        if (keyProperties == null)
            useAllObjectProperties = true;
        else
            propertiesOfKey = keyProperties
                .Body.Type
                .GetProperties()
                .Select(x => x.Name)
                .ToList();

        var propertiesFromObject = obj
            .GetType()
            .GetProperties()
            .Where(x => useAllObjectProperties || propertiesOfKey.Contains(x.Name));
        return propertiesFromObject.ToList();
    }
}