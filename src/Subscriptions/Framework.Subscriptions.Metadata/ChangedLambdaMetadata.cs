using Framework.Subscriptions.Domain;

namespace Framework.Subscriptions.Metadata;

public class ChangedLambdaMetadata<TSourceDomainObject, TTargetDomainObject, TResult>(
    LambdaMetadata<TTargetDomainObject, TResult> baseLambdaMetadata,
    Func<IServiceProvider, TSourceDomainObject, TTargetDomainObject> selector) : LambdaMetadata<TSourceDomainObject, TResult>
    where TSourceDomainObject : class
    where TTargetDomainObject : class
{
    public sealed override DomainObjectChangeType DomainObjectChangeType { get; protected init; } = baseLambdaMetadata.DomainObjectChangeType;

    public sealed override Func<IServiceProvider, DomainObjectVersions<TSourceDomainObject>, TResult>? Lambda { get; protected init; } =

        (service, versions) => baseLambdaMetadata.Lambda!(service, versions.ChangeDomainObject(c => selector(service, c)));
}
