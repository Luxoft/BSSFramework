using Framework.Authorization.Generated.DTO;
using Framework.Core.TypeResolving;
using Framework.Infrastructure.Integration;

using Microsoft.AspNetCore.Mvc;
using SampleSystem.Generated.DTO;
using SecuritySystem;

namespace SampleSystem.WebApiCore.Controllers.Main;

[Route("api/[controller]/[action]")]
[ApiController]
public class IntegrationController(
    ISecuritySystem securitySystem,
    TimeProvider timeProvider,
    IEventXsdExporter2 eventXsdExporter)
    : IntegrationSchemaControllerBase(securitySystem, timeProvider, eventXsdExporter)
{
    protected override string IntegrationNamespace => "http://sampleSystem.example.com/integrationEvent";

    protected override IReadOnlyCollection<Type> GetEventDTOTypes() =>
        TypeSource.FromSample(typeof(EmployeeSaveEventDTO))
                  .Types
                  .Where(z => typeof(Generated.DTO.EventDTOBase).IsAssignableFrom(z))
                  .ToList();

    protected override IReadOnlyCollection<Type> GetAuthEventDTOTypes() =>
        TypeSource.FromSample(typeof(PermissionSaveEventDTO))
                  .Types
                  .Where(z => typeof(Framework.Authorization.Generated.DTO.EventDTOBase).IsAssignableFrom(z))
                  .ToList();
}
