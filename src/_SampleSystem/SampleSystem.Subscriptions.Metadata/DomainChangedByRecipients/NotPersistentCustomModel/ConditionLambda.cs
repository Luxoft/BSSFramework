using Framework.Subscriptions.Domain;
using Framework.Subscriptions.Metadata;

namespace SampleSystem.Subscriptions.Metadata.DomainChangedByRecipients.NotPersistentCustomModel;

public sealed class ConditionLambda : ConditionLambdaBase<Domain.Country>
{
    public ConditionLambda()
    {
        this.DomainObjectChangeType = DomainObjectChangeType.Update;
        this.Lambda = (service, versions) => true;
    }
}
