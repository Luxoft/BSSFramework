using System.Collections.Generic;

using Framework.Configuration.SubscriptionModeling;
using Framework.Persistent;

using DomainObjectChangeType = Framework.Configuration.SubscriptionModeling.DomainObjectChangeType;

namespace Framework.Configuration.BLL.Core.Tests.Unit.SubscriptionSystemService3.Subscriptions.Metadata
{
    internal sealed class SecurityItemSourceLambda : SecurityItemSourceLambdaMetadata<object, object, ManagementUnit>
    {
        public SecurityItemSourceLambda()
        {
            this.DomainObjectChangeType = DomainObjectChangeType.Update;
            this.ExpandType = NotificationExpandType.Direct;
            this.Lambda = (context, versions) => new List<ManagementUnit>();
        }

        internal void SetDomainObjectChangeType(DomainObjectChangeType changeType)
        {
            this.DomainObjectChangeType = changeType;
        }
    }
}
