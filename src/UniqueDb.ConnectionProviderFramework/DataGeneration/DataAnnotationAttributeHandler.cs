using System.ComponentModel.DataAnnotations;

namespace UniqueDb.ConnectionProvider.DataGeneration
{
    public static class DataAnnotationAttributeHandler
    {
        public static void ProcessAttribute(NullableSqlType nullableSqlType, object attribute)
        {
            HandleStringLengthAnnotation(nullableSqlType, attribute);
        }

        private static void HandleStringLengthAnnotation(NullableSqlType nullableSqlType, object attribute)
        {
            if (!(attribute is StringLengthAttribute))
                return;
            var stringLengthAttribute = (StringLengthAttribute) attribute;
            nullableSqlType.SqlType.MaximumCharLength = stringLengthAttribute.MaximumLength;
        }
    }
}