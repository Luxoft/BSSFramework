using Framework.Configuration.BLL.SubscriptionSystemService3.Subscriptions;
using Framework.DomainDriven;
using Framework.DomainDriven.ServiceModel.IAD;

using Microsoft.AspNetCore.Mvc;

using SampleSystem.BLL;
using SampleSystem.ServiceEnvironment;

namespace SampleSystem.WebApiCore.Controllers;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class InitializationController : ControllerBase
{
    private readonly IContextEvaluator<ISampleSystemBLLContext> contextEvaluator;

    private readonly SubscriptionMetadataStore subscriptionMetadataStore;

    private readonly IInitializeManager initializeManager;

    public InitializationController(
            IContextEvaluator<ISampleSystemBLLContext> contextEvaluator,
            SubscriptionMetadataStore
                    subscriptionMetadataStore,
            IInitializeManager initializeManager)
    {
        this.contextEvaluator = contextEvaluator;
        this.subscriptionMetadataStore = subscriptionMetadataStore;
        this.initializeManager = initializeManager;
    }

    [HttpGet]
    public void SampleSystemInitializer()
    {
        var service = new SampleSystemInitializer(this.contextEvaluator, this.subscriptionMetadataStore, this.initializeManager);
        service.Initialize();
    }
}
