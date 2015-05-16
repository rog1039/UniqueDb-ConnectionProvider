using System;
using System.Collections.Generic;
using System.Linq;

namespace UniqueDb.ConnectionProvider.DataGeneration
{
    public static class CSharpPropertyFactoryFromSqlColumn
    {
        public static CSharpProperty ToCSharpProperty(SqlColumn tableColumnDto)
        {
            var cSharpProperty = new CSharpProperty();

            var propertyName = GetNameWithRewriting(tableColumnDto.Name);
            cSharpProperty.Name = propertyName;
            cSharpProperty.ClrAccessModifier = ClrAccessModifier.Public;
            cSharpProperty.DataType = SqlTypeStringToClrTypeStringConverter.GetClrDataType(tableColumnDto.SqlDataType.TypeName);
            cSharpProperty.IsNullable = tableColumnDto.IsNullable;
            
           cSharpProperty.DataAnnotationDefinitionBases.AddRange(GetDataAnnotations(tableColumnDto));

            return cSharpProperty;
        }

        private static IEnumerable<DataAnnotationDefinitionBase> GetDataAnnotations(SqlColumn sqlColumn)
        {
            if (sqlColumn.CharacterMaxLength.HasValue 
                && sqlColumn.CharacterMaxLength > 0 
                && SqlTypes.IsCharType(sqlColumn.SqlDataType.TypeName))
            {
                yield return new DataAnnotationDefinitionMaxCharacterLength(sqlColumn.CharacterMaxLength.Value);
            }
        }

        private static string GetNameWithRewriting(string name)
        {
            var rewriters = AutomaticPropertyNameRewrites.Rewriters.FirstOrDefault(x => x.ShouldRewrite(name));
            
            return rewriters != null 
                ? rewriters.Rewrite(name) 
                : name;
        }
    }
}