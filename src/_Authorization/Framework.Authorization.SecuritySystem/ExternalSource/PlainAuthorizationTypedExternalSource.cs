using Framework.Authorization.Domain;
using Framework.DomainDriven.Repository;
using Framework.Persistent;
using Framework.SecuritySystem;
using Microsoft.Extensions.DependencyInjection;

namespace Framework.Authorization.SecuritySystem.ExternalSource;

public class PlainAuthorizationTypedExternalSource<TSecurityContext> : AuthorizationTypedExternalSourceBase<TSecurityContext>
    where TSecurityContext : class, IIdentityObject<Guid>, ISecurityContext
{
    private readonly ISecurityContextDisplayService<TSecurityContext> displayService;

    public PlainAuthorizationTypedExternalSource(
        [FromKeyedServices(nameof(SecurityRule.Disabled))] IRepository<TSecurityContext> securityContextRepository,
        ISecurityContextDisplayService<TSecurityContext> displayService)
        : base(securityContextRepository)
    {
        this.displayService = displayService;
    }

    protected override SecurityEntity CreateSecurityEntity(TSecurityContext securityContext) =>

        new SecurityEntity
        {
            Name = this.displayService.ToString(securityContext),
            Id = securityContext.Id
        };

    protected override IEnumerable<TSecurityContext> GetSecurityEntitiesWithMasterExpand(TSecurityContext startSecurityObject)
    {
        return new[] { startSecurityObject };
    }
}
