using Framework.Subscriptions;
using Framework.Subscriptions.Domain;

using DomainObjectChangeType = Framework.Subscriptions.DomainObjectChangeType;

namespace Framework.Configuration.SubscriptionModeling.Tests.Unit.Metadata;

internal sealed class ConditionLambda : LambdaMetadata<object, object, bool>
{
    public ConditionLambda()
    {
        this.Lambda = (context, versions) => true;
        this.DomainObjectChangeType = DomainObjectChangeType.Update;
    }

    internal void SetFunc(Func<object, DomainObjectVersions<object>, bool> func)
    {
        this.Lambda = func;
    }
}
