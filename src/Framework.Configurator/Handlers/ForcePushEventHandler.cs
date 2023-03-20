using System;
using System.Linq;
using System.Threading.Tasks;

using Framework.Authorization.BLL;
using Framework.Configuration;
using Framework.Configuration.BLL;
using Framework.Configuration.Domain;
using Framework.Configurator.Interfaces;
using Framework.DomainDriven;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLL.Security;

using Microsoft.AspNetCore.Http;

namespace Framework.Configurator.Handlers;

public class ForcePushEventHandler : BaseWriteHandler, IForcePushEventHandler
{
    private readonly IDomainTypeBLLFactory domainTypeBllFactory;

    private readonly IConfigurationBLLFactoryContainer configurationBllFactoryContainer;

    public ForcePushEventHandler(
            IDomainTypeBLLFactory domainTypeBllFactory,
            IConfigurationBLLFactoryContainer configurationBllFactoryContainer)
    {
        this.domainTypeBllFactory = domainTypeBllFactory;
        this.configurationBllFactoryContainer = configurationBllFactoryContainer;
    }

    public async Task Execute(HttpContext context)
    {
        var operationId = (string)context.Request.RouteValues["operationId"] ?? throw new InvalidOperationException();
        var body = await this.ParseRequestBodyAsync<RequestBodyDto>(context);

        this.ForcePush(new Guid(operationId), body);
    }

    private void ForcePush(Guid operationId, RequestBodyDto body)
    {
        var domainTypeEventOperation = this.configurationBllFactoryContainer.Default.Create<DomainTypeEventOperation>()
                                           .GetById(operationId, true);

        this.domainTypeBllFactory.Create(ConfigurationSecurityOperationCode.ForceDomainTypeEvent)
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
        }
    }
}
