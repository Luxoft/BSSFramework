using Framework.DomainDriven;

using Microsoft.AspNetCore.Mvc;

using SampleSystem.ServiceEnvironment;

namespace SampleSystem.WebApiCore.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class InitializationController(
    IDBSessionEvaluator sessionEvaluator,
    IInitializeManager initializeManager)
    : ControllerBase
{
    [HttpPost]
    public async Task RunInitialize(CancellationToken cancellationToken)
    {
        var service = new SampleSystemInitializer(sessionEvaluator, initializeManager);

        await service.InitializeAsync(cancellationToken);
    }
}
