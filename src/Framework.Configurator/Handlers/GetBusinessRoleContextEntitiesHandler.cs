using Framework.Authorization.Domain;
using Framework.Authorization.Environment.Security;
using Framework.Authorization.SecuritySystem.ExternalSource;
using Framework.Configurator.Interfaces;
using Framework.Configurator.Models;
using Framework.DomainDriven.Repository;
using Framework.SecuritySystem;

using Microsoft.AspNetCore.Http;

namespace Framework.Configurator.Handlers;

public class GetBusinessRoleContextEntitiesHandler(
    IRepositoryFactory<SecurityContextType> contextTypeRepositoryFactory,
    IAuthorizationExternalSource externalSource,
    IAuthorizationSystem authorizationSystem)
    : BaseReadHandler, IGetBusinessRoleContextEntitiesHandler
{
    protected override async Task<object> GetDataAsync(HttpContext context, CancellationToken cancellationToken)
    {
        if (!authorizationSystem.IsSecurityAdministrator()) return new List<EntityDto>();

        var securityContextTypeId = new Guid((string)context.Request.RouteValues["id"]!);
        var searchToken = context.Request.Query["searchToken"];

        var contextType = await contextTypeRepositoryFactory.Create().LoadAsync(securityContextTypeId, cancellationToken);
        var entities = externalSource.GetTyped(contextType).GetSecurityEntities();

        if (!string.IsNullOrWhiteSpace(searchToken))
            entities = entities.Where(p => p.Name.Contains(searchToken!, StringComparison.OrdinalIgnoreCase));

        return entities
               .Select(x => new EntityDto { Id = x.Id, Name = x.Name })
               .OrderByDescending(x => x.Name.Equals(searchToken, StringComparison.OrdinalIgnoreCase))
               .ThenBy(x => x.Name)
               .Take(70)
               .ToList();
    }
}
