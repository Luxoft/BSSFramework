using Framework.Configuration.SubscriptionModeling;

using SampleSystem.BLL;

namespace SampleSystem.Subscriptions.Metadata
{
    public abstract class SubscriptionMetadataBase<TDomainObject, TTemplate> :
            SubscriptionMetadata<ISampleSystemBLLContext, TDomainObject, TTemplate>
        where TDomainObject : class
        where TTemplate : IRazorTemplate
    {
    }

    public abstract class SubscriptionWithCustomModelMetadataBase<TDomainObject, TCustomObject, TTemplate> :
            SubscriptionWithCustomModelMetadata<ISampleSystemBLLContext, TDomainObject, TCustomObject, TTemplate>
        where TDomainObject : class
        where TCustomObject : class
        where TTemplate : IRazorTemplate
    {
    }
}
