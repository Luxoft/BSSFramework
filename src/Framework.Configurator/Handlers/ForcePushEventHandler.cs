using Framework.Configuration.Domain;
using Framework.Configurator.Interfaces;
using Framework.DomainDriven.Repository;
using Framework.SecuritySystem;

using Microsoft.AspNetCore.Http;

// ReSharper disable UnusedAutoPropertyAccessor.Local
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local

namespace Framework.Configurator.Handlers;

public class ForcePushEventHandler(
    IRepositoryFactory<DomainTypeEventOperation> eventOperationRepoFactory,
    [CurrentUserWithoutRunAs]ISecuritySystem securitySystem,
    ILegacyForceEventSystem? legacyForceEventSystem = null) : BaseWriteHandler, IForcePushEventHandler
{
    public async Task Execute(HttpContext context, CancellationToken cancellationToken)
    {
        securitySystem.CheckAccess(SecurityRole.Administrator);

        var operationId = (string)context.Request.RouteValues["operationId"]!;
        var body = await this.ParseRequestBodyAsync<RequestBodyDto>(context);

        await this.ForcePushAsync(new Guid(operationId), body, cancellationToken);
    }

    private async Task ForcePushAsync(Guid operationId, RequestBodyDto body, CancellationToken cancellationToken)
    {
        var domainTypeEventOperation = await eventOperationRepoFactory.Create().LoadAsync(operationId, cancellationToken);

        if (legacyForceEventSystem == null)
        {
            throw new Exception($"{nameof(legacyForceEventSystem)} not implemented");
        }
        else
        {
            legacyForceEventSystem.ForceEvent(new DomainTypeEventModel
                                              {
                                                  Operation = domainTypeEventOperation,
                                                  Revision = body.Revision,
                                                  DomainObjectIdents = body.Ids.Split(',').Select(i => new Guid(i)).ToList()
                                              });
        }
    }

    private class RequestBodyDto
    {
        public long? Revision { get; set; }

        public string Ids { get; set; } = default!;
    }
}
