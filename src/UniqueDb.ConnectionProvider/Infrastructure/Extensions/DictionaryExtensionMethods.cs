using UniqueDb.ConnectionProvider.SqlMetadata;

namespace UniqueDb.ConnectionProvider.Infrastructure.Extensions;

internal static class DictionaryExtensionMethods
{
   public static Dictionary<K, IList<T>> ToDictionaryMany<T, K>(this IEnumerable<T> list, Func<T, K> keyFunc)
   {
      var dict = new Dictionary<K, IList<T>>();
      foreach (var item in list)
      {
         dict.AddToDictionary(keyFunc(item), item);
      }

      return dict;
   }

   public static DictionaryResult<TValue> TryGet<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key)
   {
      if (dict.TryGetValue(key, out var val))
      {
         return DictionaryResult<TValue>.Found(val);
      }

      return DictionaryResult<TValue>.NotFound();
   }

   public static void AddToDictionary<K, V>(this IDictionary<K, IList<V>> dict, K key, V value)
   {
      var existingList = dict.TryGet(key);
      if (existingList.WasFound)
         existingList.Value.Add(value);
      else
         dict.Add(key, new List<V>() { value });
   }
}