using Framework.Configurator.Interfaces;
using Framework.Core;
using Framework.DomainDriven.ApplicationCore.Security;
using Framework.SecuritySystem;

using Microsoft.AspNetCore.Http;

namespace Framework.Configurator.Handlers;

public record UpdatePermissionsHandler(
    ISecuritySystem SecuritySystem,
    ISecurityRoleSource SecurityRoleSource,
    ISecurityContextSource SecurityContextSource,
    IConfiguratorApi ConfiguratorApi,
    IConfiguratorIntegrationEvents? ConfiguratorIntegrationEvents = null) : BaseWriteHandler, IUpdatePermissionsHandler
{
    public async Task Execute(HttpContext context, CancellationToken cancellationToken)
    {
        this.SecuritySystem.CheckAccess(ApplicationSecurityRule.SecurityAdministrator);

        var principalId = new Guid((string)context.Request.RouteValues["id"]!);
        var permissions = await this.ParseRequestBodyAsync<List<RequestBodyDto>>(context);

        var typedPermissions = permissions.Select(this.ToTypedPermission).ToList();

        var mergeResult = await this.ConfiguratorApi.UpdatePermissionsAsync(principalId, typedPermissions, cancellationToken);
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
                pair => pair.Entities.ToReadOnlyListI(e => new Guid(e))));
    }

    private class RequestBodyDto
    {
        public string PermissionId { get; set; } = default!;

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
