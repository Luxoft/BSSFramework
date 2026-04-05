using Framework.Subscriptions.Domain;

namespace Framework.Subscriptions.Metadata;

public interface IDomainObjectVersionsConverter<TSource, TTarget>
    where TSource : class
    where TTarget : class
{
    DomainObjectVersions<TTarget> Convert(DomainObjectVersions<TSource> source);
}
