using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace UniqueDb.ConnectionProvider.Tests.DataGeneration;

public static class RoslynHelper
{
    public static RoslynCompileResults TryCompile(string text)
    {
        var syntaxTree                         = CSharpSyntaxTree.ParseText(text);
        var root                               = syntaxTree.GetRoot();
        var incompleteMemberSyntaxCollection   = root.DescendantNodes().OfType<IncompleteMemberSyntax>().ToList();
        var skippedTokenTriviaSyntaxCollection = root.DescendantNodes().OfType<SkippedTokensTriviaSyntax>().ToList();
        var compilationResult = new RoslynCompileResults(incompleteMemberSyntaxCollection,
                                                   skippedTokenTriviaSyntaxCollection);
        return compilationResult;
    }
}