using System.Text.RegularExpressions;

public static class StringExtensions
{
    public static string ReplaceDoubleQuotes(this string str)
    {
        if (string.IsNullOrEmpty(str))
        {
            return str;
        }

        // 使用正则表达式替换连续的两个引号为一个引号
        return Regex.Replace(str, "\"{2}", "\"");
    }
}