using UniqueDb.ConnectionProvider.Tracing.Hashing;

namespace UniqueDb.ConnectionProvider.Tracing;

public class SqlExecutionTimingDto
{
    public int Id { get; set; }

    public DateTime                   StartTimestamp  { get; set; }
    public DateTime?                  EndTimestamp    { get; set; }
    public TimeSpan?                  Duration        => EndTimestamp - StartTimestamp;
    public string                     CommandText     { get; set; }
    public string                     CommandTextHash { get; set; }
    public QueryExecutionRecordStatus QueryStatus     { get; set; }

    public string WindowsUser { get; set; }
    public string MachineName { get; set; }

    public SqlExecutionTimingDto()
    {
            
    }

    public SqlExecutionTimingDto(QueryExecutionRecord @record)
    {
        StartTimestamp = record.StartTimestamp;
        EndTimestamp   = record.EndTimestamp;
        CommandText    = record.CommandText;
        //No need for better hash algo's like SHA256 in this case since it is solely to make it easier to group identical command texts together.
        CommandTextHash = record.CommandText.GetMd5hash();
        QueryStatus     = record.Status;
    }
}