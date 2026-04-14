using Framework.Validation.Attributes._Base;
using Framework.Validation.Validators;

namespace Framework.Validation.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class Int64ValueValidatorAttribute : PropertyValidatorAttribute
{
    public long Min { get; set; } = long.MinValue;

    public long Max { get; set; } = long.MaxValue;


    public override IPropertyValidator CreateValidator() => new Int64ValueValidator(this.Min, this.Max);
}
