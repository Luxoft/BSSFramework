// ReSharper disable once CheckNamespace
namespace Framework.Core;

public static class RangeExtensions
{
    public static Period ToPeriod(this Range<DateTime> range)
    {
        if (range is null) throw new ArgumentNullException(nameof(range));

        return new Period(range.Min, range.Max);
    }
}
