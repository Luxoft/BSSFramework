using CommonFramework;

using Framework.Core;

namespace Framework.Validation;

public class AvailablePeriodValidator : RangeClassValidator<Period, DateTime>
{
    private AvailablePeriodValidator()
    {
    }


    protected override Func<Range<DateTime>, Period, bool> IsValidValueFunc { get; } = RangePropertyValidatorHelper.Period.AvailableFunc;


    public static AvailablePeriodValidator Value { get; } = new AvailablePeriodValidator();
}
