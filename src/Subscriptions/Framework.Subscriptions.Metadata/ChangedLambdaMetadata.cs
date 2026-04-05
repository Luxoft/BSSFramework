using Framework.Subscriptions.Domain;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.Subscriptions.Metadata;

public class ChangedLambdaMetadata<TSourceDomainObject, TTargetDomainObject, TResult>(
    LambdaMetadata<TTargetDomainObject, TResult> baseLambdaMetadata) : LambdaMetadata<TSourceDomainObject, TResult>
    where TSourceDomainObject : class
    where TTargetDomainObject : class
{
    public sealed override Func<IServiceProvider, DomainObjectVersions<TSourceDomainObject>, TResult>? Lambda { get; protected init; } =

        (service, versions) => baseLambdaMetadata.Lambda(
            service,
            service.GetRequiredService<IDomainObjectVersionsConverter<TSourceDomainObject, TTargetDomainObject>>().Convert(versions));
}
