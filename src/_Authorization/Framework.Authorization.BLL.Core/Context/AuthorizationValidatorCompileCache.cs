using System;

using Framework.Core;
using Framework.Validation;

using Framework.DomainDriven;
using Framework.DomainDriven.BLL;

using AvailableValues = Framework.DomainDriven.AvailableValues;

namespace Framework.Authorization.BLL;

public class AuthorizationValidatorCompileCache : ValidatorCompileCache
{
    public AuthorizationValidatorCompileCache(AvailableValues availableValues) :
            base(availableValues
                 .ToValidation()
                 .ToBLLContextValidationExtendedData<IAuthorizationBLLContext, Framework.Authorization.Domain.PersistentDomainObjectBase, Guid>()
                 .Pipe(extendedValidationData => new AuthorizationValidationMap(extendedValidationData)))
    {
    }
}
