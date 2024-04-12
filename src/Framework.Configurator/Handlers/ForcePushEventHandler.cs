using Framework.Configuration.BLL;
using Framework.Configuration.Domain;
using Framework.Configurator.Interfaces;
using Framework.DomainDriven.Repository;
using Framework.SecuritySystem;
using Framework.SecuritySystem.Bss;

using Microsoft.AspNetCore.Http;

namespace Framework.Configurator.Handlers;

public record ForcePushEventHandler(
    IRepositoryFactory<DomainTypeEventOperation> EventOperationRepoFactory,
    IDomainTypeBLLFactory DomainTypeBllFactory,
    AdministratorRoleInfo AdministratorRoleInfo,
    IOperationAccessor OperationAccessor) : BaseWriteHandler, IForcePushEventHandler
{
    public async Task Execute(HttpContext context, CancellationToken cancellationToken)
    {
        this.OperationAccessor.CheckAccess(this.AdministratorRoleInfo.AdministratorRole);

        var operationId = (string)context.Request.RouteValues["operationId"]!;
        var body = await this.ParseRequestBodyAsync<RequestBodyDto>(context);

        await this.ForcePushAsync(new Guid(operationId), body, cancellationToken);
    }

    private async Task ForcePushAsync(Guid operationId, RequestBodyDto body, CancellationToken token)
    {
        var domainTypeEventOperation = await this.EventOperationRepoFactory.Create().LoadAsync(operationId, token);

        this.DomainTypeBllFactory.Create()
            .ForceEvent(
                new DomainTypeEventModel
                {
                    Operation = domainTypeEventOperation,
                    Revision = body.Revision,
                    DomainObjectIdents = body.Ids.Split(',').Select(i => new Guid(i)).ToList()
                });
    }

    private class RequestBodyDto
    {
        public long? Revision { get; }

        public string Ids { get; } = default!;
    }
}
