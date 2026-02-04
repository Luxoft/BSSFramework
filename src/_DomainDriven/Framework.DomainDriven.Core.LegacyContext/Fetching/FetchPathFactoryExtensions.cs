using Framework.Core;

namespace Framework.DomainDriven;

public static class FetchPathFactoryExtensions
{
    public static IFetchPathFactory<TSource> WithCompress<TSource>(this IFetchPathFactory<TSource> fetchPathFactory)
    {
        if (fetchPathFactory == null) throw new ArgumentNullException(nameof(fetchPathFactory));

        return new FuncFetchPathFactory<TSource>((domainType, source) => fetchPathFactory.Create(domainType, source).Compress());
    }

    private class FuncFetchPathFactory<T>(Func<Type, T, IEnumerable<PropertyPath>> createFunc)
        : FuncFactory<Type, T, IEnumerable<PropertyPath>>(createFunc), IFetchPathFactory<T>;
}
