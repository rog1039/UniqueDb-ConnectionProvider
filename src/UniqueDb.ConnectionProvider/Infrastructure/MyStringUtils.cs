namespace UniqueDb.ConnectionProvider.Infrastructure;

public enum SearchNotFoundOption
{
   ReturnEmpty,
   ReturnAllText
}

internal class MyStringUtils
{
   public static string StartTo(string               text,
                                string               characterToStopAt,
                                SearchNotFoundOption searchNotFoundOption = SearchNotFoundOption.ReturnAllText)
   {
      var indexOfCharacterToStopAt = text.IndexOf(characterToStopAt);
      if (indexOfCharacterToStopAt < 0)
         return searchNotFoundOption == SearchNotFoundOption.ReturnAllText ? text : string.Empty;
      return text.Substring(0, indexOfCharacterToStopAt);
   }

   public static string EndTo(string               text,
                              string               characterToStopAt,
                              SearchNotFoundOption searchNotFoundOption = SearchNotFoundOption.ReturnEmpty)
   {
      var indexOfCharacterToStopAt = text.IndexOf(characterToStopAt);
      if (indexOfCharacterToStopAt < 0)
         return searchNotFoundOption == SearchNotFoundOption.ReturnAllText ? text : string.Empty;
      return text.Substring(indexOfCharacterToStopAt + 1, text.Length - indexOfCharacterToStopAt - 1);
   }
}