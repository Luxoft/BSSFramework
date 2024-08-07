﻿using Framework.Authorization.Domain;
using Framework.DomainDriven.Repository;
using Framework.Persistent;
using Framework.SecuritySystem;

namespace Framework.Authorization.SecuritySystem.ExternalSource;

public class PlainAuthorizationTypedExternalSource<TSecurityContext>(
    [DisabledSecurity] IRepository<TSecurityContext> securityContextRepository,
    LocalStorage<TSecurityContext> localStorage,
    ISecurityContextDisplayService<TSecurityContext> displayService)
    : AuthorizationTypedExternalSourceBase<TSecurityContext>(securityContextRepository, localStorage)
    where TSecurityContext : class, IIdentityObject<Guid>, ISecurityContext
{
    protected override SecurityEntity CreateSecurityEntity(TSecurityContext securityContext) =>

        new SecurityEntity
        {
            Name = displayService.ToString(securityContext),
            Id = securityContext.Id
        };

    protected override IEnumerable<TSecurityContext> GetSecurityEntitiesWithMasterExpand(TSecurityContext startSecurityObject)
    {
        return new[] { startSecurityObject };
    }
}
