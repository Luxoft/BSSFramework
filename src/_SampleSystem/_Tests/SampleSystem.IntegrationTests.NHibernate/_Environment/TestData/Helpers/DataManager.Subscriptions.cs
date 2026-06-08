using Framework.Application;
using Framework.AutomationCore.RootServiceProviderContainer;
using Framework.Core;
using Framework.Database;
using Framework.Subscriptions;
using Framework.Subscriptions.Domain;

using Microsoft.Extensions.DependencyInjection;

namespace SampleSystem.IntegrationTests._Environment.TestData.Helpers;

public partial class DataManager
{
    public Task<List<ITryResult<SubscriptionHeader>>> ProcessSubscriptionAsync<T>(T? prev, T? next, CancellationToken ct)

        where T : class =>

        this.EvaluateAsync(
            DBSessionMode.Write,
            async context =>
            {
                var subscriptionService = context.ServiceProvider.GetRequiredService<ISubscriptionService>();

                return await subscriptionService.ProcessAsync(new DomainObjectVersions<T>(prev, next)).ToListAsync(ct);
            });
}
