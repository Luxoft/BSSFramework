﻿using Framework.DomainDriven;
using Framework.DomainDriven.WebApiNetCore;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using SampleSystem.BLL;
using SampleSystem.BLL._Command.CreateIntegrationEvent;
using SampleSystem.BLL._Command.CreateManagementUnitFluentMapping;
using SampleSystem.BLL._Query.GetEmployees;
using SampleSystem.BLL._Query.GetManagementUnitFluentMappings;
using SampleSystem.Generated.DTO;

namespace SampleSystem.WebApiCore.Controllers;

[ApiController]
[Route("authApi/[controller]/[action]")]
public class MediatrController(IMediator mediator) : ApiControllerBase<ISampleSystemBLLContext, ISampleSystemDTOMappingService>
{
    [HttpGet]
    public GetEmployeesResponse[] GetEmployees() =>
        this.Evaluate(
            DBSessionMode.Read,
            _ => mediator.Send(new GetEmployeesQuery()).GetAwaiter().GetResult());

    [HttpGet]
    public GetManagementUnitFluentMappingsResponse[] GetManagementUnitFluentMappings() =>
        this.Evaluate(
            DBSessionMode.Read,
            _ => mediator.Send(new GetManagementUnitFluentMappingsQuery()).GetAwaiter().GetResult());

    [HttpPost]
    public Guid CreateManagementUnitFluentMappings([FromBody] CreateManagementUnitFluentMappingCommand command) =>
        this.Evaluate(
            DBSessionMode.Write,
            _ => mediator.Send(command).GetAwaiter().GetResult());

    [HttpPost]
    public void CreateIntegrationEvent() =>
        this.Evaluate(
            DBSessionMode.Write,
            _ => mediator.Send(new CreateIntegrationEventCommand()).GetAwaiter().GetResult());
}
