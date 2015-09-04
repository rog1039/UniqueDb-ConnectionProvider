using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using UniqueDb.ConnectionProvider.DataGeneration.Crud;

namespace UniqueDb.ConnectionProvider
{
    public static class SqlConnectionProviderBulkCopyInsert
    {
        public static void BulkInsert<T>(ISqlConnectionProvider sqlConnectionProvider, IList<T> list, string tableName, string schemaName ="dbo")
        {
            if (list.Count == 0) return;
            var dataTable = CreateDataTable(list);
            ExecuteBulkCopy(sqlConnectionProvider, schemaName, tableName, dataTable);
        }

        private static DataTable CreateDataTable<T>(IList<T> list)
        {
            var propertiesToInsert = SqlTextFunctions.GetRelevantPropertyInfos(list[0], null);
            var dataTable = CreateDataTable(propertiesToInsert);
            PopulateDataTable(list, dataTable, propertiesToInsert);
            return dataTable;
        }

        private static DataTable CreateDataTable(List<PropertyInfo> propertiesToInsert)
        {
            var dataTable = new DataTable();
            for (int index = 0; index < propertiesToInsert.Count; index++)
            {
                var propertyInfo = propertiesToInsert[index];
                dataTable.Columns.Add(propertyInfo.Name, propertyInfo.PropertyType);
            }
            return dataTable;
        }

        private static void PopulateDataTable<T>(IList<T> list, DataTable dataTable, List<PropertyInfo> propertiesToInsert)
        {
            foreach (var item in list)
            {
                var row = CreateDataRowFromItem(dataTable, propertiesToInsert, item);
                dataTable.Rows.Add(row);
            }
        }

        private static DataRow CreateDataRowFromItem<T>(DataTable dataTable, List<PropertyInfo> propertiesToInsert, T item)
        {
            var row = dataTable.NewRow();
            for (int index = 0; index < propertiesToInsert.Count; index++)
            {
                var propertyInfo = propertiesToInsert[index];
                row[index] = propertyInfo.GetValue(item);
            }
            return row;
        }

        private static void ExecuteBulkCopy(ISqlConnectionProvider sqlConnectionProvider, string schemaName, string tableName, DataTable dataTable)
        {
            using (var sqlBulkCopy = new SqlBulkCopy(sqlConnectionProvider.GetSqlConnectionString()))
            {
                sqlBulkCopy.BulkCopyTimeout = 480;
                sqlBulkCopy.BatchSize = 35000;
                sqlBulkCopy.DestinationTableName = $"{schemaName}.{tableName}";
                sqlBulkCopy.WriteToServer(dataTable);
                sqlBulkCopy.Close();
            }
        }
    }
}