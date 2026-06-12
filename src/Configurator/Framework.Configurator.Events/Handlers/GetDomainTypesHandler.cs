using Anch.SecuritySystem;
using Anch.SecuritySystem.Attributes;
using Anch.SecuritySystem.Configurator.Handlers;

using Framework.Application.Events;
using Framework.Configurator.Interfaces;
using Framework.Configurator.Models;

using Microsoft.AspNetCore.Http;

namespace Framework.Configurator.Handlers;

public class GetDomainTypesHandler([WithoutRunAs] ISecuritySystem securitySystem, IEventSystem eventSystem)
    : BaseReadHandler, IGetDomainTypesHandler
{
    protected override async Task<object> GetDataAsync(HttpContext context, CancellationToken ct)
    {
        if (await securitySystem.HasAccessAsync(SecurityRole.Administrator, ct))
        {
            return eventSystem.TypeResolver
                              .Types
                              .OrderBy(t => t.AssemblyQualifiedName)
                              .ThenBy(t => t.Name)
                              .Select(t => new DomainTypeDto
                              {
                                  Name = t.Name,
                                  FullName = t.FullName!,
                                  Operations = eventSystem.DomainObjectEventMetadata.GetEventOperations(t)
                                                                       .OrderBy(o => o.Name)
                                                                       .ToList()
                              })
                              .ToList();
        }
        else
        {
            return new List<DomainTypeDto>();
        }
    }
}

