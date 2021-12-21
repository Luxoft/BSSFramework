using Framework.Configuration.SubscriptionModeling;

namespace Framework.Configuration.BLL.Core.Tests.Unit.SubscriptionSystemService3.Subscriptions.Metadata
{
    public class NonDefaultCtorSubscription : SubscriptionMetadata<object, object, RazorTemplate<object>>
    {
        public NonDefaultCtorSubscription(string dummy)
        {
        }
    }
}
