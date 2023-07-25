namespace UniqueDb.ConnectionProvider.CSharpGeneration;

public class DataAnnotationDefinitionNumericRange : DataAnnotationDefinitionBase
{
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