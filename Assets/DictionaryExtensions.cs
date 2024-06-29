using System.Collections.Generic;
using UnityEngine;

public static class DictionaryExtensions
{
    public static void AddOrUpdate<TKey>(this Dictionary<TKey, int> dictionary, TKey key, int value)
    {
        if (dictionary.TryGetValue(key, out int existingValue))
        {
            dictionary[key] = existingValue + value;
        }
        else
        {
            dictionary[key] = value;
        }
    }
}