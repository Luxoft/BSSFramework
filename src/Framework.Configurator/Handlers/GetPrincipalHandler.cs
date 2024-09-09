using Framework.Configurator.Interfaces;
using Framework.Configurator.Models;
using Framework.DomainDriven.ApplicationCore.ExternalSource;
using Framework.DomainDriven.ApplicationCore.Security;
using Framework.Exceptions;
using Framework.SecuritySystem;
using Framework.SecuritySystem.ExternalSystem.Management;

using Microsoft.AspNetCore.Http;

namespace Framework.Configurator.Handlers;

public class GetPrincipalHandler(
    IPrincipalManagementService configuratorApi,
    ISecurityEntitySource externalSource,
    ISecurityContextSource securityContextSource,
    ISecurityRoleSource securityRoleSource,
    ISecuritySystem securitySystem) : BaseReadHandler, IGetPrincipalHandler
{
    protected override async Task<object> GetDataAsync(HttpContext context, CancellationToken cancellationToken)
    {
        if (!securitySystem.IsSecurityAdministrator()) return new PrincipalDetailsDto();

        var principalId = new Guid((string)context.Request.RouteValues["id"]!);

        var permissions = await this.GetPermissionsAsync(principalId, cancellationToken);

        var contexts = this.GetContextsAsync(permissions);

        return new PrincipalDetailsDto { Permissions = ToDto(permissions, contexts) };
    }

    private async Task<List<PermissionDetails>> GetPermissionsAsync(Guid principalId, CancellationToken cancellationToken)
    {
        var principal = await configuratorApi.TryGetPrincipalAsync(principalId, cancellationToken)
                        ?? throw new BusinessLogicException($"Principal with id {principalId} not found");

        return principal
               .Permissions
               .Select(
                   typedPermission =>
                       new PermissionDetails
                       {
                           Id = typedPermission.Id,
                           IsVirtual = typedPermission.IsVirtual,
                           Role = typedPermission.SecurityRole.Name,
                           RoleId = securityRoleSource.GetSecurityRole(typedPermission.SecurityRole).Id,
                           Comment = typedPermission.Comment,
                           StartDate = typedPermission.Period.StartDate,
                           EndDate = typedPermission.Period.EndDate,
                           Contexts = typedPermission
                                      .Restrictions
                                      .SelectMany(
                                          pair =>
                                              pair.Value.Select(
                                                  securityContextId =>
                                                      new KeyValuePair<Guid, Guid>(
                                                          securityContextSource.GetSecurityContextInfo(pair.Key).Id,
                                                          securityContextId)))
                                      .ToList()
                       })
               .ToList();

    }

    private Dictionary<Guid, ContextItem> GetContextsAsync(IEnumerable<PermissionDetails> permissions)
    {
        var request = from permission in permissions

                      from securityContext in permission.Contexts

                      group securityContext.Value by securityContext.Key

                      into g

                      let securityContextTypeInfo = securityContextSource.GetSecurityContextInfo(g.Key)

                      let entities = externalSource.GetTyped(g.Key)
                                                   .GetSecurityEntitiesByIdents(g.Distinct().ToList())
                                                   .ToDictionary(e => e.Id, e => e.Name)

                      select (securityContextTypeInfo.Id, new ContextItem { Context = securityContextTypeInfo.Name, Entities = entities });

        return request.ToDictionary();
    }

    private static List<PermissionDto> ToDto(IEnumerable<PermissionDetails> permissions, IReadOnlyDictionary<Guid, ContextItem> contexts) =>
        permissions
            .Select(
                x => new PermissionDto
                     {
                         Id = x.Id,
                         Role = x.Role,
                         RoleId = x.RoleId,
                         Comment = x.Comment,
                         StartDate = x.StartDate,
                         EndDate = x.EndDate,
                         IsVirtual = x.IsVirtual,
                         Contexts =
                             x.Contexts
                              .GroupBy(c => c.Key, c => c.Value)
                              .Select(
                                  g => new ContextDto
                                       {
                                           Id = g.Key,
                                           Name = contexts[g.Key].Context,
                                           Entities =
                                               g.Select(e => new EntityDto { Id = e, Name = contexts[g.Key].Entities[e] })
                                                .OrderBy(e => e.Name)
                                                .ToList()
                                       })
                              .OrderBy(c => c.Name)
                              .ToList()
                     })
            .OrderBy(x => x.Role)
            .ToList();

    private class PermissionDetails
    {
        public Guid Id { get; set; }

        public string Role { get; set; }

        public Guid RoleId { get; set; }

        public string Comment { get; set; }

        public List<KeyValuePair<Guid, Guid>> Contexts { get; set; }

        public DateTime? EndDate { get; set; }

        public DateTime StartDate { get; set; }

        public bool IsVirtual { get; set; }
    }

    private class ContextItem
    {
        public string Context { get; set; }

        public Dictionary<Guid, string> Entities { get; set; }
    }
}
