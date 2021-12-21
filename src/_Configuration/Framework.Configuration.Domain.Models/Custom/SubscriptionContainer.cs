using System.Collections.Generic;

using Framework.DomainDriven;
using Framework.Persistent;

namespace Framework.Configuration.Domain
{
    [CreateRole(true)]
    [DetailRole(true)]
    public class SubscriptionContainer : DomainObjectBase
    {
        public SubscriptionContainer()
        {
            this.Subscriptions = new List<Subscription>();
            this.Lambdas = new List<SubscriptionLambda>();
            this.MessageTemplates = new List<MessageTemplate>();
            this.AttachmentContainers = new List<AttachmentContainer>();
        }

        public IList<Subscription> Subscriptions { get; set; }

        [MappingPriority(-1)]
        public IList<SubscriptionLambda> Lambdas { get; set; }

        [MappingPriority(-1)]
        public IList<AttachmentContainer> AttachmentContainers { get; set; }

        [MappingPriority(-2)]
        public IList<MessageTemplate> MessageTemplates { get; set; }
    }
}