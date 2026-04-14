using Framework.Validation.Validators;

namespace Framework.Validation.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class EnglishAlphabetValidatorAttribute : PropertyValidatorAttribute
{
    public string ExternalChars { get; set; }


    public override IPropertyValidator CreateValidator() => new EnglishAlphabetValidator(this.ExternalChars);
}
