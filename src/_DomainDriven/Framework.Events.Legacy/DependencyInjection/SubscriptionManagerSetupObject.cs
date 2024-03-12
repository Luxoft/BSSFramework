using Microsoft.Extensions.DependencyInjection;

namespace Framework.Events.Legacy;

public class SubscriptionManagerSetupObject : ISubscriptionManagerSetupObject
{
    private readonly List<Action<IServiceCollection>> initActions = new();

    public IReadOnlyList<Action<IServiceCollection>> InitActions => this.initActions;

    public ISubscriptionManagerSetupObject Add<TSubscriptionManager>()
        where TSubscriptionManager : class, IEventOperationReceiver
    {
        this.initActions.Add(
            services =>
                services.AddScoped<IEventOperationReceiver, TSubscriptionManager>()
                        .AddKeyedScoped<IEventOperationReceiver, TSubscriptionManager>("BLL"));

        return this;
    }
}
