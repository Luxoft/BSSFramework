using Framework.Subscriptions.Domain;
using Framework.Subscriptions.Metadata;

namespace Framework.Subscriptions;

public class DomainObjectVersionsConverter<TSource, TTarget>(IDomainObjectConverter<TSource, TTarget> domainObjectConverter) : IDomainObjectVersionsConverter<TSource, TTarget>
    where TSource : class
    where TTarget : class
{
    public DomainObjectVersions<TTarget> Convert(DomainObjectVersions<TSource> source) => source.ChangeDomainObject(domainObjectConverter.Convert);
}
