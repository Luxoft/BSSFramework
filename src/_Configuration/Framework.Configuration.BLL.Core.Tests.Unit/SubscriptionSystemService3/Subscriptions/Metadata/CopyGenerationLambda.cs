using System.Collections.Generic;

using Framework.Notification;

namespace Framework.Configuration.BLL.Core.Tests.Unit.SubscriptionSystemService3.Subscriptions.Metadata
{
    internal sealed class CopyGenerationLambda :
        LambdaMetadataBase<object, IEnumerable<NotificationMessageGenerationInfo>>
    {
        public CopyGenerationLambda()
        {
            this.Lambda = (context, versions) => new List<NotificationMessageGenerationInfo>();
        }
    }
}
