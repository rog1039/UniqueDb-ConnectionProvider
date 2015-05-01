using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace UniqueDb.ConnectionProvider.Tests.DataGeneration
{
    public static class SqlDmlGeneratorFromInformationSchema
    {
        public static string GenerateCreateTableScript(InformationSchemaTableDefinition tableDefinition)
        {
            if (tableDefinition.InformationSchemaTable.TABLE_TYPE != Table_Types.BaseTable && tableDefinition.InformationSchemaTable.TABLE_TYPE != Table_Types.View)
            {
                throw new NotSupportedException(
                    string.Format("Table is of a type other than 'Base Table' or 'View'.  Table type is {0}",
                        tableDefinition.InformationSchemaTable.TABLE_TYPE));
            }

            string createScript = string.Empty;

            if (tableDefinition.InformationSchemaTable.TABLE_TYPE == Table_Types.BaseTable)
                createScript = CreateTableScript(tableDefinition);
            if (tableDefinition.InformationSchemaTable.TABLE_TYPE == Table_Types.View)
                createScript = CreateViewScript(tableDefinition);

            return createScript;
        }

        private static string CreateViewScript(InformationSchemaTableDefinition tableDefinition)
        {
            throw new NotImplementedException();
        }

        private static string CreateTableScript(InformationSchemaTableDefinition tableDefinition)
        {
            var sb = new StringBuilder();
            sb.AppendFormat("CREATE TABLE {0} (\r\n", tableDefinition.InformationSchemaTable.TABLE_NAME.Bracketize());
            AddColumnCreation(sb, tableDefinition);
            sb.AppendLine(");");
            return sb.ToString();
        }

        private static void AddColumnCreation(StringBuilder sb, InformationSchemaTableDefinition tableDefinition)
        {
            var internalSb = new StringBuilder();
            var maxColumnNameWidth = tableDefinition.InformationSchemaColumns.Max(x => x.COLUMN_NAME.Length)+2;
            var maxDataTypeWidth = tableDefinition.InformationSchemaColumns.Select(x => GetDataTypeString(x)).Max(x => x.Length);

            foreach (var column in tableDefinition.InformationSchemaColumns)
            {
                internalSb.Clear();
                var formatString =
                    "    " +
                    LeftAlignedPosition(0, maxColumnNameWidth)+
                    " " +
                    LeftAlignedPosition(1, maxDataTypeWidth) +
                    " " +
                    LeftAlignedPosition(2, 8) +
                    ",\r\n";

                sb.AppendFormat(formatString,
                    column.COLUMN_NAME.Bracketize(),
                    GetDataTypeString(column).ToUpper(),
                    NullNotNullToString(column));
            }
        }

        private static string GetDataTypeString(InformationSchemaColumn informationSchemaColumn)
        {
            if (IsTextualDataType(informationSchemaColumn))
            {
                return GetTextualDataTypeString(informationSchemaColumn);
            }
            if (IsPrecisionNumber(informationSchemaColumn))
            {
                return GetPrecisionNumberDataTypeString(informationSchemaColumn);
            }
            return informationSchemaColumn.DATA_TYPE;
        }

        private static bool IsTextualDataType(InformationSchemaColumn informationSchemaColumn)
        {
            var dt = informationSchemaColumn.DATA_TYPE;
            var isTextualDataType = dt.StartsWith("nv") || dt.StartsWith("nch") || dt.StartsWith("varc") ||
                                dt.StartsWith("char");
            return isTextualDataType;
        }

        private static string GetTextualDataTypeString(InformationSchemaColumn informationSchemaColumn)
        {
            var textDataTypeLength = informationSchemaColumn.CHARACTER_MAXIMUM_LENGTH == -1
                ? "MAX"
                : informationSchemaColumn.CHARACTER_MAXIMUM_LENGTH.ToString();

            var text = string.Format("{0} ({1})",
                informationSchemaColumn.DATA_TYPE,
                textDataTypeLength);
            return text;
        }

        private static bool IsPrecisionNumber(InformationSchemaColumn informationSchemaColumn)
        {
            var dt = informationSchemaColumn.DATA_TYPE;
            var isPrecisionNumber = dt.StartsWith("doub") || dt.StartsWith("flo") || dt.StartsWith("deci");
            return isPrecisionNumber;
        }

        private static string GetPrecisionNumberDataTypeString(InformationSchemaColumn informationSchemaColumn)
        {
            var text = string.Format("{0} ({1}, {2})",
                informationSchemaColumn.DATA_TYPE,
                informationSchemaColumn.NUMERIC_PRECISION,
                informationSchemaColumn.NUMERIC_SCALE);
            return text;
        }

        private static string NullNotNullToString(InformationSchemaColumn column)
        {
            return column.IS_NULLABLE.Equals("YES", StringComparison.CurrentCultureIgnoreCase) ? "NULL" : "NOT NULL";
        }

        private static string LeftAlignedPosition(int positionNumber, int alignmentLength)
        {
            return "{" + positionNumber + ",-" + alignmentLength + "}";
        }
    }

    public static class Table_Types
    {
        public static string BaseTable = "BASE TABLE";
        public static string View = "VIEW";
    }

    public static class TableManipulation
    {
        public static void CreateTable(SqlTableReference sourceTable, SqlTableReference targetTable)
        {
            var infschTable = InformationSchemaMetadataExplorer.GetInformationSchemaTableDefinition(sourceTable);
            var createTableScript = SqlDmlGeneratorFromInformationSchema.GenerateCreateTableScript(infschTable);
            targetTable.SqlConnectionProvider.ExecuteScript(createTableScript);
        }

        public static void DropTable(SqlTableReference sourceTable)
        {
            var informationSchemaTableDefinition = InformationSchemaMetadataExplorer.GetInformationSchemaTableDefinition(sourceTable);
            var dropTableScript = SqlDmlGeneratorFromInformationSchema.GenerateCreateTableScript(informationSchemaTableDefinition);
            sourceTable.SqlConnectionProvider.ExecuteScript(dropTableScript);
        }
    }

    public static class ISqlConnectionProviderDapperOperations
    {
        public static void ExecuteScript(this ISqlConnectionProvider sqlConnectionProvider, string script)
        {
            sqlConnectionProvider.GetSqlConnection().Execute(script);
        }

        public static IEnumerable<T> Query<T>(this ISqlConnectionProvider sqlConnectionProvider, string script)
        {
            return sqlConnectionProvider.GetSqlConnection().Query<T>(script);
        }
    }

    public static class StringExtensions
    {
        public static string Bracketize(this string input)
        {
            return "[" + input + "]";
        }

        public static string Bracify(this string input)
        {
            return "{" + input + "}";
        }
    }
}
