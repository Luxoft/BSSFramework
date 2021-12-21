using Framework.Core;
using Framework.Events;

using JetBrains.Annotations;

using SampleSystem.BLL;
using SampleSystem.Domain;

namespace SampleSystem.Events
{
    public class SampleSystemAribaEventsSubscriptionManager : EventsSubscriptionManagerBase<ISampleSystemBLLContext, PersistentDomainObjectBase>
    {
        public SampleSystemAribaEventsSubscriptionManager(ISampleSystemBLLContext context, [NotNull] IMessageSender<IDomainOperationSerializeData<PersistentDomainObjectBase>> messageSender)
            : base(context, messageSender)
        {
        }

        public override void Subscribe()
        {
            this.SubscribeForSaveOperation<Employee>();
        }
    }
}
