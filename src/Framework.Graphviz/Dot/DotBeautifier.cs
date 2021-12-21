using System.Text.RegularExpressions;

namespace Framework.Graphviz.Dot
{
    public static class DotBeautifier
    {
        public static string OptmizeDot(this string dot)
        {
            var result = dot;
            const string pattern = @"(label[\t ]*=[\t ]*[""'])([^""']+)([""'])";

            result = Regex.Replace(result, pattern, match => match.Groups[1] + "  " + match.Groups[2] + "  " + match.Groups[3]);

            return result;
        }
    }
}
