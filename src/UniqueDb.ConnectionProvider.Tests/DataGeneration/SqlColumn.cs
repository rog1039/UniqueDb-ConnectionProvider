namespace UniqueDb.ConnectionProvider.Tests.DataGeneration
{
    public class SqlColumn
    {
        public string Name { get; set; }
        public int OrdinalPosition { get; set; }
        public string DataType { get; set; }
        public bool IsNullable { get; set; }
        public string Default { get; set; }
    }
}