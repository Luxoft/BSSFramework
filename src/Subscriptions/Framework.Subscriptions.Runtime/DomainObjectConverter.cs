using CommonFramework;

using Framework.Subscriptions.Metadata;

namespace Framework.Subscriptions;

public class DomainObjectConverter<TSource, TTarget>(IServiceProxyFactory serviceProxyFactory) : IDomainObjectConverter<TSource, TTarget>
    where TSource : class
    where TTarget : class
{
    private readonly Func<TSource, TTarget> convertFunc = typeof(TSource) == typeof(TTarget)
                                                              ? (Func<TSource, TTarget>)(object)FuncHelper.Create((TSource source) => source)
                                                              : source => serviceProxyFactory.Create<TTarget>(source);

    public TTarget Convert(TSource source) => this.convertFunc(source);
}
