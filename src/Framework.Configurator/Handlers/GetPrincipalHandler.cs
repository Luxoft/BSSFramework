using Framework.Authorization.Domain;
using Framework.Authorization.SecuritySystem.ExternalSource;
using Framework.Configurator.Interfaces;
using Framework.Configurator.Models;
using Framework.DomainDriven.Repository;
using Framework.SecuritySystem;

using Microsoft.AspNetCore.Http;

using NHibernate.Linq;

namespace Framework.Configurator.Handlers;

public class GetPrincipalHandler(
    IRepositoryFactory<Permission> permissionRepoFactory,
    IRepositoryFactory<SecurityContextType> contextTypeRepoFactory,
    IAuthorizationExternalSource externalSource,
    IOperationAccessor operationAccessor) : BaseReadHandler, IGetPrincipalHandler
{
    protected override async Task<object> GetDataAsync(HttpContext context, CancellationToken cancellationToken)
    {
        if (!operationAccessor.IsAdmin()) return new PrincipalDetailsDto();

        var principalId = new Guid((string)context.Request.RouteValues["id"]!);

        var permissions = await this.GetPermissionsAsync(principalId, cancellationToken);
        var contexts = await this.GetContextsAsync(permissions, cancellationToken);
        return new PrincipalDetailsDto { Permissions = ToDto(permissions, contexts) };
    }

    private Task<List<PermissionDetails>> GetPermissionsAsync(Guid principalId, CancellationToken token) =>
        permissionRepoFactory.Create()
                             .GetQueryable()
                             .Where(x => x.Principal.Id == principalId)
                             .Select(
                                 x => new PermissionDetails
                                      {
                                          Id = x.Id,
                                          Role = x.Role.Name,
                                          Comment = x.Comment,
                                          StartDate = x.Period.StartDate,
                                          EndDate = x.Period.EndDate,
                                          Contexts = x.Restrictions
                                                      .Select(
                                                          f => new KeyValuePair<Guid, Guid>(
                                                              f.SecurityContextType.Id,
                                                              f.SecurityContextId))
                                                      .ToList()
                                      })
                             .ToListAsync(token);

    private async Task<Dictionary<Guid, ContextItem>> GetContextsAsync(IEnumerable<PermissionDetails> permissions, CancellationToken token)
    {
        var result = new Dictionary<Guid, ContextItem>();
        foreach (var group in permissions.SelectMany(x => x.Contexts).GroupBy(x => x.Key, x => x.Value))
        {
            var contextType = await contextTypeRepoFactory.Create().LoadAsync(group.Key, token);
            var entities = externalSource.GetTyped(contextType)
                                         .GetSecurityEntitiesByIdents(group.Distinct().ToList())
                                         .ToDictionary(e => e.Id, e => e.Name);

            result.Add(contextType.Id, new ContextItem { Context = contextType.Name, Entities = entities });
        }

        return result;
    }

    private static List<PermissionDto> ToDto(IEnumerable<PermissionDetails> permissions, IReadOnlyDictionary<Guid, ContextItem> contexts) =>
        permissions
            .Select(
                x => new PermissionDto
                     {
                         Id = x.Id,
                         Role = x.Role,
                         Comment = x.Comment,
                         StartDate = x.StartDate,
                         EndDate = x.EndDate,
                         Contexts = x.Contexts
                                     .GroupBy(c => c.Key, c => c.Value)
                                     .Select(
                                         g => new ContextDto
                                              {
                                                  Id = g.Key,
                                                  Name = contexts[g.Key].Context,
                                                  Entities = g
                                                             .Select(e => new EntityDto { Id = e, Name = contexts[g.Key].Entities[e] })
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

        public string Comment { get; set; }

        public List<KeyValuePair<Guid, Guid>> Contexts { get; set; }

        public DateTime? EndDate { get; set; }

        public DateTime StartDate { get; set; }
    }

    private class ContextItem
    {
        public string Context { get; set; }

        public Dictionary<Guid, string> Entities { get; set; }
    }
}
