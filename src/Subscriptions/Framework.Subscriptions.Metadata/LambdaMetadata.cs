using Framework.Subscriptions.Domain;

namespace Framework.Subscriptions.Metadata;

public abstract class LambdaMetadata<TDomainObject, TResult> : ILambdaMetadata
    where TDomainObject : class
{
    public virtual Func<IServiceProvider, DomainObjectVersions<TDomainObject>, TResult> Lambda { get; protected init; } = null!;

    public LambdaMetadata<TNewInputDomainObject, TResult> ChangeInput<TNewInputDomainObject>()
        where TNewInputDomainObject : class =>
        new ChangedLambdaMetadata<TNewInputDomainObject, TDomainObject, TResult>(this);

    public static implicit operator LambdaMetadata<TDomainObject, TResult>(Func<IServiceProvider, DomainObjectVersions<TDomainObject>, TResult> func) => new LambdaMetadataImplement<TDomainObject, TResult>(func);

    public static LambdaMetadata<TDomainObject, IEnumerable<TResult>> EmptyCollection { get; } = LambdaMetadata<TDomainObject>.Create((_, _) => Enumerable.Empty<TResult>());

    Delegate? ILambdaMetadata.Lambda => this.Lambda;
}

public static class LambdaMetadata<TDomainObject>
    where TDomainObject : class
{
    public static LambdaMetadata<TDomainObject, TResult> Create<TResult>(Func<IServiceProvider, DomainObjectVersions<TDomainObject>, TResult> func) =>
        new LambdaMetadataImplement<TDomainObject, TResult>(func);
}
