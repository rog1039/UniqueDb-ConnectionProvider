using UniqueDb.ConnectionProvider.CoreTypes;

namespace UniqueDb.ConnectionProvider.Converters;

public static class SqlTypeFromSpecification
{
   public static SqlType Convert(SqlTypeSpecification sqlTypeSpecification)
   {
      var typeName = sqlTypeSpecification.TypeName;
      return typeName switch
      {
         "int"        => SqlTypeFactory.Int(),
         "tinyint"    => SqlTypeFactory.TinyInt(),
         "smallint"   => SqlTypeFactory.SmallInt(),
         "decimal"    => SqlTypeFactory.Decimal(sqlTypeSpecification.NumericPrecision.Value, sqlTypeSpecification.NumericScale),
         "numeric"    => SqlTypeFactory.Numeric(sqlTypeSpecification.NumericPrecision.Value, sqlTypeSpecification.NumericScale),
         "float"      => SqlTypeFactory.Float(sqlTypeSpecification.NumericPrecision.Value),
         "real"       => SqlTypeFactory.Real(),
         "money"      => SqlTypeFactory.Money(),
         "smallmoney" => SqlTypeFactory.SmallMoney(),
         "date"       => SqlTypeFactory.Date(),
         "time"       => SqlTypeFactory.Time(),
         "datetime"   => SqlTypeFactory.DateTime(),
         "datetime2"  => SqlTypeFactory.DateTime2(sqlTypeSpecification.FractionalSecondsPrecision),
         "char"       => SqlTypeFactory.Char(SqlTextLengthFactory.FromSqlTypeSpecification(sqlTypeSpecification)),
         "varchar"    => SqlTypeFactory.VarChar(SqlTextLengthFactory.FromSqlTypeSpecification(sqlTypeSpecification)),
         "nchar"      => SqlTypeFactory.NChar(SqlTextLengthFactory.FromSqlTypeSpecification(sqlTypeSpecification)),
         "nvarchar"   => SqlTypeFactory.NVarChar(SqlTextLengthFactory.FromSqlTypeSpecification(sqlTypeSpecification)),
         "xml"        => SqlTypeFactory.Xml(),
         _            => SqlType.Type(typeName)
      };
   }
}