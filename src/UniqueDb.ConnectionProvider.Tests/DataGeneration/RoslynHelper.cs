using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace UniqueDb.ConnectionProvider.Tests.DataGeneration;

public static class RoslynHelper
{
    public static CompileResults TryCompile(string text)
    {
        var syntaxTree                         = CSharpSyntaxTree.ParseText(text);
        var root                               = syntaxTree.GetRoot();
        var incompleteMemberSyntaxCollection   = root.DescendantNodes().OfType<IncompleteMemberSyntax>().ToList();
        var skippedTokenTriviaSyntaxCollection = root.DescendantNodes().OfType<SkippedTokensTriviaSyntax>().ToList();
        var compilationResult = new CompileResults(incompleteMemberSyntaxCollection,
                                                   skippedTokenTriviaSyntaxCollection);
        return compilationResult;
    }
}