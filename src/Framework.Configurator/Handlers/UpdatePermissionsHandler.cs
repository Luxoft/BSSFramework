using Framework.Configurator.Interfaces;
using Framework.Core;
using Framework.DomainDriven.ApplicationCore.Security;
using Framework.SecuritySystem;
using Framework.SecuritySystem.ExternalSystem.Management;

using Microsoft.AspNetCore.Http;

namespace Framework.Configurator.Handlers;

public class UpdatePermissionsHandler(
    [CurrentUserWithoutRunAs] ISecuritySystem securitySystem,
    ISecurityRoleSource securityRoleSource,
    ISecurityContextSource securityContextSource,
    IPrincipalManagementService principalManagementService,
    IConfiguratorIntegrationEvents? configuratorIntegrationEvents = null) : BaseWriteHandler, IUpdatePermissionsHandler
{
    public async Task Execute(HttpContext context, CancellationToken cancellationToken)
    {
        securitySystem.CheckAccess(ApplicationSecurityRule.SecurityAdministrator);

        var principalId = new Guid((string)context.Request.RouteValues["id"]!);
        var permissions = await this.ParseRequestBodyAsync<List<RequestBodyDto>>(context);

        var typedPermissions = permissions.Select(this.ToTypedPermission).ToList();

        var mergeResult = await principalManagementService.UpdatePermissionsAsync(principalId, typedPermissions, cancellationToken);

        if (configuratorIntegrationEvents != null)
        {
            foreach (var permission in mergeResult.AddingItems)
            {
                await configuratorIntegrationEvents.PermissionCreatedAsync(permission, cancellationToken);
            }

            foreach (var (permission, _) in mergeResult.CombineItems)
            {
                await configuratorIntegrationEvents.PermissionChangedAsync(permission, cancellationToken);
            }

            foreach (var permission in mergeResult.RemovingItems)
            {
                await configuratorIntegrationEvents.PermissionRemovedAsync(permission, cancellationToken);
            }
        }
    }

    private TypedPermission ToTypedPermission(RequestBodyDto permission)
    {
        return new TypedPermission(
            string.IsNullOrWhiteSpace(permission.PermissionId) ? Guid.Empty : new Guid(permission.PermissionId),
            permission.IsVirtual,
            securityRoleSource.GetSecurityRole(new Guid(permission.RoleId)),
            new Period(permission.StartDate, permission.EndDate),
            permission.Comment,
            permission.Contexts.ToDictionary(
                pair => securityContextSource.GetSecurityContextInfo(new Guid(pair.Id)).Type,
                pair => pair.Entities.ToReadOnlyListI(e => new Guid(e))));
    }

    private class RequestBodyDto
    {
        public string PermissionId { get; set; } = default!;

        public bool IsVirtual { get; set; }

        public string RoleId { get; set; } = default!;

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string Comment { get; set; } = default!;

        public List<ContextDto> Contexts { get; set; } = default!;

        public class ContextDto
        {
            public string Id { get; set; } = default!;

            public List<string> Entities { get; set; } = default!;
        }
    }
}
