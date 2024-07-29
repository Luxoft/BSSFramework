using Framework.Configuration.BLL.SubscriptionSystemService3.Subscriptions;
using Framework.DomainDriven;
using Framework.DomainDriven.ServiceModel.IAD;

using Microsoft.AspNetCore.Mvc;

using SampleSystem.BLL;
using SampleSystem.ServiceEnvironment;

namespace SampleSystem.WebApiCore.Controllers;

[Route("api/[controller]")]
[ApiController]
public class InitializationController(
    IServiceEvaluator<ISampleSystemBLLContext> contextEvaluator,
    IInitializeManager initializeManager)
    : ControllerBase
{
    [HttpGet]
    public async Task SampleSystemInitializer(CancellationToken cancellationToken)
    {
        var service = new SampleSystemInitializer(contextEvaluator, initializeManager);

        await service.InitializeAsync(cancellationToken);
    }
}
