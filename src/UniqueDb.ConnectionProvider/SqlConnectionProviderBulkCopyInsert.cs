using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using UniqueDb.ConnectionProvider.DataGeneration;
using Microsoft.Data.SqlClient;
using UniqueDb.ConnectionProvider.DataGeneration.Crud;

namespace UniqueDb.ConnectionProvider
{
    public static class SqlConnectionProviderBulkCopyInsert
    {
        public static void BulkInsert<T>(ISqlConnectionProvider sqlConnectionProvider, IList<T> list, string tableName, string schemaName ="dbo")
        {
            if (list.Count == 0) return;
            var dataTable = CreateDataTable(list);
            ExecuteBulkCopy(sqlConnectionProvider, schemaName, tableName.BracketizeSafe(), dataTable);
        }

        private static DataTable CreateDataTable<T>(IList<T> list)
        {
            var propertiesToInsert = SqlClrHelpers.GetRelevantPropertyInfos(list[0], null);
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
                var type = GetUnderlyingNullableTypeIfNullable(propertyInfo.PropertyType);
                dataTable.Columns.Add(propertyInfo.Name, type);
            }
            return dataTable;
        }

        private static Type GetUnderlyingNullableTypeIfNullable(Type propertyType)
        {
            return Nullable.GetUnderlyingType(propertyType) ?? propertyType;
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
                row[index] = propertyInfo.GetValue(item) ?? DBNull.Value;
            }
            return row;
        }

        private static void ExecuteBulkCopy(ISqlConnectionProvider sqlConnectionProvider, string schemaName, string tableName, DataTable dataTable)
        {
            using (var sqlBulkCopy = new SqlBulkCopy(sqlConnectionProvider.GetSqlConnectionString()))
            {
                VerifyDataTableColumnsMatchDbTableSchema(sqlConnectionProvider, schemaName, tableName, dataTable);
                SetColumnMappings(sqlBulkCopy, dataTable);
                sqlBulkCopy.BulkCopyTimeout      = 480;
                sqlBulkCopy.BatchSize            = 35000;
                sqlBulkCopy.DestinationTableName = $"{schemaName}.{tableName}";
                sqlBulkCopy.WriteToServer(dataTable);
                sqlBulkCopy.Close();
            }
        }

        private static void VerifyDataTableColumnsMatchDbTableSchema(ISqlConnectionProvider sqlConnectionProvider,
                                                                     string                 schemaName,
                                                                     string                 tableName,
                                                                     DataTable              dataTable)
        {
            var dbTableColumns =
                InformationSchemaMetadataExplorer.GetInformationSchemaColumns(
                    new SqlTableReference(sqlConnectionProvider, schemaName, tableName));
            var columnsFromDbDict = dbTableColumns.ToDictionary(z => z.COLUMN_NAME);
            
            var columnsInDataTableButMissingInDbTable = (
                from DataColumn dataTableColumn in dataTable.Columns
                let dbContainsDatatableColumn = columnsFromDbDict.ContainsKey(dataTableColumn.ColumnName)
                where !dbContainsDatatableColumn
                select dataTableColumn.ColumnName).ToList();
            
            

            if (columnsInDataTableButMissingInDbTable.Count > 0)
            {
                var message =
                    $"SqlBulkInsert will fail because the following columns are present in the DataTable but not in the Sql Database table: {string.Join(", ", columnsInDataTableButMissingInDbTable)}";

                throw new InvalidDataException(message);
            }
        }

        /// <summary>
        /// Note: this is necessary at least for the following reason:
        /// 
        /// Without explicit mapping based on column names, SqlBulkCopy inserts based on column ordinals.
        /// So if the datatable has the following properties:
        /// -------------------------------------------
        /// PartNumber (int) | PartDescription(string)
        /// -------------------------------------------
        /// 
        /// and the database table has the following properties:
        /// -------------------------------------------
        /// PartDescription(string) | PartNumber (int) 
        /// -------------------------------------------
        /// 
        /// then the bulk copy will fail because it will try to insert the part number data into the 
        /// part description column.  
        /// 
        /// So the mapping below will instead perform bulk copies on the names of the CLR properties 
        /// and the names of the columns in the database rather than on CLR and SQL column ordinals.
        /// 
        /// </summary>
        /// <param name="sqlBulkCopy"></param>
        /// <param name="dataTable"></param>
        private static void SetColumnMappings(SqlBulkCopy sqlBulkCopy, DataTable dataTable)
        {
            foreach (DataColumn column in dataTable.Columns)
            {
                var columnMapping = new SqlBulkCopyColumnMapping(column.ColumnName, column.ColumnName);
                sqlBulkCopy.ColumnMappings.Add(columnMapping);
            }
        }
    }
}