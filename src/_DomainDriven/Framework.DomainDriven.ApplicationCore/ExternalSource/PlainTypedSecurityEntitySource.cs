using Framework.DomainDriven.Repository;
using Framework.SecuritySystem;

namespace Framework.DomainDriven.ApplicationCore.ExternalSource;

public class PlainTypedSecurityEntitySource<TSecurityContext>(
    [DisabledSecurity] IRepository<TSecurityContext> securityContextRepository,
    LocalStorage<TSecurityContext> localStorage,
    ISecurityContextDisplayService<TSecurityContext> displayService)
    : TypedSecurityEntitySourceBase<TSecurityContext>(securityContextRepository, localStorage)
    where TSecurityContext : class, ISecurityContext
{
    protected override SecurityEntity CreateSecurityEntity(TSecurityContext securityContext) =>

        new (securityContext.Id, displayService.ToString(securityContext), Guid.Empty);

    protected override IEnumerable<TSecurityContext> GetSecurityEntitiesWithMasterExpand(TSecurityContext startSecurityObject)
    {
        return new[] { startSecurityObject };
    }
}
