using System.Text;
using UniqueDb.ConnectionProvider.DataGeneration.SqlMetadata;

namespace UniqueDb.ConnectionProvider.DataGeneration.SqlManipulation;

public enum CreateIfExistsModification
{
    CreateAnyway,
    DropAndRecreate,
    PreventCreation
}

public static class InfSchemaToSqlDmlCreateStatementGenerator
{
    public static string GenerateCreateTableScript(string                         schemaName, string tableName,
                                                   IList<InformationSchemaColumn> columns,
                                                   CreateIfExistsModification ifExists =
                                                       CreateIfExistsModification.CreateAnyway)
    {
        string createScript = string.Empty;
        createScript = CreateTableScript(schemaName, tableName, columns, ifExists);
        return createScript;
    }

    public static string GenerateCreateTableScript(InformationSchemaTableDefinition tableDefinition,
                                                   CreateIfExistsModification createIfExistsModification =
                                                       CreateIfExistsModification.CreateAnyway)
    {
        if (tableDefinition.InformationSchemaTable.TABLE_TYPE != TableTypes.BaseTable &&
            tableDefinition.InformationSchemaTable.TABLE_TYPE != TableTypes.View)
        {
            throw new NotSupportedException(
                string.Format("Table is of a type other than 'Base Table' or 'View'.  Table type is {0}",
                              tableDefinition.InformationSchemaTable.TABLE_TYPE));
        }

        string createScript = string.Empty;

        if (tableDefinition.InformationSchemaTable.TABLE_TYPE == TableTypes.BaseTable)
            createScript = CreateTableScript(tableDefinition, createIfExistsModification);
        if (tableDefinition.InformationSchemaTable.TABLE_TYPE == TableTypes.View)
            throw new NotImplementedException("Have not implemented View creation yet.");

        return createScript;
    }

    private static string CreateTableScript(InformationSchemaTableDefinition tableDefinition,
                                            CreateIfExistsModification       ifExists)
    {
        var infSchColumns = tableDefinition.InformationSchemaColumns;
        var infSchTable   = tableDefinition.InformationSchemaTable;
        var script        = CreateTableScript(infSchTable.TABLE_SCHEMA, infSchTable.TABLE_NAME, infSchColumns, ifExists);
        return script;
    }

    public static string CreateTableScript(string                         schemaName, string tableName,
                                           IList<InformationSchemaColumn> columns,
                                           CreateIfExistsModification     ifExists)
    {
        var fullTableName = GetTableName(schemaName, tableName);

        var sb = new StringBuilder();
        sb.AppendFormat("CREATE TABLE {0} (\r\n", fullTableName);
        AddColumnDefinitionsToCreateTableStringBuilder(sb, columns);
        sb.AppendLine(");");
        var createTablePortion = sb.ToString();

        var dbTableName       = new DbTableName(schemaName, tableName);
        var totalCreateScript = AddExistingTableHandling(ifExists, dbTableName, createTablePortion);
        return totalCreateScript;
    }

    private static string AddExistingTableHandling(CreateIfExistsModification createIfExistsModification,
                                                   DbTableName                dbTableName, 
                                                   string                     createTablePortion)
    {
        switch (createIfExistsModification)
        {
            case CreateIfExistsModification.CreateAnyway:
                return createTablePortion;

            case CreateIfExistsModification.DropAndRecreate:
                return $@"
DROP TABLE IF EXISTS {dbTableName};
{createTablePortion}";

            case CreateIfExistsModification.PreventCreation:
                return $@"
IF (OBJECT_ID('{dbTableName}', 'U')) IS NULL
BEGIN
    {createTablePortion}
END
";
            default:
                throw new ArgumentOutOfRangeException(nameof(createIfExistsModification),
                                                      createIfExistsModification, null);
        }
    }

    private static string GetTableName(string tableSchema, string tableName)
    {
        if (string.IsNullOrWhiteSpace(tableSchema))
            return tableName.Bracketize();
        return $"{tableSchema.Bracketize()}.{tableName.Bracketize()}";
    }

    private static void AddColumnDefinitionsToCreateTableStringBuilder(
        StringBuilder sb, IList<InformationSchemaColumn> informationSchemaColumns)
    {
        var internalSb         = new StringBuilder();
        var maxColumnNameWidth = informationSchemaColumns.Max(x => x.COLUMN_NAME.Length) + 2;
        var maxDataTypeWidth   = informationSchemaColumns.Select(x => GetStringForDataType(x)).Max(x => x.Length);

        foreach (var column in informationSchemaColumns)
        {
            internalSb.Clear();
            var formatString =
                "    " +
                AlignLeft(0, maxColumnNameWidth) +
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
        return $"{{{positionNumber},-{alignmentLength}}}";
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
        var dt                = informationSchemaColumn.DATA_TYPE;
        var isPrecisionNumber = dt.StartsWith("doub") || dt.StartsWith("flo") || dt.StartsWith("deci");
        return isPrecisionNumber;
    }

    private static string GetStringForPrecisionNumberDataType(InformationSchemaColumn informationSchemaColumn)
    {
        if (informationSchemaColumn.DATA_TYPE.InsensitiveEquals("float"))
        {
            var text2 = string.Format("{0} ({1})",
                                      informationSchemaColumn.DATA_TYPE,
                                      informationSchemaColumn.NUMERIC_PRECISION);
            return text2;
        }

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