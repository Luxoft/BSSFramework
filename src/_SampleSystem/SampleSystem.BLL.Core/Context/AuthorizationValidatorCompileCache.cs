using Framework.Core;
using Framework.Validation;

using Framework.DomainDriven.BLL;

namespace SampleSystem.BLL;

public class SampleSystemValidatorCompileCache : ValidatorCompileCache
{
    public SampleSystemValidatorCompileCache(IAvailableValues availableValues) :
            base(availableValues
                 .ToBLLContextValidationExtendedData<ISampleSystemBLLContext, SampleSystem.Domain.PersistentDomainObjectBase, Guid>()
                 .Pipe(extendedValidationData => new SampleSystemValidationMap(extendedValidationData)))
    {
    }
}
