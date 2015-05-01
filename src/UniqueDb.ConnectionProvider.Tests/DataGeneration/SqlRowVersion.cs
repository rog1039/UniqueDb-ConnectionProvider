namespace UniqueDb.ConnectionProvider.Tests.DataGeneration
{
    public class SqlRowVersion
    {
        private readonly byte[] _bytes;

        public SqlRowVersion(byte[] bytes)
        {
            _bytes = bytes;
        }
    }
}