using System.Collections.Generic;
using UnityEngine;

public static class DictionaryExtensions
{
    // int 类型的 AddOrUpdate 方法
    public static void AddOrUpdate(this Dictionary<string, int> dictionary, string key, int value)
    {
        if (dictionary.ContainsKey(key))
        {
            dictionary[key] += value;
        }
        else
        {
            dictionary[key] = value;
        }
    }

    // float 类型的 AddOrUpdate 方法
    public static void AddOrUpdate(this Dictionary<string, float> dictionary, string key, float value)
    {
        if (dictionary.ContainsKey(key))
        {
            dictionary[key] += value;
        }
        else
        {
            dictionary[key] = value;
        }
    }

    // List<T> 类型的 AddOrUpdate 方法
    public static void AddOrUpdate<T>(this Dictionary<string, List<T>> dictionary, string key, T value)
    {
        if (dictionary.ContainsKey(key))
        {
            dictionary[key].Add(value);
        }
        else
        {
            dictionary[key] = new List<T> { value };
        }
    }

    // // Dictionary<TKey, TValue> 类型的 AddOrUpdate 方法
    // public static void AddOrUpdate<TKey, TValue>(this Dictionary<string, Dictionary<TKey, TValue>> dictionary, string key, TKey subKey, TValue subValue)
    // {
    //     if (dictionary.ContainsKey(key))
    //     {
    //         dictionary[key][subKey] = subValue;
    //     }
    //     else
    //     {
    //         dictionary[key] = new Dictionary<TKey, TValue> { { subKey, subValue } };
    //     }
    // }
}