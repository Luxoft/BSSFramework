using Framework.Authorization.Domain;
using Framework.Core;
using Framework.DomainDriven.Repository;
using Framework.HierarchicalExpand;
using Framework.Persistent;
using Framework.SecuritySystem;
using Microsoft.Extensions.DependencyInjection;

namespace Framework.Authorization.SecuritySystem.ExternalSource;

public class HierarchicalAuthorizationTypedExternalSource<TSecurityContext> : AuthorizationTypedExternalSourceBase<TSecurityContext>
    where TSecurityContext : class, IIdentityObject<Guid>, IParentSource<TSecurityContext>, ISecurityContext
{
    private readonly ISecurityContextDisplayService<TSecurityContext> displayService;

    public HierarchicalAuthorizationTypedExternalSource(
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
            Id = securityContext.Id,
            ParentId = securityContext.Parent.Maybe(v => v.Id)
        };

    protected override IEnumerable<TSecurityContext> GetSecurityEntitiesWithMasterExpand(TSecurityContext startSecurityObject)
    {
        return startSecurityObject.GetAllParents();
    }
}
