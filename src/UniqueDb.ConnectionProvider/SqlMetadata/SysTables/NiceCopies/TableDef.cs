using UniqueDb.ConnectionProvider.SqlMetadata.SysTables.ExactCopies;

namespace UniqueDb.ConnectionProvider.SqlMetadata.SysTables.NiceCopies;

public class TableDef
{
   public SysTables_Exact SysTablesExact { get; set; }

   public List<ColumnDef>            ColumnDefs            { get; set; }
   public List<IndexDef>             IndexDefs             { get; set; }
   public List<ForeignKeyDef>        ForeignKeyDefs        { get; set; }
   public List<CheckConstraintDef>   CheckConstraintDefs   { get; set; }
   public List<DefaultConstraintDef> DefaultConstraintDefs { get; set; }
}

public class ColumnDef
{
   public SysColumn_Exact          SysColumnExact         { get; set; }
   public SysIdentity_Column_Exact SysIdentityColumnExact { get; set; }
   public SysComputed_Column_Exact SysComputedColumnExact { get; set; }
}

public class IndexDef { }

public class IndexColumnDef { }

public class ForeignKeyDef { }

public class ForeignKeyColumnDef { }

public class CheckConstraintDef { }

public class DefaultConstraintDef { }
