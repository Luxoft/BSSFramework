namespace Framework.Core;

public static class UriExtensions
{
    public static Dictionary<string, List<string>> GetParameters(this Uri uri, char delimiter)
    {
        var subString = uri.ToString().After('?');
        var strings = subString.Split('&');

        return strings.Where(z => !string.IsNullOrEmpty(z)).Select(argument => argument.Split('=')).ToDictionary(values => values[0].ToLower(), values => values[1].Split(delimiter).ToList());
    }
}
