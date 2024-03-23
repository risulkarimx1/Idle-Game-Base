using System.Collections.Generic;

namespace GameCode.Utils
{
    public static class DictionaryUtils
    {
        public static Dictionary<TKey, TValue> Clone<TKey, TValue>(this Dictionary<TKey, TValue> original)
        {
            var clone = new Dictionary<TKey, TValue>();
            foreach (var entry in original)
            {
                clone.Add(entry.Key, entry.Value);
            }

            return clone;
        }
    }
}