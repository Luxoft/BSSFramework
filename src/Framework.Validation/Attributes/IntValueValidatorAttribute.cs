using System.Reflection;

namespace Framework.Validation;

[AttributeUsage(AttributeTargets.Property)]
public class IntValueValidatorAttribute : PropertyValidatorAttribute
{
    public int Min { get; set; } = int.MinValue;

    public int Max { get; set; } = int.MaxValue;


    public override IPropertyValidator CreateValidator(PropertyInfo property, IServiceProvider serviceProvider)
    {
        return new IntValueValidator(this.Min, this.Max);
    }
}
