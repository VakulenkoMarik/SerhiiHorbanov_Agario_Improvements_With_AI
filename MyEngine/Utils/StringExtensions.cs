namespace MyEngine.Utils;

public static class StringExtensions
{
    public static string CutOffBefore(this string str, char after)
        => str.CutOffBefore(after.ToString());
    
    public static string CutOffBefore(this string str, string before)
    {
        int index = str.IndexOf(before);
        
        if (index == -1)
            return str;

        return str.Remove(0, index + 1);
    }
    
    public static string CutOffAfter(this string str, char after)
        => str.CutOffAfter(after.ToString());
    
    public static string CutOffAfter(this string str, string after)
    {
        int index = str.IndexOf(after);
        
        if (index == -1)
            return str;

        return str.Remove(index, str.Length - index);
    }

    public static string TrimFirstLast(this string str, char ch)
        => str.TrimFirstLast(ch, ch);
    
    public static string TrimFirstLast(this string str, char first, char last)
    {
        int substrLen = str.Length;
        int substrBegin = 0;
        
        if (str[0] == first)
        {
            substrLen--;
            substrBegin = 1;
        }
        if (str[^1] == last)
            substrLen--;
        
        return str.Substring(substrBegin, substrLen);
    }

    public static string TrimBrackets(this string str)
        => str.Trim('(', ')', '{', '}', '[', ']', '<', '>');
}