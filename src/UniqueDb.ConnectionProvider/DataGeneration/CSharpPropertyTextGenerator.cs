using System.Collections.Generic;
using System.Linq;

namespace UniqueDb.ConnectionProvider.DataGeneration
{
    public class CSharpPropertyTextGenerator
    {
        private readonly CSharpProperty Property;

        public static List<string> typesThatCanBeNullable = new List<string>() { "int", "Int", "Int16", "Int32", "Int64", "DateTime", "bool", "double", "decimal" };


        public CSharpPropertyTextGenerator(CSharpProperty property)
        {
            Property = property;
        }

        public string Generate()
        {
            return
                $"{AttributeText}    {AccessModifier} {DataTypeString} {PropertyName} {{{GetterAccessModifier} get;{SetterAccessModifier} set; }}";
        }


        private string AttributeText => Property.DataAnnotationDefinitionBases.Aggregate(string.Empty, (s, @base) => s+"    "+@base.ToAttributeString()+"\r\n" );

        private string AccessModifier => Property.ClrAccessModifier.ToString().ToLower();

        public string DataTypeString => !Property.IsNullable || !typesThatCanBeNullable.Contains(Property.DataType)
            ? Property.DataType
            : Property.DataType + "?";

        public string PropertyName => Property.Name;

        public string GetterAccessModifier
            =>
                Property.GetterClrAccessModifier != ClrAccessModifier.Public
                    ? " " + Property.GetterClrAccessModifier.ToString().ToLower()
                    : string.Empty;

        public string SetterAccessModifier
            =>
                Property.SetterClrAccessModifier != ClrAccessModifier.Public
                    ? " " + Property.SetterClrAccessModifier.ToString().ToLower()
                    : string.Empty;

    }
}