namespace Framework.Restriction;

[AttributeUsage(AttributeTargets.Property)]
public class MaxLengthAttribute : Attribute, IRestrictionAttribute
{
    public MaxLengthAttribute()
            : this(int.MaxValue)
    {

    }

    public MaxLengthAttribute(int value)
    {
        this.Value = value;
    }


    public int Value { get; private set; }
}
