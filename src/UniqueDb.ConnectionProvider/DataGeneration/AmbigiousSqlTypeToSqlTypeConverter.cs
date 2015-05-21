using System;

namespace UniqueDb.ConnectionProvider.DataGeneration
{
    public static class AmbigiousSqlTypeToSqlTypeConverter
    {
        public static SqlType Convert(AmbigiousSqlType ambigiousSqlType)
        {
            var typeName = ambigiousSqlType.TypeName;
            if (typeName == "int")
            {
                return SqlTypeFactory.Int();
            }
            if (typeName == "tinyint")
            {
                return SqlTypeFactory.TinyInt();
            }
            if (typeName == "smallint")
            {
                return SqlTypeFactory.SmallInt();
            }
            if (typeName == "decimal")
            {
                return SqlTypeFactory.Decimal(ambigiousSqlType.NumericPrecision.Value, ambigiousSqlType.NumericScale);
            }
            if (typeName == "numeric")
            {
                return SqlTypeFactory.Numeric(ambigiousSqlType.NumericPrecision.Value, ambigiousSqlType.NumericScale);
            }
            if (typeName == "float")
            {
                return SqlTypeFactory.Float(ambigiousSqlType.NumericPrecision.Value);
            }
            if (typeName == "real")
            {
                return SqlTypeFactory.Real();
            }
            if (typeName == "money")
            {
                return SqlTypeFactory.Money();
            }
            if (typeName == "smallmoney")
            {
                return SqlTypeFactory.SmallMoney();
            }
            if (typeName == "date")
            {
                return SqlTypeFactory.Date();
            }
            if (typeName == "time")
            {
                return SqlTypeFactory.Time();
            }
            if (typeName == "datetime")
            {
                return SqlTypeFactory.DateTime();
            }
            if (typeName == "datetime2")
            {
                return SqlTypeFactory.DateTime2(ambigiousSqlType.FractionalSecondsPrecision);
            }
            if (typeName == "char")
            {
                return SqlTypeFactory.Char(SqlTextualDataTypeOptionsFactory.FromAmbigiousSqlType(ambigiousSqlType));
            }
            if (typeName == "varchar")
            {
                return SqlTypeFactory.VarChar(SqlTextualDataTypeOptionsFactory.FromAmbigiousSqlType(ambigiousSqlType));
            }
            if (typeName == "nchar")
            {
                return SqlTypeFactory.NChar(SqlTextualDataTypeOptionsFactory.FromAmbigiousSqlType(ambigiousSqlType));
            }
            if (typeName == "nvarchar")
            {
                return SqlTypeFactory.NVarChar(SqlTextualDataTypeOptionsFactory.FromAmbigiousSqlType(ambigiousSqlType));
            }
            Console.WriteLine($"This is bad mkay: {typeName}");
            return new SqlType(typeName);
        }
    }

}