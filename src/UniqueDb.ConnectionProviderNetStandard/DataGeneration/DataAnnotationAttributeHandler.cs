using System.ComponentModel.DataAnnotations;
using UniqueDb.ConnectionProviderNetStandard;

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
            if (attribute is StringLengthAttribute stringLengthAttribute)
            {
                nullableSqlType.SqlType.MaximumCharLength = stringLengthAttribute.MaximumLength;
            }
        }
    }
}