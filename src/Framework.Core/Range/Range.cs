namespace Framework.Core;

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
