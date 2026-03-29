using Framework.BLL.Domain.Extensions;

namespace Framework.BLL.Domain.Attributes.Round;

[AttributeUsage(AttributeTargets.Property)]
public class PercentAttribute  : RoundDecimalAttribute
{
    public PercentAttribute() : base(NumberExtensions.PercentRoundDecimals)
    {

    }
}
