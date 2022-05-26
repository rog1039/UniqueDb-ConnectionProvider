using StackExchange.Profiling.Internal;

namespace UniqueDb.ConnectionProvider.DataGeneration;

public static class DbTableNameWithNullableSchemaParser
{
   public static DbTableNameWithNullableSchema ParseFullTableName(string input)
   {
      var chars = input.AsSpan();

      bool insideBrackets  = false,
         seenSeparatingDot = false;

      string schemaName = String.Empty,
         tableName      = String.Empty,
         currentName    = String.Empty;

      for (var i = 0; i < chars.Length; i++)
      {
         char current = chars[i];
         switch (current)
         {
            case '.' when !insideBrackets:
            {
               if (seenSeparatingDot) throw new Exception("Seen more than 1 separating dot in name: {input}");

               //We are not in a bracket, so let's skip.
               schemaName        = currentName;
               currentName       = String.Empty;
               seenSeparatingDot = true;
               break;
            }
            case '[':
               insideBrackets = true;
               break;
            case ']':
               insideBrackets = false;
               break;
            default:
               currentName += current;
               break;
         }
      }

      if (schemaName.IsNullOrWhiteSpace()) return new DbTableNameWithNullableSchema(null, currentName);

      tableName = currentName;

      return new DbTableNameWithNullableSchema(schemaName, tableName);
   }
}