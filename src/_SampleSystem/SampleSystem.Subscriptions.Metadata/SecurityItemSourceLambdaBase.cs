using Framework.Configuration.SubscriptionModeling;
using SecuritySystem;

using SampleSystem.BLL;

namespace SampleSystem.Subscriptions.Metadata;

/// <inheritdoc />
public abstract class SecurityItemSourceLambdaBase<TDomainObject, TResult> :
        SecurityItemSourceLambdaMetadata<ISampleSystemBLLContext, TDomainObject, TResult>
        where TDomainObject : class
        where TResult : ISecurityContext
{
}
