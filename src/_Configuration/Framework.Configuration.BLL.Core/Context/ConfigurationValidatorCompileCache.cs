using Framework.Validation;

namespace Framework.Configuration.BLL;

public class ConfigurationValidatorCompileCache : ValidatorCompileCache
{
    public ConfigurationValidatorCompileCache(ConfigurationValidationMap configurationValidationMap) :
            base(configurationValidationMap)
    {
    }
}
