using System;
using System.Threading;

using DotNetCore.CAP;

using Framework.DomainDriven.BLL;
using Framework.DomainDriven.ServiceModel.Service;
using Framework.DomainDriven.WebApiNetCore;
using Framework.Exceptions;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using WorkflowSampleSystem.BLL;
using WorkflowSampleSystem.BLL.Core.IntegrationEvens;
using WorkflowSampleSystem.Generated.DTO;

namespace WorkflowSampleSystem.WebApiCore
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    public class CapIntegrationController : ApiControllerBase<
            IServiceEnvironment<IWorkflowSampleSystemBLLContext>,
            IWorkflowSampleSystemBLLContext, EvaluatedData<
            IWorkflowSampleSystemBLLContext, IWorkflowSampleSystemDTOMappingService>>
    {
        private readonly IMediator mediator;

        public CapIntegrationController(
                IServiceEnvironment<IWorkflowSampleSystemBLLContext> serviceEnvironment,
                IExceptionProcessor exceptionProcessor,
                IMediator mediator,
                IServiceProvider serviceProvider)
                : base(serviceEnvironment, exceptionProcessor)
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

        protected override EvaluatedData<IWorkflowSampleSystemBLLContext, IWorkflowSampleSystemDTOMappingService> GetEvaluatedData(
                IDBSession session,
                IWorkflowSampleSystemBLLContext context) =>
                new(session, context, new WorkflowSampleSystemServerPrimitiveDTOMappingService(context));
    }
}
