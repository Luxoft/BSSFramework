using Framework.Core;
using Framework.Validation.Validators.DynamicClass.Available.Base;

namespace Framework.Validation.Validators.DynamicClass.Available;

public class AvailableDateTimeValidator : RangeClassValidator<DateTime, DateTime>
{
    private AvailableDateTimeValidator()
    {
    }


    protected override Func<Range<DateTime>, DateTime, bool> IsValidValueFunc { get; } = RangePropertyValidatorHelper.DateTime.AvailableFunc;


    public static AvailableDateTimeValidator Value { get; } = new();
}
