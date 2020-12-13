namespace UniqueDb.CSharpClassGenerator.Features.DatabaseSelection
{
    public class SqlRecordDto
    {
        public string ConnectionName { get; set; }
        public string ServerName { get; set; }
        public string DatabaseName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool UseIntegratedAuth { get; set; }
    }
}