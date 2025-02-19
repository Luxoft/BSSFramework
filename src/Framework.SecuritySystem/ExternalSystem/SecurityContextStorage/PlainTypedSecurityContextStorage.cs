using Framework.QueryableSource;

namespace Framework.SecuritySystem.ExternalSystem.SecurityContextStorage;

public class PlainTypedSecurityContextStorage<TSecurityContext>(
    IQueryableSource queryableSource,
    LocalStorage<TSecurityContext> localStorage,
    ISecurityContextDisplayService<TSecurityContext> displayService)
    : TypedSecurityContextStorageBase<TSecurityContext>(queryableSource, localStorage)
    where TSecurityContext : class, ISecurityContext
{
    protected override SecurityContextData CreateSecurityContextData(TSecurityContext securityContext) =>

        new (securityContext.Id, displayService.ToString(securityContext), Guid.Empty);

    protected override IEnumerable<TSecurityContext> GetSecurityContextsWithMasterExpand(TSecurityContext startSecurityObject)
    {
        return new[] { startSecurityObject };
    }
}
