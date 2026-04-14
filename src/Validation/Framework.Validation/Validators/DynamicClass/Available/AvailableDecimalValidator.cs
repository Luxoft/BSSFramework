using Framework.Core;
using Framework.Validation.Validators.DynamicClass.Available.Base;

namespace Framework.Validation.Validators.DynamicClass.Available;

public class AvailableDecimalValidator : RangeClassValidator<decimal, decimal>
{
    private AvailableDecimalValidator ()
    {
    }


    protected override Func<Range<decimal>, decimal, bool> IsValidValueFunc { get; } = RangePropertyValidatorHelper.Decimal.AvailableFunc;


    public static AvailableDecimalValidator Value { get; } = new AvailableDecimalValidator();
}
