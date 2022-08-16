using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace SampleSystem.IntegrationTests.__Support.Utils.Framework
{
    public static class StringUtil
    {
        private static readonly Random Random = new Random((int)DateTime.Now.Ticks);

        public static string UniqueString(string startsWith = "")
        {
            string ticks = startsWith + (DateTime.Now.Ticks - new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).Ticks);

            return ticks;
        }

        public static string RandomString(string startsWith, int size)
        {
            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < size - startsWith.Length; i++)
            {
                var ch = Convert.ToChar(Convert.ToInt32(Math.Floor((26 * Random.NextDouble()) + 65)));
                builder.Append(ch);
            }

            return startsWith + builder;
        }

        public static string RandomString(int size)
        {
            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < size; i++)
            {
                var ch = Convert.ToChar(Convert.ToInt32(Math.Floor((26 * Random.NextDouble()) + 65)));
                builder.Append(ch);
            }

            return builder.ToString();
        }

        public static string GetParameterValue(string value, string startsWith, string uniqueString)
        {
            if (value != null)
            {
                return value;
            }

            return startsWith + uniqueString;
        }

        public static string RemoveSpecialChars(this string message)
        {
            return Regex.Replace(message, "\n|\t|\r", string.Empty);
        }
    }
}
