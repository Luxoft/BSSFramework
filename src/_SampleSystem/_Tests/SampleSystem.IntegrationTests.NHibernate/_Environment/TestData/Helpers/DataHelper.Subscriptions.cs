using Framework.AutomationCore.RootServiceProviderContainer;
using Framework.Core;
using Framework.Subscriptions;
using Framework.Subscriptions.Domain;

using Microsoft.Extensions.DependencyInjection;

namespace SampleSystem.IntegrationTests._Environment.TestData.Helpers;

public partial class DataHelper
{
    public List<ITryResult<SubscriptionHeader>> ProcessSubscription<T>(T? prev, T? next)

        where T : class =>

        this.EvaluateWrite(context =>
        {
            var subscriptionService = context.ServiceProvider.GetRequiredService<ISubscriptionService>();

            return subscriptionService.Process(new DomainObjectVersions<T>(prev, next)).ToList();
        });
}
