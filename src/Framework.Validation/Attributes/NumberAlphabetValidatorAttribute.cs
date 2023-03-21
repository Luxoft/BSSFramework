using System;

namespace Framework.Validation;

[AttributeUsage(AttributeTargets.Property)]
public class NumberAlphabetValidatorAttribute : PropertyValidatorAttribute
{
    public string ExternalChars { get; set; }


    public override IPropertyValidator CreateValidator()
    {
        return new NumberAlphabetValidator(this.ExternalChars);
    }
}
