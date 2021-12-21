using System;

namespace Framework.Core
{
    public static class DateTimeExtension
    {
        [Obsolete("v10 For Remove")]
        public static DateTime RoundDate(this DateTime source)
        {
            return new DateTime(source.Year, source.Month, source.Day);
        }

        [Obsolete("v10 For Remove")]
        public static DateTime? RoundDate(this DateTime? source)
        {
            return source.MaybeNullableToNullable(v => v.RoundDate());
        }
    }
}
