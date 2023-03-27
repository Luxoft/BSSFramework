namespace Framework.Persistent;

[AttributeUsage(AttributeTargets.Property)]
public class RoundDecimalAttribute : NormalizeAttribute
{
    public RoundDecimalAttribute(int decimals)
    {
        this.Decimals = decimals;
    }

    public int Decimals { get; private set; }
}
