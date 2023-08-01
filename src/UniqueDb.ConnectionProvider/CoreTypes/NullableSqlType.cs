namespace UniqueDb.ConnectionProvider.CoreTypes;

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