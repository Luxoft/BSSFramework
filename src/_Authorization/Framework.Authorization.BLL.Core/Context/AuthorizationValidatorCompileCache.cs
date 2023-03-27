using Framework.Core;
using Framework.Validation;

using Framework.DomainDriven.BLL;

namespace Framework.Authorization.BLL;

public class AuthorizationValidatorCompileCache : ValidatorCompileCache
{
    public AuthorizationValidatorCompileCache(IAvailableValues availableValues) :
            base(availableValues
                 .ToBLLContextValidationExtendedData<IAuthorizationBLLContext, Framework.Authorization.Domain.PersistentDomainObjectBase, Guid>()
                 .Pipe(extendedValidationData => new AuthorizationValidationMap(extendedValidationData)))
    {
    }
}
