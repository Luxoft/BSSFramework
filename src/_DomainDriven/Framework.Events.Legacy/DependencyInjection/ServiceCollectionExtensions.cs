using Microsoft.Extensions.DependencyInjection;

namespace Framework.Events.Legacy;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterSubscriptionManagers(this IServiceCollection services, Action<ISubscriptionManagerSetupObject> setup)
    {
        var setupObject = new SubscriptionManagerSetupObject();

        setup(setupObject);

        foreach (var setupObjectInitAction in setupObject.InitActions)
        {
            setupObjectInitAction(services);
        }

        return services;
    }
}
