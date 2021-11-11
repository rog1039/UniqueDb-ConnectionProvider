using System;

namespace UniqueDb.ConnectionProvider.Tracing;

/// <summary>
/// Represents a query that we saw through the event tracing infrastructure. Will create this upon the beginning event and will
/// finish it with the EndTimestamp once we get the finish event.
/// </summary>
public class QueryExecutionRecord
{
    public QueryExecutionId           QueryExecutionId { get; set; }
    public DateTime                   StartTimestamp   { get; set; }
    public DateTime?                  EndTimestamp     { get; set; }
    public string                     CommandText      { get; set; }
    public TimeSpan?                  Duration         => EndTimestamp - StartTimestamp;
    public QueryExecutionRecordStatus Status           { get; set; }

    /// <summary>
    /// Represents the time we created this object from an incoming event. Will be close to the event Timestamp but we are using
    /// this solely so we can have a stable time to use for eviction of records from the tracking list if the query crashes
    /// or something and we never get a corresponding finish event.
    /// </summary>
    public DateTime TimeRecordAddedToList { get; set; }
}