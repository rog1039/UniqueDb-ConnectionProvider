﻿using Microsoft.Data.SqlClient;

namespace UniqueDb.ConnectionProvider;

public class StaticSqlConnectionProvider : BaseSqlConnectionProvider
{
   public StaticSqlConnectionProvider(string serverName, string databaseName)
   {
      ServerName                  = serverName;
      DatabaseName                = databaseName;
      UseIntegratedAuthentication = true;
   }

   public StaticSqlConnectionProvider(string serverName, string databaseName, string userName, string password)
      : this(serverName, databaseName)
   {
      UserName                    = userName;
      Password                    = password;
      UseIntegratedAuthentication = false;
   }

   protected StaticSqlConnectionProvider() { }

   public StaticSqlConnectionProvider(string connectionString)
   {
      var connectionStringBuilder = new SqlConnectionStringBuilder(connectionString);

      ServerName   = connectionStringBuilder.DataSource;
      DatabaseName = connectionStringBuilder.InitialCatalog;

      var hasPassword = !string.IsNullOrWhiteSpace(connectionStringBuilder.Password);
      var hasUser     = !string.IsNullOrWhiteSpace(connectionStringBuilder.UserID);
      if (hasUser && hasPassword)
      {
         UserName                    = connectionStringBuilder.UserID;
         Password                    = connectionStringBuilder.Password;
         UseIntegratedAuthentication = false;
      }
      else
      {
         UseIntegratedAuthentication = true;
      }
   }

   public static ISqlConnectionProvider Blank => new StaticSqlConnectionProvider("", "");

   public override string ToString()
   {
      if (UseIntegratedAuthentication)
         return $"{ServerName}\\{DatabaseName} | IntegAuth: Yes";
      
      return $"{ServerName}\\{DatabaseName} | NoIntegAuth; User: {UserName}";
   }
}