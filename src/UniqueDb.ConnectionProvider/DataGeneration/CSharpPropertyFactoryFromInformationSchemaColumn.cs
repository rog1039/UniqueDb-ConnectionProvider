using System.Collections.Generic;
using System.Linq;
using UniqueDb.ConnectionProvider.DataGeneration.SqlMetadata;

namespace UniqueDb.ConnectionProvider.DataGeneration
{
    public static class CSharpPropertyFactoryFromInformationSchemaColumn
    {
        public static CSharpProperty ToCSharpProperty(InformationSchemaColumn schemaColumn)
        {
            var property = new CSharpProperty();
            property.Name = GetNameWithRewriting(schemaColumn.COLUMN_NAME);
            property.ClrAccessModifier = ClrAccessModifier.Public;
            property.DataType = SqlTypeStringToClrTypeStringConverter.GetClrDataType(schemaColumn.DATA_TYPE);
            property.IsNullable = schemaColumn.IS_NULLABLE == "YES";

            property.DataAnnotationDefinitionBases.AddRange(GetDataAnnotations(schemaColumn));

            return property;
        }

        private static IEnumerable<DataAnnotationDefinitionBase> GetDataAnnotations(InformationSchemaColumn schemaColumn)
        {
            if (schemaColumn.CHARACTER_MAXIMUM_LENGTH.HasValue && SqlTypes.IsCharType(schemaColumn.DATA_TYPE))
            {
                var stringLengthDataAnnotationDefinition = new DataAnnotationDefinitionMaxCharacterLength(schemaColumn.CHARACTER_MAXIMUM_LENGTH.Value);
                yield return stringLengthDataAnnotationDefinition;
            }
        }

        private static string GetNameWithRewriting(string name)
        {
            var rewriters = AutomaticPropertyNameRewrites.Rewriters.Where(x => x.ShouldRewrite(name)).FirstOrDefault();
            if (rewriters != null)
            {
                return rewriters.Rewrite(name);
            }
            return name;
        }
    }
}