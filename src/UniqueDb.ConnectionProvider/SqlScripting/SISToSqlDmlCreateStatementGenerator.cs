using System.Text;
using UniqueDb.ConnectionProvider.Converters;
using UniqueDb.ConnectionProvider.DataGeneration;
using UniqueDb.ConnectionProvider.Infrastructure.Extensions;
using UniqueDb.ConnectionProvider.SqlMetadata.InformationSchema;

namespace UniqueDb.ConnectionProvider.SqlScripting;

public static class SISToSqlDmlCreateStatementGenerator
{
   public static string CreateTableScript(string                     schemaName,
                                          string                     tableName,
                                          IList<SISColumn>           columns,
                                          CreateIfExistsModification ifExists = CreateIfExistsModification.CreateAnyway)
   {
      var dbTableName = new DbTableName(schemaName, tableName);

      var sb = new StringBuilder();
      sb.Append($"CREATE TABLE {dbTableName} (\r\n");
      AddColumnDefinitions(sb, columns);
      sb.AppendLine(");");
      var createTablePortion = sb.ToString();

      var totalCreateScript = WrapInExistingTableHandling(ifExists, dbTableName, createTablePortion);
      return totalCreateScript;
   }

   public static string GenerateCreateTableScript(
      SISTableDefinition tableDefinition,
      CreateIfExistsModification       createIfExistsModification = CreateIfExistsModification.CreateAnyway)
   {
      var informationSchemaTable = tableDefinition.InformationSchemaTable;

      if (informationSchemaTable.TABLE_TYPE != TableTypes.BaseTable &&
          informationSchemaTable.TABLE_TYPE != TableTypes.View)
      {
         throw new NotSupportedException(
            $"Table is of a type other than 'Base Table' or 'View'.  Table type is {tableDefinition.InformationSchemaTable.TABLE_TYPE}");
      }

      switch (tableDefinition.InformationSchemaTable.TABLE_TYPE)
      {
         case TableTypes.View:
            throw new NotImplementedException("Have not implemented View creation yet.");
         case TableTypes.BaseTable:
            var schema    = informationSchemaTable.TABLE_SCHEMA;
            var tableName = informationSchemaTable.TABLE_NAME;
            var columns   = tableDefinition.InformationSchemaColumns;
            var script    = CreateTableScript(schema, tableName, columns, createIfExistsModification);
            return script;
         default:
            throw new ArgumentOutOfRangeException();
      }
   }

   public static string WrapInExistingTableHandling(CreateIfExistsModification createIfExistsModification,
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

         case CreateIfExistsModification.CreateIfNotExists:
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

   private static void AddColumnDefinitions(
      StringBuilder    sb,
      IList<SISColumn> informationSchemaColumns)
   {
      var internalSb         = new StringBuilder();
      var maxColumnNameWidth = informationSchemaColumns.Max(x => x.COLUMN_NAME.Length) + 2;
      var maxDataTypeWidth   = informationSchemaColumns.Select(GetStringForDataType).Max(x => x.Length);

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
            column.COLUMN_NAME.BracketizeSafe(),
            GetStringForDataType(column).ToUpper(),
            NullNotNullToString(column));
      }
   }

   private static string AlignLeft(int positionNumber, int alignmentLength)
   {
      return $"{{{positionNumber},-{alignmentLength}}}";
   }

   private static string GetStringForDataType(SISColumn sisColumn)
   {
      SqlType sqlType = SqlTypeFromSISColumn.FromInformationSchemaColumn(sisColumn);

      if (IsTextualDataType(sisColumn))
      {
         return GetStringForTextualDataType(sisColumn);
      }

      if (IsPrecisionNumber(sisColumn))
      {
         return GetStringForPrecisionNumberDataType(sisColumn);
      }

      return sisColumn.DATA_TYPE;
   }

   private static bool IsTextualDataType(SISColumn sisColumn)
   {
      var dt = sisColumn.DATA_TYPE;
      var isTextualDataType = dt.StartsWith("nv") || dt.StartsWith("nch") || dt.StartsWith("varc") ||
                              dt.StartsWith("char");
      return isTextualDataType;
   }

   private static string GetStringForTextualDataType(SISColumn sisColumn)
   {
      var textDataTypeLength = sisColumn.CHARACTER_MAXIMUM_LENGTH == -1
         ? "MAX"
         : sisColumn.CHARACTER_MAXIMUM_LENGTH.ToString();

      var text = string.Format("{0} ({1})",
         sisColumn.DATA_TYPE,
         textDataTypeLength);
      return text;
   }

   private static bool IsPrecisionNumber(SISColumn sisColumn)
   {
      var dt                = sisColumn.DATA_TYPE;
      var isPrecisionNumber = dt.StartsWith("doub") || dt.StartsWith("flo") || dt.StartsWith("deci");
      return isPrecisionNumber;
   }

   private static string GetStringForPrecisionNumberDataType(SISColumn sisColumn)
   {
      if (sisColumn.DATA_TYPE.InsensitiveEquals("float"))
      {
         var text2 = string.Format("{0} ({1})",
            sisColumn.DATA_TYPE,
            sisColumn.NUMERIC_PRECISION);
         return text2;
      }

      var text = string.Format("{0} ({1}, {2})",
         sisColumn.DATA_TYPE,
         sisColumn.NUMERIC_PRECISION,
         sisColumn.NUMERIC_SCALE);
      return text;
   }

   private static string NullNotNullToString(SISColumn column)
   {
      return column.IS_NULLABLE.Equals("YES", StringComparison.CurrentCultureIgnoreCase) ? "NULL" : "NOT NULL";
   }
}