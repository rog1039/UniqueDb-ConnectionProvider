using UniqueDb.ConnectionProvider.CoreTypes;
using UniqueDb.ConnectionProvider.Infrastructure.Extensions;

namespace UniqueDb.ConnectionProvider.SqlScriptingEngine.Attempt2;

public abstract class SqlTemplateNode
{
   protected readonly IReadOnlyList<SqlTemplateNode> EmptyTemplateList = new List<SqlTemplateNode>();

   public virtual string GetSql()
   {
      var output = Children()
         .Select(x => x.GetSql())
         .StringJoin(" ");
      return output;
   }

   public override string ToString()
   {
      return GetSql();
   }

   public virtual IEnumerable<SqlTemplateNode> Children() => EmptyTemplateList;
}

public class SqlTextNode : SqlTemplateNode
{
   public string Text { get; set; }


   public override string GetSql()
   {
      return Text;
   }

   public override IEnumerable<SqlTemplateNode> Children()
   {
      return EmptyTemplateList;
   }
}

public class ColumnTemplate : SqlTemplateNode
{
   public ColumnNameTemplate         ColumnNameTemplate         { get; set; }
   public ColumnTypeTemplate         ColumnTypeTemplate         { get; set; }
   public ColumnNullabilityTemplate  ColumnNullabilityTemplate  { get; set; }
   public ColumnTemporalTimeTemplate ColumnTemporalTimeTemplate { get; set; }
   public ComputedColumnTemplate     ComputedColumnTemplate     { get; set; }

   public ColumnTemplate() { }

   public ColumnTemplate(string columnName,
                         string computedDefinition)
   {
      ColumnNameTemplate     = new ColumnNameTemplate() { ColumnName     = columnName };
      ComputedColumnTemplate = new ComputedColumnTemplate() { Definition = computedDefinition };
   }

   public ColumnTemplate(string             columnName,
                         SqlType            columnType,
                         bool               allowNull,
                         TemporalColumnType temporalColumnType)
   {
      ColumnNameTemplate         = new ColumnNameTemplate() { ColumnName                 = columnName };
      ColumnTypeTemplate         = new ColumnTypeTemplate() { SqlType                    = columnType };
      ColumnNullabilityTemplate  = new ColumnNullabilityTemplate() { AllowNull           = allowNull };
      ColumnTemporalTimeTemplate = new ColumnTemporalTimeTemplate() { TemporalColumnType = temporalColumnType };
   }

   public override IEnumerable<SqlTemplateNode> Children()
   {
      if (ComputedColumnTemplate is null)
      {
         return new List<SqlTemplateNode>()
         {
            ColumnNameTemplate,
            ColumnTypeTemplate,
            ColumnTemporalTimeTemplate,
            ColumnNullabilityTemplate,
         };
      }
      else
      {
         return new List<SqlTemplateNode>()
         {
            ColumnNameTemplate,
            ComputedColumnTemplate,
         };
      }
   }
}

public class ColumnNameTemplate : SqlTemplateNode
{
   public string ColumnName { get; set; }

   public override string GetSql()
   {
      return ColumnName.BracketizeSafe();
   }
}

public class ColumnTypeTemplate : SqlTemplateNode
{
   public SqlType SqlType { get; set; }

   public override string GetSql()
   {
      return SqlType.ToString();
   }
}

public class ColumnNullabilityTemplate : SqlTemplateNode
{
   public bool AllowNull { get; set; }

   public override string GetSql()
   {
      return AllowNull
         ? "NULL"
         : "NOT NULL";
   }
}

public class ColumnTemporalTimeTemplate : SqlTemplateNode
{
   public TemporalColumnType TemporalColumnType { get; set; }

   public override string GetSql()
   {
      return TemporalColumnType switch
      {
         TemporalColumnType.Start => "GENERATED ALWAYS AS ROW START",
         TemporalColumnType.End   => "GENERATED ALWAYS AS ROW END",
         _                        => String.Empty,
      };
   }
}

public class ComputedColumnTemplate : SqlTemplateNode
{
   public string Definition { get; set; }

   public override string GetSql()
   {
      return $"AS ({Definition})";
   }
}

public enum TemporalColumnType
{
   None,
   Start,
   End
}

public class CreateTableTemplate : SqlTemplateNode
{
   public string SchemaName { get; set; }
   public string TableName  { get; set; }

   public List<ColumnTemplate> ColumnTemplates { get; private set; } = new();

   public override string GetSql()
   {
      var columnText = ColumnTemplates
         .Select(col => col.GetSql())
         .StringJoinCommaNewLine()
         .IndentWithTabs(1);

      return $"""
              CREATE TABLE [{SchemaName}].[{TableName}]
              (
              {columnText}
              )
              """;
   }
}

public class ConstraintTemplate : SqlTemplateNode
{
   public string              ConstraintName { get; set; }
   public ConstraintType      ConstraintType { get; set; }
   public ClusteredOrNot      ClusterType    { get; set; }
   public List<ColumnAndSort> ColumnAndSorts { get; set; }

   public override string GetSql()
   {
      var primaryKeyText = ConstraintType == ConstraintType.PrimaryKey ? "PRIMARY KEY " : "UNIQUE ";
      var clusterText    = ClusterType == ClusteredOrNot.Clustered ? "CLUSTERED" : "NONCLUSTERED";
      var columnText = ColumnAndSorts.Select(x => x.GetSql()).StringJoinCommaNewLine()
         .IndentWithTabs(1);
      return $"""
              CONSTRAINT {ConstraintName.BracketizeSafe()} {primaryKeyText}{clusterText}
              (
              {columnText}
              )
              """;
   }
}

public class ColumnAndSort : SqlTemplateNode
{
   public string        ColumnName    { get; init; }
   public SortDirection SortDirection { get; init; }

   public ColumnAndSort(string columnName, SortDirection sortDirection)
   {
      this.ColumnName    = columnName;
      this.SortDirection = sortDirection;
   }

   public override string GetSql()
   {
      var sortDir = SortDirection == SortDirection.Asc ? "ASC" : "DESC";
      return $"{ColumnName.BracketizeSafe()} {sortDir}";
   }
}

public enum SortDirection
{
   Asc,
   Desc
}

public enum ConstraintType
{
   PrimaryKey,
   Unique
}

public enum ClusteredOrNot
{
   Clustered,
   NonClustered
}

public class DefaultUsingSequenceTemplate : SqlTemplateNode
{
   public string ConstraintName { get; set; }
   public string SchemaName     { get; set; }
   public string SequenceName   { get; set; }
}

public class PeriodColumnDeclarationTemplate : SqlTemplateNode
{
   public string StartColumnName { get; set; }
   public string EndColumnName   { get; set; }

   public override string GetSql()
   {
      return $"PERIOD FOR SYSTEM TIME ({StartColumnName.BracketizeSafe()}, {EndColumnName.BracketizeSafe()})";
   }
}

public class EnableSystemVersioningTemplate : SqlTemplateNode
{
   public DbTableName HistoryTableName { get; set; }

   public override string GetSql()
   {
      return $"""
              WITH
              (
                SYSTEM_VERSIONING = ON (HISTORY_TABLE = {HistoryTableName})
              )
              """;
   }
}

public class AddIndexTemplate : SqlTemplateNode
{
   public DbTableName         TableName      { get; set; }
   public string              IndexName      { get; set; }
   public List<ColumnAndSort> ColumnAndSorts { get; set; }

   public override string GetSql()
   {
      var multiline = ColumnAndSorts.Count > 1;
      if (multiline)
         return $"""
                 CREATE INDEX {IndexName}
                   ON {TableName} (
                 {ColumnAndSorts.ColumnsAndSortsToStrings().IndentWithTabs(2)}
                    )
                 """;

      return $"""
              CREATE INDEX {IndexName}
                ON {TableName} ({ColumnAndSorts.ColumnsAndSortsToStrings()})
              """;
   }
}

public static class SqlTemplateExtensions
{
   public static string ColumnsAndSortsToStrings(this IEnumerable<ColumnAndSort> columnAndSorts)
   {
      return columnAndSorts
         .Select(x => x.GetSql())
         .StringJoinCommaNewLine();
   }
}