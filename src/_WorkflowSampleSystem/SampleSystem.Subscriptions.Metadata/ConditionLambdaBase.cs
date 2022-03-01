using Framework.Configuration.SubscriptionModeling;

using SampleSystem.BLL;

namespace SampleSystem.Subscriptions.Metadata
{
    /// <inheritdoc />
    public abstract class ConditionLambdaBase<TDomainObject> :
            LambdaMetadata<ISampleSystemBLLContext, TDomainObject, bool>
        where TDomainObject : class
    {
    }
}
