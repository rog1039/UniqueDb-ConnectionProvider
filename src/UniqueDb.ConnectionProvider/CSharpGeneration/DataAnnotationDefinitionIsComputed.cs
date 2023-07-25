namespace UniqueDb.ConnectionProvider.CSharpGeneration;

public class DataAnnotationDefinitionIsComputed : DataAnnotationDefinitionBase
{
    public override string ToAttributeString()
    {
        return "[DatabaseGenerated(DatabaseGeneratedOption.Computed)]";
    }
}