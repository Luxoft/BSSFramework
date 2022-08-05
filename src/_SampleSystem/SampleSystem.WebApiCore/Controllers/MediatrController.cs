using System;

using Framework.DomainDriven.BLL;
using Framework.DomainDriven.ServiceModel.Service;
using Framework.DomainDriven.WebApiNetCore;
using Framework.Exceptions;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using SampleSystem.BLL;
using SampleSystem.BLL._Command.CreateIntegrationEvent;
using SampleSystem.BLL._Command.CreateManagementUnitFluentMapping;
using SampleSystem.BLL._Query.GetEmployees;
using SampleSystem.BLL._Query.GetManagementUnitFluentMappings;
using SampleSystem.Generated.DTO;

namespace SampleSystem.WebApiCore.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("authApi/v{version:apiVersion}/[controller]/[action]")]
    public class MediatrController : ApiControllerBase<
            ISampleSystemBLLContext, EvaluatedData<
            ISampleSystemBLLContext, ISampleSystemDTOMappingService>>
    {
        private readonly IMediator mediator;

        public MediatrController(IMediator mediator) =>
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
    }
}
