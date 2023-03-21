using Framework.Configuration.SubscriptionModeling;

namespace SampleSystem.Subscriptions.Metadata.DomainChangedByRecipients.NotPersistentCustomModel;

public sealed class ConditionLambda : ConditionLambdaBase<Domain.Country>
{
    public ConditionLambda()
    {
        this.DomainObjectChangeType = DomainObjectChangeType.Update;
        this.Lambda = (context, versions) => true;
    }
}
