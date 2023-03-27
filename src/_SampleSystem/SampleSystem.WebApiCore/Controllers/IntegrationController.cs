using Framework.Authorization.BLL;
using Framework.Authorization.Generated.DTO;
using Framework.Core;
using Framework.DomainDriven;
using Framework.DomainDriven.WebApiNetCore.Integration;

using Microsoft.AspNetCore.Mvc;

using SampleSystem.Generated.DTO;

namespace SampleSystem.WebApiCore.Controllers;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class IntegrationController : IntegrationSchemaControllerBase
{
    public IntegrationController(
            IAuthorizationBLLContext context,
            IDateTimeService dateTimeService,
            IEventXsdExporter2 eventXsdExporter)
            : base(context, dateTimeService, eventXsdExporter)
    {
    }

    protected override string IntegrationNamespace => "http://sampleSystem.example.com/integrationEvent";

    protected override IReadOnlyCollection<Type> GetEventDTOTypes() =>
            TypeSource.FromSample(typeof(EmployeeSaveEventDTO))
                      .GetTypes()
                      .Where(z => typeof(Generated.DTO.EventDTOBase).IsAssignableFrom(z))
                      .ToList();

    protected override IReadOnlyCollection<Type> GetAuthEventDTOTypes() =>
            TypeSource.FromSample(typeof(PermissionSaveEventDTO))
                      .GetTypes()
                      .Where(z => typeof(Framework.Authorization.Generated.DTO.EventDTOBase).IsAssignableFrom(z))
                      .ToList();
}
