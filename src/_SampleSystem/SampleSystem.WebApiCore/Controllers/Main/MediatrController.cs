using Framework.Infrastructure;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using SampleSystem.BLL;
using SampleSystem.BLL._Query.GetEmployees;
using SampleSystem.BLL._Query.GetManagementUnitFluentMappings;
using SampleSystem.BLL.Command.CreateIntegrationEvent;
using SampleSystem.BLL.Command.CreateManagementUnitFluentMapping;
using SampleSystem.Generated.DTO;

namespace SampleSystem.WebApiCore.Controllers.Main;

[ApiController]
[Route("authApi/[controller]/[action]")]
public class MediatrController(IMediator mediator) : ApiControllerBase<ISampleSystemBLLContext, ISampleSystemDTOMappingService>
{
    [HttpGet]
    public Task<GetEmployeesResponse[]> GetEmployees(CancellationToken ct) => mediator.Send(new GetEmployeesQuery(), ct);

    [HttpGet]
    public Task<GetManagementUnitFluentMappingsResponse[]> GetManagementUnitFluentMappings(CancellationToken ct) =>
        mediator.Send(new GetManagementUnitFluentMappingsQuery(), ct);

    [HttpPost]
    public Task<Guid> CreateManagementUnitFluentMappings([FromBody] CreateManagementUnitFluentMappingCommand command, CancellationToken ct) =>
        mediator.Send(command, ct);

    [HttpPost]
    public Task CreateIntegrationEvent(CancellationToken ct) =>
        mediator.Send(new CreateIntegrationEventCommand(), ct);
}
