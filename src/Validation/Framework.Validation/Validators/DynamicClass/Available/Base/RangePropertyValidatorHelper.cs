using Framework.Core;

namespace Framework.Validation.Validators.DynamicClass.Available.Base;

public static class RangePropertyValidatorHelper
{
    public static readonly RangePropertyValidatorInfo<DateTime, DateTime> DateTime = new((range, value) => range.ToPeriod().Contains(value));

    public static readonly RangePropertyValidatorInfo<DateTime, Period> Period = new((range, value) => range.ToPeriod().Contains(value));

    public static readonly RangePropertyValidatorInfo<decimal, decimal> Decimal = new((range, value) => range.Min <= value && value <= range.Max);


    public class RangePropertyValidatorInfo<TRange, TProperty>(Func<Range<TRange>, TProperty, bool> availableFunc)
        where TProperty : struct
    {
        public readonly Func<Range<TRange>, TProperty, bool> AvailableFunc = availableFunc;

        public RangePropertyValidator<TSource, TProperty, TRange> Create<TSource>(Range<TRange> availableRange)
        {
            if (availableRange == null) throw new ArgumentNullException(nameof(availableRange));

            return new RangePropertyValidator<TSource, TProperty, TRange>(availableRange, this.AvailableFunc);
        }

        public NullableRangePropertyValidator<TSource, TProperty, TRange> CreateNullable<TSource>(Range<TRange> availableRange)
        {
            if (availableRange == null) throw new ArgumentNullException(nameof(availableRange));

            return new NullableRangePropertyValidator<TSource, TProperty, TRange>(availableRange, this.AvailableFunc);
        }
    }
}
