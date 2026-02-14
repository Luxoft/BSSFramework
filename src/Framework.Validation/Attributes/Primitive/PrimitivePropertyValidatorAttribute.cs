using System.Reflection;

using CommonFramework;

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


    public override IPropertyValidator CreateValidator(PropertyInfo property, IServiceProvider serviceProvider)
    {
        return serviceProvider.GetRequiredService<IServiceProxyFactory>().Create<IPropertyValidator>(this.validatorType);
    }
}
