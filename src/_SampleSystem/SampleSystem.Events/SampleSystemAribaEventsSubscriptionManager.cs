using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.Events;

using JetBrains.Annotations;

using SampleSystem.Domain;

namespace SampleSystem.Events
{
    public class SampleSystemAribaEventsSubscriptionManager : EventsSubscriptionManagerBase<PersistentDomainObjectBase>
    {
        public SampleSystemAribaEventsSubscriptionManager(IOperationEventListenerContainer<PersistentDomainObjectBase> operationListeners, [NotNull] SampleSystemAribaLocalDBEventMessageSender messageSender)
            : base(operationListeners, messageSender)
        {
        }

        public override void Subscribe()
        {
            this.SubscribeForSaveOperation<Employee>();
        }
    }
}
