using Framework.Configurator.Interfaces;
using Framework.Configurator.Models;
using Framework.DomainDriven.ApplicationCore.ExternalSource;
using Framework.DomainDriven.ApplicationCore.Security;
using Framework.SecuritySystem;

using Microsoft.AspNetCore.Http;

namespace Framework.Configurator.Handlers;

public class GetBusinessRoleContextEntitiesHandler(
    ISecurityEntitySource externalSource,
    ISecuritySystem securitySystem)
    : BaseReadHandler, IGetBusinessRoleContextEntitiesHandler
{
    protected override async Task<object> GetDataAsync(HttpContext context, CancellationToken cancellationToken)
    {
        if (!securitySystem.IsSecurityAdministrator()) return new List<EntityDto>();

        var securityContextTypeId = new Guid((string)context.Request.RouteValues["id"]!);
        var searchToken = context.Request.Query["searchToken"];

        var entities = externalSource.GetTyped(securityContextTypeId).GetSecurityEntities();

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
