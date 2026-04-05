using Framework.Subscriptions.Domain;

namespace Framework.Subscriptions.Metadata;

public class LambdaMetadataImplement<TDomainObject, TResult>(Func<IServiceProvider, DomainObjectVersions<TDomainObject>, TResult> func) : LambdaMetadata<TDomainObject, TResult>
    where TDomainObject : class
{
    public override Func<IServiceProvider, DomainObjectVersions<TDomainObject>, TResult> Lambda { get; protected init; } = func;
}
