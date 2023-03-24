using Framework.Configuration.SubscriptionModeling;

namespace Framework.Configuration.BLL.Core.Tests.Unit.SubscriptionSystemService3.Subscriptions.Metadata;

public abstract class AbstractSubscription : SubscriptionMetadata<object, object, RazorTemplate<object>>
{
    protected AbstractSubscription()
    {
    }
}
