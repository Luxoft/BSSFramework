using Framework.Core;
using Framework.Validation.Validators.DynamicClass.Available.Base;

namespace Framework.Validation.Validators.DynamicClass.Available;

public class AvailablePeriodValidator : RangeClassValidator<Period, DateTime>
{
    private AvailablePeriodValidator()
    {
    }


    protected override Func<Range<DateTime>, Period, bool> IsValidValueFunc { get; } = RangePropertyValidatorHelper.Period.AvailableFunc;


    public static AvailablePeriodValidator Value { get; } = new AvailablePeriodValidator();
}
