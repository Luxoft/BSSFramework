namespace Framework.Persistent.Mapping;

/// <summary>
/// Указание размера и точности для decimal типа
/// <see href="confluence/display/IADFRAME/LengthAndPrecisionAttribute"/>
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class LengthAndPrecisionAttribute : Attribute
{
    public static readonly LengthAndPrecisionAttribute Default = new LengthAndPrecisionAttribute(19,4);

    private readonly int length;
    private readonly int precision;

    public LengthAndPrecisionAttribute(int length, int precision)
    {
        this.length = length;
        this.precision = precision;
    }

    public int Precision => this.precision;

    public int Length => this.length;
}
