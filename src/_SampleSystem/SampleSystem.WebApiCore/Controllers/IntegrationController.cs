using Framework.Authorization.Generated.DTO;
using Framework.Core;
using Framework.DomainDriven.WebApiNetCore.Integration;
using SecuritySystem;

using Microsoft.AspNetCore.Mvc;

using SampleSystem.Generated.DTO;

namespace SampleSystem.WebApiCore.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class IntegrationController : IntegrationSchemaControllerBase
{
    public IntegrationController(
        ISecuritySystem securitySystem,
        TimeProvider timeProvider,
        IEventXsdExporter2 eventXsdExporter)
        : base(securitySystem, timeProvider, eventXsdExporter)
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
