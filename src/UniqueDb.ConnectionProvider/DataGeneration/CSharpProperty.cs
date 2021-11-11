using UniqueDb.ConnectionProvider.DataGeneration.CSharpGeneration;

namespace UniqueDb.ConnectionProvider.DataGeneration;

public class CSharpProperty
{
    public ClrAccessModifier ClrAccessModifier { get; set; }
    public string DataType { get; set; }
    public string Name { get; set; }
    public bool IsNullable { get; set; }
    public ClrAccessModifier GetterClrAccessModifier { get; set; }
    public ClrAccessModifier SetterClrAccessModifier { get; set; }
    public IList<DataAnnotationDefinitionBase> DataAnnotationDefinitionBases { get; set; } = new List<DataAnnotationDefinitionBase>();


    public override string ToString()
    {
        var generator = new CSharpPropertyTextGenerator(this);
        return generator.Generate();
    }

    public string ToString(CSharpClassTextGeneratorOptions options)
    {
        var generator = new CSharpPropertyTextGenerator(this, options);
        return generator.Generate();
    }
}