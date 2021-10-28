using System;
using System.Data;
using System.Data.SqlTypes;
using System.Reflection;
using System.Xml.Linq;
using Microsoft.Data.SqlClient;

namespace UniqueDb.ConnectionProvider.DataGeneration.Crud
{
    public static class SqlTextFunctions
    {
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
                LoggerHelper.Log($"*****UDT Type: {obj.GetType()} - {propertyInfo} - {propertyInfo.Name} ");
                sqlParameter.UdtTypeName = propertyInfo.PropertyType.Name;
            }
            if (propertyInfo.PropertyType == typeof(XElement))
            {
                sqlParameter.DbType = DbType.Xml;
                var xElement = (XElement)propertyInfo.GetValue(obj, null);
                sqlParameter.Value = new SqlXml(xElement.CreateReader());
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
    }
}