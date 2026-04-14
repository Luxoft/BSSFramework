using System.Reflection;

using CommonFramework;

using Framework.Validation.Validators;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.Validation;

[AttributeUsage(AttributeTargets.Property)]
public class PrimitivePropertyValidatorAttribute(Type validatorType) : PropertyValidatorAttribute
{
    public override IPropertyValidator CreateValidator() => new PrimitivePropertyValidator(validatorType);

    public class PrimitivePropertyValidator(Type validatorType) : IDynamicPropertyValidator
    {
        public IPropertyValidator GetValidator(PropertyInfo _, IServiceProvider serviceProvider) =>
            serviceProvider.GetRequiredService<IServiceProxyFactory>().Create<IPropertyValidator>(validatorType);
    }
}
