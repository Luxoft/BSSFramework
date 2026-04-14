namespace Framework.BLL.Domain.Attributes.Round;

[AttributeUsage(AttributeTargets.Property)]
public class AwayFromZeroRoundDecimalAttribute(int decimals) : NormalizeAttribute
{
    public int Decimals { get; private set; } = decimals;
}
