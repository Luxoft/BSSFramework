using CommonFramework;

namespace Framework.Core;

public static class RangeExtensions
{
    public static Period ToPeriod(this Range<DateTime> range)
    {
        if (range == null) throw new ArgumentNullException(nameof(range));

        return new Period(range.Min, range.Max);
    }
}
