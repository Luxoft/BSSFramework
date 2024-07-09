using Framework.Authorization.Domain;
using Framework.Core;
using Framework.DomainDriven.Repository;
using Framework.HierarchicalExpand;
using Framework.Persistent;
using Framework.SecuritySystem;

namespace Framework.Authorization.SecuritySystem.ExternalSource;

public class HierarchicalAuthorizationTypedExternalSource<TSecurityContext>(
    [DisabledSecurity] IRepository<TSecurityContext> securityContextRepository,
    LocalStorage<TSecurityContext> localStorage,
    ISecurityContextDisplayService<TSecurityContext> displayService)
    : AuthorizationTypedExternalSourceBase<TSecurityContext>(securityContextRepository, localStorage)
    where TSecurityContext : class, IIdentityObject<Guid>, IParentSource<TSecurityContext>, ISecurityContext
{
    protected override SecurityEntity CreateSecurityEntity(TSecurityContext securityContext) =>

        new SecurityEntity
        {
            Name = displayService.ToString(securityContext),
            Id = securityContext.Id,
            ParentId = securityContext.Parent.Maybe(v => v.Id)
        };

    protected override IEnumerable<TSecurityContext> GetSecurityEntitiesWithMasterExpand(TSecurityContext startSecurityObject)
    {
        return startSecurityObject.GetAllParents();
    }
}
