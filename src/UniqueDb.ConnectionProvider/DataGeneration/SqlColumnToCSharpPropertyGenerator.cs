using System;
using System.Collections.Generic;
using System.Linq;

namespace UniqueDb.ConnectionProvider.DataGeneration
{
    public static class SchemaColumnToCSharpPropertyConverter
    {
        public static CSharpProperty ToCSharpProperty(InformationSchemaColumn schemaColumn)
        {
            var property = new CSharpProperty();
            property.Name = GetNameWithRewriting(schemaColumn.COLUMN_NAME);
            property.ClrAccessModifier = ClrAccessModifier.Public;
            property.DataType = SqlTypeStringToClrTypeStringConverter.GetClrDataType(schemaColumn.DATA_TYPE, schemaColumn.IS_NULLABLE == "YES");
            property.IsNullable = schemaColumn.IS_NULLABLE == "YES";
            return property;
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

    public static class SqlColumnToCSharpPropertyGenerator
    {
        public static CSharpProperty ToCSharpProperty(SqlColumn tableColumnDto)
        {
            var cSharpProperty = new CSharpProperty();

            var propertyName = GetNameWithRewriting(tableColumnDto.Name);
            cSharpProperty.Name = propertyName;
            cSharpProperty.ClrAccessModifier = ClrAccessModifier.Public;
            //cSharpProperty.DataType = SqlTypeStringToClrTypeStringConverter.GetClrDataType(tableColumnDto);
            cSharpProperty.DataType = "string";
            return cSharpProperty;
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