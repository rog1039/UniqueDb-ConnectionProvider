using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Xunit;

namespace UniqueDb.ConnectionProvider.Tests.DataGeneration;

public class RoslynApiExploration
{
    [Fact()]
    [Trait("Category", "Instant")]
    public void MessingAroundWithRoslynApi()
    {
        SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(@"
                using System;

                namespace RoslynCompileSample
                {
                    public class Writer
                    {
                        public void Write(string message)
                        {
                            Console.WriteLine(message);
                        }
                    }asdfadssd
                }");
        var root                       = (CompilationUnitSyntax)syntaxTree.GetRoot();
        var incompleteMemberSyntaxList = root.DescendantNodes().OfType<IncompleteMemberSyntax>();
        var skippedTokenTrivias        = root.DescendantNodes().OfType<SkippedTokensTriviaSyntax>();
        Console.WriteLine(incompleteMemberSyntaxList.Count());
        Console.WriteLine(skippedTokenTrivias.Count());

        //descendentNodes.Select(x => x.GetText().ToString() + "\r\n==================================================").ToList().ForEach(Console.WriteLine);
        //descendentNodes.Select(x => new {x, KindName = (SyntaxKind) x.RawKind})
        //    .ToList()
        //    .ForEach(x => Console.WriteLine(x.KindName.ToString()));

        //Console.WriteLine(
        //    descendentNodes.ToStringTable(
        //        x => x.ContainsSkippedText,
        //        x => x.HasLeadingTrivia,
        //        x => x.HasStructuredTrivia,
        //        x => x.HasTrailingTrivia,
        //        x => x.IsMissing,
        //        x => x.IsStructuredTrivia,
        //        x => x.Language,
        //        x => x.RawKind,
        //        x => x.SpanStart,
        //        x => x.Span.Length,
        //        x => x.ContainsDiagnostics
        //        ));
    }
}