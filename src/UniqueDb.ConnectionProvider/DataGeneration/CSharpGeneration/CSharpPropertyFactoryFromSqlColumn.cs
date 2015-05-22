using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using FluentAssertions;

namespace UniqueDb.ConnectionProvider.DataGeneration.CSharpGeneration
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
                var maximumCharLength = sqlColumn.SqlDataType.TypeName.StartsWith("n")
                    ? sqlColumn.SqlDataType.MaximumCharLength.Value/2
                    : sqlColumn.SqlDataType.MaximumCharLength.Value;

                yield return new DataAnnotationDefinitionMaxCharacterLength(maximumCharLength);
            }

            if (sqlColumn.IsComputed == true)
            {
                yield return new DataAnnotationDefinitionIsComputed();
            }
            if (sqlColumn.IsIdentity == true)
            {
                yield return new DataAnnotationDefinitionIsIdentity();
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

    public class DataAnnotationDefinitionIsIdentity : DataAnnotationDefinitionBase
    {
        public override string ToAttributeString()
        {
            return "[DatabaseGenerated(DatabaseGeneratedOption.Identity)]";
        }
    }

    public class DataAnnotationDefinitionIsComputed : DataAnnotationDefinitionBase
    {
        public override string ToAttributeString()
        {
            return "[DatabaseGenerated(DatabaseGeneratedOption.Computed)]";
        }
    }

    public class DataAnnotationDefinitionNumericRange : DataAnnotationDefinitionBase
    {
        private readonly int _numericPrecision;
        private readonly int _numericScale;
        private NumericRange numericRange;

        public double UpperBound => numericRange.UpperBound;
        public double LowerBound => numericRange.LowerBound;

        private DataAnnotationDefinitionNumericRange()
        {
        }

        public static DataAnnotationDefinitionNumericRange FromRange(double lowerBound, double upperBound)
        {
            return new DataAnnotationDefinitionNumericRange() {numericRange = new NumericRange(lowerBound, upperBound)};
        }

        public override string ToAttributeString()
        {
            return $"[Range({LowerBound}, {UpperBound})]";
        }

        
    }

    public static class DoubleTypeRangeCalculator
    {
        public static NumericRange CalculateRange(int numericPrecision, int? numericScale)
        {
            int numericScaleNotNull = numericScale ?? 0;
            try
            {
                var size = numericPrecision - numericScaleNotNull;
                var upperBoundString = "9".Repeat(size) + "." + "9".Repeat(numericScaleNotNull);
                
                var upperBound = double.Parse(upperBoundString);
                var lowerBound = -upperBound;
                return new NumericRange(lowerBound, upperBound);

            }
            catch (OverflowException e)
            {
                Console.WriteLine(e);
                Debugger.Break();
                return new NumericRange(double.MinValue, double.MaxValue);
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

    public class NumericRange : GenericRange<double>
    {
        public NumericRange(double lowerBound, double upperBound) : base(lowerBound, upperBound)
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