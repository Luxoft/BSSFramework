using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Framework.Authorization.BLL;
using Framework.Configuration.BLL;
using Framework.Configuration.Domain;
using Framework.Configurator.Interfaces;
using Framework.SecuritySystem.Exceptions;

using Microsoft.AspNetCore.Http;

namespace Framework.Configurator.Handlers;

public record ForcePushEventHandler(
        IDomainTypeBLLFactory DomainTypeBllFactory,
        IConfigurationBLLFactoryContainer ConfigurationBllFactoryContainer,
        IAuthorizationBLLContext AuthorizationSystem) : BaseWriteHandler, IForcePushEventHandler
{
    public async Task Execute(HttpContext context, CancellationToken cancellationToken)
    {
        if (!this.AuthorizationSystem.HasAccess(ConfigurationSecurityOperation.ForceDomainTypeEvent))
        {
            throw new AccessDeniedException<Guid>(
                                                  string.Empty,
                                                  Guid.Empty,
                                                  $"You are not authorized to perform {ConfigurationSecurityOperation.ForceDomainTypeEvent} operation");
        }

        var operationId = (string?)context.Request.RouteValues["operationId"] ?? throw new InvalidOperationException();
        var body = await this.ParseRequestBodyAsync<RequestBodyDto>(context);

        this.ForcePush(new Guid(operationId), body);
    }

    private void ForcePush(Guid operationId, RequestBodyDto body)
    {
        var domainTypeEventOperation = this.ConfigurationBllFactoryContainer.Default.Create<DomainTypeEventOperation>()
                                           .GetById(operationId, true);

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
        public long? Revision
        {
            get;
            set;
        }

        public string Ids
        {
            get;
            set;
        } = null!;
    }
}
