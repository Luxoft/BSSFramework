using Framework.Core;
using Framework.Persistent;

using Framework.Authorization.Domain;
using Framework.DomainDriven.Repository;
using Framework.HierarchicalExpand;
using Framework.SecuritySystem;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.Authorization.SecuritySystem.ExternalSource;

public class AuthorizationExternalSource : IAuthorizationExternalSource
{
    private readonly IServiceProvider serviceProvider;

    private readonly ISecurityContextInfoService securityContextInfoService;

    private readonly IDictionaryCache<EntityType, IAuthorizationTypedExternalSource> typedCache;


    public AuthorizationExternalSource(IServiceProvider serviceProvider, ISecurityContextInfoService securityContextInfoService)
    {
        this.serviceProvider = serviceProvider;
        this.securityContextInfoService = securityContextInfoService;

        this.typedCache = new DictionaryCache<EntityType, IAuthorizationTypedExternalSource>(entityType => this.GetTypedInternal(entityType, true));
    }

    public IAuthorizationTypedExternalSource GetTyped(EntityType entityType, bool withCache = true)
    {
        if (entityType == null) throw new ArgumentNullException(nameof(entityType));

        return withCache ? this.typedCache[entityType] : this.GetTypedInternal(entityType, false);
    }

    private IAuthorizationTypedExternalSource GetTypedInternal(EntityType entityType, bool withCache)
    {
        if (entityType == null) throw new ArgumentNullException(nameof(entityType));

        var domainType = this.securityContextInfoService.GetSecurityContextInfo(entityType.Name).Type;

        var authorizationTypedExternalSourceType = entityType.Expandable
                                                           ? typeof(HierarchicalAuthorizationTypedExternalSource<>)
                                                           : typeof(PlainAuthorizationTypedExternalSource<>);

        var authorizationTypedExternalSourceImplType = authorizationTypedExternalSourceType.MakeGenericType(domainType);

        var result = (IAuthorizationTypedExternalSource)
            ActivatorUtilities.CreateInstance(this.serviceProvider, authorizationTypedExternalSourceImplType, entityType);

        return withCache ? result.WithCache() : result;
    }

    private abstract class AuthorizationTypedExternalSourceBase<TSecurityContext> : IAuthorizationTypedExternalSource
        where TSecurityContext : class, IIdentityObject<Guid>, ISecurityContext
    {
        private readonly IRepository<TSecurityContext> securityContextRepository;

        private readonly IRepository<EntityType> entityTypeRepository;

        private readonly IRepository<PermissionFilterEntity> permissionFilterEntityRepository;

        private readonly SecurityContextInfo securityContextInfo;

        protected AuthorizationTypedExternalSourceBase(
            IRepository<TSecurityContext> securityContextRepository,
            IRepository<EntityType> entityTypeRepository,
            IRepository<PermissionFilterEntity> permissionFilterEntityRepository,
            SecurityContextInfo securityContextInfo)
        {
            this.securityContextRepository = securityContextRepository;
            this.entityTypeRepository = entityTypeRepository;
            this.permissionFilterEntityRepository = permissionFilterEntityRepository;
            this.securityContextInfo = securityContextInfo;
        }

        protected abstract SecurityEntity CreateSecurityEntity(TSecurityContext securityContext);

        public IEnumerable<SecurityEntity> GetSecurityEntities()
        {
            return this.securityContextRepository.GetQueryable().ToList().Select(this.CreateSecurityEntity);
        }

        public IEnumerable<SecurityEntity> GetSecurityEntitiesByIdents(IEnumerable<Guid> preSecurityEntityIdents)
        {
            var securityEntityIdents = preSecurityEntityIdents.ToList();

            return this.securityContextRepository.GetQueryable().Where(obj => securityEntityIdents.Contains(obj.Id)).ToList()
                       .Select(this.CreateSecurityEntity);
        }

        public IEnumerable<SecurityEntity> GetSecurityEntitiesWithMasterExpand(Guid startSecurityEntityId)
        {
            var securityObject = this.securityContextRepository.GetQueryable().Single(obj => obj.Id == startSecurityEntityId);

            return this.GetSecurityEntitiesWithMasterExpand(securityObject).Select(this.CreateSecurityEntity);
        }

        protected abstract IEnumerable<TSecurityContext> GetSecurityEntitiesWithMasterExpand(TSecurityContext startSecurityObject);

        public PermissionFilterEntity AddSecurityEntity(SecurityEntity securityEntity, bool disableExistsCheck = false)
        {
            if (securityEntity == null) throw new ArgumentNullException(nameof(securityEntity));

            var entityType = this.entityTypeRepository.GetQueryable()
                                 .Single(entityType => entityType.Name == this.securityContextInfo.Name);

            var existsFilterEntity = this.permissionFilterEntityRepository
                                         .GetQueryable()
                                         .SingleOrDefault(v => v.EntityType == entityType && v.EntityId == securityEntity.Id);

            if (disableExistsCheck)
            {
                return existsFilterEntity ?? new PermissionFilterEntity
                {
                    EntityType = entityType,
                    EntityId = securityEntity.Id
                }.Self(v => this.permissionFilterEntityRepository.SaveAsync(v).GetAwaiter().GetResult());
            }
            else
            {
                var entity = this.securityContextRepository.GetQueryable().Single(obj => obj.Id == securityEntity.Id);

                return existsFilterEntity ?? new PermissionFilterEntity
                {
                    EntityType = entityType,
                    EntityId = entity.Id
                }.Self(v => this.permissionFilterEntityRepository.SaveAsync(v).GetAwaiter().GetResult());
            }
        }
    }

    private class PlainAuthorizationTypedExternalSource<TSecurityContext> : AuthorizationTypedExternalSourceBase<TSecurityContext>
            where TSecurityContext : class, IIdentityObject<Guid>, IActiveObject, ISecurityContext
    {
        private readonly ISecurityContextDisplayService<TSecurityContext> displayService;

        public PlainAuthorizationTypedExternalSource(
            IRepository<TSecurityContext> securityContextRepository,
            IRepository<EntityType> entityTypeRepository,
            IRepository<PermissionFilterEntity> permissionFilterEntityRepository,
            SecurityContextInfo securityContextInfo,
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

    private class HierarchicalAuthorizationTypedExternalSource<TSecurityContext> : AuthorizationTypedExternalSourceBase<TSecurityContext>
            where TSecurityContext : class, IIdentityObject<Guid>, IActiveObject, IParentSource<TSecurityContext>, ISecurityContext
    {
        private readonly ISecurityContextDisplayService<TSecurityContext> displayService;

        public HierarchicalAuthorizationTypedExternalSource(
            IRepository<TSecurityContext> securityContextRepository,
            IRepository<EntityType> entityTypeRepository,
            IRepository<PermissionFilterEntity> permissionFilterEntityRepository,
            SecurityContextInfo securityContextInfo,
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
}


