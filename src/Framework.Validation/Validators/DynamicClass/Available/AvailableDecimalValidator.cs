using Framework.Core;
using Framework.Core.Range;

namespace Framework.Validation;

public class AvailableDecimalValidator : RangeClassValidator<decimal, decimal>
{
    private AvailableDecimalValidator ()
    {
    }


    protected override Func<Range<decimal>, decimal, bool> IsValidValueFunc { get; } = RangePropertyValidatorHelper.Decimal.AvailableFunc;


    public static AvailableDecimalValidator Value { get; } = new AvailableDecimalValidator();
}
