namespace UniqueDb.ConnectionProvider.DataGeneration
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