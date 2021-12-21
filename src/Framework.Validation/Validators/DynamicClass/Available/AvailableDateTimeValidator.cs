using System;

using Framework.Core;

namespace Framework.Validation
{
    public class AvailableDateTimeValidator : RangeClassValidator<DateTime, DateTime>
    {
        private AvailableDateTimeValidator()
        {
        }


        protected override Func<Range<DateTime>, DateTime, bool> IsValidValueFunc { get; } = RangePropertyValidatorHelper.DateTime.AvailableFunc;


        public static AvailableDateTimeValidator Value { get; } = new AvailableDateTimeValidator();
    }
}