using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace UniqueDb.ConnectionProvider.Tests.DataGeneration
{
    public class CompileResults
    {
        public CompileResults(IList<IncompleteMemberSyntax> incompleteMemberSyntaxCollection, IList<SkippedTokensTriviaSyntax> skippedTokensTriviaSyntaxCollection)
        {
            IncompleteMemberSyntaxCollection = incompleteMemberSyntaxCollection;
            SkippedTokensTriviaSyntaxCollection = skippedTokensTriviaSyntaxCollection;
        }

        public IList<IncompleteMemberSyntax> IncompleteMemberSyntaxCollection { get; set; }
        public IList<SkippedTokensTriviaSyntax> SkippedTokensTriviaSyntaxCollection { get; set; }

        public bool IsValid()
        {
            return IncompleteMemberSyntaxCollection.Count == 0 && SkippedTokensTriviaSyntaxCollection.Count == 0;
        }
    }
}