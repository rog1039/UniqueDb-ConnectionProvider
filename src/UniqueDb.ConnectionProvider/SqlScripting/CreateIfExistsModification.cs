namespace UniqueDb.ConnectionProvider.SqlScripting;

public enum CreateIfExistsModification
{
   /// <summary>
   /// Try to create the table anyway without any special handling.
   /// </summary>
   CreateAnyway,
   /// <summary>
   /// Drop the existing table so it can be recreated.
   /// </summary>
   DropAndRecreate,
   /// <summary>
   /// Wrap the table creation in an If statement so create table will only execute if the table does not exist.
   /// </summary>
   CreateIfNotExists
}