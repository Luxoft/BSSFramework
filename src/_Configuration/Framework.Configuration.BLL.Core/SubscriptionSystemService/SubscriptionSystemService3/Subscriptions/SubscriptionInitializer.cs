using Framework.Configuration.Domain;
using Framework.Core;
using Framework.DomainDriven.Repository;
using Framework.DomainDriven.Tracking;
using SecuritySystem;
using SecuritySystem.Attributes;

namespace Framework.Configuration.BLL.SubscriptionSystemService3.Subscriptions;

public class SubscriptionInitializer(
    [DisabledSecurity] IRepository<CodeFirstSubscription> subscriptionRepository,
    IConfigurationBLLContext context,
    SubscriptionMetadataStore metadataStore,
    IPersistentInfoService persistentInfoService) : ISubscriptionInitializer
{
    public async Task Initialize(CancellationToken cancellationToken)
    {
        var subscriptions = metadataStore
                            .store
                            .SelectMany(g => g)
                            .Select(
                                m => new CodeFirstSubscription(
                                    m.Code,
                                    context.GetDomainType(
                                        m.DomainObjectType,
                                        persistentInfoService.IsPersistent(m.DomainObjectType))
                                    ?? this.CreateRuntime(m.DomainObjectType)))
                            .ToArray();

        var mergeResult = subscriptionRepository.GetQueryable().GetMergeResult(subscriptions, s => s.Code, s => s.Code);

        foreach (var subscription in mergeResult.AddingItems)
        {
            await subscriptionRepository.SaveAsync(subscription, cancellationToken);
        }

        foreach (var subscription in mergeResult.RemovingItems)
        {
            await subscriptionRepository.RemoveAsync(subscription, cancellationToken);
        }
    }

    private DomainType CreateRuntime(Type type)
    {
        return new DomainType(new TargetSystem(false, false, false) { Name = "Runtime TargetSystem" })
               {
                   Name = type.Name, NameSpace = type.Namespace
               };
    }
}
