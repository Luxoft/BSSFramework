using System;

using Framework.DomainDriven.BLL;
using Framework.DomainDriven.ServiceModel.Service;
using Framework.DomainDriven.WebApiNetCore;
using Framework.Exceptions;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using WorkflowSampleSystem.BLL;
using WorkflowSampleSystem.BLL._Command.CreateIntegrationEvent;
using WorkflowSampleSystem.BLL._Command.CreateManagementUnitFluentMapping;
using WorkflowSampleSystem.BLL._Query.GetEmployees;
using WorkflowSampleSystem.BLL._Query.GetManagementUnitFluentMappings;
using WorkflowSampleSystem.Generated.DTO;

namespace WorkflowSampleSystem.WebApiCore.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("authApi/v{version:apiVersion}/[controller]/[action]")]
    public class MediatrController : ApiControllerBase<
            IServiceEnvironment<IWorkflowSampleSystemBLLContext>,
            IWorkflowSampleSystemBLLContext, EvaluatedData<
            IWorkflowSampleSystemBLLContext, IWorkflowSampleSystemDTOMappingService>>
    {
        private readonly IMediator mediator;

        public MediatrController(
                IServiceEnvironment<IWorkflowSampleSystemBLLContext> serviceEnvironment,
                IExceptionProcessor exceptionProcessor,
                IMediator mediator)
                : base(serviceEnvironment, exceptionProcessor) =>
                this.mediator = mediator;

        [HttpGet]
        public GetEmployeesResponse[] GetEmployees() =>
                this.Evaluate(
                              DBSessionMode.Read,
                              _ => this.mediator.Send(new GetEmployeesQuery()).Result);

        [HttpGet]
        public GetManagementUnitFluentMappingsResponse[] GetManagementUnitFluentMappings() =>
                this.Evaluate(
                              DBSessionMode.Read,
                              _ => this.mediator.Send(new GetManagementUnitFluentMappingsQuery()).Result);

        [HttpPost]
        public Guid CreateManagementUnitFluentMappings([FromBody] CreateManagementUnitFluentMappingCommand command) =>
                this.Evaluate(
                              DBSessionMode.Write,
                              _ => this.mediator.Send(command).Result);

        [HttpPost]
        public void CreateIntegrationEvent() =>
                this.Evaluate(
                              DBSessionMode.Write,
                              _ => this.mediator.Send(new CreateIntegrationEventCommand()).Result);

        protected override
                EvaluatedData<IWorkflowSampleSystemBLLContext,
                IWorkflowSampleSystemDTOMappingService> GetEvaluatedData(
                IDBSession session,
                IWorkflowSampleSystemBLLContext context) =>
                new(
                    session,
                    context,
                    new WorkflowSampleSystemServerPrimitiveDTOMappingService(context));
    }
}
