using Framework.Authorization.Domain;
using Framework.Authorization.SecuritySystem;
using Framework.Configurator.Interfaces;
using Framework.Core;
using Framework.DomainDriven.Repository;
using Framework.Persistent;
using Framework.SecuritySystem;

using Microsoft.AspNetCore.Http;

// ReSharper disable UnusedAutoPropertyAccessor.Local
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local

namespace Framework.Configurator.Handlers;

public record UpdatePermissionsHandler(
    IRepositoryFactory<Principal> PrincipalRepositoryFactory,
    IRepositoryFactory<BusinessRole> BusinessRoleRepositoryFactory,
    IRepositoryFactory<Permission> PermissionRepositoryFactory,
    IRepositoryFactory<PermissionRestriction> PermissionRestrictionRepositoryFactory,
    IRepositoryFactory<SecurityContextType> SecurityContextTypeRepositoryFactory,
    IPermissionValidator PermissionValidator,
    IConfiguratorIntegrationEvents? ConfiguratorIntegrationEvents = null) : BaseWriteHandler, IUpdatePermissionsHandler
{
    public async Task Execute(HttpContext context, CancellationToken cancellationToken)
    {
        var principalId = (string)context.Request.RouteValues["id"]!;
        var permissions = await this.ParseRequestBodyAsync<List<RequestBodyDto>>(context);

        await this.UpdateAsync(new Guid(principalId), permissions, cancellationToken);
    }

    private async Task UpdateAsync(Guid id, IEnumerable<RequestBodyDto> permissions, CancellationToken cancellationToken)
    {
        var principal = await this.PrincipalRepositoryFactory.Create().LoadAsync(id, cancellationToken);

        var mergeResult = principal.Permissions
                                   .GetMergeResult(
                                       permissions,
                                       x => x.Id,
                                       x => string.IsNullOrWhiteSpace(x.PermissionId) ? Guid.NewGuid() : new Guid(x.PermissionId));

        await this.CreatePermissionsAsync(principal, mergeResult.AddingItems, cancellationToken);
        await this.UpdatePermissionsAsync(mergeResult.CombineItems, cancellationToken);
        principal.RemoveDetails(mergeResult.RemovingItems);

        await this.PrincipalRepositoryFactory.Create(SecurityRole.Administrator).SaveAsync(principal, cancellationToken);
        if (this.ConfiguratorIntegrationEvents != null)
            foreach (var removingItem in mergeResult.RemovingItems)
                await this.ConfiguratorIntegrationEvents.PermissionRemovedAsync(removingItem, cancellationToken);

        principal.Permissions.Foreach(this.PermissionValidator.Validate);
    }

    private async Task CreatePermissionsAsync(Principal principal, IEnumerable<RequestBodyDto> dtoModels, CancellationToken token)
    {
        foreach (var dto in dtoModels)
        {
            var permission = new Permission(principal)
                             {
                                 Role = await this.BusinessRoleRepositoryFactory.Create().LoadAsync(new Guid(dto.RoleId), token),
                                 Comment = dto.Comment,
                                 Period = dto.StartDate.ToPeriod(dto.EndDate),
                                 Active = true
                             };

            await this.PermissionRepositoryFactory.Create(SecurityRole.Administrator).SaveAsync(permission, token);

            foreach (var context in dto.Contexts)
            {
                foreach (var contextEntity in context.Entities)
                    await this.PermissionRestrictionRepositoryFactory
                              .Create()
                              .SaveAsync(
                                  await this.CreatePermissionRestrictionAsync(
                                      permission,
                                      new Guid(context.Id),
                                      new Guid(contextEntity),
                                      token),
                                  token);
            }

            if (this.ConfiguratorIntegrationEvents != null)
                await this.ConfiguratorIntegrationEvents.PermissionCreatedAsync(permission, token);
        }
    }

    private async Task UpdatePermissionsAsync(IEnumerable<(Permission, RequestBodyDto)> updatedItems, CancellationToken token)
    {
        foreach (var item in updatedItems)
        {
            item.Item1.Comment = item.Item2.Comment;
            item.Item1.Period = item.Item2.StartDate.ToPeriod(item.Item2.EndDate);

            var mergeResult = item.Item1.Restrictions
                                  .GetMergeResult(
                                      item.Item2.Contexts.SelectMany(
                                          x => x.Entities.Select(
                                              e => new { TypeId = new Guid(x.Id), ObjectId = new Guid(e) })),
                                      x => new { TypeId = x.SecurityContextType.Id, ObjectId = x.SecurityContextId },
                                      x => new { x.TypeId, x.ObjectId });

            foreach (var permissionRestriction in mergeResult.AddingItems)
                await this.PermissionRestrictionRepositoryFactory.Create()
                          .SaveAsync(
                              await this.CreatePermissionRestrictionAsync(
                                  item.Item1,
                                  permissionRestriction.TypeId,
                                  permissionRestriction.ObjectId,
                                  token),
                              token);

            item.Item1.RemoveDetails(mergeResult.RemovingItems);

            await this.PermissionRepositoryFactory.Create(SecurityRole.Administrator).SaveAsync(item.Item1, token);

            if (this.ConfiguratorIntegrationEvents != null)
                await this.ConfiguratorIntegrationEvents.PermissionChangedAsync(item.Item1, token);
        }
    }

    private async Task<PermissionRestriction> CreatePermissionRestrictionAsync(
        Permission permission,
        Guid securityContextTypeId,
        Guid securityContextId,
        CancellationToken token) =>
        new(permission)
        {
            SecurityContextId = securityContextId,
            SecurityContextType = await this.SecurityContextTypeRepositoryFactory.Create().LoadAsync(securityContextTypeId, token)
        };

    private class RequestBodyDto
    {
        public string PermissionId { get; set; } = default!;

        public string RoleId { get; set; } = default!;

        public string Comment { get; set; } = default!;

        public List<ContextDto> Contexts { get; set; } = default!;

        public DateTime? EndDate { get; set; }

        public DateTime StartDate { get; set; }

        public class ContextDto
        {
            public string Id { get; set; } = default!;

            public List<string> Entities { get; set; } = default!;
        }
    }
}
