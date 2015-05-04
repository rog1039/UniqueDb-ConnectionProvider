using System.Collections.Generic;

namespace UniqueDb.ConnectionProvider.DataGeneration
{
    public class CSharpProperty
    {
        public ClrAccessModifier ClrAccessModifier { get; set; }
        public string DataType { get; set; }
        public string Name { get; set; }
        public bool IsNullable { get; set; }
        public ClrAccessModifier GetterClrAccessModifier { get; set; }
        public ClrAccessModifier SetterClrAccessModifier { get; set; }


        public static List<string> typesThatCanBeNullable = new List<string>() { "int", "Int", "Int16", "Int32", "Int64", "DateTime", "bool", "double", "decimal" }; 

        public override string ToString()
        {
            return string.Format(@"{0} {1} {2} {{{3} get;{4} set; }}",
                ClrAccessModifier.ToString().ToLower(),
                !IsNullable || !typesThatCanBeNullable.Contains(DataType)
                    ? DataType
                    : DataType + "?",
                Name,
                GetterClrAccessModifier != ClrAccessModifier.Public ? " " + GetterClrAccessModifier.ToString().ToLower() : string.Empty,
                SetterClrAccessModifier != ClrAccessModifier.Public ? " " + SetterClrAccessModifier.ToString().ToLower() : string.Empty);
        }
    }
}