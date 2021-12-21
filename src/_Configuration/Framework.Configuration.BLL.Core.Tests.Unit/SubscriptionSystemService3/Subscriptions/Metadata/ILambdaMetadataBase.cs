using Framework.Configuration.SubscriptionModeling;

namespace Framework.Configuration.BLL.Core.Tests.Unit.SubscriptionSystemService3.Subscriptions.Metadata
{
    public interface ILambdaMetadataBase : ILambdaMetadata
    {
        void SetDomainObjectChangeType(DomainObjectChangeType changeType);
    }
}