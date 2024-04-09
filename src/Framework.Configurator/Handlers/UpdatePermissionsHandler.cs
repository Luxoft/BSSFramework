using Framework.Authorization.Domain;
using Framework.Configurator.Interfaces;
using Framework.Core;
using Framework.DomainDriven.Repository;
using Framework.Persistent;
using Framework.SecuritySystem;
using Framework.SecuritySystem.Bss;

using Microsoft.AspNetCore.Http;

using NHibernate.Linq;

namespace Framework.Configurator.Handlers;

public record UpdatePermissionsHandler(
        IRepositoryFactory<Principal> PrincipalRepositoryFactory,
        IRepositoryFactory<BusinessRole> BusinessRoleRepositoryFactory,
        IRepositoryFactory<Permission> PermissionRepositoryFactory,
        IRepositoryFactory<PermissionFilterItem> PermissionFilterItemRepositoryFactory,
        IRepositoryFactory<PermissionFilterEntity> PermissionFilterEntityRepositoryFactory,
        IRepositoryFactory<EntityType> EntityTypeRepositoryFactory,
        IConfiguratorIntegrationEvents? ConfiguratorIntegrationEvents = null) : BaseWriteHandler, IUpdatePermissionsHandler
{
    public async Task Execute(HttpContext context, CancellationToken cancellationToken)
    {
        var principalId = (string?)context.Request.RouteValues["id"] ?? throw new InvalidOperationException();
        var permissions = await this.ParseRequestBodyAsync<List<RequestBodyDto>>(context);

        await this.Update(new Guid(principalId), permissions, cancellationToken);
    }

    private async Task Update(Guid id, IEnumerable<RequestBodyDto> permissions, CancellationToken cancellationToken)
    {
        var cache = new HashSet<PermissionFilterEntity>();
        var principalRepository = this.PrincipalRepositoryFactory.Create(SecurityRule.Edit);
        var principal = await principalRepository
                              .GetQueryable()
                              .Where(x => x.Id == id)
                              .SingleAsync(cancellationToken);

        var mergeResult = principal.Permissions.GetMergeResult(
                                                               permissions,
                                                               p => p.Id,
                                                               p => string.IsNullOrWhiteSpace(p.PermissionId)
                                                                            ? Guid.NewGuid()
                                                                            : new Guid(p.PermissionId));

        await this.CreatePermissions(principal, mergeResult.AddingItems, cache, cancellationToken);
        await this.UpdatePermissions(mergeResult.CombineItems, cache, cancellationToken);
        principal.RemoveDetails(mergeResult.RemovingItems);

        await principalRepository.SaveAsync(principal, cancellationToken);
        if (this.ConfiguratorIntegrationEvents != null)
        {
            foreach (var removingItem in mergeResult.RemovingItems)
            {
                await this.ConfiguratorIntegrationEvents.PermissionRemovedAsync(removingItem, cancellationToken);
            }
        }
    }

    private async Task CreatePermissions(
            Principal principal,
            IEnumerable<RequestBodyDto> dtoModels,
            ISet<PermissionFilterEntity> cache,
            CancellationToken cancellationToken)
    {
        foreach (var dto in dtoModels)
        {
            var permission = new Permission(principal)
                             {
                                     Role =
                                             await this.BusinessRoleRepositoryFactory.Create()
                                                       .GetQueryable()
                                                       .Where(x => x.Id == new Guid(dto.RoleId))
                                                       .SingleAsync(cancellationToken),
                                     Comment = dto.Comment,
                                     Period = dto.StartDate.ToPeriod(dto.EndDate),
                                     Active = true
                             };

            await this.PermissionRepositoryFactory.Create(SecurityRule.Edit).SaveAsync(permission, cancellationToken);

            foreach (var context in dto.Contexts)
            {
                foreach (var contextEntity in context.Entities)
                {
                    var filterEntity = await this.GetOrCreatePermissionFilterEntity(
                                                                                    (TypeId: new Guid(context.Id),
                                                                                        ObjectId: new Guid(contextEntity)),
                                                                                    cache,
                                                                                    cancellationToken);

                    await this.PermissionFilterItemRepositoryFactory
                              .Create()
                              .SaveAsync(new PermissionFilterItem(permission, filterEntity), cancellationToken);
                }
            }

            if (this.ConfiguratorIntegrationEvents != null)
            {
                await this.ConfiguratorIntegrationEvents.PermissionCreatedAsync(permission, cancellationToken);
            }
        }
    }

    private async Task UpdatePermissions(
            IList<ValueTuple<Permission, RequestBodyDto>> updatedItems,
            ISet<PermissionFilterEntity> cache,
            CancellationToken cancellationToken)
    {
        foreach (var item in updatedItems)
        {
            item.Item1.Comment = item.Item2.Comment;
            item.Item1.Period = item.Item2.StartDate.ToPeriod(item.Item2.EndDate);

            var mergeResult = item.Item1.FilterItems
                                  .GetMergeResult(
                                      item.Item2.Contexts.SelectMany(
                                          x => x.Entities.Select(
                                              e => new
                                                   {
                                                       TypeId = new Guid(x.Id),
                                                       ObjectId = new Guid(e)
                                                   })),
                                      x => new { TypeId = x.EntityType.Id, ObjectId = x.Entity.EntityId },
                                      x => new { x.TypeId, x.ObjectId });

            foreach (var filterEntityItem in mergeResult.AddingItems)
            {
                var filterEntity = await this.GetOrCreatePermissionFilterEntity(
                                                                                (filterEntityItem.TypeId,
                                                                                    filterEntityItem.ObjectId),
                                                                                cache,
                                                                                cancellationToken);
                await this.PermissionFilterItemRepositoryFactory.Create()
                          .SaveAsync(
                                     new PermissionFilterItem(item.Item1, filterEntity),
                                     cancellationToken);
            }

            item.Item1.RemoveDetails(mergeResult.RemovingItems);

            await this.PermissionRepositoryFactory.Create(SecurityRule.Edit).SaveAsync(item.Item1, cancellationToken);

            if (this.ConfiguratorIntegrationEvents != null)
            {
                await this.ConfiguratorIntegrationEvents.PermissionChangedAsync(item.Item1, cancellationToken);
            }
        }
    }

    private async Task<PermissionFilterEntity> GetOrCreatePermissionFilterEntity(
            (Guid TypeId, Guid ObjectId) key,
            ISet<PermissionFilterEntity> cache,
            CancellationToken cancellationToken)
    {
        var result = cache.SingleOrDefault(x => x.EntityType.Id == key.TypeId && x.EntityId == key.ObjectId)
                     ?? await this.PermissionFilterEntityRepositoryFactory.Create()
                                  .GetQueryable()
                                  .SingleOrDefaultAsync(
                                                        x => x.EntityType.Id == key.TypeId && x.EntityId == key.ObjectId,
                                                        cancellationToken);
        if (result != null)
        {
            return result;
        }

        var entityType = await this.EntityTypeRepositoryFactory
                                   .Create()
                                   .GetQueryable()
                                   .Where(x => x.Id == key.TypeId)
                                   .SingleAsync(cancellationToken);
        var filterEntity = new PermissionFilterEntity { EntityType = entityType, EntityId = key.ObjectId, Active = true };

        await this.PermissionFilterEntityRepositoryFactory.Create().SaveAsync(filterEntity, cancellationToken);
        cache.Add(filterEntity);

        return filterEntity;
    }

    private class RequestBodyDto
    {
        public string PermissionId
        {
            get;
            set;
        } = default!;

        public string RoleId
        {
            get;
            set;
        } = default!;

        public string Comment
        {
            get;
            set;
        } = default!;

        public List<ContextDto> Contexts
        {
            get;
            set;
        } = default!;

        public DateTime? EndDate { get; set; }

        public DateTime StartDate { get; set; }

        public class ContextDto
        {
            public string Id
            {
                get;
                set;
            } = default!;

            public List<string> Entities
            {
                get;
                set;
            } = default!;
        }
    }
}
