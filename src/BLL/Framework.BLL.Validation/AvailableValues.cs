using System.Data.SqlTypes;

using Anch.Core;

using Framework.Core;

namespace Framework.BLL.Validation;

public record AvailableValues(Range<decimal> DecimalRange, Range<DateTime> DateTimeRange, int DefaultMaxStringSize)
    : IRangeContainer<decimal>, IRangeContainer<DateTime>, ISizeContainer<string>
{
    Range<decimal> IRangeContainer<decimal>.Range => this.DecimalRange;

    Range<DateTime> IRangeContainer<DateTime>.Range => this.DateTimeRange;

    int ISizeContainer<string>.Size => this.DefaultMaxStringSize;

    public static readonly AvailableValues Infinity = new(Range<decimal>.Infinity, Range<DateTime>.Infinity, int.MaxValue);

    public static readonly AvailableValues Default =
        ((decimal)Math.Pow(10, 15) - 1M)
        .Pipe(decimalLimit =>
                  new AvailableValues(
                      new Range<decimal>(-decimalLimit, decimalLimit),
                      new Range<DateTime>(SqlDateTime.MinValue.Value, SqlDateTime.MaxValue.Value),
                      byte.MaxValue));
}
