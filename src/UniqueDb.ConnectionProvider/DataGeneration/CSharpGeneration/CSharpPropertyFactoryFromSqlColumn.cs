using System;
using System.Collections.Generic;
using System.Linq;

namespace UniqueDb.ConnectionProvider.DataGeneration
{
    public static class CSharpPropertyFactoryFromSqlColumn
    {
        public static CSharpProperty ToCSharpProperty(SqlColumn sqlColumn)
        {
            var cSharpProperty = new CSharpProperty();

            var propertyName = GetNameWithRewriting(sqlColumn.Name);
            cSharpProperty.Name = propertyName;
            cSharpProperty.ClrAccessModifier = ClrAccessModifier.Public;
            cSharpProperty.IsNullable = sqlColumn.IsNullable;

            cSharpProperty.DataType = SqlTypes.IsSystemType(sqlColumn.SqlDataType.TypeName)
                ? SqlTypeStringToClrTypeStringConverter.GetClrDataType(sqlColumn.SqlDataType.TypeName)
                : sqlColumn.SqlDataType.TypeName;
                
            
           cSharpProperty.DataAnnotationDefinitionBases.AddRange(GetDataAnnotations(sqlColumn));

            return cSharpProperty;
        }

        private static IEnumerable<DataAnnotationDefinitionBase> GetDataAnnotations(SqlColumn sqlColumn)
        {
            if (sqlColumn.SqlDataType.MaximumCharLength > 0
                && SqlTypes.IsCharType(sqlColumn.SqlDataType.TypeName))
            {
                yield return new DataAnnotationDefinitionMaxCharacterLength(sqlColumn.SqlDataType.MaximumCharLength.Value);
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