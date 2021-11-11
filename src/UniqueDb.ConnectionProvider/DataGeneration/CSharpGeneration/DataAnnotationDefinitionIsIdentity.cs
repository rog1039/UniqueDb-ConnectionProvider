namespace UniqueDb.ConnectionProvider.DataGeneration.CSharpGeneration;

public class DataAnnotationDefinitionIsIdentity : DataAnnotationDefinitionBase
{
    public override string ToAttributeString()
    {
        return "[DatabaseGenerated(DatabaseGeneratedOption.Identity)]";
    }
}