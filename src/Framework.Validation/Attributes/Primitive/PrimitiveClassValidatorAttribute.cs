using CommonFramework;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.Validation;

[AttributeUsage(AttributeTargets.Class)]
public class PrimitiveClassValidatorAttribute(Type validatorType) : ClassValidatorAttribute
{
    public override IClassValidator CreateValidator()
    {
        return new PrimitiveClassValidator(validatorType);
    }

    public class PrimitiveClassValidator(Type validatorType) : IDynamicClassValidator
    {
        public IClassValidator GetValidator(Type _, IServiceProvider serviceProvider) =>
            serviceProvider.GetRequiredService<IServiceProxyFactory>().Create<IClassValidator>(validatorType);
    }
}
