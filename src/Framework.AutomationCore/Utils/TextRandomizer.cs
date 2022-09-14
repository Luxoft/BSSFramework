using System;
using System.Text;

namespace Automation.Utils;

public static class TextRandomizer
{
    private static readonly Random Rnd = new Random();

    public static string UniqueString(string startsWith = "") => $"{startsWith}{Rnd.Next(1000000, 9999999)}";

    public static string RandomString(string startsWith, int size) => $"{startsWith}{RandomString(size - startsWith.Length)}";

    public static string RandomString(int size)
    {
        var builder = new StringBuilder();

        for (var i = 0; i < size; i++)
        {
            var ch = Convert.ToChar(Convert.ToInt32(Math.Floor((26 * Rnd.NextDouble()) + 65)));
            builder.Append(ch);
        }

        return builder.ToString();
    }
}