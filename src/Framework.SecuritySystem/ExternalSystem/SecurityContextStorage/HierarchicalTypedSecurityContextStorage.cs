using Framework.Core;
using Framework.HierarchicalExpand;
using Framework.Persistent;
using Framework.QueryableSource;

namespace Framework.SecuritySystem.ExternalSystem.SecurityContextStorage;

public class HierarchicalTypedSecurityContextStorage<TSecurityContext>(
    IQueryableSource queryableSource,
    LocalStorage<TSecurityContext> localStorage,
    ISecurityContextDisplayService<TSecurityContext> displayService)
    : TypedSecurityContextStorageBase<TSecurityContext>(queryableSource, localStorage)
    where TSecurityContext : class, ISecurityContext, IParentSource<TSecurityContext>
{
    protected override SecurityContextData CreateSecurityContextData(TSecurityContext securityContext) =>

        new(securityContext.Id, displayService.ToString(securityContext), securityContext.Parent.Maybe(v => v.Id));

    protected override IEnumerable<TSecurityContext> GetSecurityContextsWithMasterExpand(TSecurityContext startSecurityObject)
    {
        return startSecurityObject.GetAllParents();
    }
}
