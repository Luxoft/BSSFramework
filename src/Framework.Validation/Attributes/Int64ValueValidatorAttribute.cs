namespace Framework.Validation;

[AttributeUsage(AttributeTargets.Property)]
public class Int64ValueValidatorAttribute : PropertyValidatorAttribute
{
    public long Min { get; set; } = long.MinValue;

    public long Max { get; set; } = long.MaxValue;


    public override IPropertyValidator CreateValidator(IServiceProvider serviceProvider)
    {
        return new Int64ValueValidator(this.Min, this.Max);
    }
}
