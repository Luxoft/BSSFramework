using System;
using System.Threading;

using DotNetCore.CAP;

using Framework.DomainDriven.BLL;
using Framework.DomainDriven.ServiceModel.Service;
using Framework.DomainDriven.WebApiNetCore;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using SampleSystem.BLL;
using SampleSystem.BLL.Core.IntegrationEvens;
using SampleSystem.Generated.DTO;

namespace SampleSystem.WebApiCore
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    public class CapIntegrationController : ApiControllerBase<
            ISampleSystemBLLContext, EvaluatedData<
            ISampleSystemBLLContext, ISampleSystemDTOMappingService>>
    {
        private readonly IMediator mediator;

        public CapIntegrationController(
                IMediator mediator,
                IServiceProvider serviceProvider)
        {
            this.mediator = mediator;

            // Very important line! Passing Scoped ServiceProvider to BSS Framework.
            // CAP calling TestIntegrationEvent method without HttpContext
            this.ServiceProvider = serviceProvider;
        }

        [CapSubscribe(nameof(TestIntegrationEvent))]
        [NonAction]
        public void TestIntegrationEvent(
                TestIntegrationEvent @event,
                CancellationToken token) =>
                this.Evaluate(
                              DBSessionMode.Write,
                              _ => this.mediator.Send(@event, token).Result);

        protected override EvaluatedData<ISampleSystemBLLContext, ISampleSystemDTOMappingService> GetEvaluatedData(
                IDBSession session,
                ISampleSystemBLLContext context) =>
                new(session, context, new SampleSystemServerPrimitiveDTOMappingService(context));
    }
}
