using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniqueDb.ConnectionProvider.Tests.DataGeneration
{
    public static class CSharpClassGenerator
    {
        public static string GenerateClass(SqlTable table)
        {
            var cSharpColumns = table.SqlColumns.Select(SqlColumnToCSharpPropertyGenerator.ToCSharpProperty).ToList();
            var cSharpClass = GenerateClassText(table, cSharpColumns);
            return cSharpClass;
        }

        private static string GenerateClassText(SqlTable sqlTable, IEnumerable<CSharpProperty> cSharpProperties)
        {
            var sb = new StringBuilder();
            sb.AppendLine(string.Format("public class {0}", sqlTable.Name));
            sb.AppendLine("{");
            foreach (var cSharpProperty in cSharpProperties)
            {
                sb.AppendLine("    " + cSharpProperty.ToString());
            }
            sb.AppendLine("}");
            return sb.ToString();
        }
    }

    public class SqlTable
    {
        public string Schema { get; set; }
        public string Name { get; set; }
        public IList<SqlColumn> SqlColumns { get; set; }
    }

    public class SqlColumn
    {
        public string Name { get; set; }
        public int OrdinalPosition { get; set; }
        public string DataType { get; set; }
        public bool IsNullable { get; set; }
        public string Default { get; set; }
    }

    public static class SqlTableFactory
    {
        public static SqlTable Create(SqlTableReference sqlTableReference)
        {
            var columns = InformationSchemaMetadataExplorer.GetInformationSchemaColumns(sqlTableReference);
            var sqlColumns = columns.Select(SqlColumnFactory.Create).ToList();
            var sqlTable = new SqlTable()
            {
                Name = sqlTableReference.TableName,
                Schema = sqlTableReference.SchemaName,
                SqlColumns = sqlColumns
            };
            return sqlTable;
        }
    }

    public static class SqlColumnFactory
    {
        public static SqlColumn Create(InformationSchemaColumn infSchColumn)
        {
            var sqlColumn = new SqlColumn()
            {
                Name = infSchColumn.COLUMN_NAME,
                DataType = infSchColumn.DATA_TYPE,
                IsNullable = infSchColumn.IS_NULLABLE == "YES",
                OrdinalPosition = infSchColumn.ORDINAL_POSITION,
                Default = infSchColumn.COLUMN_DEFAULT
            };
            return sqlColumn;
        }
    }
}