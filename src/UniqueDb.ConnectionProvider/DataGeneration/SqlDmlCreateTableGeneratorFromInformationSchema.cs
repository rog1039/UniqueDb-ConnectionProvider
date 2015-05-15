using System;
using System.Linq;
using System.Text;
using UniqueDb.ConnectionProvider.DataGeneration.SqlMetadata;

namespace UniqueDb.ConnectionProvider.DataGeneration
{
    public static class SqlDmlCreateTableGeneratorFromInformationSchema
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
                throw new NotImplementedException("Have not implemented View creation yet.");

            return createScript;
        }
        
        private static string CreateTableScript(InformationSchemaTableDefinition tableDefinition)
        {
            var sb = new StringBuilder();
            sb.AppendFormat("CREATE TABLE {0} (\r\n", tableDefinition.InformationSchemaTable.TABLE_NAME.Bracketize());
            AddColumnDefinitionsToCreateTableStringBuilder(sb, tableDefinition);
            sb.AppendLine(");");
            return sb.ToString();
        }

        private static void AddColumnDefinitionsToCreateTableStringBuilder(StringBuilder sb, InformationSchemaTableDefinition tableDefinition)
        {
            var internalSb = new StringBuilder();
            var maxColumnNameWidth = tableDefinition.InformationSchemaColumns.Max(x => x.COLUMN_NAME.Length)+2;
            var maxDataTypeWidth = tableDefinition.InformationSchemaColumns.Select(x => GetStringForDataType(x)).Max(x => x.Length);

            foreach (var column in tableDefinition.InformationSchemaColumns)
            {
                internalSb.Clear();
                var formatString =
                    "    " +
                    AlignLeft(0, maxColumnNameWidth)+
                    " " +
                    AlignLeft(1, maxDataTypeWidth) +
                    " " +
                    AlignLeft(2, 8) +
                    ",\r\n";

                sb.AppendFormat(formatString,
                    column.COLUMN_NAME.Bracketize(),
                    GetStringForDataType(column).ToUpper(),
                    NullNotNullToString(column));
            }
        }

        private static string AlignLeft(int positionNumber, int alignmentLength)
        {
            return "{" + positionNumber + ",-" + alignmentLength + "}";
        }

        private static string GetStringForDataType(InformationSchemaColumn informationSchemaColumn)
        {
            if (IsTextualDataType(informationSchemaColumn))
            {
                return GetStringForTextualDataType(informationSchemaColumn);
            }
            if (IsPrecisionNumber(informationSchemaColumn))
            {
                return GetStringForPrecisionNumberDataType(informationSchemaColumn);
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

        private static string GetStringForTextualDataType(InformationSchemaColumn informationSchemaColumn)
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

        private static string GetStringForPrecisionNumberDataType(InformationSchemaColumn informationSchemaColumn)
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
    }
}
