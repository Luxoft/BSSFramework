using Framework.Application.Events;
using Framework.Configurator.Interfaces;

using Microsoft.AspNetCore.Http;

using Anch.SecuritySystem;
using Anch.SecuritySystem.Attributes;
using Anch.SecuritySystem.Configurator.Handlers;

// ReSharper disable UnusedAutoPropertyAccessor.Local

namespace Framework.Configurator.Handlers;

public class ForcePushEventHandler([WithoutRunAs] ISecuritySystem securitySystem, IEventSystem eventSystem)
    : BaseWriteHandler, IForcePushEventHandler
{
    public async Task Execute(HttpContext context, CancellationToken cancellationToken)
    {
        await securitySystem.CheckAccessAsync(SecurityRole.Administrator, cancellationToken);

        var body = await this.ParseRequestBodyAsync<RequestBodyDto>(context);

        await eventSystem.ForceEventAsync(
            new EventModel(
                eventSystem.TypeResolver.Resolve(body.DomainTypeFullName),
                [.. body.Ids.Split(',').Select(i => new Guid(i))],
                new EventOperation(body.OperationName),
                body.Revision),
            cancellationToken);
    }

    private class RequestBodyDto
    {
        public string DomainTypeFullName { get; set; } = null!;

        public string OperationName { get; set; } = null!;

        public long? Revision { get; set; }

        public string Ids { get; set; } = null!;
    }
}
