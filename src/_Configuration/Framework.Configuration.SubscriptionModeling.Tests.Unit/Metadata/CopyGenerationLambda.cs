using Framework.Configuration.Core;
using Framework.Notification;

namespace Framework.Configuration.SubscriptionModeling.Tests.Unit.Metadata;

internal sealed class CopyGenerationLambda :
        LambdaMetadata<object, object, IEnumerable<NotificationMessageGenerationInfo>>
{
    public CopyGenerationLambda()
    {
        this.Lambda = (context, versions) => new List<NotificationMessageGenerationInfo>();
        this.DomainObjectChangeType = DomainObjectChangeType.Update;
    }

    internal void SetFunc(Func<object, DomainObjectVersions<object>, IEnumerable<NotificationMessageGenerationInfo>> func)
    {
        this.Lambda = func;
    }
}
