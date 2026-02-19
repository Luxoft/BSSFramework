using CommonFramework;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.Validation;

[AttributeUsage(AttributeTargets.Class)]
public class PrimitiveClassValidatorAttribute(Type validatorType) : ClassValidatorAttribute
{
    public override IClassValidator CreateValidator(IServiceProvider serviceProvider)
    {
        return serviceProvider.GetRequiredService<IServiceProxyFactory>().Create<IClassValidator>(validatorType);
    }
}
