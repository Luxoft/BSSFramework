using Framework.Validation.Validators;

// ReSharper disable once CheckNamespace
namespace Framework.Validation.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public abstract class ClassValidatorAttribute : ValidatorAttribute
{
    protected ClassValidatorAttribute()
    {

    }


    public abstract IClassValidator CreateValidator();
}
