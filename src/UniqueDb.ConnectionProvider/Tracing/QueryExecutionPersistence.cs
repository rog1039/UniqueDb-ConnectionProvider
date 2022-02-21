using System.Collections.Concurrent;
using Serilog;
using UniqueDb.ConnectionProvider.DataGeneration.Crud;

namespace UniqueDb.ConnectionProvider.Tracing;

/// <summary>
/// Saves QueryExecutionRecords to a database.
/// </summary>
public class QueryExecutionPersistence
{
    private readonly ILogger                                  _logger;
    private readonly ISqlConnectionProvider                   _sqlConnectionProvider;
    private readonly string                                   _tableName;
    private readonly string                                   _schemaName;
    private readonly BlockingCollection<QueryExecutionRecord> _queryExecutionRecordBlockingCollection = new(8000);

    private static readonly string[] ColumnsToIgnore = { "Id" };

    private SqlClientEventSourceListener? _listener;
    private bool                          _shouldProcessRecords;

    private readonly string WindowsUserName = Environment.UserName;
    private readonly string Machine         = Environment.MachineName;

    public QueryExecutionPersistence(ILogger                logger,
                                     ISqlConnectionProvider sqlConnectionProvider,
                                     string                 tableName,
                                     string                 schemaName)
    {
        _logger                = logger;
        _sqlConnectionProvider = sqlConnectionProvider;
        _tableName             = tableName;
        _schemaName            = schemaName;
    }

    public void Start()
    {
        _shouldProcessRecords = true;
        _listener             = new SqlClientEventSourceListener();

        Task.Run(ProcessQueryExecutionRecords);

        _listener.NewQueryExecutionRecord += OnNewQueryExecutionRecord;
    }

    private void OnNewQueryExecutionRecord(QueryExecutionRecord @record)
    {
        _queryExecutionRecordBlockingCollection.TryAdd(record, TimeSpan.FromMilliseconds(100));
    }

    public void Stop()
    {
        _listener?.Dispose();
        _listener             = null;
        _shouldProcessRecords = false;
    }

    private void ProcessQueryExecutionRecords()
    {
        while (_shouldProcessRecords)
        {
            var hasItem = _queryExecutionRecordBlockingCollection.TryTake(out var record, TimeSpan.FromSeconds(1));
            if (hasItem)
            {
                Persist(record);
            }
        }
    }

    public void Persist(QueryExecutionRecord @record)
    {
        try
        {
            var insertRecord = new SqlExecutionTimingDto(record)
            {
                WindowsUser = WindowsUserName,
                MachineName = Machine
            };
            _sqlConnectionProvider.Insert(
                insertRecord,
                _tableName,
                _schemaName,
                ColumnsToIgnore
            );
        }
        catch (Exception e)
        {
            _logger.Error(e, $"Error trying to save {nameof(SqlExecutionTimingDto)}");
        }
    }
}