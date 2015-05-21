using System.Collections.Generic;
using System.Data;

namespace UniqueDb.ConnectionProvider.DataGeneration.CSharpGeneration
{
    public static class CSharpPropertyFactoryFromAdoSchemaTableDataColumn
    {
        public static CSharpProperty ToCSharpProperty(DataColumn column)
        {
            var cSharpProperty = new CSharpProperty();
            cSharpProperty.Name = column.ColumnName;
            cSharpProperty.IsNullable = column.AllowDBNull;
            cSharpProperty.ClrAccessModifier = ClrAccessModifier.Public;
            cSharpProperty.DataType = ConvertDataColumnClrTypeNameToString(column);

            cSharpProperty.DataAnnotationDefinitionBases.AddRange(CreateDataAnnotations(column));
            return cSharpProperty;
        }

        private static IEnumerable<DataAnnotationDefinitionBase> CreateDataAnnotations(DataColumn column)
        {
            if (column.MaxLength > 0)
            {
                yield return new DataAnnotationDefinitionMaxCharacterLength(column.MaxLength);
            }
        }

        private static string ConvertDataColumnClrTypeNameToString(DataColumn column)
        {
            if (column.DataType.Name == "String")
            {
                return "string";
            }
            if (column.DataType.Name == "Boolean")
            {
                return "bool";
            }
            if (column.DataType.Name == "Int32")
            {
                return "int";
            }
            if (column.DataType.Name == "Int16")
            {
                return "short";
            }
            return column.DataType.Name;
        }
    }
}