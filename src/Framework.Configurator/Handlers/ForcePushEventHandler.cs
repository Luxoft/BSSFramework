using Framework.Configurator.Interfaces;
using Framework.Events;
using Framework.SecuritySystem;

using Microsoft.AspNetCore.Http;

// ReSharper disable UnusedAutoPropertyAccessor.Local
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local

namespace Framework.Configurator.Handlers;

public class ForcePushEventHandler(
    [CurrentUserWithoutRunAs] ISecuritySystem securitySystem,
    IEventSystem? eventSystem = null) : BaseWriteHandler, IForcePushEventHandler
{
    public async Task Execute(HttpContext context, CancellationToken cancellationToken)
    {
        securitySystem.CheckAccess(SecurityRole.Administrator);

        if (eventSystem == null)
        {
            throw new Exception($"{nameof(eventSystem)} not implemented");
        }

        var body = await this.ParseRequestBodyAsync<RequestBodyDto>(context);

        await eventSystem.ForceEventAsync(
            new EventModel(
                eventSystem.TypeResolver.Resolve(body.DomainTypeFullName),
                body.Ids.Split(',').Select(i => new Guid(i)).ToList(),
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
