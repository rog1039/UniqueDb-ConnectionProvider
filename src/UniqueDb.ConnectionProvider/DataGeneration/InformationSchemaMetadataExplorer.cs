using UniqueDb.ConnectionProvider.DataGeneration.SqlMetadata;

namespace UniqueDb.ConnectionProvider.DataGeneration;

public static class InformationSchemaMetadataExplorer
{
    public static (List<SISTable> schemaTables, List<SISColumn> schemaColumns)
        GetAllTableAndColumnInfoForDatabase(ISqlConnectionProvider sqlConnectionProvider)
    {
        var informationSchemaTables  = GetAllTables(sqlConnectionProvider);
        var informationSchemaColumns = GetAllColumns(sqlConnectionProvider);
        return (informationSchemaTables, informationSchemaColumns);
    }

    private static List<SISTable> GetAllTables(ISqlConnectionProvider sqlConnectionProvider)
    {
        var informationSchemaTables = sqlConnectionProvider
            .Query<SISTable>(
                "SELECT * FROM INFORMATION_SCHEMA.TABLES ORDER BY TABLE_SCHEMA, TABLE_NAME")
            .ToList();
        return informationSchemaTables;
    }

    private static List<SISColumn> GetAllColumns(ISqlConnectionProvider sqlConnectionProvider)
    {
        var informationSchemaColumns = sqlConnectionProvider
            .Query<SISColumn>(
                "SELECT * FROM INFORMATION_SCHEMA.COLUMNS ORDER BY TABLE_NAME, ORDINAL_POSITION")
            .ToList();
        return informationSchemaColumns;
    }

    public static IList<SISTable> GetInformationSchemaTablesOnly(
        ISqlConnectionProvider sqlConnectionProvider)
    {
        var informationSchemaTables = sqlConnectionProvider
            .Query<SISTable>("SELECT * FROM INFORMATION_SCHEMA.TABLES")
            .ToList();
        return informationSchemaTables;
    }

    public static InformationSchemaTableDefinition GetInformationSchemaTableDefinition(
        SqlTableReference sqlTableReference)
    {
        var definition = new InformationSchemaTableDefinition();
        definition.InformationSchemaTable   = GetInformationSchemaTable(sqlTableReference);
        definition.InformationSchemaColumns = GetInformationSchemaColumns(sqlTableReference);
        definition.TableConstraints         = GetTableConstraints(sqlTableReference);
        return definition;
    }

    public static SISTable GetInformationSchemaTable(SqlTableReference sqlTableReference)
    {
        var sqlQuery =
            InformationSchemaMetadataSqlQueryGenerator.GetInformationSchemaTableSqlQuery(sqlTableReference);
        var tables = sqlTableReference.SqlConnectionProvider.Query<SISTable>(sqlQuery).ToList();

        CheckOnlyOneTableWasReturned(tables);
        return tables.SingleOrDefault();
    }

    private static void CheckOnlyOneTableWasReturned(List<SISTable> infSchTable)
    {
        if (infSchTable.Count > 1)
        {
            throw new InvalidOperationException(
                "More than one matching table found.  Please specify a schema name for the table to prevent this.");
        }
    }

    public static IList<SISColumn> GetInformationSchemaColumns(SqlTableReference sqlTableReference)
    {
        var sqlQuery =
            InformationSchemaMetadataSqlQueryGenerator.GetInformationSchemaColumnsSqlQuery(sqlTableReference);
        var tableColumns = sqlTableReference.SqlConnectionProvider.Query<SISColumn>(sqlQuery)
            .ToList();
        return tableColumns;
    }

    private static List<TableConstraintInfoDto> GetTableConstraints(SqlTableReference sqlTableReference)
    {
        var whereclause =
            $"WHERE tableConstraint.TABLE_SCHEMA = @schemaName AND tableConstraint.TABLE_NAME = @tableName ";
        var query       = TableConstraintInfoDto.SqlQuery.Replace("--WHERE", whereclause);
        var queryParams = new {schemaName = sqlTableReference.SchemaName, tableName = sqlTableReference.TableName};
        var result = sqlTableReference.SqlConnectionProvider.Query<TableConstraintInfoDto>(query, queryParams)
            .ToList();
        return result;
    }


    #region Get All TableDefinitions

    public static async Task<List<InformationSchemaTableDefinition>> GetInformationSchemaTableDefinitions(
        ISqlConnectionProvider sqlConnectionProvider)
    {
        var tables         = GetAllTables(sqlConnectionProvider);
        var allColumns     = GetAllColumns(sqlConnectionProvider);
        var allConstraints = await GetAllTableConstraints(sqlConnectionProvider);

        var columnsByTable     = allColumns.ToDictionaryMany(z => new {z.TABLE_SCHEMA, z.TABLE_NAME});
        var constraintsByTable = allConstraints.ToDictionaryMany(z => new {z.TABLE_SCHEMA, z.TABLE_NAME});

        var results = new List<InformationSchemaTableDefinition>();
        foreach (var table in tables)
        {
            var key         = new {table.TABLE_SCHEMA, table.TABLE_NAME};
            var columns     = columnsByTable.TryGet(key);
            var constraints = constraintsByTable.TryGet(key);

            var tableDef = new InformationSchemaTableDefinition()
            {
                InformationSchemaTable   = table,
                InformationSchemaColumns = columns.Value ?? new List<SISColumn>(),
                TableConstraints         = constraints.Value ?? new List<TableConstraintInfoDto>()
            };
            results.Add(tableDef);
        }

        return results;
    }


    public static async Task<IList<TableConstraintInfoDto>> GetAllTableConstraints(ISqlConnectionProvider scp)
    {
        var query  = TableConstraintInfoDto.SqlQuery;
        var result = scp.Query<TableConstraintInfoDto>(query).ToList();
        return result;
    }

    #endregion
}

internal static class DictionaryExtensionMethods
{
    public static Dictionary<K, IList<T>> ToDictionaryMany<T, K>(this IEnumerable<T> list, Func<T, K> keyFunc)
    {
        var dict = new Dictionary<K, IList<T>>();
        foreach (var item in list)
        {
            dict.AddToDictionary(keyFunc(item), item);
        }

        return dict;
    }
    public static DictionaryResult<TValue> TryGet<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key)
    {
        if(dict.TryGetValue(key, out var val))
        {
            return DictionaryResult<TValue>.Found(val);
        }
        return DictionaryResult<TValue>.NotFound();
    }
    public static void AddToDictionary<K, V>(this IDictionary<K, IList<V>> dict, K key, V value)
    {
        var existingList = dict.TryGet(key);
        if (existingList.WasFound)
            existingList.Value.Add(value);
        else
            dict.Add(key, new List<V>() {value});
    }

        
}
public class DictionaryResult<T>
{
    private DictionaryResult()
    {
        WasFound = false;
    }

    private DictionaryResult(T value)
    {
        WasFound = true;
        Value    = value;
    }

    public bool WasFound { get;  }
    public T    Value    { get; }

    public static DictionaryResult<T> Found(T value)
    {
        var result = new DictionaryResult<T>(value);
        return result;
    }

    public static DictionaryResult<T> NotFound()
    {
        return new DictionaryResult<T>();
    }
        
}