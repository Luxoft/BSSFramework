using System;

using Framework.Core;
using Framework.Validation;

using Framework.DomainDriven;
using Framework.DomainDriven.BLL;

using AvailableValues = Framework.DomainDriven.AvailableValues;

namespace Framework.Configuration.BLL;

public class ConfigurationValidatorCompileCache : ValidatorCompileCache
{
    public ConfigurationValidatorCompileCache(AvailableValues availableValues) :
            base(availableValues
                 .ToValidation()
                 .ToBLLContextValidationExtendedData<IConfigurationBLLContext, Framework.Configuration.Domain.PersistentDomainObjectBase, Guid>()
                 .Pipe(extendedValidationData => new ConfigurationValidationMap(extendedValidationData)))
    {
    }
}
