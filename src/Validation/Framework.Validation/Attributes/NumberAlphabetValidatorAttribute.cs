using Framework.Validation.Attributes._Base;
using Framework.Validation.Validators;

namespace Framework.Validation.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class NumberAlphabetValidatorAttribute : PropertyValidatorAttribute
{
    public string ExternalChars { get; set; }


    public override IPropertyValidator CreateValidator() => new NumberAlphabetValidator(this.ExternalChars);
}
