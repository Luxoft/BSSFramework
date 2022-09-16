using Framework.Validation;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.Configuration.BLL;

public partial class ConfigurationValidator : IConfigurationValidator
{
    [ActivatorUtilitiesConstructor]
    public ConfigurationValidator(Framework.Configuration.BLL.IConfigurationBLLContext context, ConfigurationValidatorCompileCache cache) :
            this(context, (ValidatorCompileCache)cache)
    {
    }
}
