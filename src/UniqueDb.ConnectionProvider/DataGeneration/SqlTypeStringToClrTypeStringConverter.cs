namespace UniqueDb.ConnectionProvider.DataGeneration;

public static class SqlTypeStringToClrTypeStringConverter
{
    public static bool UseNativeSqlTypes = true;

    public static string GetClrDataType(string sqlDataType)
    {
        if (string.IsNullOrWhiteSpace(sqlDataType))
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(sqlDataType));
            
        string dataTypeName = ConvertSqlTypeNameToClrTypeName(sqlDataType);
        return dataTypeName;
    }

    private static string ConvertSqlTypeNameToClrTypeName(string sqlDataType)
    {
        if (string.IsNullOrWhiteSpace(sqlDataType))
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(sqlDataType));
            
        if (sqlDataType == "int")
        {
            return "int";
        }
        if (sqlDataType == "tinyint")
        {
            return "byte";
        }
        if (sqlDataType == "varchar")
        {
            return "string";
        }
        if (sqlDataType == "datetime")
        {
            return "DateTime";
        }
        if (sqlDataType == "smalldatetime")
        {
            return "DateTime";
        }
        if (sqlDataType == "date")
        {
            return "DateTime";
        }
        if (sqlDataType == "time")
        {
            return "TimeSpan";
        }
        if (sqlDataType == "decimal")
        {
            return "decimal";
        }
        if (sqlDataType == "bigint")
        {
            return "Int64";
        }
        if (sqlDataType == "timestamp")
        {
            return "byte[]";
        }
        if (sqlDataType == "sql_variant")
        {
            return "object";
        }
        if (sqlDataType == "text")
        {
            return "string";
        }
        if (sqlDataType == "bit")
        {
            return "bool";
        }
        if (sqlDataType == "nvarchar")
        {
            return "string";
        }
        if (sqlDataType == "ntext")
        {
            return "string";
        }
        if (sqlDataType == "float")
        {
            return "double";
        }
        if (sqlDataType == "real")
        {
            return "double";
        }
        if (sqlDataType == "nchar")
        {
            return "string";
        }
        if (sqlDataType == "image")
        {
            return "byte[]";
        }
        if (sqlDataType == "uniqueidentifier")
        {
            return "Guid";
        }
        if (sqlDataType == "money")
        {
            return "decimal";
        }
        if (sqlDataType == "smallint")
        {
            return "Int16";
        }
        if (sqlDataType == "geography")
        {
            //Could perhaps use SqlGeography and/or DbGeography here rather than byte[].
            //See http://stackoverflow.com/questions/23186832/entity-framework-sqlgeography-vs-dbgeography
            //and http://stackoverflow.com/questions/15107977/sql-geography-to-dbgeography/29200641#29200641
            return UseNativeSqlTypes
                ? "SqlGeography"
                : "byte[]";
        }
        if (sqlDataType == "geometry")
        {
            return UseNativeSqlTypes
                ? "SqlGeometry"
                : "byte[]";
        }
        if (sqlDataType == "hierarchyid")
        {
            //Perhaps we could do something else here??
            //See http://blogs.msdn.com/b/jimoneil/archive/2009/02/23/h-is-for-hierarchyid.aspx
            return UseNativeSqlTypes
                ? "SqlHierarchyId"
                : "byte[]";
        }
        if (sqlDataType == "smallmoney")
        {
            return "decimal";
        }
        if (sqlDataType == "varbinary")
        {
            return "byte[]";
        }
        if (sqlDataType == "smallmoney")
        {
            return "decimal";
        }
        if (sqlDataType == "meric")
        {
            return "decimal";
        }
        if (sqlDataType == "binary")
        {
            return "byte[]";
        }
        if (sqlDataType == "char")
        {
            return "string";
        }
        if (sqlDataType == "datetime2")
        {
            return "DateTime";
        }
        if (sqlDataType == "datetimeoffset")
        {
            return "DateTimeOffset";
        }
        if (sqlDataType == "sysname")
        {
            return "string";
        }
        if (sqlDataType == "xml")
        {
            return "XElement";
        }
            
        throw new NotImplementedException(
            $"SQL column type {sqlDataType} cannot be translated to a C# property type");
    }
}
    
public class SqlColumnDeclaration
{
    public SqlColumnDeclaration(string name, NullableSqlType nullableSqlType)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(name));
        Name            = name;
        NullableSqlType = nullableSqlType ?? throw new ArgumentNullException(nameof(nullableSqlType));
    }

    public string          Name            { get; set; }
    public NullableSqlType NullableSqlType { get; set; }

    public string ToCreateDeclarationSegment()
    {
        return $"{Name} {NullableSqlType}";
    }

    public override string ToString()
    {
        return ToCreateDeclarationSegment();
    }
}

public class NullableSqlType
{
    public NullableSqlType(SqlType sqlType, bool isNullable)
    {
        SqlType    = sqlType ?? throw new ArgumentNullException(nameof(sqlType));
        IsNullable = isNullable;
    }

    public bool    IsNullable { get; set; }
    public SqlType SqlType    { get; set; }

    public override string ToString()
    {
        return IsNullable
            ? SqlType + " NULL"
            : SqlType + " NOT NULL";
    }
}