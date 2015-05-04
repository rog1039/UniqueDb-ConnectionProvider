using System.Data;

namespace UniqueDb.ConnectionProvider.DataGeneration
{
    public static class DataColumnToCSharpPropertyGenerator
    {
        public static CSharpProperty ToCSharpProperty(DataColumn column)
        {
            var cSharpProperty = new CSharpProperty();
            cSharpProperty.Name = column.ColumnName;
            cSharpProperty.ClrAccessModifier = ClrAccessModifier.Public;
            cSharpProperty.DataType = ConvertDataColumnClrTypeNameToString(column);
            return cSharpProperty;
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