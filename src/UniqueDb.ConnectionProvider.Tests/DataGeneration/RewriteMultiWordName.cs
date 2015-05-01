using System.Linq;

namespace UniqueDb.ConnectionProvider.Tests.DataGeneration
{
    class RewriteMultiWordName : PropertyNameRewrite
    {
        public override bool ShouldRewrite(string originalName)
        {
            if (originalName.Contains(' '))
            {
                return true;
            }
            return false;
        }

        public override string Rewrite(string originalName)
        {
            return originalName.Replace(' ', '_');
        }
    }
}