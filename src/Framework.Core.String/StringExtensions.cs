using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using JetBrains.Annotations;

namespace Framework.Core
{
    public static class StringExtensions
    {
        public static string ReplaceAny([NotNull] this string source, [NotNull] IEnumerable<char> oldChars, char newChar)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (oldChars == null) throw new ArgumentNullException(nameof(oldChars));

            return oldChars.Aggregate(source, (current, c) => current.Replace(c, newChar));
        }

        public static bool Contains(this string str, string value, StringComparison stringComparison)
        {
            if (str == null) throw new ArgumentNullException(nameof(str));
            if (value == null) throw new ArgumentNullException(nameof(value));

            return str.IndexOf(value, stringComparison) != -1;
        }

        [Obsolete("v10 This method will be protected in future")]
        public static Guid ToGuid(this string str)
        {
            return new Guid(str);
        }

        public static string TakeWhile(this string input, Func<char, bool> predicate)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            return new string(Enumerable.TakeWhile(input, predicate).ToArray());
        }

        public static string Skip(this string input, string pattern)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));
            if (pattern == null) throw new ArgumentNullException(nameof(pattern));

            return input.Skip(pattern, StringComparison.CurrentCulture, false);
        }

        public static string Skip(this string input, string pattern, StringComparison stringComparison)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));
            if (pattern == null) throw new ArgumentNullException(nameof(pattern));

            return input.Skip(pattern, stringComparison, false);
        }

        public static string Skip(this string input, string pattern, bool raiseIfNotEquals)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));
            if (pattern == null) throw new ArgumentNullException(nameof(pattern));

            return input.Skip(pattern, StringComparison.CurrentCulture, raiseIfNotEquals);
        }

        public static Maybe<string> SkipMaybe(this string input, string pattern)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));
            if (pattern == null) throw new ArgumentNullException(nameof(pattern));

            return input.SkipMaybe(pattern, StringComparison.CurrentCulture);
        }

        public static Maybe<string> SkipMaybe(this string input, string pattern, StringComparison stringComparison)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));
            if (pattern == null) throw new ArgumentNullException(nameof(pattern));

            return Maybe.OfCondition(input.StartsWith(pattern, stringComparison), () => input.Skip(pattern, stringComparison, true));
        }

        public static Maybe<string> SkipLastMaybe(this string input, string pattern)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));
            if (pattern == null) throw new ArgumentNullException(nameof(pattern));

            return input.SkipLastMaybe(pattern, StringComparison.CurrentCulture);
        }

        public static Maybe<string> SkipLastMaybe(this string input, string pattern, StringComparison stringComparison)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));
            if (pattern == null) throw new ArgumentNullException(nameof(pattern));

            return Maybe.OfCondition(input.EndsWith(pattern, stringComparison), () => input.SkipLast(pattern, stringComparison, true));
        }

        public static string Skip(this string input, string pattern, StringComparison stringComparison, bool raiseIfNotEquals)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));
            if (pattern == null) throw new ArgumentNullException(nameof(pattern));

            if (input.StartsWith(pattern, stringComparison))
            {
                return input.Substring(pattern.Length, input.Length - pattern.Length);
            }
            else if (raiseIfNotEquals)
            {
                throw new System.ArgumentException($"Invalid input: {input}. Expected start element: {pattern}", nameof(input));
            }
            else
            {
                return input;
            }
        }

        public static string SkipLast(this string input, string pattern)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));
            if (pattern == null) throw new ArgumentNullException(nameof(pattern));

            return input.SkipLast(pattern, StringComparison.CurrentCulture, false);
        }

        public static string SkipLast(this string input, string pattern, StringComparison stringComparison)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));
            if (pattern == null) throw new ArgumentNullException(nameof(pattern));

            return input.SkipLast(pattern, stringComparison, false);
        }

        public static string SkipLast(this string input, string pattern, bool raiseIfNotEquals)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));
            if (pattern == null) throw new ArgumentNullException(nameof(pattern));

            return input.SkipLast(pattern, StringComparison.CurrentCulture, raiseIfNotEquals);
        }

        public static string SkipLast(this string input, string pattern, StringComparison stringComparison, bool raiseIfNotEquals)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));
            if (pattern == null) throw new ArgumentNullException(nameof(pattern));

            if (input.EndsWith(pattern, stringComparison))
            {
                return input.Substring(0, input.Length - pattern.Length);
            }
            else if (raiseIfNotEquals)
            {
                throw new System.ArgumentException($"Invalid input: {input}. Expected last element: {pattern}", nameof(input));
            }
            else
            {
                return input;
            }
        }

        public static string TakeWhileNot(this string input, string pattern)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));
            if (pattern == null) throw new ArgumentNullException(nameof(pattern));

            return input.TakeWhileNot(pattern, StringComparison.CurrentCulture);
        }

        public static string TakeWhileNot(this string input, string pattern, StringComparison stringComparison)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));
            if (pattern == null) throw new ArgumentNullException(nameof(pattern));

            var patterIndex = input.IndexOf(pattern, stringComparison);

            return patterIndex == -1 ? input : input.Substring(0, patterIndex);
        }

        public static string SubStringUnsafe(this string source, int length)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return source.Substring(0, Math.Min(length, source.Length));
        }

        public static string TrimNull(this string str)
        {
            // TODO gtsaplin: use String.Empty
            return (str ?? "").Trim();
        }

        public static string TrimNull(this string str, bool replaceNull)
        {
            if (replaceNull)
            {
                return str.TrimNull();
            }
            else
            {
                return str.Maybe(s => s.Trim());
            }
        }

        private static readonly string dirSer = Path.DirectorySeparatorChar.ToString();

        public static string ToDirectoryPath(this string str)
        {
            var s = str.TrimNull();

            return s == string.Empty || s.EndsWith(dirSer) ? s : s + dirSer;
        }

        public static string Join<T>(this IEnumerable<T> source, char separator)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return source.Join(separator.ToString());
        }

        public static string Join<T>(this IEnumerable<T> source, string separator)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (separator == null) throw new ArgumentNullException(nameof(separator));

            return string.Join(separator, source);
        }

        public static string Join<TSource, TResult>(this IEnumerable<TSource> source, char separator, Func<TSource, TResult> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return source.Select(selector).Join(separator);
        }

        public static string Join<TSource, TResult>(this IEnumerable<TSource> source, string separator, Func<TSource, TResult> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (separator == null) throw new ArgumentNullException(nameof(separator));

            return source.Select(selector).Join(separator);
        }

        public static string Concat<T>(this IEnumerable<T> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return string.Concat(source);
        }

        public static string Concat<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            return source.Select(selector).Concat();
        }

        [Obsolete("v10 This method will be protected in future")]
        public static string WithArgs(this string source, params object[] args)
        {
            return string.Format(source, args);
        }

        /// <summary>
        /// Переводит сроку в набор строк. По правилу:SendToApprove->Send to approve
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IEnumerable<string> SplitByUpper(this string source)
        {
            Func<char, char> processChar = z =>
                                               {
                                                   var count = 0;
                                                   return count++ > 0 ? char.ToLower(z) : z;
                                               };
            return source.While(z => char.IsUpper(z), processChar).Select(z => new string(z.ToArray()));
        }

        [Obsolete("v10 This method will be protected in future")]
        public static IEnumerable<string> SplitByCase(this string source)
        {
            var preRequest = from str in source.SplitByUpper()
                             select new string(new[] { char.ToUpper(str.First()) }.Concat(str.Skip(1)).ToArray());

            var l = preRequest.ToList();

            for (var i = 0; i < l.Count;)
            {
                if (char.IsUpper(l[i].Last()) && (i + 1) < l.Count && char.IsUpper(l[i + 1].Last()))
                {
                    l[i] += l[i + 1];
                    l.RemoveAt(i + 1);
                }
                else
                {
                    i++;
                }
            }

            return l;
        }

        [Obsolete("v10 This method will be protected in future")]
        public static string JoinSplitByCase(this string source, string joinSeparator = " ")
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return source.SplitByCase().Join(joinSeparator);
        }

        [Obsolete("v10 This method will be protected in future")]
        public static string After(this string source, char afterChar)
        {
            return new string(source.GetCharsAfter(afterChar).ToArray());
        }

        [Obsolete("v10 This method will be protected in future")]
        public static IEnumerable<char> GetChartWhile(this string source, char stopChar)
        {
            foreach (var s in source)
            {
                if (s == stopChar)
                {
                    yield break;
                }
                yield return s;
            }
        }

        [Obsolete("v10 This method will be protected in future")]
        public static IEnumerable<char> GetCharsAfter(this string source, char afterChar)
        {
            bool isReturn = false;
            foreach (var s in source)
            {
                if (isReturn)
                {
                    yield return s;
                }
                else
                {
                    if (s == afterChar)
                    {
                        isReturn = true;
                    }
                }
            }
        }

        [Obsolete("v10 This method will be protected in future")]
        public static string ToStartUpperCase(this string source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return source.Any() ? char.ToUpper(source.First()) + source.Substring(1) : source;
        }

        [Obsolete("v10 This method will be protected in future")]
        public static string ToStartLowerCase(this string source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return source.Any() ? char.ToLower(source.First()) + source.Substring(1) : source;
        }

        [Obsolete("v10 This method will be protected in future")]
        public static string NullIfEmpty(this string value)
        {
            return string.IsNullOrWhiteSpace(value) ? null : value;
        }

        [Obsolete("v10 This method will be protected in future")]
        public static bool IsNullOrWhiteSpace(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        [Obsolete("v10 This method will be protected in future")]
        public static string JoinLines(this string value, string separator = " ")
        {
            return value.Replace("\r\n", separator)
                        .Replace("\r", separator)
                        .Replace("\n", separator);
        }

        [Obsolete("v10 This method will be protected in future")]
        public static string ToPluralize(this string source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return source.SkipLastMaybe("y").Select(v => v + "ies")
         .Or(() => source.SkipLastMaybe("ss").Select(v => v + "sses"))
         .GetValueOrDefault(() => source + "s");
        }

        [Obsolete("v10 This method will be protected in future")]
        public static string FromPluralize(this string source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return source.SkipLastMaybe("ies").Select(v => v + "y")
         .Or(() => source.SkipLastMaybe("sses"))
         .Or(() => source.SkipLastMaybe("s"))
         .GetValueOrDefault(source);
        }
    }
}
