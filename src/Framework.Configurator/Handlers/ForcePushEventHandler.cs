using Framework.Configuration.Domain;
using Framework.Configurator.Interfaces;
using Framework.DomainDriven.Repository;
using Framework.SecuritySystem;

using Microsoft.AspNetCore.Http;

// ReSharper disable UnusedAutoPropertyAccessor.Local
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local

namespace Framework.Configurator.Handlers;

public record ForcePushEventHandler(
    IRepositoryFactory<DomainTypeEventOperation> EventOperationRepoFactory,
    ISecuritySystem SecuritySystem,
    ILegacyForceEventSystem? LegacyForceEventSystem = null) : BaseWriteHandler, IForcePushEventHandler
{
    public async Task Execute(HttpContext context, CancellationToken cancellationToken)
    {
        this.SecuritySystem.CheckAccess(SecurityRole.Administrator);

        var operationId = (string)context.Request.RouteValues["operationId"]!;
        var body = await this.ParseRequestBodyAsync<RequestBodyDto>(context);

        await this.ForcePushAsync(new Guid(operationId), body, cancellationToken);
    }

    private async Task ForcePushAsync(Guid operationId, RequestBodyDto body, CancellationToken cancellationToken)
    {
        var domainTypeEventOperation = await this.EventOperationRepoFactory.Create().LoadAsync(operationId, cancellationToken);

        if (this.LegacyForceEventSystem == null)
        {
            throw new Exception($"{nameof(this.LegacyForceEventSystem)} not implemented");
        }
        else
        {
            this.LegacyForceEventSystem.ForceEvent(new DomainTypeEventModel
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
