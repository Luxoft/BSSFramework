namespace Framework.Database.Mapping;

/// <summary>
/// Specifying size and precision for the decimal type
/// <see href="confluence/display/IADFRAME/LengthAndPrecisionAttribute"/>
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class LengthAndPrecisionAttribute(int length, int precision) : Attribute
{
    public static readonly LengthAndPrecisionAttribute Default = new(19, 4);

    public int Precision => precision;

    public int Length => length;
}
