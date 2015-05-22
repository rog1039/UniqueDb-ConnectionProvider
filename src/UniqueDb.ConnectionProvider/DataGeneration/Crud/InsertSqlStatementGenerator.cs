using System;
using System.Collections.Generic;
using System.Linq;

namespace UniqueDb.ConnectionProvider.DataGeneration.Crud
{
    public static class InsertSqlStatementGenerator
    {
        internal static string Generate(string tableName, object obj, IEnumerable<string> columnsToIgnoreList = null)
        {
            columnsToIgnoreList = columnsToIgnoreList ?? new List<string>();
            var columnHeaders = GetColumnHeaderListBlacklist(obj, columnsToIgnoreList);
            var columnValues = GetColumnValuesBlacklist(obj, columnsToIgnoreList);

            var generatedSql = string.Format($"INSERT INTO {tableName} ({columnHeaders}) VALUES ({columnValues});");
            return generatedSql;
        }

        public static string GetColumnHeaderListBlacklist(object o, IEnumerable<string> columnsToIgnoreList)
        {
            var propertyInfos = o.GetType().GetProperties().Where(x => !columnsToIgnoreList.Contains(x.Name));
            return string.Join(", ", propertyInfos.Select(x => x.Name));
        }

        public static object GetColumnValuesBlacklist(object o, IEnumerable<string> columnsToIgnoreList)
        {
            var propertyInfos = o.GetType().GetProperties().Where(x => !columnsToIgnoreList.Contains(x.Name));
            var valuesString = string.Join(", ", propertyInfos.Select(x => SqlValueEncoder.ConvertPropertyToSqlString(o, x)));
            return valuesString;
        }

        public static string GenerateInsertForSpecificProperties(string tableName, object obj, IEnumerable<string> columnsToAdd)
        {
            var columnHeaders = GetColumnHeaderListWhitelist(obj, columnsToAdd);
            var columnValues = GetColumnValuesWhitelist(obj, columnsToAdd);

            var generatedSql = string.Format($"INSERT INTO {tableName} ({columnHeaders}) VALUES ({columnValues});");
            Console.WriteLine(generatedSql);
            return generatedSql;
        }


        private static string GetColumnHeaderListWhitelist(object o, IEnumerable<string> columnsToIgnoreList)
        {
            var propertyInfos = o.GetType().GetProperties().Where(x => columnsToIgnoreList.Contains(x.Name));
            return string.Join(", ", propertyInfos.Select(x => x.Name));
        }

        private static object GetColumnValuesWhitelist(object o, IEnumerable<string> columnsToIgnoreList)
        {
            var propertyInfos = o.GetType().GetProperties().Where(x => columnsToIgnoreList.Contains(x.Name));
            var valuesString = string.Join(", ", propertyInfos.Select(x => SqlValueEncoder.ConvertPropertyToSqlString(o, x)));
            return valuesString;
        }
    }
}