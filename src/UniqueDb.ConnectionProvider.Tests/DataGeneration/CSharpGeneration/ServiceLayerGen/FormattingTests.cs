using System;
using System.IO;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Formatting;
using Microsoft.CodeAnalysis.Formatting;
using Microsoft.CodeAnalysis.Options;
using Microsoft.CodeAnalysis.Text;
using Xunit;
using Xunit.Abstractions;

namespace UniqueDb.ConnectionProvider.Tests.DataGeneration.CSharpGeneration.ServiceLayerGen
{
    public class FormattingTests : UnitTestBaseWithConsoleRedirection
    {
        public static string ClassText = "namespace MyNamespace{public class Hi{public Hi(){}}}";

        [Fact()]
        [Trait("Category", "Instant")]
        public async Task TestFormat()
        {
            var _ = typeof(CSharpFormattingOptions);

            var sourceText     = SourceText.From(ClassText);
            var adhocWorkspace = new AdhocWorkspace();
            // var solution       = adhocWorkspace.CurrentSolution;
            // var project        = solution.AddProject("MyProj", "MyProjAss", LanguageNames.CSharp);
            // var parseOptions   = new CSharpParseOptions();
            var parseTree      = CSharpSyntaxTree.ParseText(sourceText);
            var optionSet      = adhocWorkspace.Options;
            optionSet = optionSet.WithChangedOption(CSharpFormattingOptions.NewLinesForBracesInMethods,    true);
            optionSet = optionSet.WithChangedOption(CSharpFormattingOptions.NewLinesForBracesInProperties, true);
            optionSet = optionSet.WithChangedOption(CSharpFormattingOptions.NewLinesForBracesInTypes,      true);
            optionSet = optionSet.WithChangedOption(CSharpFormattingOptions.WrappingPreserveSingleLine,    false);
            var formatted = Formatter.Format(parseTree.GetRoot(), adhocWorkspace, optionSet);
            var newText   = formatted.GetText();

            Console.WriteLine(newText.ToString());


            // var doc = project.AddDocument("Test.cs", sourceText);
            // doc = doc.WithText(sourceText);
            // var formattedDoc = await Formatter.FormatAsync(doc);
            // formattedDoc.TryGetText(out var sourceTextFormatted);
            // Console.WriteLine(sourceTextFormatted.ToString());
        }

        public FormattingTests(ITestOutputHelper outputHelperHelper) : base(outputHelperHelper) { }
    }

    public static class CustomFormattingEngineForText
    {
        public static string Format(string code, Func<OptionSet, OptionSet> formattingOptions = default)
        {
            formattingOptions ??= StandardFormattingRules;

            var sourceText     = SourceText.From(code);
            var adhocWorkspace = new AdhocWorkspace();
            var parseTree      = CSharpSyntaxTree.ParseText(sourceText);
            
            var optionSet      = adhocWorkspace.Options;
            optionSet = formattingOptions(optionSet);
            
            var formatted = Formatter.Format(parseTree.GetRoot(), adhocWorkspace, optionSet);
            var newText   = formatted.GetText();
            return newText.ToString();
        }

        private static OptionSet StandardFormattingRules(OptionSet optionSet)
        {
            optionSet = optionSet.WithChangedOption(CSharpFormattingOptions.NewLinesForBracesInMethods,    true);
            optionSet = optionSet.WithChangedOption(CSharpFormattingOptions.NewLinesForBracesInProperties, true);
            optionSet = optionSet.WithChangedOption(CSharpFormattingOptions.NewLinesForBracesInTypes,      true);
            optionSet = optionSet.WithChangedOption(CSharpFormattingOptions.WrappingPreserveSingleLine,    false);
            return optionSet;
        }
    }
}