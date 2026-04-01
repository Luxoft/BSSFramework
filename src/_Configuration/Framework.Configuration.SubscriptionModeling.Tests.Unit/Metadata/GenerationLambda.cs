using Framework.Subscriptions;
using Framework.Subscriptions.Domain;

using DomainObjectChangeType = Framework.Subscriptions.DomainObjectChangeType;

namespace Framework.Configuration.SubscriptionModeling.Tests.Unit.Metadata;

internal sealed class GenerationLambda :
        LambdaMetadata<object, object, IEnumerable<NotificationMessageGenerationInfo>>
{
    public GenerationLambda()
    {
        this.Lambda = (context, versions) => new List<NotificationMessageGenerationInfo>();
        this.DomainObjectChangeType = DomainObjectChangeType.Update;
    }

    internal void SetFunc(
            Func<object, DomainObjectVersions<object>, IEnumerable<NotificationMessageGenerationInfo>> func) =>
        this.Lambda = func;
}
