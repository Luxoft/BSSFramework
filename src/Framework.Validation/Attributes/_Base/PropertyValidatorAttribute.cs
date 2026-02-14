using System.Reflection;

namespace Framework.Validation;

[AttributeUsage(AttributeTargets.Property)]
public abstract class PropertyValidatorAttribute : ValidatorAttribute
{
    protected PropertyValidatorAttribute()
    {

    }


    public abstract IPropertyValidator CreateValidator(PropertyInfo property, IServiceProvider serviceProvider);
}
