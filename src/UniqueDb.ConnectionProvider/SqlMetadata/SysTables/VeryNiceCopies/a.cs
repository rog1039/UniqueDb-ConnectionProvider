using UniqueDb.ConnectionProvider.CoreTypes;
using UniqueDb.ConnectionProvider.SqlScriptingEngine.Attempt2;

namespace UniqueDb.ConnectionProvider.SqlMetadata.SysTables.VeryNiceCopies;

public class TableSpec
{
   public List<ColumnSpec> ColumnSpecs { get; set; }

   public string       TableName         { get; set; }
   public string       SchemaName        { get; set; }
   public TemporalType TemporalType      { get; set; }
   public bool         IsMemoryOptimized { get; set; }
}

public enum TemporalType
{
   None,
   HistoryTable,
   SystemVersionedTable
}

public class ColumnSpec
{
   public string  SchemaName { get; set; }
   public string  TableName  { get; set; }
   public string  ColumnName { get; set; }
   public SqlType SqlType    { get; set; }
   public bool    IsNullable { get; set; }

   /// <summary>
   /// 1-based column id.
   /// </summary>
   public int ColumnId { get; set; }

   public bool               IsComputed          { get; set; }
   public bool               IsIdentity          { get; set; }
   public int                ObjectId            { get; set; }
   public byte               SystemTypeId        { get; set; }
   public int                UserTypeId          { get; set; }
   public string?            Definition          { get; set; }
   public bool?              IsPersisted         { get; set; }
   public object?            SeedValue           { get; set; }
   public object?            IncrementValue      { get; set; }
   public object?            LastValue           { get; set; }
   public TemporalColumnType GeneratedAlwaysType { get; set; }
}

public enum IndexType
{
   Unknown,
   Heap,
   Clustered,
   NonClustered,
   XML,
   Spatial,
   ClusteredColumnStore,
   NonClusteredColumnStore,
   NonClusteredHashIndex,
}

public class IndexSpec
{
   public TableSpec             TableSpec        { get; set; }
   public List<IndexColumnSpec> IndexColumnSpecs { get; set; }

   public string    SchemaName         { get; set; }
   public string    TableName          { get; set; }
   public string    IndexName          { get; set; }
   public IndexType IndexType          { get; set; }
   public bool      IsPrimaryKey       { get; set; }
   public bool      IsUnique           { get; set; }
   public bool      IsUniqueConstraint { get; set; }
   public int       IndexId            { get; set; }
}

public class IndexColumnSpec
{
   public string  SchemaName { get; set; }
   public string  TableName  { get; set; }
   public string  IndexName  { get; set; }
   public string  ColumnName { get; set; }
   public SqlType SqlType    { get; set; }
   public bool    IsNullable { get; set; }

   /// <summary>
   /// 1-based Id of the column in the table.
   /// </summary>
   public int ColumnId { get; set; }

   /// <summary>
   /// 1-based ordering of the column in the index.
   /// </summary>
   public int IndexColumnNumber { get; set; }

   /// <summary>
   /// 1-based ordering of all 'key' values in the index. Included columns have a value of 0.
   /// </summary>
   public int KeyOrdinal { get; set; }
   
   public int IndexId { get; set; }

   public SortDirection SortDirection { get; set; }

   public bool IsIncludedColumn { get; set; }
}

public class ForeignKeySpec { }

public class ForeignKeyColumnSpec { }

public enum SortDirection
{
   None,
   Asc,
   Desc
}