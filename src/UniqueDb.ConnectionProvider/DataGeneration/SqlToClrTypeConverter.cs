using System.Xml.Linq;

namespace UniqueDb.ConnectionProvider.DataGeneration;

public static class SqlToClrTypeConverter
{
   public static bool UseNativeSqlTypes = true;

   public static Type GetClrType(string sqlDataType)
   {
      if (string.IsNullOrWhiteSpace(sqlDataType))
         throw new ArgumentException("Value cannot be null or whitespace.", nameof(sqlDataType));

      var type = ConvertSqlTypeNameToClrType(sqlDataType);
      return type;
   }

   public static string GetClrTypeName(string sqlDataType)
   {
      if (string.IsNullOrWhiteSpace(sqlDataType))
         throw new ArgumentException("Value cannot be null or whitespace.", nameof(sqlDataType));

      sqlDataType = StripParensFromTypeName(sqlDataType);

      return sqlDataType switch
      {
         "int"              => "int",
         "tinyint"          => "byte",
         "varchar"          => "string",
         "datetime"         => "DateTime",
         "smalldatetime"    => "DateTime",
         "date"             => "DateTime",
         "time"             => "TimeSpan",
         "decimal"          => "decimal",
         "bigint"           => "Int64",
         "timestamp"        => "byte[]",
         "sql_variant"      => "object",
         "text"             => "string",
         "bit"              => "bool",
         "nvarchar"         => "string",
         "ntext"            => "string",
         "float"            => "double",
         "real"             => "double",
         "nchar"            => "string",
         "image"            => "byte[]",
         "uniqueidentifier" => "Guid",
         "money"            => "decimal",
         "smallint"         => "Int16",
         "smallmoney"       => "decimal",
         "varbinary"        => "byte[]",
         "meric"            => "decimal",
         "binary"           => "byte[]",
         "char"             => "string",
         "datetime2"        => "DateTime",
         "datetimeoffset"   => "DateTimeOffset",
         "sysname"          => "string",
         "xml"              => "XElement",

         "geography" =>
            //Could perhaps use SqlGeography and/or DbGeography here rather than byte[].
            //See http://stackoverflow.com/questions/23186832/entity-framework-sqlgeography-vs-dbgeography
            //and http://stackoverflow.com/questions/15107977/sql-geography-to-dbgeography/29200641#29200641
            UseNativeSqlTypes ? "SqlGeography" : "byte[]",
         "geometry" => UseNativeSqlTypes ? "SqlGeometry" : "byte[]",
         "hierarchyid" =>
            //Perhaps we could do something else here??
            //See http://blogs.msdn.com/b/jimoneil/archive/2009/02/23/h-is-for-hierarchyid.aspx
            UseNativeSqlTypes ? "SqlHierarchyId" : "byte[]",

         _ => throw new NotImplementedException(
            $"SQL column type {sqlDataType} cannot be translated to a C# property type")
      };
   }

   private static string StripParensFromTypeName(string sqlDataType)
   {
      /*
       * Want to also be able to handle cases with more info: decimal(32,8) as decimal
       */
      var parenIndex = sqlDataType.IndexOf("(", StringComparison.Ordinal);
      if (parenIndex > 0)
         sqlDataType = sqlDataType.Substring(0, parenIndex);

      return sqlDataType;
   }

   private static Type ConvertSqlTypeNameToClrType(string sqlDataType)
   {
      if (string.IsNullOrWhiteSpace(sqlDataType))
         throw new ArgumentException("Value cannot be null or whitespace.", nameof(sqlDataType));

      sqlDataType = StripParensFromTypeName(sqlDataType);

      return sqlDataType switch
      {
         "int"              => typeof(int),
         "tinyint"          => typeof(byte),
         "varchar"          => typeof(string),
         "datetime"         => typeof(DateTime),
         "smalldatetime"    => typeof(DateTime),
         "date"             => typeof(DateTime),
         "time"             => typeof(TimeSpan),
         "decimal"          => typeof(decimal),
         "bigint"           => typeof(Int64),
         "timestamp"        => typeof(byte[]),
         "sql_variant"      => typeof(object),
         "text"             => typeof(string),
         "bit"              => typeof(bool),
         "nvarchar"         => typeof(string),
         "ntext"            => typeof(string),
         "float"            => typeof(double),
         "real"             => typeof(double),
         "nchar"            => typeof(string),
         "image"            => typeof(byte[]),
         "uniqueidentifier" => typeof(Guid),
         "money"            => typeof(decimal),
         "smallint"         => typeof(Int16),
         "smallmoney"       => typeof(decimal),
         "varbinary"        => typeof(byte[]),
         "meric"            => typeof(decimal),
         "binary"           => typeof(byte[]),
         "char"             => typeof(string),
         "datetime2"        => typeof(DateTime),
         "datetimeoffset"   => typeof(DateTimeOffset),
         "sysname"          => typeof(string),
         "xml"              => typeof(XElement),
         "geography"        => typeof(byte[]),
         "geometry"         => typeof(byte[]),
         "hierarchyid"      => typeof(byte[]),

         _ => throw new NotImplementedException(
            $"SQL column type {sqlDataType} cannot be translated to a C# property type")
      };
   }
}

public class SqlColumnDeclaration
{
   public SqlColumnDeclaration(string name, NullableSqlType nullableSqlType)
   {
      if (string.IsNullOrWhiteSpace(name))
         throw new ArgumentException("Value cannot be null or whitespace.", nameof(name));
      Name            = name;
      NullableSqlType = nullableSqlType ?? throw new ArgumentNullException(nameof(nullableSqlType));
   }

   public string          Name            { get; set; }
   public NullableSqlType NullableSqlType { get; set; }

   public string ToCreateDeclarationSegment()
   {
      return $"{Name} {NullableSqlType}";
   }

   public override string ToString()
   {
      return ToCreateDeclarationSegment();
   }
}

public class NullableSqlType
{
   public NullableSqlType(SqlType sqlType, bool isNullable)
   {
      SqlType    = sqlType ?? throw new ArgumentNullException(nameof(sqlType));
      IsNullable = isNullable;
   }

   public bool    IsNullable { get; set; }
   public SqlType SqlType    { get; set; }

   public override string ToString()
   {
      return IsNullable
         ? SqlType + " NULL"
         : SqlType + " NOT NULL";
   }
}