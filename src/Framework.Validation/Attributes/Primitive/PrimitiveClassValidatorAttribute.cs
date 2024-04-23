using Framework.Core;
using Microsoft.Extensions.DependencyInjection;

namespace Framework.Validation;

[AttributeUsage(AttributeTargets.Class)]
public class PrimitiveClassValidatorAttribute : ClassValidatorAttribute
{
    private readonly Type validatorType;


    public PrimitiveClassValidatorAttribute(Type validatorType)
    {
        if (validatorType == null) throw new ArgumentNullException(nameof(validatorType));

        this.validatorType = validatorType;
    }


    public override IClassValidator CreateValidator(IServiceProvider serviceProvider)
    {
        return (IClassValidator)ActivatorUtilities.CreateInstance(serviceProvider, this.validatorType);
    }
}
