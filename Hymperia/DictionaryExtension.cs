using System.Collections.Generic;

namespace Hymperia.Facade
{
  public static class DictionaryExtension
  {
    public static void Deconstruct<TKey, TValue>(this KeyValuePair<TKey, TValue> pair, out TKey key, out TValue value)
    {
      key = pair.Key;
      value = pair.Value;
    }
  }
}
