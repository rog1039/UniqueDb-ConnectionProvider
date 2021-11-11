namespace UniqueDb.ConnectionProvider.Tracing;

/// <summary>
/// A way to uniquely identify a query that comes through to our event listener.
/// </summary>
/// <param name="ConnectionId">This is a guid.</param>
/// <param name="ObjectId">Seems to just be a monotonically increasing integer.</param>
public record QueryExecutionId(string ConnectionId, string ObjectId);