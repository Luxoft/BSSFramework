using Framework.Core;

using Anch.Core;

namespace Framework.Validation;

public class AvailableValues(object source) : IAvailableValues
{
    public Range<T> GetAvailableRange<T>() =>

        (source as IRangeContainer<T>)?.Range ?? throw new InvalidOperationException($"Range for {typeof(T).Name} not found");

    public int GetAvailableSize<T>() => (source as ISizeContainer<T>).Maybe(c => c.Size);

    public static IAvailableValues Empty { get; } = new AvailableValues(new object());

    public static IAvailableValues Infinity { get; } = new InfinityAvailableValues();


    private class InfinityAvailableValues : IAvailableValues
    {
        public Range<T> GetAvailableRange<T>() => Range<T>.Infinity;

        public int GetAvailableSize<T>() => int.MaxValue;
    }
}
