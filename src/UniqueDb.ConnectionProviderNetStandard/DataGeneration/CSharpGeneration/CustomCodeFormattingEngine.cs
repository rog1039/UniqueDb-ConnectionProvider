using System;

namespace UniqueDb.ConnectionProvider.DataGeneration.CSharpGeneration
{
    public static class CustomCodeFormattingEngine
    {
        public static string Format(string code, Func<OptionSet, OptionSet> formattingOptions = default)
        {
            formattingOptions ??= StandardFormattingRules;

            var sourceText = SourceText.From(code);
            var parseTree  = CSharpSyntaxTree.ParseText(sourceText);

            var adhocWorkspace = new AdhocWorkspace();
            var optionSet      = adhocWorkspace.Options;
            optionSet = formattingOptions(optionSet);

            var formattedSyntaxNode = Formatter.Format(parseTree.GetRoot(), adhocWorkspace, optionSet);
            var formattedCode       = ExtractTextFromSyntaxNode(formattedSyntaxNode);
            return formattedCode;
        }

        private static string ExtractTextFromSyntaxNode(SyntaxNode syntaxNode)
        {
            var sourceText = syntaxNode.GetText();
            var code       = sourceText.ToString();
            code = code.Replace("\r\n\r\n", "\r\n");
            return code;
        }

        public static OptionSet StandardFormattingRules(OptionSet optionSet)
        {
            optionSet = optionSet.WithChangedOption(CSharpFormattingOptions.NewLinesForBracesInMethods,        true);
            optionSet = optionSet.WithChangedOption(CSharpFormattingOptions.NewLinesForBracesInProperties,     true);
            optionSet = optionSet.WithChangedOption(CSharpFormattingOptions.NewLinesForBracesInTypes,          true);
            optionSet = optionSet.WithChangedOption(CSharpFormattingOptions.WrappingPreserveSingleLine,        false);
            optionSet = optionSet.WithChangedOption(CSharpFormattingOptions.NewLineForMembersInAnonymousTypes, true);
            optionSet = optionSet.WithChangedOption(CSharpFormattingOptions.NewLineForMembersInObjectInit,     true);
            return optionSet;
        }
    }
}