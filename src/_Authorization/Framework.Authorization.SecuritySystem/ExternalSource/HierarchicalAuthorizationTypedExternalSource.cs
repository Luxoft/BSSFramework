using Framework.Authorization.Domain;
using Framework.Core;
using Framework.DomainDriven.Repository;
using Framework.HierarchicalExpand;
using Framework.Persistent;
using Framework.SecuritySystem;
using Microsoft.Extensions.DependencyInjection;

namespace Framework.Authorization.SecuritySystem.ExternalSource;

public class HierarchicalAuthorizationTypedExternalSource<TSecurityContext> : AuthorizationTypedExternalSourceBase<TSecurityContext>
    where TSecurityContext : class, IIdentityObject<Guid>, IActiveObject, IParentSource<TSecurityContext>, ISecurityContext
{
    private readonly ISecurityContextDisplayService<TSecurityContext> displayService;

    public HierarchicalAuthorizationTypedExternalSource(
        [FromKeyedServices(SecurityRule.Disabled)] IRepository<TSecurityContext> securityContextRepository,
        [FromKeyedServices(SecurityRule.Disabled)] IRepository<EntityType> entityTypeRepository,
        [FromKeyedServices(SecurityRule.Disabled)] IRepository<PermissionFilterEntity> permissionFilterEntityRepository,
        SecurityContextInfo<TSecurityContext, Guid> securityContextInfo,
        ISecurityContextDisplayService<TSecurityContext> displayService)
        : base(securityContextRepository, entityTypeRepository, permissionFilterEntityRepository, securityContextInfo)
    {
        this.displayService = displayService;
    }

    protected override SecurityEntity CreateSecurityEntity(TSecurityContext securityContext) =>

        new SecurityEntity
        {
            Active = securityContext.Active,
            Name = this.displayService.ToString(securityContext),
            Id = securityContext.Id,
            ParentId = securityContext.Parent.Maybe(v => v.Id)
        };

    protected override IEnumerable<TSecurityContext> GetSecurityEntitiesWithMasterExpand(TSecurityContext startSecurityObject)
    {
        return startSecurityObject.GetAllParents();
    }
}
