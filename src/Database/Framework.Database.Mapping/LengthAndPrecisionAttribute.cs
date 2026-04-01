namespace Framework.Database.Mapping;

/// <summary>
/// Указание размера и точности для decimal типа
/// <see href="confluence/display/IADFRAME/LengthAndPrecisionAttribute"/>
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class LengthAndPrecisionAttribute(int length, int precision) : Attribute
{
    public static readonly LengthAndPrecisionAttribute Default = new LengthAndPrecisionAttribute(19,4);

    public int Precision => precision;

    public int Length => length;
}
