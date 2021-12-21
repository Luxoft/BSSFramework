using System;

using Framework.Core;

namespace Framework.CustomReports.Services.ExcelBuilder
{
    internal static class CustomEvaluateExtensions
    {
        public static T GetMaybeValue<T>(this Maybe<T> maybeValue)
        {
            if (maybeValue == null) throw new ArgumentNullException(nameof(maybeValue));

            return maybeValue.GetValueOrDefault(default(T));
        }

        public static object GetNullableObjectValue<T>(this NullableObject<T> source) where T : struct
        {
            return source?.Value;
        }

        public static object GetValueOrNull<T>(this Nullable<T> source) where T : struct
        {
            if (source.HasValue)
            {
                return source.Value;
            }

            return null;
        }
    }
}
