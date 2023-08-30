namespace Framework.Validation;

[AttributeUsage(AttributeTargets.Property)]
public class MaxLengthValidatorAttribute : PropertyValidatorAttribute
{
    public int MaxLength { get; set; } = int.MaxValue;


    public override IPropertyValidator CreateValidator()
    {
        return new MaxLengthValidator(this.MaxLength);
    }
}
