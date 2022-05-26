namespace UniqueDb.ConnectionProvider.DataGeneration;

public static class AmbiguousSqlTypeToSqlTypeConverter
{
   public static SqlType Convert(AmbiguousSqlType ambiguousSqlType)
   {
      var typeName = ambiguousSqlType.TypeName;
      return typeName switch
      {
         "int"        => SqlTypeFactory.Int(),
         "tinyint"    => SqlTypeFactory.TinyInt(),
         "smallint"   => SqlTypeFactory.SmallInt(),
         "decimal"    => SqlTypeFactory.Decimal(ambiguousSqlType.NumericPrecision.Value, ambiguousSqlType.NumericScale),
         "numeric"    => SqlTypeFactory.Numeric(ambiguousSqlType.NumericPrecision.Value, ambiguousSqlType.NumericScale),
         "float"      => SqlTypeFactory.Float(ambiguousSqlType.NumericPrecision.Value),
         "real"       => SqlTypeFactory.Real(),
         "money"      => SqlTypeFactory.Money(),
         "smallmoney" => SqlTypeFactory.SmallMoney(),
         "date"       => SqlTypeFactory.Date(),
         "time"       => SqlTypeFactory.Time(),
         "datetime"   => SqlTypeFactory.DateTime(),
         "datetime2"  => SqlTypeFactory.DateTime2(ambiguousSqlType.FractionalSecondsPrecision),
         "char"       => SqlTypeFactory.Char(SqlTextLengthFactory.FromAmbiguousSqlType(ambiguousSqlType)),
         "varchar"    => SqlTypeFactory.VarChar(SqlTextLengthFactory.FromAmbiguousSqlType(ambiguousSqlType)),
         "nchar"      => SqlTypeFactory.NChar(SqlTextLengthFactory.FromAmbiguousSqlType(ambiguousSqlType)),
         "nvarchar"   => SqlTypeFactory.NVarChar(SqlTextLengthFactory.FromAmbiguousSqlType(ambiguousSqlType)),
         "xml"        => SqlTypeFactory.Xml(),
         _            => SqlType.Type(typeName)
      };
   }
}