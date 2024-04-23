namespace Framework.Validation;

[AttributeUsage(AttributeTargets.Class)]
public abstract class ClassValidatorAttribute : ValidatorAttribute
{
    protected ClassValidatorAttribute()
    {

    }


    public abstract IClassValidator CreateValidator(IServiceProvider serviceProvider);
}
