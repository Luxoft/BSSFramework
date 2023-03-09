using System;
using System.Linq;
using System.Threading.Tasks;

using Framework.Configuration;
using Framework.Configuration.BLL;
using Framework.Configuration.Domain;
using Framework.Configurator.Interfaces;
using Framework.DomainDriven;
using Framework.DomainDriven.BLL;

using Microsoft.AspNetCore.Http;

namespace Framework.Configurator.Handlers
{
    public class ForcePushEventHandler<TBllContext> : BaseWriteHandler, IForcePushEventHandler
        where TBllContext : DomainDriven.BLL.Configuration.IConfigurationBLLContextContainer<IConfigurationBLLContext>
    {
        private readonly IContextEvaluator<TBllContext> _contextEvaluator;

        public ForcePushEventHandler(IContextEvaluator<TBllContext> contextEvaluator) => this._contextEvaluator = contextEvaluator;

        public async Task Execute(HttpContext context)
        {
            var operationId = (string)context.Request.RouteValues["operationId"];
            var body = await this.ParseRequestBodyAsync<RequestBodyDto>(context);

            this.ForcePush(new Guid(operationId), body);
        }

        private void ForcePush(Guid operationId, RequestBodyDto body) =>
            this._contextEvaluator.Evaluate(
                DBSessionMode.Write,
                x =>
                {
                    x.Configuration.Logics.DomainTypeFactory.Create(ConfigurationSecurityOperationCode.ForceDomainTypeEvent)
                     .ForceEvent(
                         new DomainTypeEventModel
                         {
                             Operation = x.Configuration.Logics.Default.Create<DomainTypeEventOperation>().GetById(operationId, true),
                             Revision = body.Revision,
                             DomainObjectIdents = body.Ids.Split(',').Select(i => new Guid(i)).ToList()
                         });
                });

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
}
