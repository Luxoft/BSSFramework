using System;

using Framework.Authorization.BLL.Core.Context;
using Framework.Authorization.Domain;
using Framework.DomainDriven.Repository;
using Framework.SecuritySystem;

namespace Framework.Authorization.BLL;

public class AuthorizationRepositoryFactory<TDomainObject> : RepositoryFactory<TDomainObject, Guid, AuthorizationSecurityOperationCode>,
                                                             IAuthorizationRepositoryFactory<TDomainObject>
        where TDomainObject : PersistentDomainObjectBase
{
    public AuthorizationRepositoryFactory(
            IServiceProvider serviceProvider,
            INotImplementedDomainSecurityServiceContainer notImplementedDomainSecurityServiceContainer,
            IDomainSecurityService<TDomainObject, AuthorizationSecurityOperationCode> domainSecurityService)
            : base(serviceProvider, notImplementedDomainSecurityServiceContainer, domainSecurityService)
    {
    }
}
