namespace UniqueDb.ConnectionProvider.DataGeneration.CSharpGeneration
{
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
}