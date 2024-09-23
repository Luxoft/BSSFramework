using Framework.Core;
using Framework.DomainDriven.Repository;
using Framework.HierarchicalExpand;
using Framework.Persistent;
using Framework.SecuritySystem;

namespace Framework.DomainDriven.ApplicationCore.ExternalSource;

public class HierarchicalTypedSecurityEntitySource<TSecurityContext>(
    [DisabledSecurity] IRepository<TSecurityContext> securityContextRepository,
    LocalStorage<TSecurityContext> localStorage,
    ISecurityContextDisplayService<TSecurityContext> displayService)
    : TypedSecurityEntitySourceBase<TSecurityContext>(securityContextRepository, localStorage)
    where TSecurityContext : class, ISecurityContext, IParentSource<TSecurityContext>
{
    protected override SecurityEntity CreateSecurityEntity(TSecurityContext securityContext) =>

        new(securityContext.Id, displayService.ToString(securityContext), securityContext.Parent.Maybe(v => v.Id));

    protected override IEnumerable<TSecurityContext> GetSecurityEntitiesWithMasterExpand(TSecurityContext startSecurityObject)
    {
        return startSecurityObject.GetAllParents();
    }
}
