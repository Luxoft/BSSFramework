using Framework.Subscriptions.Domain;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.Subscriptions.Metadata;

public abstract class LambdaMetadata<TDomainObject, TResult> : ILambdaMetadata
    where TDomainObject : class
{
    public virtual Func<IServiceProvider, DomainObjectVersions<TDomainObject>, TResult>? Lambda { get; protected init; }

    public LambdaMetadata<TNewInputDomainObject, TResult> ChangeInput<TNewInputDomainObject>()
        where TNewInputDomainObject : class =>
        new ChangedLambdaMetadata<TNewInputDomainObject, TDomainObject, TResult>(
            this,
            (sp, newDomainObject) => sp.GetRequiredService<IDomainObjectConverter<TNewInputDomainObject, TDomainObject>>().Convert(newDomainObject));

    public static implicit operator LambdaMetadata<TDomainObject, TResult>(Func<IServiceProvider, DomainObjectVersions<TDomainObject>, TResult> func) => new LambdaMetadataImplement<TDomainObject, TResult>(func);

    Delegate? ILambdaMetadata.Lambda => this.Lambda;
}

public class LambdaMetadataImplement<TDomainObject, TResult>(Func<IServiceProvider, DomainObjectVersions<TDomainObject>, TResult> func) : LambdaMetadata<TDomainObject, TResult>
    where TDomainObject : class
{
    public override Func<IServiceProvider, DomainObjectVersions<TDomainObject>, TResult>? Lambda { get; protected init; } = func;
}

public static class LambdaMetadata<TDomainObject>
    where TDomainObject : class
{
    public static LambdaMetadata<TDomainObject, TResult> Create<TResult>(Func<IServiceProvider, DomainObjectVersions<TDomainObject>, TResult> func) =>
        new LambdaMetadataImplement<TDomainObject, TResult>(func);
}
