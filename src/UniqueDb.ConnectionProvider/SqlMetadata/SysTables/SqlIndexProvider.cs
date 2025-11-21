namespace UniqueDb.ConnectionProvider.SqlMetadata.SysTables;

#nullable enable
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Data;
using Microsoft.Data.SqlClient;
using Dapper;

public sealed class SqlIndex
{
    public string SchemaName { get; init; } = "";
    public string TableName  { get; init; } = "";
    public string IndexName  { get; init; } = "";
    public int    IndexId    { get; init; }

    public string TypeDesc           { get; init; } = ""; // CLUSTERED/NONCLUSTERED/XML/SPATIAL
    public bool   IsUnique           { get; init; }
    public bool   IsPrimaryKey       { get; init; }
    public bool   IsUniqueConstraint { get; init; }
    public bool   IsDisabled         { get; init; }
    public bool   HasFilter          { get; init; }
    public string? FilterDefinition  { get; init; }
    public string? DataSpaceName     { get; init; } // filegroup or partition scheme

    // Options
    public int    FillFactor     { get; init; } // 0..100 (0 = server default)
    public bool   IsPadded       { get; init; }
    public bool   IgnoreDupKey   { get; init; }
    public bool   AllowRowLocks  { get; init; }
    public bool   AllowPageLocks { get; init; }

    public List<SqlIndexKeyColumn>      KeyColumns      { get; } = new();
    public List<SqlIndexIncludedColumn> IncludedColumns { get; } = new();
}

public sealed class SqlIndexKeyColumn
{
    public string ColumnName { get; init; } = "";
    public int    Ordinal    { get; init; } // 1-based
    public bool   Descending { get; init; }
}

public sealed class SqlIndexIncludedColumn
{
    public string ColumnName { get; init; } = "";
    public int    Ordinal    { get; init; } // order among includes
}

internal sealed class SqlIndexRow
{
    public string SchemaName { get; init; } = "";
    public string TableName  { get; init; } = "";
    public string? IndexName { get; init; }
    public int    IndexId    { get; init; }

    public string TypeDesc           { get; init; } = "";
    public bool   IsUnique           { get; init; }
    public bool   IsPrimaryKey       { get; init; }
    public bool   IsUniqueConstraint { get; init; }
    public bool   IsDisabled         { get; init; }
    public bool   IsHypothetical     { get; init; }
    public bool   HasFilter          { get; init; }
    public string? FilterDefinition  { get; init; }
    public string? DataSpaceName     { get; init; }

    public int    FillFactor         { get; init; }    // tinyint maps fine to int
    public bool   IsPadded           { get; init; }
    public bool   IgnoreDupKey       { get; init; }
    public bool   AllowRowLocks      { get; init; }
    public bool   AllowPageLocks     { get; init; }

    public bool?  IsIncludedColumn   { get; init; }
    public int?   KeyOrdinal         { get; init; }
    public int?   IndexColumnId      { get; init; }
    public bool?  IsDescendingKey    { get; init; }
    public string? ColumnName        { get; init; }
}

public static class SqlIndexReaderDapper
{
    public static async Task<List<SqlIndex>> GetIndexesAsync(
        string connectionString,
        string? schema = null,
        string? table  = null,
        bool includeHypothetical = false,
        CancellationToken cancellationToken = default)
    {
        const string sql = @"
SELECT
    s.name  AS SchemaName,
    t.name  AS TableName,
    i.name  AS IndexName,
    i.index_id AS IndexId,
    i.type_desc AS TypeDesc,
    i.is_unique AS IsUnique,
    i.is_primary_key AS IsPrimaryKey,
    i.is_unique_constraint AS IsUniqueConstraint,
    i.is_disabled AS IsDisabled,
    i.is_hypothetical AS IsHypothetical,
    i.has_filter AS HasFilter,
    i.filter_definition AS FilterDefinition,
    ds.name AS DataSpaceName,
    i.fill_factor AS [FillFactor],
    i.is_padded AS IsPadded,
    i.ignore_dup_key AS IgnoreDupKey,
    i.allow_row_locks AS AllowRowLocks,
    i.allow_page_locks AS AllowPageLocks,

    ic.is_included_column AS IsIncludedColumn,
    ic.key_ordinal AS KeyOrdinal,
    ic.index_column_id AS IndexColumnId,
    ic.is_descending_key AS IsDescendingKey,
    c.name AS ColumnName
FROM sys.indexes AS i
JOIN sys.tables  AS t  ON t.object_id = i.object_id
JOIN sys.schemas AS s  ON s.schema_id = t.schema_id
LEFT JOIN sys.index_columns AS ic
       ON ic.object_id = i.object_id AND ic.index_id = i.index_id
LEFT JOIN sys.columns AS c
       ON c.object_id = ic.object_id AND c.column_id = ic.column_id
LEFT JOIN sys.data_spaces AS ds
       ON ds.data_space_id = i.data_space_id
WHERE
    i.index_id > 0                              -- exclude HEAP info
    AND t.is_ms_shipped = 0
    AND (@schema IS NULL OR s.name = @schema)
    AND (@table  IS NULL OR t.name = @table)
    AND (@includeHypothetical = 1 OR i.is_hypothetical = 0)
ORDER BY
    s.name, t.name, i.index_id,
    CASE WHEN ic.is_included_column = 1 THEN 1 ELSE 0 END, -- key cols first
    ic.key_ordinal, ic.index_column_id;";

        var parameters = new
        {
            schema,
            table,
            includeHypothetical = includeHypothetical ? 1 : 0
        };

        var dict = new Dictionary<(string Schema, string Table, string IndexName, int IndexId), SqlIndex>(TupleComparer.OrdinalIgnoreCase);

        await using var conn = new SqlConnection(connectionString);
        await conn.OpenAsync(cancellationToken);

        var cmd = new CommandDefinition(sql, parameters, commandType: CommandType.Text, cancellationToken: cancellationToken);
        var rows = await conn.QueryAsync<SqlIndexRow>(cmd);

        foreach (var r in rows)
        {
            var idxName = r.IndexName ?? "(unnamed)";
            var key = (r.SchemaName, r.TableName, idxName, r.IndexId);

            if (!dict.TryGetValue(key, out var idx))
            {
                idx = new SqlIndex
                {
                    SchemaName        = r.SchemaName,
                    TableName         = r.TableName,
                    IndexName         = idxName,
                    IndexId           = r.IndexId,
                    TypeDesc          = r.TypeDesc,
                    IsUnique          = r.IsUnique,
                    IsPrimaryKey      = r.IsPrimaryKey,
                    IsUniqueConstraint= r.IsUniqueConstraint,
                    IsDisabled        = r.IsDisabled,
                    HasFilter         = r.HasFilter,
                    FilterDefinition  = r.FilterDefinition,
                    DataSpaceName     = r.DataSpaceName,
                    FillFactor        = r.FillFactor,
                    IsPadded          = r.IsPadded,
                    IgnoreDupKey      = r.IgnoreDupKey,
                    AllowRowLocks     = r.AllowRowLocks,
                    AllowPageLocks    = r.AllowPageLocks
                };
                dict.Add(key, idx);
            }

            // Add column details if present
            if (!string.IsNullOrEmpty(r.ColumnName))
            {
                if (r.IsIncludedColumn == true)
                {
                    idx.IncludedColumns.Add(new SqlIndexIncludedColumn
                    {
                        ColumnName = r.ColumnName!,
                        Ordinal    = r.IndexColumnId ?? 0
                    });
                }
                else
                {
                    idx.KeyColumns.Add(new SqlIndexKeyColumn
                    {
                        ColumnName = r.ColumnName!,
                        Ordinal    = r.KeyOrdinal ?? int.MaxValue,
                        Descending = r.IsDescendingKey ?? false
                    });
                }
            }
        }

        // Order columns
        foreach (var ix in dict.Values)
        {
            ix.KeyColumns.Sort((a, b) => a.Ordinal.CompareTo(b.Ordinal));
            ix.IncludedColumns.Sort((a, b) => a.Ordinal.CompareTo(b.Ordinal));
        }

        return new List<SqlIndex>(dict.Values);
    }

    private sealed class TupleComparer : IEqualityComparer<(string, string, string, int)>
    {
        public static readonly TupleComparer OrdinalIgnoreCase = new();
        public bool Equals((string, string, string, int) x, (string, string, string, int) y) =>
            string.Equals(x.Item1, y.Item1, StringComparison.OrdinalIgnoreCase) &&
            string.Equals(x.Item2, y.Item2, StringComparison.OrdinalIgnoreCase) &&
            string.Equals(x.Item3, y.Item3, StringComparison.OrdinalIgnoreCase) &&
            x.Item4 == y.Item4;

        public int GetHashCode((string, string, string, int) obj) =>
            HashCode.Combine(
                StringComparer.OrdinalIgnoreCase.GetHashCode(obj.Item1),
                StringComparer.OrdinalIgnoreCase.GetHashCode(obj.Item2),
                StringComparer.OrdinalIgnoreCase.GetHashCode(obj.Item3),
                obj.Item4);
    }
}
