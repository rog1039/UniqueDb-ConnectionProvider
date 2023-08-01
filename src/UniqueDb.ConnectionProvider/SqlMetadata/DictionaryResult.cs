namespace UniqueDb.ConnectionProvider.SqlMetadata;

public class DictionaryResult<T>
{
   private DictionaryResult()
   {
      WasFound = false;
   }

   private DictionaryResult(T value)
   {
      WasFound = true;
      Value    = value;
   }

   public bool WasFound { get; }
   public T    Value    { get; }

   public static DictionaryResult<T> Found(T value)
   {
      var result = new DictionaryResult<T>(value);
      return result;
   }

   public static DictionaryResult<T> NotFound()
   {
      return new DictionaryResult<T>();
   }
}