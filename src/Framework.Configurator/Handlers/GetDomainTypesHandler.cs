using Framework.Configurator.Interfaces;
using Framework.Configurator.Models;
using Framework.Events;
using Framework.SecuritySystem;

using Microsoft.AspNetCore.Http;

namespace Framework.Configurator.Handlers;

public class GetDomainTypesHandler([CurrentUserWithoutRunAs] ISecuritySystem securitySystem, IEventSystem eventSystem)
    : BaseReadHandler, IGetDomainTypesHandler
{
    protected override async Task<object> GetDataAsync(HttpContext context, CancellationToken cancellationToken)
    {
        if (!securitySystem.IsAdministrator()) return new List<DomainTypeDto>();

        return eventSystem.TypeResolver.GetTypes()
                          .OrderBy(t => t.AssemblyQualifiedName)
                          .ThenBy(t => t.Name)
                          .Select(
                              t => new DomainTypeDto
                                   {
                                       Name = t.Name,
                                       FullName = t.FullName!,
                                       Operations = eventSystem.DomainObjectEventMetadata.GetEventOperations(t)
                                                               .OrderBy(o => o.Name)
                                                               .ToList()
                                   })
                          .ToList();
    }
}
