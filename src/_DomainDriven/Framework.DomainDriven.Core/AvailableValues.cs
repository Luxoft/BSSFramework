using System;

using Framework.Core;

namespace Framework.DomainDriven
{
    public class AvailableValues :
        IRangeContainer<decimal>,
        IRangeContainer<DateTime>,
        ISizeContainer<string>
    {
        public AvailableValues(Range<decimal> decimalAvailableValues, Range<DateTime> dateTimeAvailableValues, int defaultMaxStringSize)
        {
            if (decimalAvailableValues == null) throw new ArgumentNullException(nameof(decimalAvailableValues));
            if (dateTimeAvailableValues == null) throw new ArgumentNullException(nameof(dateTimeAvailableValues));
            if (defaultMaxStringSize < 0) throw new ArgumentOutOfRangeException(nameof(defaultMaxStringSize));

            this.DecimalRange = decimalAvailableValues;
            this.DateTimeRange = dateTimeAvailableValues;
            this.DefaultMaxStringSize = defaultMaxStringSize;
        }


        public Range<decimal> DecimalRange { get; }

        public Range<DateTime> DateTimeRange { get; }

        public int DefaultMaxStringSize { get; }


        Range<decimal> IRangeContainer<decimal>.Range => this.DecimalRange;

        Range<DateTime> IRangeContainer<DateTime>.Range => this.DateTimeRange;

        int ISizeContainer<string>.Size => this.DefaultMaxStringSize;

        public static readonly AvailableValues Infinity = new AvailableValues(Range<decimal>.Infinity, Range<DateTime>.Infinity, int.MaxValue);
    }


    public static class AvailableValuesExtensions
    {
        public static Validation.IAvailableValues ToValidation(this AvailableValues availableValues)
        {
            if (availableValues == null) throw new ArgumentNullException(nameof(availableValues));

            return new Validation.AvailableValues(availableValues);
        }
    }
}