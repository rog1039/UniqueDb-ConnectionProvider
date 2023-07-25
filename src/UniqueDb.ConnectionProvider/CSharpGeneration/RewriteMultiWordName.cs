namespace UniqueDb.ConnectionProvider.CSharpGeneration;

public class RewriteMultiWordName : PropertyNameRewrite
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