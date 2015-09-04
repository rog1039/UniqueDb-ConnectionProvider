using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Xml.Linq;
using Microsoft.SqlServer.Types;

namespace UniqueDb.ConnectionProvider.DataGeneration.Crud
{
    public static class SqlTextFunctions
    {
        public static void LogSqlStatement(string sqlStatement)
        {
            LogSqlStatementAction?.Invoke(sqlStatement);
        }

        public static Action<string> LogSqlStatementAction { get; set; } = null;

        public static bool UnUnderscoreColumnNames = true;
        public static string GetParameterName(PropertyInfo propertyInfo) => "@" + propertyInfo.Name;
        public static string GetColumnNameFromPropertyInfo(PropertyInfo propertyInfo)
        {
            return UnUnderscoreColumnNames
                ? propertyInfo.Name.Replace("_", " ").Replace("Property", String.Empty).Bracketize()
                : propertyInfo.Name.Replace("Property", String.Empty).Bracketize();
        }
        public static string GetSetClauseParameterName(PropertyInfo propertyInfo) => ("@sc" + propertyInfo.Name);
        public static string GetWhereClauseParameterName(PropertyInfo propertyInfo) => ("@wc" + propertyInfo.Name);
        public static string UnUnderscoreName(string name) => name.Replace("_", " ");


        public static SqlParameter GetParameter(object obj, PropertyInfo propertyInfo, string parameterName = null)
        {
            parameterName = parameterName ?? propertyInfo.Name;
            var sqlParameter = new SqlParameter(parameterName, propertyInfo.GetValue(obj, null));
            var typeOfParameter = GetParameterType(propertyInfo);

            if (!SqlTypes.IsClrTypeASqlSystemType(typeOfParameter))
            {
                Console.WriteLine($"*****UDT Type: {obj.GetType()} - {propertyInfo} - {propertyInfo.Name} ");
                sqlParameter.UdtTypeName = propertyInfo.PropertyType.Name;
            }
            if (propertyInfo.PropertyType == typeof(XElement))
            {
                sqlParameter.DbType = DbType.Xml;
                var xElement = (XElement)propertyInfo.GetValue(obj, null);
                sqlParameter.Value = new SqlXml(xElement.CreateReader());
            }
            if (propertyInfo.PropertyType == typeof(SqlHierarchyId))
            {
                sqlParameter.SqlValue = propertyInfo.GetValue(obj, null);
            }
            if (propertyInfo.PropertyType.IsEnum)
            {
                sqlParameter.SqlValue = (int)propertyInfo.GetValue(obj, null);
            }
            else if (propertyInfo.PropertyType == typeof(DateTime))
            {
                sqlParameter.DbType = DbType.DateTime2;
            }
            if (sqlParameter.Value == null)
            {
                sqlParameter.Value = DBNull.Value;
            }
            return sqlParameter;
        }

        private static Type GetParameterType(PropertyInfo propertyInfo)
        {
            if (propertyInfo.PropertyType.IsGenericType)
            {
                return Nullable.GetUnderlyingType(propertyInfo.PropertyType);
            }
            if (propertyInfo.PropertyType.IsEnum)
            {
                return typeof(int);
            }
            return propertyInfo.PropertyType;
        }

        public static string GetTableName(Type obj, string tableName, string schemaName)
        {
            tableName = tableName ?? obj.Name;
            if (!String.IsNullOrWhiteSpace(schemaName)) tableName = schemaName + "." + tableName;
            return tableName;
        }

        public static List<PropertyInfo> GetRelevantPropertyInfos(object obj, IEnumerable<string> columnsToIgnore)
        {
            columnsToIgnore = columnsToIgnore ?? new List<string>();
            var propertyInfos = obj.GetType()
                .GetProperties()
                .Where(x => ShouldTranslateClrPropertyToSqlColumn(x, columnsToIgnore))
                .ToList();
            return propertyInfos;
        }

        public static bool ShouldTranslateClrPropertyToSqlColumn(PropertyInfo arg, IEnumerable<string> columnsToIgnore = null)
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
                .Any(a => a.AttributeType == typeof (DatabaseGeneratedAttribute));

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

        public static void LogSqlCommand(SqlCommand myCommand)
        {
            LogSqlStatement(myCommand.CommandText);
            LogParameters(myCommand);
        }

        private static void LogParameters(SqlCommand myCommand)
        {
            var parameterTable = myCommand.Parameters
                .Cast<SqlParameter>()
                .ToList()
                .ToStringTable(z => z.ParameterName, z => z.SqlDbType, z => z.Value);
            LogSqlStatement(parameterTable);
        }
    }
}