namespace UniqueDb.ConnectionProvider.CSharpGeneration;

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