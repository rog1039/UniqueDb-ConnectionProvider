using System.Collections.Concurrent;
using System.Diagnostics.Tracing;
using System.Text.RegularExpressions;

namespace UniqueDb.ConnectionProvider.Tracing;

public enum ShouldAutoStart
{
    Yes,
    No
}

public sealed class SqlClientEventSourceListener : EventListener
{
    public event Action<QueryExecutionRecord> NewQueryExecutionRecord;

    /// <summary>
    /// Tracks all sql queries that we think are open.
    /// </summary>
    private readonly List<QueryExecutionRecord> _inFlightQueryExecutionRecords = new();

    private static readonly string ExecuteDbReaderRegexPattern =
        @"Object Id (?<ObjectId>\d+), (Behavior \d+, )?Activity Id (?<ActivityId>[a-z0-9\-:]+), Client Connection Id (?<ClientConnectionId>[a-z0-9\-]+), Command Text '(?<CommandText>.*)'$";

    private static readonly Regex ExecuteDbReaderRegex;

    private static readonly string CloseSqlConnectionRegexPattern =
        @"SqlConnection\.Close \| API \| Correlation .* Object Id (?<ObjectId>\d+), Activity Id (?<ActivityId>[a-z0-9\-:]+), Client Connection Id (?<ClientConnectionId>[a-z0-9\-]+)";

    private static readonly Regex CloseSqlConnectionRegex;

    /// <summary>
    /// Holds the data from the incoming event sources. We don't want to block on the ingest so we transform the incoming
    /// data into an EventDataRecord and store it here for processing. We should never have much of a backlog of these records
    /// since we should be processing them about as quickly as they come in so a limit of 500 should be fine.
    /// </summary>
    private readonly BlockingCollection<EventDataRecord> IncomingQueryRecords = new(5000);

    private bool _isRunning = false;

    /// <summary>
    /// The Task that reads the blocking collection for new events.
    /// </summary>
    private Task _workerTask;

    static SqlClientEventSourceListener()
    {
        ExecuteDbReaderRegex    = new Regex(ExecuteDbReaderRegexPattern,    RegexOptions.Singleline);
        CloseSqlConnectionRegex = new Regex(CloseSqlConnectionRegexPattern, RegexOptions.Singleline);
    }

    public SqlClientEventSourceListener(ShouldAutoStart autostart = ShouldAutoStart.Yes)
    {
        if (autostart == ShouldAutoStart.Yes) Start();
    }

    /// <summary>
    /// How does tracing work for SQL Server? Documentation is not great. See here for decent source:
    /// https://docs.microsoft.com/en-us/sql/connect/ado-net/enable-eventsource-tracing?view=sql-server-ver15
    ///
    /// This method tells the event source which events we want to be notified about.
    /// </summary>
    /// <param name="eventSource"></param>
    protected override void OnEventSourceCreated(EventSource eventSource)
    {
        if (!eventSource.Name.Equals("Microsoft.Data.SqlClient.EventSource")) return;

        /*
         * There is two ways I believe to trace SQL server events.
         * 
         * First, is to use native SNI tracing. You can use either 8192 or 16384 as the event command value.
         * 
         * From the documentation:
         *
         *      // Enables trace events:
                EventSource.SendCommand(eventSource, (EventCommand)8192, null);

                // Enables flow events:
                EventSource.SendCommand(eventSource, (EventCommand)16384, null);

                // Enables both trace and flow events:
                EventSource.SendCommand(eventSource, (EventCommand)(8192 | 16384), null);
         */

        /*
             * Second, we can use the tracing built into SqlClient's EventSource implementation.
             * That has the following table we can use to determine what to trace via the event command value:
             *
             *  Keyword name	    Value	Description
                ExecutionTrace	    1	    Turns on capturing Start/Stop events before and after command execution.
                Trace	            2	    Turns on capturing basic application flow trace events.
                Scope	            4	    Turns on capturing enter and exit events
                NotificationTrace	8	    Turns on capturing SqlNotification trace events
                NotificationScope	16	    Turns on capturing SqlNotification scope enter and exit events
                PoolerTrace	        32	    Turns on capturing connection pooling flow trace events.
                PoolerScope	        64	    Turns on capturing connection pooling scope trace events.
                AdvancedTrace	    128	    Turns on capturing advanced flow trace events.
                AdvancedTraceBin	256	    Turns on capturing advanced flow trace events with additional information.
                CorrelationTrace	512	    Turns on capturing correlation flow trace events.   <-- this is what we are using for our logging
                StateDump	        1024	Turns on capturing full state dump of SqlConnection
                SNITrace	        2048	Turns on capturing flow trace events from Managed Networking implementation (only applicable in .NET Core)
                SNIScope	        4096	Turns on capturing scope events from Managed Networking implementation (only applicable in .NET Core)
             * 
        */


        /*
         * Below code hooks into the EventSource for specific events.
         */
        EnableEvents(eventSource, EventLevel.Informational, (EventKeywords) (0b_0000_0010_0000_0000));
        var explanation1 = "                                                    ____ __^_ __#_ _###                 ";
        var explanation2 = "                                                    fedC BA09 8765 4321                 ";

        /*
             ^ means best source of data
             # means has some data but not the best
             _ means nothing of value or no events.
             
             List of positions and decimal numbers
             * 1,1; 2,2; 3,4; 4,8; 5,16; 6,32; 7,64; 8,128; 9,256; 10,512; A,1024; B,2048; C,4096; D,8192, E,16384; F,32768
         *
         * Simple explanation of events for different Keyword/Position Values.
             * 
             * 0 contains connection and sqlcommand events with activity, correlation, and object ID's which we can use. ***WINNER***
             * 6 contains information about connection pooling events. Lots of info, not super useful.
             * 3 contains data reader events and sql connection open without correlation id's - tons of data when there is lots of data. Not useful.
             * 2 contains lots of TdsParser code and still sql command code. Some useful events that provide good amounts of detail but not useful for our specific use case. Looks like
             *
9/29/2021 9:25:07 PM -  -  - message -SqlCommand.RunExecuteReaderTds | Info | Object Id 1, Activity Id 73e1d6bd-62c3-4340-944f-de8b9200e2e5:2, Client Connection Id 6ff0e013-3b6f-4bb2-853a-aee70ffe8cae, Command executed as SQLBATCH, Command Text 'select top 100000 * from PartTran' 
9/29/2021 9:25:09 PM -  -  - message -TdsParserStateObject.DecrementOpenResultCount | INFO | State Object Id 1, Processing Attention.

                could be useful as a start and end event for processing perhaps.
                
             * 1 Contains WriteBeginExecuteEvent and EndBeginExecuteEvent. However, EndBegin happens 1 ms after the WriteBegin so doesn't track actual sql query time!
             */
    }

    /// <summary>
    /// Logged during synchronous db call.
    /// </summary>
    const string SynchronousDbReaderLogText = "SqlCommand.ExecuteDbDataReader";

    /// <summary>
    /// Logged during asynchronous db call.
    /// </summary>
    const string AsynchronousDbReaderLogText = "SqlCommand.InternalExecuteReaderAsync | API | Correlation";

    /// <summary>
    /// A holder class that we can populate with data from the incoming EventWrittenEventArgs object.
    /// </summary>
    /// <param name="Timestamp"></param>
    /// <param name="PayloadData"></param>
    private record EventDataRecord(DateTime Timestamp, string PayloadData);

    /// <summary>
    /// This callback runs whenever an event is written by SqlClientEventSource.
    /// Event data is accessed through the EventWrittenEventArgs parameter.
    /// </summary>
    /// <param name="eventData"></param>
    protected override void OnEventWritten(EventWrittenEventArgs eventData)
    {
        var payloadData = GetPayloadData(eventData);
        if (string.IsNullOrWhiteSpace(payloadData)) return;

        var timestamp       = eventData.TimeStamp;
        var eventDataRecord = new EventDataRecord(timestamp, payloadData);

        IncomingQueryRecords.TryAdd(eventDataRecord, TimeSpan.FromMilliseconds(20));
    }

    public void Start()
    {
        if (_isRunning) return;

        _isRunning  = true;
        _workerTask = Task.Run(ProcessingLoop);
    }

    /// <summary>
    /// Start processing the saved EventWrittenEventArgs data.
    /// </summary>
    private void ProcessingLoop()
    {
        while (_isRunning)
        {
            if (IncomingQueryRecords.TryTake(out var eventDataRecord, TimeSpan.FromMilliseconds(100)))
            {
                ProcessEventData(eventDataRecord);
            }
        }
    }

    /// <summary>
    /// Stops processing the data from the saved EventWrittenEventArgs data.
    /// </summary>
    public void Stop()
    {
        _isRunning = false;
    }

    /// <summary>
    /// Processes the data saved from the EventWrittenEventArgs object.
    /// </summary>
    /// <param name="eventDataRecord"></param>
    private void ProcessEventData(EventDataRecord eventDataRecord)
    {
        var (timestamp, payloadData) = eventDataRecord;

        var isStartTimingEvent = payloadData.Contains(AsynchronousDbReaderLogText) ||
                                 payloadData.Contains(SynchronousDbReaderLogText);

        if (isStartTimingEvent)
        {
            var groupCollection = ExecuteDbReaderRegex.Match(payloadData).Groups;
            if (groupCollection.Values.Count() < 3) return;

            var queryExecutionId = CreateQueryExecutionIdFromRegex(groupCollection);

            var queryRecord = new QueryExecutionRecord()
            {
                QueryExecutionId      = queryExecutionId,
                CommandText           = groupCollection["CommandText"].Value,
                StartTimestamp        = timestamp,
                TimeRecordAddedToList = DateTime.UtcNow,
                Status                = QueryExecutionRecordStatus.Started,
            };

            _inFlightQueryExecutionRecords.Add(queryRecord);

            return;
        }

        var closeSqlConnectionMatches = CloseSqlConnectionRegex.Match(payloadData).Groups;
        var isEndTimingEvent          = closeSqlConnectionMatches.Count > 2;
        if (isEndTimingEvent)
        {
            var queryExecutionId = CreateQueryExecutionIdFromRegex(closeSqlConnectionMatches);
            for (var index = 0; index < _inFlightQueryExecutionRecords.Count; index++)
            {
                var storedQueryRecord = _inFlightQueryExecutionRecords[index];
                if (storedQueryRecord.QueryExecutionId == queryExecutionId)
                {
                    //Found the query record for this query, so add end timestamp and log.
                    storedQueryRecord.EndTimestamp = timestamp;
                    storedQueryRecord.Status       = QueryExecutionRecordStatus.Finished;

                    //Remove from the collection.
                    _inFlightQueryExecutionRecords.RemoveAt(index);
                    index--;

                    PublishQueryExecutionRecord(storedQueryRecord);
                }
                else
                {
                    //This stored query is not associated with the incoming event but let's make sure its not too old and it's not a stale query.
                    var queryElapsedTime = DateTime.UtcNow - storedQueryRecord.StartTimestamp;
                    if (queryElapsedTime > TimeSpan.FromMinutes(5))
                    {
                        storedQueryRecord.EndTimestamp = DateTime.UtcNow;
                        storedQueryRecord.Status       = QueryExecutionRecordStatus.GaveUpTracking;

                        //Consider it abandoned and remove it..
                        _inFlightQueryExecutionRecords.RemoveAt(index);
                        index--;
                    }
                }
            }
        }
    }

    private void PublishQueryExecutionRecord(QueryExecutionRecord @record)
    {
        // Console.WriteLine($"Finished: {record.StartTimestamp} - {record.Duration.Value.TotalSeconds} - {record.CommandText}");
        OnNewQueryExecutionRecord(record);
    }

    /// <summary>
    /// Extract the payload data from the Event. Sql client events contain the logging information in the first slot of the Payload array.
    /// </summary>
    /// <param name="eventData"></param>
    /// <returns></returns>
    private static string? GetPayloadData(EventWrittenEventArgs? eventData)
    {
        if (eventData?.Payload is null || eventData.Payload.Count < 1) return string.Empty;

        var payloadData = eventData.Payload[0]!.ToString();
        return payloadData;
    }

    /// <summary>
    /// From the regex match results, extract out the data necessary to create a QueryExecutionId.
    /// </summary>
    /// <param name="groupCollection"></param>
    /// <returns></returns>
    private static QueryExecutionId CreateQueryExecutionIdFromRegex(GroupCollection? groupCollection)
    {
        var objectId           = groupCollection["ObjectId"].Value;
        var clientConnectionId = groupCollection["ClientConnectionId"].Value;

        var queryExecutionId = new QueryExecutionId(clientConnectionId, objectId);
        return queryExecutionId;
    }

    private void OnNewQueryExecutionRecord(QueryExecutionRecord obj)
    {
        NewQueryExecutionRecord?.Invoke(obj);
    }
}