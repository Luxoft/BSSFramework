using System;

using Framework.Restriction;

namespace Framework.Configuration.Domain
{
    public class TestSubscriptionModel : DomainObjectBase
    {
        [Required]
        public Subscription Subscription { get; set; }

        [Required]
        public Guid DomainObjectId { get; set; }

        public long? Revision { get; set; }
    }
}