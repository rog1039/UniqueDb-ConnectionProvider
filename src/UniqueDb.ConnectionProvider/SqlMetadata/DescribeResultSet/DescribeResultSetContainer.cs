using UniqueDb.ConnectionProvider.SqlMetadata.SysTables;

namespace UniqueDb.ConnectionProvider.SqlMetadata.DescribeResultSet;

public class DescribeResultSetContainer
{
   public DescribeResultSetRow DescribeResultSetRow { get; set; }
   public SqlSysType?          UserDefinedType      { get; set; }
   public SqlSysType           SystemType           { get; set; }

   public DescribeResultSetContainer(DescribeResultSetRow describeResultSetRow, SqlSysType systemType)
   {
      DescribeResultSetRow = describeResultSetRow;
      SystemType           = systemType;
   }

   public DescribeResultSetContainer(DescribeResultSetRow describeResultSetRow, SqlSysType userDefinedType,
                                     SqlSysType           systemType)
   {
      DescribeResultSetRow = describeResultSetRow;
      UserDefinedType      = userDefinedType;
      SystemType           = systemType;
   }
}