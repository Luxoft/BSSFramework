using CommonFramework;

using Framework.Subscriptions.Domain;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.Subscriptions.Metadata;

public abstract class LambdaMetadata<TDomainObject, TResult> : ILambdaMetadata
    where TDomainObject : class
{
    public virtual Func<IServiceProvider, DomainObjectVersions<TDomainObject>, TResult>? Lambda { get; protected init; }

    public virtual DomainObjectChangeType DomainObjectChangeType { get; protected init; }


    public LambdaMetadata<TNewInputDomainObject, TResult> ChangeInput<TNewInputDomainObject>(Func<IServiceProvider, TNewInputDomainObject, TDomainObject> selector)
        where TNewInputDomainObject : class =>
        new ChangedLambdaMetadata<TNewInputDomainObject, TDomainObject, TResult>(this, selector);

    public LambdaMetadata<TNewInputDomainObject, TResult> ChangeInput<TNewInputDomainObject>()
        where TNewInputDomainObject : class =>
        new ChangedLambdaMetadata<TNewInputDomainObject, TDomainObject, TResult>(this, (sp, newDomainObject) => sp.GetRequiredService<IServiceProxyFactory>().Create<TDomainObject>(newDomainObject));

    Delegate? ILambdaMetadata.Lambda => this.Lambda;
}
