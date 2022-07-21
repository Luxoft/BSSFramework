using Framework.Events;

using JetBrains.Annotations;

using SampleSystem.Domain;

namespace SampleSystem.Events
{
    public class SampleSystemAribaEventsSubscriptionManager : EventsSubscriptionManagerBase<PersistentDomainObjectBase>
    {
        public SampleSystemAribaEventsSubscriptionManager([NotNull] SampleSystemAribaLocalDBEventMessageSender messageSender)
            : base(messageSender)
        {
        }

        public override void Subscribe()
        {
            this.SubscribeForSaveOperation<Employee>();
        }
    }
}
