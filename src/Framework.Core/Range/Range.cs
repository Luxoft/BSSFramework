// ReSharper disable once CheckNamespace
namespace Framework.Core;

public record Range<T>(T Min, T Max)
{
    public static Range<T> Infinity => InfinityHelper.RangeValue;

    private static class InfinityHelper
    {
        private static readonly T MinValue = (T)typeof(T).GetField("MinValue")!.GetValue(null)!;

        private static readonly T MaxValue = (T)typeof(T).GetField("MaxValue")!.GetValue(null)!;

        public static readonly Range<T> RangeValue = new (MinValue, MaxValue);
    }
}
