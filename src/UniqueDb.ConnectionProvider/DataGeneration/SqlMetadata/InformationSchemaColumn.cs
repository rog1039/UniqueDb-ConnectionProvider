namespace UniqueDb.ConnectionProvider.DataGeneration.SqlMetadata;

public class InformationSchemaColumn
{
    public string TABLE_CATALOG            { get; set; }
    public string TABLE_SCHEMA             { get; set; }
    public string TABLE_NAME               { get; set; }
    public string COLUMN_NAME              { get; set; }
    public int    ORDINAL_POSITION         { get; set; }
    public string COLUMN_DEFAULT           { get; set; }
    public string IS_NULLABLE              { get; set; }
    public string DATA_TYPE                { get; set; }
    public int?   CHARACTER_MAXIMUM_LENGTH { get; set; }
    public int?   CHARACTER_OCTET_LENGTH   { get; set; }
    public int?   NUMERIC_PRECISION        { get; set; }
    public int?   NUMERIC_PRECISION_RADIX  { get; set; }
    public int?   NUMERIC_SCALE            { get; set; }
    public int?   DATETIME_PRECISION       { get; set; }
    public string CHARACTER_SET_CATALOG    { get; set; }
    public string CHARACTER_SET_SCHEMA     { get; set; }
    public string CHARACTER_SET_NAME       { get; set; }
    public string COLLATION_CATALOG        { get; set; }
    public string COLLATION_SCHEMA         { get; set; }
    public string COLLATION_NAME           { get; set; }
    public string DOMAIN_CATALOG           { get; set; }
    public string DOMAIN_SCHEMA            { get; set; }
    public string DOMAIN_NAME              { get; set; }

    public string GetSqlDataTypeStringWithNullable()
    {
        return GetSqlDataTypeString() + (IS_NULLABLE== "NO" ? " NOT NULL" : " NULL");
    }

    public string GetSqlDataTypeString()
    {
        switch (DATA_TYPE.ToLower())
        {
            case "numeric":
            case "decimal": return $"{DATA_TYPE}({NUMERIC_PRECISION},{NUMERIC_SCALE})";

            case "real":
            case "float": return $"{DATA_TYPE}({NUMERIC_PRECISION})";


            case "bit":
            case "tinyint":
            case "smallint":
            case "int":
            case "bigint":
                return DATA_TYPE;

            case "date":
            case "datetime":
            case "smalldatetime":
                return DATA_TYPE;

            case "datetime2":
            case "datetimeoffset":
            case "time":
                return $"{DATA_TYPE}({DATETIME_PRECISION})";

            case "char":
            case "varchar":
            case "nchar":
            case "nvarchar":
            {
                var length = (CHARACTER_MAXIMUM_LENGTH == -1 ? "MAX" : CHARACTER_MAXIMUM_LENGTH.ToString());
                return $"{DATA_TYPE}({length})";
            }

            case "xml":
            case "uniqueidentifier":
            case "timestamp":
            case "rowversion":
                return DATA_TYPE;

            case "binary":
            case "varbinary":
            {
                var length = (CHARACTER_MAXIMUM_LENGTH == -1 ? "MAX" : CHARACTER_MAXIMUM_LENGTH.ToString());
                return $"{DATA_TYPE}({length})";
            }

            default:
                throw new InvalidDataException($"No cases defined to translate data type: {DATA_TYPE}.");
        }
    }
}