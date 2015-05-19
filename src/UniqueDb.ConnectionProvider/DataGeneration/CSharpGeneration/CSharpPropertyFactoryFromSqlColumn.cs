using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace UniqueDb.ConnectionProvider.DataGeneration
{
    public static class CSharpPropertyFactoryFromSqlColumn
    {
        public static CSharpProperty ToCSharpProperty(SqlColumn sqlColumn)
        {
            var cSharpProperty = new CSharpProperty();

            var propertyName = AutomaticPropertyNameRewrites.GetNameWithRewriting(sqlColumn.Name);
            cSharpProperty.Name = propertyName;
            cSharpProperty.ClrAccessModifier = ClrAccessModifier.Public;
            cSharpProperty.IsNullable = sqlColumn.IsNullable;

            cSharpProperty.DataType = SqlTypes.IsSystemType(sqlColumn.SqlDataType.TypeName)
                ? SqlTypeStringToClrTypeStringConverter.GetClrDataType(sqlColumn.SqlDataType.TypeName)
                : sqlColumn.SqlDataType.TypeName;
                
            
           cSharpProperty.DataAnnotationDefinitionBases.AddRange(GetDataAnnotations(sqlColumn));

            return cSharpProperty;
        }

        private static IEnumerable<DataAnnotationDefinitionBase> GetDataAnnotations(SqlColumn sqlColumn)
        {
            if (SqlTypes.IsNumeric(sqlColumn.SqlDataType.TypeName) && sqlColumn.SqlDataType.NumericScale.HasValue)
            {
                yield return new DataAnnotationDefinitionNumericRange(
                    sqlColumn.SqlDataType.NumericPrecision.Value,
                    sqlColumn.SqlDataType.NumericScale);
            }
            if (sqlColumn.SqlDataType.MaximumCharLength > 0
                && SqlTypes.IsCharType(sqlColumn.SqlDataType.TypeName))
            {
                yield return new DataAnnotationDefinitionMaxCharacterLength(sqlColumn.SqlDataType.MaximumCharLength.Value);
            }
        }
    }

    public class DataAnnotationDefinitionNumericRange : DataAnnotationDefinitionBase
    {
        private readonly int _numericPrecision;
        private readonly int _numericScale;
        
        public decimal UpperBound { get; private set; }
        public decimal LowerBound { get; private set; }

        public DataAnnotationDefinitionNumericRange(int numericPrecision, int? numericScale)
        {
            _numericPrecision = numericPrecision;
            _numericScale = numericScale ?? 0;

            CalculateRange();
        }

        private void CalculateRange()
        {
            try
            {
                var size = _numericPrecision - _numericScale;
                var upperBoundString = "9".Repeat(size) + "." + "9".Repeat(_numericScale);
                UpperBound = decimal.Parse(upperBoundString);
                LowerBound = -UpperBound;
            }
            catch (OverflowException e)
            {
                UpperBound = Decimal.MaxValue;
                LowerBound = Decimal.MinusOne*Decimal.MaxValue;
                Console.WriteLine(e);
                Debugger.Break();
            }
        }

        public override string ToAttributeString()
        {
            return $"[Range({LowerBound}, {UpperBound})]";
        }
    }
    
}