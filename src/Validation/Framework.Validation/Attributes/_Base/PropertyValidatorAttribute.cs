using Framework.Validation.Validators;

namespace Framework.Validation.Attributes._Base;

[AttributeUsage(AttributeTargets.Property)]
public abstract class PropertyValidatorAttribute : ValidatorAttribute
{
    protected PropertyValidatorAttribute()
    {

    }


    public abstract IPropertyValidator CreateValidator();
}
