﻿using Framework.Authorization.Domain;
using Framework.Core;
using Framework.DomainDriven.Repository;
using Framework.Persistent;
using Framework.SecuritySystem;

namespace Framework.Authorization.SecuritySystem.ExternalSource;

public abstract class AuthorizationTypedExternalSourceBase<TSecurityContext> : IAuthorizationTypedExternalSource
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