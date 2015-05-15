namespace UniqueDb.ConnectionProvider.DataGeneration
{
    public class DataAnnotationDefinitionMaxCharacterLength : DataAnnotationDefinitionBase
    {
        public DataAnnotationDefinitionMaxCharacterLength(int maxLength)
        {
            MaxLength = maxLength;
        }

        public int MaxLength { get; set; }
        public override string ToAttributeString() => string.Format("[MaxLength({0})]", MaxLength);
    }
}