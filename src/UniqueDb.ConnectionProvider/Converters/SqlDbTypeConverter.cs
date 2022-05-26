using System.Data;
using UniqueDb.ConnectionProvider.DataGeneration;
using UniqueDb.ConnectionProvider.DataGeneration.SqlMetadata;

namespace UniqueDb.ConnectionProvider.Converters;

public static class SqlDbTypeConverter
{
    public static SqlDbType ConvertInformationSchemaColumnToSqlDbType(SISColumn col)
    {
        var sqlType = SqlTypeConverter.FromInformationSchemaColumn(col);
        return ConvertSqlTypeToSqlDbType(sqlType);
    }

    public static SqlDbType ConvertSqlTypeToSqlDbType(SqlType type)
    {
        return ParseSqlType(type.TypeName);
    }

    public static SqlDbType ParseSqlType(string typeName)
    {
        typeName = typeName.Split(new[] {"("}, StringSplitOptions.None)[0];
        return typeName switch
        {
            "bigint"           => SqlDbType.BigInt,
            "binary"           => SqlDbType.Binary,
            "bit"              => SqlDbType.Bit,
            "char"             => SqlDbType.Char,
            "date"             => SqlDbType.Date,
            "datetime"         => SqlDbType.DateTime,
            "datetime2"        => SqlDbType.DateTime2,
            "datetimeoffset"   => SqlDbType.DateTimeOffset,
            "decimal"          => SqlDbType.Decimal,
            "float"            => SqlDbType.Float,
            "image"            => SqlDbType.Image,
            "int"              => SqlDbType.Int,
            "money"            => SqlDbType.Money,
            "nchar"            => SqlDbType.NChar,
            "ntext"            => SqlDbType.NText,
            "nvarchar"         => SqlDbType.NVarChar,
            "real"             => SqlDbType.Real,
            "smalldatetime"    => SqlDbType.SmallDateTime,
            "smallint"         => SqlDbType.SmallInt,
            "smallmoney"       => SqlDbType.SmallMoney,
            "structured"       => SqlDbType.Structured,
            "text"             => SqlDbType.Text,
            "time"             => SqlDbType.Time,
            "timestamp"        => SqlDbType.Timestamp,
            "tinyint"          => SqlDbType.TinyInt,
            "udt"              => SqlDbType.Udt,
            "uniqueidentifier" => SqlDbType.UniqueIdentifier,
            "varbinary"        => SqlDbType.VarBinary,
            "varchar"          => SqlDbType.VarChar,
            "variant"          => SqlDbType.Variant,
            "xml"              => SqlDbType.Xml,
            _                  => throw new InvalidDataException($"No conversion found for type: {typeName}")
        };
    }
}