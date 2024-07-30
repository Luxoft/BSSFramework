using Framework.Configuration.Domain;
using Framework.DomainDriven.Repository;
using Framework.DomainDriven.Tracking;
using Framework.SecuritySystem;

namespace Framework.Configuration.BLL.SubscriptionSystemService3.Subscriptions;

public class SubscriptionInitializer(
    [DisabledSecurity] IRepository<CodeFirstSubscription> targetSystemTargetSystem,
    IConfigurationBLLContext context,
    SubscriptionMetadataStore metadataStore,
    IPersistentInfoService persistentInfoService) : ISubscriptionInitializer
{
    public async Task Initialize(CancellationToken cancellationToken)
    {
        var subscriptions = metadataStore
                            .store
                            .SelectMany(g => g)
                            .Select(m => new CodeFirstSubscription(
                                        m.Code,
                                        context.GetDomainType(
                                            m.DomainObjectType,
                                            persistentInfoService.IsPersistent(m.DomainObjectType))
                                        ?? new DomainType(new TargetSystem(false, false, false))))
                            .ToArray();

        foreach (var subscription in subscriptions)
        {
            await targetSystemTargetSystem.SaveAsync(subscription, cancellationToken);
        }
    }
}
