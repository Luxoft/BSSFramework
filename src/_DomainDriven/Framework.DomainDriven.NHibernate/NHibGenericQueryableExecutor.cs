using Framework.GenericQueryable;

using Microsoft.Extensions.DependencyInjection;

using NHibernate.Linq;

namespace Framework.DomainDriven.NHibernate;

public class NHibGenericQueryableExecutor(IServiceProvider serviceProvider) : GenericQueryableExecutor
{
    protected override Type ExtensionsType { get; } = typeof(LinqExtensionMethods);

    protected override IQueryable<TSource> ApplyFetch<TSource>(IQueryable<TSource> source, string path)
    {
        return serviceProvider.GetRequiredService<INHibFetchService<TSource>>().ApplyFetch(source, path);
    }
}
