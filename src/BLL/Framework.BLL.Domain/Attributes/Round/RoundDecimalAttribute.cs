using Framework.BLL.Domain.Attributes.Round.Base;

namespace Framework.BLL.Domain.Attributes.Round;

[AttributeUsage(AttributeTargets.Property)]
public class RoundDecimalAttribute(int decimals) : NormalizeAttribute
{
    public int Decimals { get; private set; } = decimals;
}
