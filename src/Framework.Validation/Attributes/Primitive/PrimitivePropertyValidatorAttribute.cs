using Microsoft.Extensions.DependencyInjection;

namespace Framework.Validation;

[AttributeUsage(AttributeTargets.Property)]
public class PrimitivePropertyValidatorAttribute : PropertyValidatorAttribute
{
    private readonly Type validatorType;

    public PrimitivePropertyValidatorAttribute(Type validatorType)
    {
        if (validatorType == null) throw new ArgumentNullException(nameof(validatorType));

        this.validatorType = validatorType;
    }


    public override IPropertyValidator CreateValidator(IServiceProvider serviceProvider)
    {
        return (IPropertyValidator)ActivatorUtilities.CreateInstance(serviceProvider, this.validatorType);
    }
}
