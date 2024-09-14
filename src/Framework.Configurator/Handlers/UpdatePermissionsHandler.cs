using Framework.Configurator.Interfaces;
using Framework.Core;
using Framework.DomainDriven.ApplicationCore.Security;
using Framework.SecuritySystem;
using Framework.SecuritySystem.ExternalSystem.Management;

using Microsoft.AspNetCore.Http;

namespace Framework.Configurator.Handlers;

public record UpdatePermissionsHandler(
    ISecuritySystemFactory SecuritySystemFactory,
    ISecurityRoleSource SecurityRoleSource,
    ISecurityContextSource SecurityContextSource,
    IPrincipalManagementService PrincipalManagementService,
    IConfiguratorIntegrationEvents? ConfiguratorIntegrationEvents = null) : BaseWriteHandler, IUpdatePermissionsHandler
{
    public async Task Execute(HttpContext context, CancellationToken cancellationToken)
    {
        this.SecuritySystemFactory.CheckAccess(ApplicationSecurityRule.SecurityAdministrator);

        var principalId = new Guid((string)context.Request.RouteValues["id"]!);
        var permissions = await this.ParseRequestBodyAsync<List<RequestBodyDto>>(context);

        var typedPermissions = permissions.Select(this.ToTypedPermission).ToList();

        var mergeResult = await this.PrincipalManagementService.UpdatePermissionsAsync(principalId, typedPermissions, cancellationToken);

        if (this.ConfiguratorIntegrationEvents != null)
        {
            foreach (var permissionId in mergeResult.AddingItems)
            {
                await this.ConfiguratorIntegrationEvents.PermissionCreatedAsync(permissionId, cancellationToken);
            }

            foreach (var (permissionId, _) in mergeResult.CombineItems)
            {
                await this.ConfiguratorIntegrationEvents.PermissionChangedAsync(permissionId, cancellationToken);
            }

            foreach (var permissionId in mergeResult.RemovingItems)
            {
                await this.ConfiguratorIntegrationEvents.PermissionRemovedAsync(permissionId, cancellationToken);
            }
        }
    }

    private TypedPermission ToTypedPermission(RequestBodyDto permission)
    {
        return new TypedPermission(
            string.IsNullOrWhiteSpace(permission.PermissionId) ? Guid.Empty : new Guid(permission.PermissionId),
            this.SecurityRoleSource.GetSecurityRole(new Guid(permission.RoleId)),
            new Period(permission.StartDate, permission.EndDate),
            permission.Comment,
            permission.Contexts.ToDictionary(
                pair => this.SecurityContextSource.GetSecurityContextInfo(new Guid(pair.Id)).Type,
                pair => pair.Entities.ToReadOnlyListI(e => new Guid(e))),
            permission.IsVirtual);
    }

    private class RequestBodyDto
    {
        public string PermissionId { get; set; } = default!;

        public string RoleId { get; set; } = default!;

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string Comment { get; set; } = default!;

        public List<ContextDto> Contexts { get; set; } = default!;

        public bool IsVirtual { get; set; }

        public class ContextDto
        {
            public string Id { get; set; } = default!;

            public List<string> Entities { get; set; } = default!;
        }
    }
}
