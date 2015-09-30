using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace UniqueDb.ConnectionProvider.DataGeneration.Crud
{
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

        public static bool ShouldTranslateClrPropertyToSqlColumn(PropertyInfo arg,
                                                                 IEnumerable<string> columnsToIgnore = null)
        {
            columnsToIgnore = columnsToIgnore ?? Enumerable.Empty<string>();
            var inIgnoreList = columnsToIgnore.Contains(arg.Name);
            if (inIgnoreList)
                return false;

            var isString = arg.PropertyType == typeof(string);
            if (isString)
                return true;

            var implementsEnumerable = arg
                .PropertyType
                .GetInterfaces()
                .Any(interfaceType => interfaceType.Name.ToLower().Equals("ienumerable"));

            var isDatabaseGenerated = arg
                .CustomAttributes
                .Any(a => a.AttributeType == typeof(DatabaseGeneratedAttribute));

            var shouldSkip = implementsEnumerable || isDatabaseGenerated;
            return !shouldSkip;
        }

        public static IList<PropertyInfo> GetPropertiesFromObject<T>(T obj, Expression<Func<T, object>> keyProperties)
        {
            bool useAllObjectProperties = false;
            var propertiesOfKey = new List<string>();

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
}