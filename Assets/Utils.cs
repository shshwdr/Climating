using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Utils : MonoBehaviour
{
    public static string StringifyDictionary<T>(Dictionary<string, T> dict)
    {
        StringBuilder sb = new StringBuilder();
        //sb.Append("{");

        bool first = true;
        foreach (var kvp in dict)
        {
            if (!first)
            {
                sb.Append(",");
            }
            first = false;

            sb.Append(kvp.Key);

            AppendValue(sb, kvp.Value);
        }

        //sb.Append("}");
        return sb.ToString();
    }

    private static void AppendValue<T>(StringBuilder sb, T value)
    {
        if (value is string)
        {
            sb.Append(value);
        }
        else if (value is Dictionary<string, object> dict)
        {
            sb.Append(StringifyDictionary(dict));
        }
        else if (value is List<object> list)
        {
            sb.Append(StringifyList(list));
        }
        else if (value is bool boolVal)
        {
            sb.Append(boolVal.ToString().ToLower());
        }
        else if (value is null)
        {
            sb.Append("null");
        }
        else
        {
            sb.Append(value);
        }
    }

    public static string StringifyList<T>(List<T> list)
    {
        StringBuilder sb = new StringBuilder();
        //sb.Append("");

        bool first = true;
        foreach (var item in list)
        {
            if (!first)
            {
                sb.Append(",");
            }
            first = false;

            AppendValue(sb, item);
        }

        //sb.Append("]");
        return sb.ToString();
    }
    
    
   public static string formatFloat(float myFloat)
    {
        if (myFloat % 1 == 0)
        {
            // 小数部分为零，只显示整数部分
            return myFloat.ToString("F0");
        }
        else
        {
            // 显示一位小数
            return myFloat.ToString("F1");
        }
    }
}
