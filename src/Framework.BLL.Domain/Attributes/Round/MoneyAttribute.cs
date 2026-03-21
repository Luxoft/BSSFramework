using Framework.BLL.Domain.Extensions;

namespace Framework.BLL.Domain.Attributes.Round;

[AttributeUsage(AttributeTargets.Property)]
public class MoneyAttribute : RoundDecimalAttribute
{
    public MoneyAttribute() : base(NumberExtensions.MoneyRoundDecimals)
    {

    }
}
