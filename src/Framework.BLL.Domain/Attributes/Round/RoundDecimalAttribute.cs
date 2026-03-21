using Framework.BLL.Domain.Attributes.Round.Base;

namespace Framework.BLL.Domain.Attributes.Round;

[AttributeUsage(AttributeTargets.Property)]
public class RoundDecimalAttribute : NormalizeAttribute
{
    public RoundDecimalAttribute(int decimals)
    {
        this.Decimals = decimals;
    }

    public int Decimals { get; private set; }
}
