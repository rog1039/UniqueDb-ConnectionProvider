namespace UniqueDb.ConnectionProvider.DataGeneration
{
    public class DataAnnotationDefinitionString : DataAnnotationDefinitionBase
    {
        public string MaxLength { get; set; }
        public override string ToAttributeString() => string.Format("[MaxLength({0})]", MaxLength);
    }
}