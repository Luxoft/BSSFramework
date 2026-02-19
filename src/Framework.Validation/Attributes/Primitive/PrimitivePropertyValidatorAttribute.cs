using System.Reflection;

using CommonFramework;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.Validation;

[AttributeUsage(AttributeTargets.Property)]
public class PrimitivePropertyValidatorAttribute(Type validatorType) : PropertyValidatorAttribute
{
    public override IPropertyValidator CreateValidator()
    {
        return new PrimitivePropertyValidator(validatorType);
    }

    public class PrimitivePropertyValidator(Type validatorType) : IDynamicPropertyValidator
    {
        public IPropertyValidator GetValidator(PropertyInfo _, IServiceProvider serviceProvider) =>
            serviceProvider.GetRequiredService<IServiceProxyFactory>().Create<IPropertyValidator>(validatorType);
    }
}
