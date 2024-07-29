using Framework.Configuration.Domain;

namespace Framework.Configuration.BLL.SubscriptionSystemService3.Subscriptions;

public class SubscriptionInitializer(
    ICodeFirstSubscriptionBLL bll,
    IConfigurationBLLContext context,
    SubscriptionMetadataStore metadataStore) : ISubscriptionInitializer
{
    public void Init()
    {
        var subscriptions = metadataStore
                            .store
                            .SelectMany(g => g)
                            .Select(m => new CodeFirstSubscription(m.Code, context.GetDomainType(m.DomainObjectType, true)))
                            .ToArray();

        bll.Save(subscriptions);
    }
}
