using System;

using Framework.Core;
using Framework.Validation;

using Framework.DomainDriven.BLL;

namespace Framework.Configuration.BLL;

public class ConfigurationValidatorCompileCache : ValidatorCompileCache
{
    public ConfigurationValidatorCompileCache(IAvailableValues availableValues) :
            base(availableValues
                 .ToBLLContextValidationExtendedData<IConfigurationBLLContext, Framework.Configuration.Domain.PersistentDomainObjectBase, Guid>()
                 .Pipe(extendedValidationData => new ConfigurationValidationMap(extendedValidationData)))
    {
    }
}
