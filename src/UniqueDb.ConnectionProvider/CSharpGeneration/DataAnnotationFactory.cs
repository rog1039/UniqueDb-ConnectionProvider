using UniqueDb.ConnectionProvider.CoreTypes;
using UniqueDb.ConnectionProvider.Infrastructure.Extensions;

namespace UniqueDb.ConnectionProvider.CSharpGeneration;

public static class DataAnnotationFactory
{
    public static IEnumerable<DataAnnotationDefinitionBase> CreateDataAnnotations(SqlColumn sqlColumn)
    {
        if (NeedsNumericRange(sqlColumn))
        {
            var numericType = sqlColumn.SqlDataType.As<SqlTypeNumberBase>();
            yield return
                DataAnnotationDefinitionNumericRange.FromRange(numericType.LowerBound, numericType.UpperBound);
        }

        if (NeedsCharRange(sqlColumn))
        {
            var maximumCharLength = sqlColumn.SqlDataType.TypeName.StartsWith("n") && sqlColumn.SqlDataType.MaximumCharLength > 1
                ? sqlColumn.SqlDataType.MaximumCharLength.Value/2
                : sqlColumn.SqlDataType.MaximumCharLength.Value;

            yield return new DataAnnotationDefinitionMaxCharacterLength(maximumCharLength);
        }

        if (sqlColumn.IsComputed == true)
        {
            yield return new DataAnnotationDefinitionIsComputed();
        }
        if (sqlColumn.IsIdentity == true)
        {
            yield return new DataAnnotationDefinitionIsIdentity();
        }
    }

    private static bool NeedsNumericRange(SqlColumn sqlColumn)
    {
        var numericSqlColumn = sqlColumn.SqlDataType as SqlTypeNumberBase;
        return numericSqlColumn != null;
    }

    private static bool NeedsCharRange(SqlColumn sqlColumn)
    {
        return sqlColumn.SqlDataType.MaximumCharLength.HasValue && sqlColumn.SqlDataType.MaximumCharLength != -1;
    }
}