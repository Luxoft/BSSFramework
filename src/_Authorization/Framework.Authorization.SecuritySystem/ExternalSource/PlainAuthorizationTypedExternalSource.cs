using Framework.Authorization.Domain;
using Framework.DomainDriven.Repository;
using Framework.Persistent;
using Framework.SecuritySystem;
using Microsoft.Extensions.DependencyInjection;

namespace Framework.Authorization.SecuritySystem.ExternalSource;

public class PlainAuthorizationTypedExternalSource<TSecurityContext> : AuthorizationTypedExternalSourceBase<TSecurityContext>
    where TSecurityContext : class, IIdentityObject<Guid>, IActiveObject, ISecurityContext
{
    private readonly ISecurityContextDisplayService<TSecurityContext> displayService;

    public PlainAuthorizationTypedExternalSource(
        [FromKeyedServices(BLLSecurityMode.Disabled)] IRepository<TSecurityContext> securityContextRepository,
        [FromKeyedServices(BLLSecurityMode.Disabled)] IRepository<EntityType> entityTypeRepository,
        [FromKeyedServices(BLLSecurityMode.Disabled)] IRepository<PermissionFilterEntity> permissionFilterEntityRepository,
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
            Id = securityContext.Id
        };

    protected override IEnumerable<TSecurityContext> GetSecurityEntitiesWithMasterExpand(TSecurityContext startSecurityObject)
    {
        return new[] { startSecurityObject };
    }
}
