using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using FluentAssertions;

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
            return DataAnnotationFactory.CreateDataAnnotations(sqlColumn);
        }
    }

    public class DataAnnotationFactory
    {
        public static IEnumerable<DataAnnotationDefinitionBase> CreateDataAnnotations(SqlColumn sqlColumn)
        {
            if (NeedsNumericRange(sqlColumn))
            {
                var numericType = sqlColumn.SqlDataType.As<SqlTypeNumberBase>();
                yield return
                    DataAnnotationDefinitionNumericRange.FromRange(numericType.LowerBound, numericType.UpperBound);
            }

            if (NeedsCharRange(sqlColumn))
            {
                yield return new DataAnnotationDefinitionMaxCharacterLength(sqlColumn.SqlDataType.MaximumCharLength.Value);
            }
        }

        private static bool NeedsNumericRange(SqlColumn sqlColumn)
        {
            var numericSqlColumn = sqlColumn.SqlDataType as SqlTypeNumberBase;
            return numericSqlColumn != null;
        }

        private static bool NeedsCharRange(SqlColumn sqlColumn)
        {
            return sqlColumn.SqlDataType.MaximumCharLength.HasValue;
        }
        
    }

    public class DataAnnotationDefinitionNumericRange : DataAnnotationDefinitionBase
    {
        private readonly int _numericPrecision;
        private readonly int _numericScale;
        private NumericRange numericRange;

        public decimal UpperBound => numericRange.UpperBound;
        public decimal LowerBound => numericRange.LowerBound;

        private DataAnnotationDefinitionNumericRange()
        {
        }

        public static DataAnnotationDefinitionNumericRange FromRange(decimal lowerBound, decimal upperBound)
        {
            return new DataAnnotationDefinitionNumericRange() {numericRange = new NumericRange(lowerBound, upperBound)};
        }

        public override string ToAttributeString()
        {
            return $"[Range({LowerBound}, {UpperBound})]";
        }

        
    }

    public static class DecimalTypeRangeCalculator
    {
        public static NumericRange CalculateRange(int numericPrecision, int? numericScale)
        {
            int numericScaleNotNull = numericScale ?? 0;
            try
            {
                var size = numericPrecision - numericScaleNotNull;
                var upperBoundString = "9".Repeat(size) + "." + "9".Repeat(numericScaleNotNull);
                var upperBound = decimal.Parse(upperBoundString);
                var lowerBound = -upperBound;
                return new NumericRange(lowerBound, upperBound);

            }
            catch (OverflowException e)
            {
                Console.WriteLine(e);
                Debugger.Break();
                return new NumericRange(Decimal.MinusOne*Decimal.MaxValue, Decimal.MaxValue);
            }
        }
    }

    public class GenericRange<T>
    {
        public T LowerBound { get; set; }
        public T UpperBound { get; set; }

        public GenericRange(T lowerBound, T upperBound)
        {
            LowerBound = lowerBound;
            UpperBound = upperBound;
        } 
    }

    public class NumericRange : GenericRange<decimal>
    {
        public NumericRange(decimal lowerBound, decimal upperBound) : base(lowerBound, upperBound)
        {
        }
    }

    public class DateRange : GenericRange<DateTime>
    {
        public DateRange(DateTime lowerBound, DateTime upperBound) : base(lowerBound, upperBound)
        {
        }
    }
}