namespace UniqueDb.ConnectionProvider.DataGeneration
{
    public abstract class PropertyNameRewrite
    {
        public abstract bool ShouldRewrite(string originalName);
        public abstract string Rewrite(string originalName);
    }
}