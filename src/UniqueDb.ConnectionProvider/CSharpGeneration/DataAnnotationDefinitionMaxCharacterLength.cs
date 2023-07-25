namespace UniqueDb.ConnectionProvider.CSharpGeneration;

public class DataAnnotationDefinitionMaxCharacterLength : DataAnnotationDefinitionBase
{
    public DataAnnotationDefinitionMaxCharacterLength(int maxLength)
    {
        MaxLength = maxLength;
    }
        
    public int MaxLength { get; set; }

    public override string ToAttributeString() => string.Format("[StringLength({0})]", MaxLength);
}