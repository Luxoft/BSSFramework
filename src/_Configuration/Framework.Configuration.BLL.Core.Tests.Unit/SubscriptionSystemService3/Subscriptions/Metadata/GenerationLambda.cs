using Framework.Notification;

namespace Framework.Configuration.BLL.Core.Tests.Unit.SubscriptionSystemService3.Subscriptions.Metadata;

internal sealed class GenerationLambda :
        LambdaMetadataBase<object, IEnumerable<NotificationMessageGenerationInfo>>
{
    public GenerationLambda()
    {
        this.Lambda = (context, versions) => new List<NotificationMessageGenerationInfo>();
    }
}
