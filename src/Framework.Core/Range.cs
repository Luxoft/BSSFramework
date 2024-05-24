namespace Framework.Core;

public interface IRangeContainer<T>
{
    Range<T> Range { get; }
}

public interface ISizeContainer<T>
{
    int Size { get; }
}

public class Range<T>(T min, T max)
{
    public T Min { get; } = min;

    public T Max { get; } = max;

    public static Range<T> Infinity => RangeHelper.RangeValue;

    private static class RangeHelper
    {
        private static readonly T MinValue = (T)typeof(T).GetField("MinValue").GetValue(null);

        private static readonly T MaxValue = (T)typeof(T).GetField("MaxValue").GetValue(null);

        public static readonly Range<T> RangeValue = new Range<T>(MinValue, MaxValue);
    }
}

public static class RangeExtensions
{
    public static Period ToPeriod (this Range<DateTime> range)
    {
        if (range == null) throw new ArgumentNullException(nameof(range));

        return new Period(range.Min, range.Max);
    }
}
