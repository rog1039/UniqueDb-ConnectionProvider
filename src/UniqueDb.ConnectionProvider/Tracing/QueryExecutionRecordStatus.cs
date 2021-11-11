namespace UniqueDb.ConnectionProvider.Tracing;

public enum QueryExecutionRecordStatus
{
    None,
    Started,
    Finished,
    GaveUpTracking,
}