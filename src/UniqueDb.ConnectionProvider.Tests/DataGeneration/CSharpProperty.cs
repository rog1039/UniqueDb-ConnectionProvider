namespace UniqueDb.ConnectionProvider.Tests.DataGeneration
{
    public class CSharpProperty
    {
        public ClrAccessModifier ClrAccessModifier { get; set; }
        public string DataType { get; set; }
        public string Name { get; set; }
        public bool IsNullable { get; set; }
        public ClrAccessModifier GetterClrAccessModifier { get; set; }
        public ClrAccessModifier SetterClrAccessModifier { get; set; }

        public override string ToString()
        {
            return string.Format(@"{0} {1} {2} {{{3} get;{4} set; }}",
                ClrAccessModifier.ToString().ToLower(),
                DataType,
                Name,
                GetterClrAccessModifier != ClrAccessModifier.Public ? " " + GetterClrAccessModifier.ToString().ToLower() : string.Empty,
                SetterClrAccessModifier != ClrAccessModifier.Public ? " " + SetterClrAccessModifier.ToString().ToLower() : string.Empty);
        }
    }
}