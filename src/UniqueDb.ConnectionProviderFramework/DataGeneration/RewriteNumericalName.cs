namespace UniqueDb.ConnectionProvider.DataGeneration
{
    public class RewriteNumericalName : PropertyNameRewrite
    {
        public override bool ShouldRewrite(string originalName)
        {
            if (char.IsDigit(originalName[0]))
            {
                return true;
            }
            return false;
        }

        public override string Rewrite(string originalName)
        {
            return "_" + originalName;
        }
    }
}