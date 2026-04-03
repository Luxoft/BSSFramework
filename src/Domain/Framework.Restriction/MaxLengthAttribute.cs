namespace Framework.Restriction;

[AttributeUsage(AttributeTargets.Property)]
public class MaxLengthAttribute(int value = int.MaxValue) : Attribute, IRestrictionAttribute
{
    public int Value { get; } = value;
}
