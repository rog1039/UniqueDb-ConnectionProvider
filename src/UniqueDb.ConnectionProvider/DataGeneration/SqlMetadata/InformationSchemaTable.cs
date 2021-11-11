namespace UniqueDb.ConnectionProvider.DataGeneration.SqlMetadata;

public class InformationSchemaTable
{
    public string TABLE_CATALOG { get; set; }
    public string TABLE_SCHEMA  { get; set; }
    public string TABLE_NAME    { get; set; }
    public string TABLE_TYPE    { get; set; }

    public DbTableName DbTableName => new DbTableName(TABLE_SCHEMA, TABLE_NAME);
}