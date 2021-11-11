using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using UniqueDb.ConnectionProvider.DataGeneration;
using Xunit;
using Xunit.Abstractions;

namespace UniqueDb.ConnectionProvider.Tests;

public class GenerateAttributeCheckingCode : UnitTestBaseWithConsoleRedirection
{
    [Fact()]
    [Trait("Category", "Instant")]
    public void GenerateCode()
    {
        var assembly = typeof(NotMappedAttribute).Assembly;
        var assemblyTypes = assembly
            .GetExportedTypes()
            .Where(z => z.Name.EndsWith("Attribute"))
            .ToList();
        var constTemplate = "public const string <name>FullName = @\"<fullName>\";";
        var methodTemplate =
            "public static bool Is<attributeName>(this Type type) => type.IsAttributeType(<attributeName>FullName);";


        var generatedCodeObjects = assemblyTypes
            .Select(t => new
            {
                constDef = constTemplate
                    .Replace("<name>",     t.Name)
                    .Replace("<fullName>", t.FullName),
                methodDef = methodTemplate.Replace("<attributeName>", t.Name)
            })
            .ToList();
        generatedCodeObjects.PrintStringTable();

        Console.WriteLine(string.Join(Environment.NewLine, generatedCodeObjects.Select(z => z.constDef)));
        Console.WriteLine(string.Join(Environment.NewLine, generatedCodeObjects.Select(z => z.methodDef)));
    }

    public GenerateAttributeCheckingCode(ITestOutputHelper outputHelperHelper) : base(outputHelperHelper) { }
}