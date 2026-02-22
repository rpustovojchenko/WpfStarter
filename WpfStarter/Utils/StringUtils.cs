namespace WpfStarter.Utils;

public static class StringUtils
{
    public static string Capitalize(string str)
    {
        if (string.IsNullOrWhiteSpace(str))
            return str;

        str = str.Trim();

        return char.ToUpper(str[0]) + str.Substring(1).ToLower();
    }
}