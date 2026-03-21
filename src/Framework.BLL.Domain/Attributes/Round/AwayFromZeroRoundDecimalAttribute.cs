using Framework.BLL.Domain.Attributes.Round.Base;

namespace Framework.BLL.Domain.Attributes.Round;

[AttributeUsage(AttributeTargets.Property)]
public class AwayFromZeroRoundDecimalAttribute : NormalizeAttribute
{
    public AwayFromZeroRoundDecimalAttribute(int decimals)
    {
        this.Decimals = decimals;
    }

    public int Decimals { get; private set; }
}
