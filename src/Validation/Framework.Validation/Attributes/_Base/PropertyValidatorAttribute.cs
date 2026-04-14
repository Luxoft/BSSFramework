using Framework.Validation.Validators;

// ReSharper disable once CheckNamespace
namespace Framework.Validation.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public abstract class PropertyValidatorAttribute : ValidatorAttribute
{
    protected PropertyValidatorAttribute()
    {

    }


    public abstract IPropertyValidator CreateValidator();
}
