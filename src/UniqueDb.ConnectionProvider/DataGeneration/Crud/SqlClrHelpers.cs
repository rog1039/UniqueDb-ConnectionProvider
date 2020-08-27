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

        private static bool IsPropertyDatabaseGenerated(PropertyInfo arg)
        {
            var isDatabaseGenerated = arg
                .CustomAttributes
                .Any(a => a.AttributeType == typeof(DatabaseGeneratedAttribute));
            return isDatabaseGenerated;
        }

        private static bool IsColumnIgnored(PropertyInfo arg, IEnumerable<string> columnsToIgnore)
        {
            return columnsToIgnore?.Contains(arg.Name) == true;
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