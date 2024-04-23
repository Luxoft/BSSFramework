namespace Framework.Validation;

[AttributeUsage(AttributeTargets.Property)]
public class EnglishAlphabetValidatorAttribute : PropertyValidatorAttribute
{
    public string ExternalChars { get; set; }


    public override IPropertyValidator CreateValidator(IServiceProvider serviceProvider)
    {
        return new EnglishAlphabetValidator(this.ExternalChars);
    }
}
